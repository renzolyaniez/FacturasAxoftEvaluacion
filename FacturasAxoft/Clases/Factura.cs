namespace FacturasAxoft.Clases
{
    /// <summary>
    /// Clase que representa a una factura.
    /// Puede que sea necesario modificarla para hacer las implementaciones requeridas.
    /// </summary>
    public class Factura
    {
        public int Numero { get; set; }
        public DateTime Fecha { get; set; }
        public Cliente Cliente { get; set; }
        public List<RenglonFactura> Renglones { get; set; }
        public decimal TotalSinImpuestos { get; set; }
        public decimal PorcentajeIVA { get; set; }
        public decimal IVA { get; set; }
        public decimal TotalConImpuestos { get; set; }
    }

    /// <summary>
    /// Clase que representa el renglon de una factura.
    /// Puede que sea necesario modificarla para hacer las implementaciones requeridas.
    /// </summary>
    public class RenglonFactura
    {
        public Articulo Articulo { get; set; }
        public int cantidad { get; set; }
        public decimal SubTotal {  get; set; }
    }
}
