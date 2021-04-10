// Copyright (c) Sven Groot (Ookii.org) 2006
// See LICENSE for details
using System;
using System.Collections.Generic;
using System.Text;

namespace Ookii.Dialogs.WinForms
{
    class VistaFileDialogEvents : Ookii.Dialogs.WinForms.Interop.IFileDialogEvents, Ookii.Dialogs.WinForms.Interop.IFileDialogControlEvents
    {
        const uint S_OK = 0;
        const uint S_FALSE = 1;
        const uint E_NOTIMPL = 0x80004001;

        private VistaFileDialog _dialog;

        public VistaFileDialogEvents(VistaFileDialog dialog)
        {
            if( dialog == null )
                throw new ArgumentNullException("dialog");

            _dialog = dialog;
        }

        #region IFileDialogEvents Members

        public Interop.HRESULT OnFileOk(Ookii.Dialogs.WinForms.Interop.IFileDialog pfd)
        {
            if( _dialog.DoFileOk(pfd) )
                return Ookii.Dialogs.WinForms.Interop.HRESULT.S_OK;
            else
                return Ookii.Dialogs.WinForms.Interop.HRESULT.S_FALSE;
        }

        public Interop.HRESULT OnFolderChanging(Ookii.Dialogs.WinForms.Interop.IFileDialog pfd, Ookii.Dialogs.WinForms.Interop.IShellItem psiFolder)
        {
            return Ookii.Dialogs.WinForms.Interop.HRESULT.S_OK;
        }

        public void OnFolderChange(Ookii.Dialogs.WinForms.Interop.IFileDialog pfd)
        {
        }

        public void OnSelectionChange(Ookii.Dialogs.WinForms.Interop.IFileDialog pfd)
        {
        }

        public void OnShareViolation(Ookii.Dialogs.WinForms.Interop.IFileDialog pfd, Ookii.Dialogs.WinForms.Interop.IShellItem psi, out NativeMethods.FDE_SHAREVIOLATION_RESPONSE pResponse)
        {
            pResponse = NativeMethods.FDE_SHAREVIOLATION_RESPONSE.FDESVR_DEFAULT;
        }

        public void OnTypeChange(Ookii.Dialogs.WinForms.Interop.IFileDialog pfd)
        {
        }

        public void OnOverwrite(Ookii.Dialogs.WinForms.Interop.IFileDialog pfd, Ookii.Dialogs.WinForms.Interop.IShellItem psi, out NativeMethods.FDE_OVERWRITE_RESPONSE pResponse)
        {
            pResponse = NativeMethods.FDE_OVERWRITE_RESPONSE.FDEOR_DEFAULT;
        }

        #endregion

        #region IFileDialogControlEvents Members

        public void OnItemSelected(Ookii.Dialogs.WinForms.Interop.IFileDialogCustomize pfdc, int dwIDCtl, int dwIDItem)
        {
        }

        public void OnButtonClicked(Ookii.Dialogs.WinForms.Interop.IFileDialogCustomize pfdc, int dwIDCtl)
        {
            if( dwIDCtl == VistaFileDialog.HelpButtonId )
                _dialog.DoHelpRequest();
        }

        public void OnCheckButtonToggled(Ookii.Dialogs.WinForms.Interop.IFileDialogCustomize pfdc, int dwIDCtl, bool bChecked)
        {
        }

        public void OnControlActivating(Ookii.Dialogs.WinForms.Interop.IFileDialogCustomize pfdc, int dwIDCtl)
        {
        }

        #endregion


    }
}
