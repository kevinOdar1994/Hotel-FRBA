using System;
using System.Data;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;

namespace FrbaHotel {
    class DBConnection {

        string server = ConfigurationManager.AppSettings["server"].ToString();
        string user = ConfigurationManager.AppSettings["user"].ToString();
        string password = ConfigurationManager.AppSettings["password"].ToString();

        private static readonly DBConnection instance = new DBConnection();
        private SqlConnection connection;
        private Usuario usuario;
        private Cliente cliente;

        public DBConnection() { }

        public static DBConnection getInstance() { return instance; }

        public Usuario getUsuario() { return usuario; }
        public void setUsuario(Usuario usuario) { this.usuario = usuario; }
        public void setCliente(Cliente cliente) { this.cliente = cliente; }
        public Cliente getCliente() { return cliente; }

        public SqlConnection getConnection() {
            if (connection == null ) {
                connection = new SqlConnection();
                connection.ConnectionString = "SERVER=" + server + "\\SQLSERVER2012;DATABASE=GD1C2018;UID=" + user + ";PASSWORD=" + password + ";MultipleActiveResultSets=true;";
                connection.Open();
            }
            return connection;
        }

        public SqlDataReader executeQuery(String query) {
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataReader dataReader = command.ExecuteReader();
            command.Dispose();
            return dataReader;
        }

        public void executeProcedure(String procName, string[] parameters, object[] args) {
            SqlCommand command = new SqlCommand(procName, this.connection);
            command.CommandType = CommandType.StoredProcedure;

            if(args.Length != 0 && args.Length == parameters.Length) {
                for (int i = 0; i < args.Length ; i++) 
                    command.Parameters.AddWithValue(parameters[i], args[i]);
            }

            command.ExecuteNonQuery();
            command.Dispose();
        }

        public bool conexionCorrecta() {
            try {
                this.getConnection();
                return true;
            }
            catch { return false; }
        }

    }
}
