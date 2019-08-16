using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace ArquivoDAL
{
    public static class DbHelper
    {
        
        private static string _connectionString = "Aqui vai a string de conexão com o seu banco";

        public static string connectionString
        {
            get
            {
                return _connectionString;
            }
            set
            {

            }
            
        }
        

        public static void ExecuteNonQuery(string storedProcedure, params object[] parametros)
        {
           using (var conn = new SqlConnection(connectionString))
           {
               using (var command = new SqlCommand(storedProcedure, conn))
               {
                   command.CommandType = CommandType.StoredProcedure;
                   if (parametros.Length > 0)
                   {
                       for (int i = 0; i < parametros.Length; i += 2)
                       {
                           command.Parameters.AddWithValue(parametros[i].ToString(), parametros[i + 1]);
                       }
                   }
                   conn.Open();
                   command.ExecuteNonQuery();
               }
               conn.Close();
           }
        }
        
    }
}
