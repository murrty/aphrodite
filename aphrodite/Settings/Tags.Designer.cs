﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace aphrodite.Settings {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "12.0.0.0")]
    internal sealed partial class Tags : global::System.Configuration.ApplicationSettingsBase {
        
        private static Tags defaultInstance = ((Tags)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Tags())));
        
        public static Tags Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool Safe {
            get {
                return ((bool)(this["Safe"]));
            }
            set {
                this["Safe"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool Questionable {
            get {
                return ((bool)(this["Questionable"]));
            }
            set {
                this["Questionable"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool Explicit {
            get {
                return ((bool)(this["Explicit"]));
            }
            set {
                this["Explicit"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool enableScoreMin {
            get {
                return ((bool)(this["enableScoreMin"]));
            }
            set {
                this["enableScoreMin"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public int scoreMin {
            get {
                return ((int)(this["scoreMin"]));
            }
            set {
                this["scoreMin"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public int imageLimit {
            get {
                return ((int)(this["imageLimit"]));
            }
            set {
                this["imageLimit"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool separateRatings {
            get {
                return ((bool)(this["separateRatings"]));
            }
            set {
                this["separateRatings"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public int pageLimit {
            get {
                return ((int)(this["pageLimit"]));
            }
            set {
                this["pageLimit"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool scoreAsTag {
            get {
                return ((bool)(this["scoreAsTag"]));
            }
            set {
                this["scoreAsTag"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("%md5%")]
        public string fileNameSchema {
            get {
                return ((string)(this["fileNameSchema"]));
            }
            set {
                this["fileNameSchema"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool separateNonImages {
            get {
                return ((bool)(this["separateNonImages"]));
            }
            set {
                this["separateNonImages"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool downloadBlacklisted {
            get {
                return ((bool)(this["downloadBlacklisted"]));
            }
            set {
                this["downloadBlacklisted"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool DownloadNewestToOldest {
            get {
                return ((bool)(this["DownloadNewestToOldest"]));
            }
            set {
                this["DownloadNewestToOldest"] = value;
            }
        }
    }
}
