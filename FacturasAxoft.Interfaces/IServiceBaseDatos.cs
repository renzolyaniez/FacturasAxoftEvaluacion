using Microsoft.Data.SqlClient;

namespace FacturasAxoft.Interfaces
{
    public interface IServiceBaseDatos
    {
        void AbrirConexion();
        void IniciarTransaccion();
        decimal InsertarRegistroEnTabla(string query, SqlParameter[] parameters);
        int GetArticuloId(string codigoArticulo);
        int GetClienteId(string cuil);
        void ConfirmarTransaccion();
        void CancelarTransaccion();
        void CerrarConexion();

    }
}
