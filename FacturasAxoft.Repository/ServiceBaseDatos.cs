using FacturasAxoft.Interfaces;
using FacturasAxoft.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace FacturasAxoft.Repository
{
    public class ServiceBaseDatos : IServiceBaseDatos
    {
        private readonly string _connectionString;
        private SqlConnection _connection;
        private SqlTransaction _transaction;

        public ServiceBaseDatos(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AbrirConexion()
        {
            _connection = new SqlConnection(_connectionString);
            _connection.Open();
        }

        public void IniciarTransaccion()
        {
            _transaction = _connection.BeginTransaction();
        }

        public decimal InsertarRegistroEnTabla(string query, SqlParameter[] parameters)
        {
            string sentencia = query + "SELECT SCOPE_IDENTITY();";

            using (SqlCommand command = new SqlCommand(sentencia, _connection, _transaction))
            {
                command.Parameters.AddRange(parameters);

                var resultado = command.ExecuteScalar();

                if (resultado != null)
                {
                    return (decimal)resultado;
                }

                return 0;
            }
        }

        public int TraerIdArticulo(string codigoArticulo)
        {
            var query = "select id from Articulos where Codigo = @codigo";

            SqlParameter[] parameters = { new SqlParameter("@codigo", SqlDbType.VarChar)
                                                  { Value = codigoArticulo } };


            using (SqlCommand command = new SqlCommand(query, _connection, _transaction))
            {
                command.Parameters.AddRange(parameters);

                var resultado = command.ExecuteScalar();

                if (resultado != null)
                {
                    return (int)resultado;
                }

                return 0;
            }
        }
        public int TraerIdCliente(string cuil)
        {
            var query = "select id from Clientes where Cuil = @cuil";

            SqlParameter[] parameters = { new SqlParameter("@cuil", SqlDbType.VarChar)
                                                  { Value = cuil } };


            using (SqlCommand command = new SqlCommand(query, _connection, _transaction))
            {
                command.Parameters.AddRange(parameters);
                var resulado = command.ExecuteScalar();

                if (resulado != null)
                {
                    return (int)resulado;
                }

                return 0;
            }
        }
        public void ConfirmarTransaccion()
        {
            _transaction.Commit();
        }

        public void CancelarTransaccion()
        {
            _transaction.Rollback();
        }

        public void CerrarConexion()
        {
            _connection.Close();
            _connection.Dispose();
        }

        public DataTable EjecutarProcedimientoAlmacenado(string procedureName, SqlParameter[]? parameters)
        {
            using (SqlCommand command = new SqlCommand(procedureName, _connection, _transaction))
            {
                command.CommandType = CommandType.StoredProcedure;
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    DataTable dataTable = new DataTable();
                    dataTable.Load(reader);
                    return dataTable;
                }
            }
        }

        public List<Articulo> TraerTodosLosArticulos()
        {

            var query = "select id, codigo, descripcion, precio from Articulos;";

            List<Articulo> articulos = new List<Articulo>();
            using (SqlCommand command = new SqlCommand(query, _connection, _transaction))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    DataTable dataTable = new DataTable();
                    dataTable.Load(reader);
                    foreach (DataRow row in dataTable.Rows)
                    {
                        Articulo articulo = new Articulo
                        {
                            Codigo = row["Codigo"].ToString(),
                            Descripcion = row["Descripcion"].ToString(),
                            Precio = Convert.ToDouble(row["Precio"])
                        };
                        articulos.Add(articulo);
                    }
                }

            }
            return articulos;
        }

        public List<Cliente> TraerTodosLosClientes()
        {

            var query = "select id, cuil, nombre, direccion, porcentajeIva from Clientes;";

            List<Cliente> clientes = new List<Cliente>();
            using (SqlCommand command = new SqlCommand(query, _connection, _transaction))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    DataTable dataTable = new DataTable();
                    dataTable.Load(reader);
                    foreach (DataRow row in dataTable.Rows)
                    {
                        Cliente cliente = new Cliente
                        {
                            Cuil = row["Cuil"].ToString(),
                            Nombre = row["Nombre"].ToString(),
                            Direccion = row["Direccion"].ToString(),
                            PorcentajeIVA = Convert.ToDecimal(row["PorcentajeIVA"])
                        };
                        clientes.Add(cliente);
                    }
                }

            }
            return clientes;
        }

        public List<Factura> TraerTodasLasFacturas()
        {
            var query = @"select fc.Numero, fc.Fecha, fc.ClienteId, fc.TotalIVa, 
                         fc.TotalConImpuestos, fc.TotalSinImpuestos, fc.PorcentajeIva,
	                    cl.Cuil, cl.PorcentajeIva ivacliente,cl.Nombre 
                        from facturas fc inner join clientes cl on fc.ClienteId=cl.Id;";


            List<Factura> facturas = new List<Factura>();
            using (SqlCommand command = new SqlCommand(query, _connection, _transaction))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    DataTable dataTable = new DataTable();
                    dataTable.Load(reader);
                    foreach (DataRow row in dataTable.Rows)
                    {
                        Cliente clienteFactura = new Cliente();

                        clienteFactura.Cuil = row["Cuil"].ToString();
                        clienteFactura.Nombre = row["Nombre"].ToString();

                        Factura factura = new Factura
                        {



                            Numero = Convert.ToInt32(row["Numero"]),
                            Fecha = Convert.ToDateTime(row["Fecha"]),
                            Cliente = clienteFactura,
                            PorcentajeIVA = Convert.ToDecimal(row["PorcentajeIVA"]),
                            TotalConImpuestos = Convert.ToDecimal(row["TotalConImpuestos"]),
                            TotalSinImpuestos = Convert.ToDecimal(row["TotalSinImpuestos"]),
                            IVA = Convert.ToDecimal(row["Totaliva"])
                        };

                        facturas.Add(factura);

                    }
                }

            }
            return facturas;
        }
    }
}
