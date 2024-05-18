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
            throw new NotImplementedException();
        }

        public List<Cliente> TraerTodosLosClientes()
        {
            throw new NotImplementedException();
        }

        public List<Factura> TraerTodasLasFacturas()
        {
            throw new NotImplementedException();
        }
    }
}
