﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace aphrodite {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "12.0.0.0")]
    internal sealed partial class Images : global::System.Configuration.ApplicationSettingsBase {
        
        private static Images defaultInstance = ((Images)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Images())));
        
        public static Images Default {
            get {
                return defaultInstance;
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
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool separateBlacklisted {
            get {
                return ((bool)(this["separateBlacklisted"]));
            }
            set {
                this["separateBlacklisted"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool useForm {
            get {
                return ((bool)(this["useForm"]));
            }
            set {
                this["useForm"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool separateArtists {
            get {
                return ((bool)(this["separateArtists"]));
            }
            set {
                this["separateArtists"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("%artist%_%md5%")]
        public string fileNameSchema {
            get {
                return ((string)(this["fileNameSchema"]));
            }
            set {
                this["fileNameSchema"] = value;
            }
        }
    }
}
