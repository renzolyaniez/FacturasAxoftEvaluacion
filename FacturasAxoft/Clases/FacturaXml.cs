using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FacturasAxoft.Clases
{
    public class FacturaXml
    {
        [XmlElement("numero")]
        public int Numero { get; set; }

        [XmlElement("fecha")]
        public string Fecha { get; set; }

        [XmlElement("cliente")]
        public ClienteXml Cliente { get; set; }

        [XmlArray("renglones")]
        [XmlArrayItem("renglon")]
        public List<Renglon> Renglones { get; set; }

        [XmlElement("totalSinImpuestos")]
        public decimal TotalSinImpuestos { get; set; }

        [XmlElement("iva")]
        public decimal Iva { get; set; }

        [XmlElement("importeIva")]
        public decimal ImporteIva { get; set; }

        [XmlElement("totalConImpuestos")]
        public decimal TotalConImpuestos { get; set; }
    }
    public class Renglon
    {
        [XmlElement("codigoArticulo")]
        public string CodigoArticulo { get; set; }

        [XmlElement("descripcion")]
        public string Descripcion { get; set; }

        [XmlElement("precioUnitario")]
        public decimal PrecioUnitario { get; set; }

        [XmlElement("cantidad")]
        public int Cantidad { get; set; }

        [XmlElement("total")]
        public decimal Total { get; set; }
    }
    public class ClienteXml
    {
        [XmlElement("CUIL")]
        public string Cuil { get; set; }

        [XmlElement("nombre")]
        public string Nombre { get; set; }

        [XmlElement("direccion")]
        public string Direccion { get; set; }
    }

}
