using Microsoft.Data.SqlClient;
using System.Data.Common;
using System.Data;
using FacturasAxoft.Models;

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
        List<Articulo> TraerTodosLosArticulos();
        List<Cliente> TraerTodosLosClientes();
        List<Factura> TraerTodasLasFacturas();
        public DataTable EjecutarProcedimientoAlmacenado(string procedureName, SqlParameter[] parameters);

    }
}
