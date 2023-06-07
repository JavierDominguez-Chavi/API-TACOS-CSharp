﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TACOSMenuAPI.Modelos {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Mensajes {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Mensajes() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("TACOSMenuAPI.Modelos.Mensajes", typeof(Mensajes).Assembly);
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
        ///   Looks up a localized string similar to El alimento solicitado no existe..
        /// </summary>
        public static string ActualizarExistenciaAlimentos_404 {
            get {
                return ResourceManager.GetString("ActualizarExistenciaAlimentos_404", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to La existencia del alimento solicitado ya no puede decrecer..
        /// </summary>
        public static string ActualizarExistenciaAlimentos_409 {
            get {
                return ResourceManager.GetString("ActualizarExistenciaAlimentos_409", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Debe seleccionar un pedido..
        /// </summary>
        public static string ActualizarPedido_400 {
            get {
                return ResourceManager.GetString("ActualizarPedido_400", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to El pedido no puede cambiar su estado, pues ya fue pagado..
        /// </summary>
        public static string ActualizarPedido_403 {
            get {
                return ResourceManager.GetString("ActualizarPedido_403", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to El pedido solicitado no existe..
        /// </summary>
        public static string ActualizarPedido_404 {
            get {
                return ResourceManager.GetString("ActualizarPedido_404", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to El pedido solicitado pertenece a un miembro distinto..
        /// </summary>
        public static string ActualizarPedido_422 {
            get {
                return ResourceManager.GetString("ActualizarPedido_422", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to El código es incorrecto..
        /// </summary>
        public static string ConfirmarRegistro_401 {
            get {
                return ResourceManager.GetString("ConfirmarRegistro_401", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No se encontró el miembro solicitado..
        /// </summary>
        public static string ConfirmarRegistro_404 {
            get {
                return ResourceManager.GetString("ConfirmarRegistro_404", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error en el servidor..
        /// </summary>
        public static string ErrorInterno {
            get {
                return ResourceManager.GetString("ErrorInterno", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Operación exitosa..
        /// </summary>
        public static string Exito {
            get {
                return ResourceManager.GetString("Exito", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Todos los campos son obligatorios..
        /// </summary>
        public static string IniciarSesion_400 {
            get {
                return ResourceManager.GetString("IniciarSesion_400", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No se encontró ninguna cuenta con ese email y/o contraseña..
        /// </summary>
        public static string IniciarSesion_401 {
            get {
                return ResourceManager.GetString("IniciarSesion_401", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No se encontraron pedidos en el rango especificado..
        /// </summary>
        public static string ObtenerPedidosEntre_404 {
            get {
                return ResourceManager.GetString("ObtenerPedidosEntre_404", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Ya existe una cuenta con este email..
        /// </summary>
        public static string RegistrarMiembro_422 {
            get {
                return ResourceManager.GetString("RegistrarMiembro_422", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to El pedido debe contener al menos un alimento, y todos los alimentos deben tener una cantidad mayor a 0..
        /// </summary>
        public static string RegistrarPedido_400 {
            get {
                return ResourceManager.GetString("RegistrarPedido_400", resourceCulture);
            }
        }
    }
}
