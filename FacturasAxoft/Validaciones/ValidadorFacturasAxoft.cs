using FacturasAxoft.Clases;
using FacturasAxoft.Excepciones;
using FacturasAxoft.Models;

namespace FacturasAxoft.Validaciones
{
    /// <summary>
    /// En esta clase implementarán todas las validaciones.
    /// Se da una validación ya implementada a modo de ejemplo.
    /// </summary>
    public class ValidadorFacturasAxoft
    {
        private readonly List<Cliente> clientes;
        private readonly List<Articulo> articulos;
        private readonly List<Factura> facturas;

        /// <summary>
        /// Instancia un Validador facturas
        /// </summary>
        /// <param name="clientes">Clientes preexistentes, ya grabados en la base de datos</param>
        /// <param name="articulos">Artículos preexistentes, ya grabados en la base de datos</param>
        /// <param name="facturas">Facturas preexistentes, ya grabadas en la base de datos</param></param>
        public ValidadorFacturasAxoft(List<Cliente> clientes, List<Articulo> articulos, List<Factura> facturas)
        {
            this.clientes = clientes;
            this.articulos = articulos;
            this.facturas = facturas;
        }

        /// <summary>
        /// Valida la factura pasada por parámetro según lo lógica de negocios requerida.
        /// </summary>
        /// <param name="factura">Factura a validar</param>
        /// <exception>En caso de que la factura no cumpla con las reglas de negocio requeridas
        /// debe lanzar una excepción con el mensaje de error correspondiente</exception>/// 
        public void ValidarNuevaFactura(Factura factura)
        {
            if (factura.Numero < 1)
            {
                throw new FacturaConNumeracionInvalida();
            }

            if (factura.Numero > 1)
            {
                var contador = facturas.Select(e => e.Numero == factura.Numero - 1).Count();

                if (contador == 0)
                {
                    throw new FacturaSinCorrelatividad();
                }
            }

            if (factura.Numero > 1)
            {
                Factura facturaAnterior = facturas.Where(e => e.Numero == factura.Numero - 1).FirstOrDefault();


                if (facturaAnterior.Fecha > factura.Fecha)
                {
                    throw new FacturaConFechaInvalidaException();
                }
            }

            if (factura.Cliente.Cuil.Length != 11) //aca se puede implementar el algoritmo de validacion de digito vereficador
            {
                throw new CuilInvalido();
            }



            var controlClientes = clientes.Where(e => e.Cuil == factura.Cliente.Cuil &&
                e.Nombre == factura.Cliente.Nombre &&
                e.Direccion == factura.Cliente.Direccion && e.PorcentajeIVA == factura.PorcentajeIVA).Count();

            if (controlClientes == 0)
            {
                throw new DatosDelClienteInvalidos();
            }

            var listaRenglones = factura.Renglones;
            
            // variable para almacenar el total sin impuesto de los renglones
            decimal subtotalSinImpuestosRenglones = 0;

            foreach (var articuloRenglon in listaRenglones)
            {
                var controlArticulos = articulos.Where(e => e.Codigo == articuloRenglon.Articulo.Codigo &&
                                                       e.Descripcion==articuloRenglon.Articulo.Descripcion && 
                                                       e.Precio==articuloRenglon.Articulo.Precio).Count();
 
                if (controlArticulos == 0)
                {
                    throw new DatosDelArticuloInvalidos();
                }

                if ( articuloRenglon.SubTotal != Convert.ToDecimal(articuloRenglon.cantidad)  * 
                                                  Convert.ToDecimal(articuloRenglon.Articulo.Precio))
                {

                    throw new TotalDeRenglonesIncorrecto();
                }

                subtotalSinImpuestosRenglones += articuloRenglon.SubTotal;

            }

            if (subtotalSinImpuestosRenglones != factura.TotalSinImpuestos )
            {
                throw new TotalSinImpuestosIncorrecto();
            }

            if (factura.PorcentajeIVA != 0 && factura.PorcentajeIVA != Convert.ToDecimal( 10.5 ) &&
                 factura.PorcentajeIVA != 21 && factura.PorcentajeIVA != 27)
            {
                throw new PorcentajeIvaIncorrecto();
            }

            if ((factura.TotalConImpuestos - factura.TotalSinImpuestos) != factura.IVA) {

                throw new ImporteIvaIncorrecto();
            }

            if ((factura.TotalSinImpuestos + factura.IVA) != factura.TotalConImpuestos)
            {
                throw new TotalConImpuestosIncorrecto();
            }
        }
    }
}
