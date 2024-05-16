using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FacturasAxoft.Clases
{
    [XmlRoot("facturas")]
    public class Facturas
    {
        [XmlElement("factura")]
        public List<FacturaXml> Factura { get; set; }
    }
}
