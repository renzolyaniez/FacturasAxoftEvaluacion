using Microsoft.Data.SqlClient;
using System.Data.Common;
using System.Data;

namespace FacturasAxoft.Interfaces
{
    public interface IServiceBaseDatos
    {
        void AbrirConexion();
        void IniciarTransaccion();
        decimal InsertarRegistroEnTabla(string query, SqlParameter[] parameters);
        int TraerIdArticulo(string codigoArticulo);
        int TraerIdCliente(string cuil);
        void ConfirmarTransaccion();
        void CancelarTransaccion();
        void CerrarConexion();

        public DataTable EjecutarProcedimientoAlmacenado(string procedureName, SqlParameter[] parameters);

    }
}
