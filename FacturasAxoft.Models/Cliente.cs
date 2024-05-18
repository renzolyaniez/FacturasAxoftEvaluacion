namespace FacturasAxoft.Models
{
    /// <summary>
    /// Clase que representa a un cliente.
    /// Puede que sea necesario modificarla para hacer las implementaciones requeridas.
    /// </summary>
    public class Cliente
    {
        public string Cuil { get; set; }
        public string Nombre { get; set; }
        public string Direccion {  get; set; }
        public decimal PorcentajeIVA {  get; set; }
    }
}
