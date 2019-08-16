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
        //private static string _connectionString = @"Data Source=PROG02SSD\ESTUDOS;Initial Catalog=Banco_Teste_Arquivo_Prestador;Integrated Security=True";
        private static string _connectionString = @"Data Source=200.170.88.138;Initial Catalog=assecont2;Persist Security Info=True;User ID=assecont5272;Password=*h6prMvr;Connect Timeout=30";

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
