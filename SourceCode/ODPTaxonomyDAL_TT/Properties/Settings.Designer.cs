﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ODPTaxonomyDAL_TT.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "14.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=1sql234;Initial Catalog=ODP_Taxonomy_DEV;User ID=odpTaxonomy;Password" +
            "=0dpTaxonomy!")]
        public string ODP_Taxonomy_DEVConnectionString {
            get {
                return ((string)(this["ODP_Taxonomy_DEVConnectionString"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=2sql235;Initial Catalog=ODP_Taxonomy_Dev;User ID=odpTaxonomy;Password" +
            "=0dpTaxonomy!")]
        public string ODP_Taxonomy_DevConnectionString1 {
            get {
                return ((string)(this["ODP_Taxonomy_DevConnectionString1"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=2sql235;Initial Catalog=ODP_Taxonomy_Dev;Integrated Security=True")]
        public string ODP_Taxonomy_DevConnectionString2 {
            get {
                return ((string)(this["ODP_Taxonomy_DevConnectionString2"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=2sql235;Initial Catalog=ODP_Taxonomy_Dev;Persist Security Info=True;U" +
            "ser ID=odpTaxonomy")]
        public string ODP_Taxonomy_DevConnectionString3 {
            get {
                return ((string)(this["ODP_Taxonomy_DevConnectionString3"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=2sql235.iq.iqsolutions.com;Initial Catalog=ODP_Taxonomy_DEV;Persist S" +
            "ecurity Info=True;User ID=odpTaxonomy")]
        public string ODP_Taxonomy_DEVConnectionString4 {
            get {
                return ((string)(this["ODP_Taxonomy_DEVConnectionString4"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=2sql235.iq.iqsolutions.com;Initial Catalog=ODP_Taxonomy_DEV;Persist S" +
            "ecurity Info=True;User ID=odpTaxonomy;Password=0dpTaxonomy!")]
        public string ODP_Taxonomy_DEVConnectionString5 {
            get {
                return ((string)(this["ODP_Taxonomy_DEVConnectionString5"]));
            }
        }
    }
}
