﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Queue.Model.Common.Translate {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class ClientRequestState {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ClientRequestState() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Queue.Model.Common.Translate.ClientRequestState", typeof(ClientRequestState).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Отсутствует.
        /// </summary>
        public static string Absence {
            get {
                return ResourceManager.GetString("Absence", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Вызывается.
        /// </summary>
        public static string Calling {
            get {
                return ResourceManager.GetString("Calling", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Отменен.
        /// </summary>
        public static string Canceled {
            get {
                return ResourceManager.GetString("Canceled", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Отложен.
        /// </summary>
        public static string Postponed {
            get {
                return ResourceManager.GetString("Postponed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Обслужен.
        /// </summary>
        public static string Rendered {
            get {
                return ResourceManager.GetString("Rendered", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Обслуживается.
        /// </summary>
        public static string Rendering {
            get {
                return ResourceManager.GetString("Rendering", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Ожидание.
        /// </summary>
        public static string Waiting {
            get {
                return ResourceManager.GetString("Waiting", resourceCulture);
            }
        }
    }
}