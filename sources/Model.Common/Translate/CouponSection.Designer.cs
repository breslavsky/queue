﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18444
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
    public class CouponSection {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal CouponSection() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Queue.Model.Common.Translate.CouponSection", typeof(CouponSection).Assembly);
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
        ///   Looks up a localized string similar to Объектов.
        /// </summary>
        public static string Objects {
            get {
                return ResourceManager.GetString("Objects", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Позиция в очереди.
        /// </summary>
        public static string Position {
            get {
                return ResourceManager.GetString("Position", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Дата запроса.
        /// </summary>
        public static string RequestDate {
            get {
                return ResourceManager.GetString("RequestDate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Время запроса.
        /// </summary>
        public static string RequestTime {
            get {
                return ResourceManager.GetString("RequestTime", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Услуга.
        /// </summary>
        public static string Service {
            get {
                return ResourceManager.GetString("Service", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Время ожидания.
        /// </summary>
        public static string WaitingTime {
            get {
                return ResourceManager.GetString("WaitingTime", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Места вызова.
        /// </summary>
        public static string Workplaces {
            get {
                return ResourceManager.GetString("Workplaces", resourceCulture);
            }
        }
    }
}
