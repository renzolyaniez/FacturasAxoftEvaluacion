using FacturasAxoft.Clases;
using FacturasAxoft.Interfaces;
using FacturasAxoft.Repository;
using Microsoft.Data.SqlClient;
using System.Xml.Serialization;
using System.Data;
using FacturasAxoft.Validaciones;

namespace FacturasAxoft
{
    public class FacturasAxoft
    {
        private readonly string connectionString;

        /// <summary>
        /// Instancia un FacturasAxoft que usaremos como fachada de la aplicación.
        /// </summary>
        /// <param name="conectionString">Datos necesarios para conectarse a la base de datos</param>
        /// <exception>Debe tirar una excepción con mensaje de error correspondiente en caso de no poder conectar a la base de datos</exception>
        public FacturasAxoft(string connectionString)
        {
            this.connectionString = connectionString;
        }

        /// <summary>
        /// Lee las facturas desde el archivo XML y las graba en la base de datos.
        /// Da de alta los clientes o los artículos que lleguen en el xml y no estén en la base de datos.
        /// </summary>
        /// <param name="path">Ubicación del archivo xml que contiene las facturas</param>
        /// <exception>Si no se puede acceder al archivo, no es un xml válido, o no cumple con las reglas de negocio, 
        /// devuelve una excepción con el mensaje de error correspondiente</exception>/// 
        public void CargarFacturas(string path)
        {
            // Completar acá con el código necesario para cargar las facturas desde el xml.
            // Al momento de hacer las validaciones, utilizar la clase ValidadorFacturasAxoft.

            XmlSerializer serializer = new XmlSerializer(typeof(Facturas));

            IServiceBaseDatos serviceBaseDatos = new ServiceBaseDatos(connectionString);

            serviceBaseDatos.AbrirConexion();

            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                Facturas facturas = (Facturas)serializer.Deserialize(fs);

                // Ahora puedes trabajar con el objeto `facturas`

                

                serviceBaseDatos.IniciarTransaccion();

                try
                {
                    foreach (var factura in facturas.Factura)
                    {
                        //ValidadorFacturasAxoft validador = new ValidadorFacturasAxoft();

                        //- Primero insertamos la cabecera de la factura
                        string sentencia = @"insert into Facturas (Numero, Fecha, Clienteid, 
                                        TotalSinImpuestos,
                                      PorcentajeIVA, TotalIVA,TotalConImpuestos) 
                                      values (@numero, @fecha, @ClienteId, @TotalSinImpuestos, 
                                      @PorcentajeIVA, @TotalIVA,@TotalConImpuestos)";
                        SqlParameter[] parametrosFactura =
                        {
                            new SqlParameter("@numero", SqlDbType.Int) { Value = factura.Numero },
                            new SqlParameter("@fecha", SqlDbType.Date) { Value = factura.Fecha },
                            new SqlParameter("@ClienteId", SqlDbType.Int) { Value = serviceBaseDatos.TraerIdCliente(factura.Cliente.Cuil) },
                            new SqlParameter("@TotalSinImpuestos", SqlDbType.Decimal) { Value = factura.TotalSinImpuestos },
                            new SqlParameter("@PorcentajeIVA", SqlDbType.Decimal) { Value = factura.Iva },
                            new SqlParameter("@TotalIVA", SqlDbType.Decimal) { Value = factura.ImporteIva },
                            new SqlParameter("@TotalConImpuestos", SqlDbType.Decimal) { Value = factura.TotalConImpuestos }
                        };

                        var numeroFacturaInsertada = serviceBaseDatos.InsertarRegistroEnTabla(sentencia, parametrosFactura);


                        foreach (var renglon in factura.Renglones)
                        {
                            //@facturaId, @articuloId, @cantidad, @subTotal
                            sentencia = @"insert into FacturaRenglones (FacturaId, ArticuloId, Cantidad, 
                                        Subtotal) 
                                      values (@facturaId, @articuloId, @cantidad, @subTotal)";
                            SqlParameter[] parametrosRenglones =
                            {
                            new SqlParameter("@facturaId", SqlDbType.Int) { Value = numeroFacturaInsertada },
                            new SqlParameter("@articuloId", SqlDbType.Int) { Value = serviceBaseDatos.TraerIdArticulo(renglon.CodigoArticulo) },
                            new SqlParameter("@cantidad", SqlDbType.Int) { Value = renglon.Cantidad },
                            new SqlParameter("@subTotal", SqlDbType.Decimal) { Value = renglon.Total }

                            };

                            var renglonInsertado = serviceBaseDatos.InsertarRegistroEnTabla(sentencia, parametrosRenglones);

                        }

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());

                    serviceBaseDatos.CancelarTransaccion();
                }

                serviceBaseDatos.ConfirmarTransaccion();
                serviceBaseDatos.CerrarConexion();

                Console.WriteLine("Facturas Procesadas con exito");
            }


        }

        /// <summary>
        /// Obtiene los 3 artículos mas vendidos
        /// </summary>
        /// <returns>JSON con los 3 artículos mas vendidos</returns>
        /// <exception>Nunca devuelve excepción, en caso de no existir 3 artículos vendidos devolver los que existan, en caso de
        /// tener artículos con la misma cantidad de ventas devolver cualquiera</exception>
        public string Get3ArticulosMasVendidos()
        {
            try
            {
                IServiceBaseDatos serviceBaseDatos = new ServiceBaseDatos(connectionString);

                serviceBaseDatos.AbrirConexion();

                string procedureName = "SP_Traer3ArticulosMasVendidos";

                DataTable resultTable = serviceBaseDatos.EjecutarProcedimientoAlmacenado(procedureName, null);

                foreach (DataRow row in resultTable.Rows)
                {
                    Console.WriteLine($"Codigo: {row["Codigo"]}, Descripcion: {row["Descripcion"]},  Total Vendido: {row["Total_Vendido"]}");
                }

                serviceBaseDatos.CerrarConexion();

                return "Ejecucion exitosa";
            }
            catch (Exception ex)
            {

                throw;
            }

            return "Errores";
           
        }

        /// <summary>
        /// Obtiene los 3 clientes que mas compraron
        /// </summary>
        /// <returns>JSON con los 3 clientes que mas compraron</returns>
        /// <exception>Mismo criterio que para artículos</exception>
        public string Get3Compradores()
        {
            try
            {
                IServiceBaseDatos serviceBaseDatos = new ServiceBaseDatos(connectionString);

                serviceBaseDatos.AbrirConexion();

                string procedureName = "SP_Traer3ClientesQueMasCompraron";

                DataTable resultTable = serviceBaseDatos.EjecutarProcedimientoAlmacenado(procedureName, null);

                foreach (DataRow row in resultTable.Rows)
                {
                    Console.WriteLine($"Cuil: {row["Cuil"]}, Nombre: {row["Nombre"]},  Total Comprado: {row["Total_Comprado"]}");

                }

                serviceBaseDatos.CerrarConexion();

                return "Ejecucion exitosa";
            }
            catch (Exception ex)
            {

                throw;
            }

            return "Errores";
        }

        /// <summary>
        /// Devuelve el promedio de facturas y el artículo que mas compro.
        /// </summary>
        /// <param name="cuil"></param>
        /// <returns>JSON con los datos requeridos</returns>
        /// <exception>Si no existe el cliente, o si no tiene compras devolver un mensaje de error con el mensaje correspondiente</exception>
        public string GetPromedioYArticuloMasCompradoDeCliente(string cuil)
        {
            try
            {
                IServiceBaseDatos serviceBaseDatos = new ServiceBaseDatos(connectionString);

                serviceBaseDatos.AbrirConexion();

                string procedureName = "sp_PromedioComprasporClienteYarticuloMasComprado";
                SqlParameter[] parametrosCuil =
                           {
                            new SqlParameter("@cuil", SqlDbType.VarChar) { Value = cuil } };

                DataTable resultTable = serviceBaseDatos.EjecutarProcedimientoAlmacenado(procedureName, parametrosCuil);

                foreach (DataRow row in resultTable.Rows)
                {
                    Console.WriteLine($"Nombre Cliente: {row["ClienteNombre"]}, Promedio Compras: {row["PromedioCompras"]},  Articulo mas Comprado: {row["ArticuloMasComprado"]}");

                }

                serviceBaseDatos.CerrarConexion();

                return "Ejecucion exitosa";
            }
            catch (Exception ex)
            {

                throw;
            }

            return "Errores";
        }

        /// <summary>
        /// Devuelve el total y promedio facturado para una fecha.
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns>JSON con los datos requeridos</returns>
        /// <exception>Si el dato de fecha ingresado no es válido, o si no existen facturas para la fecha dada,
        /// mostrar el mensaje de error correspondiente</exception>
        public string GetTotalYPromedioFacturadoPorFecha(string fecha)
        {
            try
            {
                IServiceBaseDatos serviceBaseDatos = new ServiceBaseDatos(connectionString);

                serviceBaseDatos.AbrirConexion();

                string procedureName = "sp_TotalFacturadoyPromedioImporteFacturasPorFecha";
                SqlParameter[] parametrosFecha =
                           {
                            new SqlParameter("@fecha", SqlDbType.Date) { Value = fecha } };

                DataTable resultTable = serviceBaseDatos.EjecutarProcedimientoAlmacenado(procedureName, parametrosFecha);

                foreach (DataRow row in resultTable.Rows)
                {
                    Console.WriteLine($"Total Facturado: {row["TotalFacturado"]}, Promedio Importes Factura: {row["PromedioImportesFactura"]} ");

                }

                serviceBaseDatos.CerrarConexion();

                return "Ejecucion exitosa";
            }
            catch (Exception ex)
            {

                throw;
            }

            return "Errores";
        }

        /// <summary>
        /// Devuelve los 3 clientes que mas compraron el artículo
        /// </summary>
        /// <param name="codigoArticulo"></param>
        /// <returns>JSON con los datos pedidos</returns>
        /// <exception>Si el artículo no existe, o no fue comprado por al menos 3 clientes devolver un mensaje de error correspondiente</exception>
        public string GetTop3ClientesDeArticulo(string codigoArticulo)
        {
            try
            {
                IServiceBaseDatos serviceBaseDatos = new ServiceBaseDatos(connectionString);

                serviceBaseDatos.AbrirConexion();

                string procedureName = "sp_Top3ClientesMasCompradoresArticulo";
                SqlParameter[] parametrosArticulo =
                           {
                            new SqlParameter("@codigo", SqlDbType.VarChar) { Value = codigoArticulo } };

                DataTable resultTable = serviceBaseDatos.EjecutarProcedimientoAlmacenado(procedureName, parametrosArticulo);

                foreach (DataRow row in resultTable.Rows)
                {
                    Console.WriteLine($"Nombre Cliente: {row["NombreCliente"]}, Cantidad Comprada: {row["CantidadComprada"]},  Descripcion Articulo: {row["DescripcionArticulo"]}");

                }

                serviceBaseDatos.CerrarConexion();

                return "Ejecucion exitosa";
            }
            catch (Exception ex)
            {

                throw;
            }

            return "Errores";
        }

        /// <summary>
        /// Devuelve el total de IVA de las facturas generadas desde la fechaDesde hasta la fechaHasta, ambas inclusive.
        /// </summary>
        /// <returns>JSON con el dato requerido</returns>
        /// <exception>Si no existen facturas para las fechas ingresadas mostrar un mensaje de error</exception>
        public string GetTotalIva(string fechaDesde, string fechaHasta)
        {
            try
            {
                IServiceBaseDatos serviceBaseDatos = new ServiceBaseDatos(connectionString);

                serviceBaseDatos.AbrirConexion();

                string procedureName = "sp_TotalIvaPorFechas";
                SqlParameter[] parametrosTotalIva =
                           {
                            new SqlParameter("@fechaDesde", SqlDbType.Date) { Value = fechaDesde } ,
                            new SqlParameter("@fechaHasta", SqlDbType.Date) { Value = fechaHasta }};

                DataTable resultTable = serviceBaseDatos.EjecutarProcedimientoAlmacenado(procedureName, parametrosTotalIva);

                foreach (DataRow row in resultTable.Rows)
                {
                    Console.WriteLine($"Total de Iva: {row["TotalIva"]} ");

                }

                serviceBaseDatos.CerrarConexion();

                return "Ejecucion exitosa";
            }
            catch (Exception ex)
            {

                throw;
            }

            return "Errores";
        }
    }
}
