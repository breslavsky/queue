﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.18444
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Queue.Database {
    using System;
    
    
    /// <summary>
    ///   Класс ресурса со строгой типизацией для поиска локализованных строк и т.д.
    /// </summary>
    // Этот класс создан автоматически классом StronglyTypedResourceBuilder
    // с помощью такого средства, как ResGen или Visual Studio.
    // Чтобы добавить или удалить член, измените файл .ResX и снова запустите ResGen
    // с параметром /str или перестройте свой проект VS.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class SchemePatches {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal SchemePatches() {
        }
        
        /// <summary>
        ///   Возвращает кэшированный экземпляр ResourceManager, использованный этим классом.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Queue.Database.SchemePatches", typeof(SchemePatches).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Перезаписывает свойство CurrentUICulture текущего потока для всех
        ///   обращений к ресурсу с помощью этого класса ресурса со строгой типизацией.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на select true;.
        /// </summary>
        internal static string _001 {
            get {
                return ResourceManager.GetString("_001", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на update _config_media set TickerSpeed = 5;.
        /// </summary>
        internal static string _002 {
            get {
                return ResourceManager.GetString("_002", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на update _client_request set WaitingStartTime = RequestTime;.
        /// </summary>
        internal static string _003 {
            get {
                return ResourceManager.GetString("_003", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на update _user set Role = &apos;Administrator&apos; where Role = &apos;Manager&apos;;.
        /// </summary>
        internal static string _004 {
            get {
                return ResourceManager.GetString("_004", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на update _user set [Permissions] = 511 where Role  = &apos;Administrator&apos;;.
        /// </summary>
        internal static string _005 {
            get {
                return ResourceManager.GetString("_005", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на update _user set [Permissions] = 511 where Role  = &apos;Administrator&apos;;.
        /// </summary>
        internal static string _006 {
            get {
                return ResourceManager.GetString("_006", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на ALTER TABLE _client_request DROP CONSTRAINT ClientRequestToClientReference;
        ///ALTER TABLE _client_request ADD CONSTRAINT ClientRequestToClientReference FOREIGN KEY(ClientId)
        ///REFERENCES _client (Id)
        ///ON DELETE CASCADE;
        ///-- SEPARATOR
        ///ALTER TABLE _client_request DROP CONSTRAINT ClientRequestToOperatorReference;
        ///ALTER TABLE _client_request ADD CONSTRAINT ClientRequestToOperatorReference FOREIGN KEY(OperatorId)
        ///REFERENCES _user (Id)
        ///ON DELETE SET NULL;
        ///-- SEPARATOR
        ///ALTER TABLE _client_request DROP CONSTRAI [остаток строки не уместился]&quot;;.
        /// </summary>
        internal static string constraint {
            get {
                return ResourceManager.GetString("constraint", resourceCulture);
            }
        }
    }
}
