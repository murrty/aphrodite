//Copyright (c) 2018, wyDay
//All rights reserved.

//Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
//Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
//Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.

//THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace aphrodite.Controls {

    [ToolboxBitmap(typeof(ProgressBar))]
    public class ExtendedProgressBar : ProgressBar {
        bool showInTaskbar;
        private ProgressBarState m_State = ProgressBarState.Normal;
        ContainerControl ownerForm;

        public ExtendedProgressBar() { }

        public ExtendedProgressBar(ContainerControl parentControl) {
            ContainerControl = parentControl;
        }
        public ContainerControl ContainerControl {
            get { return ownerForm; }
            set {
                ownerForm = value;

                if (!ownerForm.Visible)
                    ((Form)ownerForm).Shown += Windows7ProgressBar_Shown;
            }
        }
        public override ISite Site {
            set {
                // Runs at design time, ensures designer initializes ContainerControl
                base.Site = value;
                if (value == null) return;
                IDesignerHost service = value.GetService(typeof(IDesignerHost)) as IDesignerHost;
                if (service == null) return;
                IComponent rootComponent = service.RootComponent;

                ContainerControl = rootComponent as ContainerControl;
            }
        }

        void Windows7ProgressBar_Shown(object sender, System.EventArgs e) {
            if (ShowInTaskbar) {
                if (Style != ProgressBarStyle.Marquee)
                    SetValueInTB();

                SetStateInTB();
            }

            ((Form)ownerForm).Shown -= Windows7ProgressBar_Shown;
        }

        /// <summary>
        /// Show progress in taskbar
        /// </summary>
        [DefaultValue(false)]
        public bool ShowInTaskbar {
            get {
                return showInTaskbar;
            }
            set {
                if (showInTaskbar != value) {
                    showInTaskbar = value;

                    // send signal to the taskbar.
                    if (ownerForm != null) {
                        if (Style != ProgressBarStyle.Marquee)
                            SetValueInTB();

                        SetStateInTB();
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the current position of the progress bar.
        /// </summary>
        /// <returns>The position within the range of the progress bar. The default is 0.</returns>
        public new int Value {
            get {
                return base.Value;
            }
            set {
                //switch (base.Value > 0) {
                //    case true:
                //        base.Value--;
                //        break;
                //}
                base.Value = value;

                // send signal to the taskbar.
                SetValueInTB();
            }
        }

        /// <summary>
        /// Gets or sets the manner in which progress should be indicated on the progress bar.
        /// </summary>
        /// <returns>One of the ProgressBarStyle values. The default is ProgressBarStyle.Blocks</returns>
        public new ProgressBarStyle Style {
            get {
                return base.Style;
            }
            set {
                base.Style = value;

                // set the style of the progress bar
                if (showInTaskbar && ownerForm != null) {
                    SetStateInTB();
                }
            }
        }


        /// <summary>
        /// The progress bar state for Windows Vista & 7
        /// </summary>
        [DefaultValue(ProgressBarState.Normal)]
        public ProgressBarState State {
            get { return m_State; }
            set {
                m_State = value;

                bool wasMarquee = Style == ProgressBarStyle.Marquee;

                if (wasMarquee)
                    // sets the state to normal (and implicity calls SetStateInTB() )
                    Style = ProgressBarStyle.Blocks;

                // set the progress bar state (Normal, Error, Paused)
                NativeMethods.SendMessage(Handle, 0x410, (IntPtr)value, IntPtr.Zero);


                if (wasMarquee)
                    // the Taskbar PB value needs to be reset
                    SetValueInTB();
                else
                    // there wasn't a marquee, thus we need to update the taskbar
                    SetStateInTB();
            }
        }

        /// <summary>
        /// Advances the current position of the progress bar by the specified amount.
        /// </summary>
        /// <param name="value">The amount by which to increment the progress bar's current position.</param>
        public new void Increment(int value) {
            base.Increment(value);

            // send signal to the taskbar.
            SetValueInTB();
        }

        /// <summary>
        /// Advances the current position of the progress bar by the amount of the System.Windows.Forms.ProgressBar.Step property.
        /// </summary>
        public new void PerformStep() {
            base.PerformStep();

            // send signal to the taskbar.
            SetValueInTB();
        }

        private void SetValueInTB() {
            if (showInTaskbar) {
                ulong maximum = (ulong)(Maximum - Minimum);
                ulong progress = (ulong)(Value - Minimum);

                TaskbarProgress.SetProgressValue(ownerForm.Handle, progress, maximum);
            }
        }

        private void SetStateInTB() {
            if (ownerForm == null) return;

            ThumbnailProgressState thmState = ThumbnailProgressState.Normal;

            if (!showInTaskbar)
                thmState = ThumbnailProgressState.NoProgress;
            else if (Style == ProgressBarStyle.Marquee)
                thmState = ThumbnailProgressState.Indeterminate;
            else if (m_State == ProgressBarState.Error)
                thmState = ThumbnailProgressState.Error;
            else if (m_State == ProgressBarState.Pause)
                thmState = ThumbnailProgressState.Paused;

            TaskbarProgress.SetProgressState(ownerForm.Handle, thmState);
        }
    }

    /// <summary>
    /// The progress bar state for Windows Vista & 7
    /// </summary>
    public enum ProgressBarState {
        /// <summary>
        /// Indicates normal progress
        /// </summary>
        Normal = 1,

        /// <summary>
        /// Indicates an error in the progress
        /// </summary>
        Error = 2,

        /// <summary>
        /// Indicates paused progress
        /// </summary>
        Pause = 3
    }

    public static class TaskbarProgress {
        private static object ListLock = new object();
        /// <summary>
        /// The primary coordinator of the Windows 7 taskbar-related activities.
        /// </summary>
        private static ITaskbarList3 _taskbarList;
        internal static ITaskbarList3 TaskbarList {
            get {
                if (_taskbarList == null) {
                    //lock (typeof(TaskbarProgress)) {
                    lock (ListLock) {
                        if (_taskbarList == null) {
                            _taskbarList = (ITaskbarList3)new CTaskbarList();
                            _taskbarList.HrInit();
                        }
                    }
                }
                return _taskbarList;
            }
        }

        static readonly OperatingSystem osInfo = Environment.OSVersion;

        internal static bool Windows7OrGreater {
            get {
                return (osInfo.Version.Major == 6 && osInfo.Version.Minor >= 1)
                    || (osInfo.Version.Major > 6);
            }
        }

        /// <summary>
        /// Sets the progress state of the specified window's
        /// taskbar button.
        /// </summary>
        /// <param name="hwnd">The window handle.</param>
        /// <param name="state">The progress state.</param>
        public static void SetProgressState(IntPtr hwnd, ThumbnailProgressState state) {
            if (Windows7OrGreater)
                TaskbarList.SetProgressState(hwnd, state);
        }
        /// <summary>
        /// Sets the progress value of the specified window's
        /// taskbar button.
        /// </summary>
        /// <param name="hwnd">The window handle.</param>
        /// <param name="current">The current value.</param>
        /// <param name="maximum">The maximum value.</param>
        public static void SetProgressValue(IntPtr hwnd, ulong current, ulong maximum) {
            if (Windows7OrGreater)
                TaskbarList.SetProgressValue(hwnd, current, maximum);
        }

    }

    /// <summary>
    /// Represents the thumbnail progress bar state.
    /// </summary>
    public enum ThumbnailProgressState {
        /// <summary>
        /// No progress is displayed.
        /// </summary>
        NoProgress = 0,
        /// <summary>
        /// The progress is indeterminate (marquee).
        /// </summary>
        Indeterminate = 0x1,
        /// <summary>
        /// Normal progress is displayed.
        /// </summary>
        Normal = 0x2,
        /// <summary>
        /// An error occurred (red).
        /// </summary>
        Error = 0x4,
        /// <summary>
        /// The operation is paused (yellow).
        /// </summary>
        Paused = 0x8
    }

    //Based on Rob Jarett's wrappers for the desktop integration PDC demos.
    [ComImportAttribute()]
    [GuidAttribute("ea1afb91-9e28-4b86-90e9-9e9f8a5eefaf")]
    [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface ITaskbarList3 {
        // ITaskbarList
        [PreserveSig]
        void HrInit();
        [PreserveSig]
        void AddTab(IntPtr hwnd);
        [PreserveSig]
        void DeleteTab(IntPtr hwnd);
        [PreserveSig]
        void ActivateTab(IntPtr hwnd);
        [PreserveSig]
        void SetActiveAlt(IntPtr hwnd);

        // ITaskbarList2
        [PreserveSig]
        void MarkFullscreenWindow(
            IntPtr hwnd,
            [MarshalAs(UnmanagedType.Bool)] bool fFullscreen);

        // ITaskbarList3
        void SetProgressValue(IntPtr hwnd, UInt64 ullCompleted, UInt64 ullTotal);
        void SetProgressState(IntPtr hwnd, ThumbnailProgressState tbpFlags);

        // yadda, yadda - there's more to the interface, but we don't need it.
    }

    [GuidAttribute("56FDF344-FD6D-11d0-958A-006097C9A090")]
    [ClassInterfaceAttribute(ClassInterfaceType.None)]
    [ComImportAttribute()]
    internal class CTaskbarList { }
}