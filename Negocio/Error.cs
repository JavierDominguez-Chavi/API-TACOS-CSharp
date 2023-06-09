namespace TACOS.Negocio
{
    /// <summary>
    /// Deprecada: Clase para manejar errores.
    /// </summary>
    public class Error
    {
        /// <summary>
        /// Mensaje de error.
        /// </summary>
        public string? Mensaje { set; get; }

        /// <summary>
        /// Objeto que se intentó procesar.
        /// </summary>
        public object? Registro { set; get; }
    }
}
