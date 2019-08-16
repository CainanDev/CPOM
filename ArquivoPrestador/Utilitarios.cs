using System;
using System.ComponentModel;
using System.Net;
using System.IO;
using System.Collections.Generic;
using ArquivoDAL;
using System.Data.SqlClient;
using System.Data;

namespace ArquivoPrestador
{
    public class Utilitarios
    {
        public List<Arquivo> listaDeArquivos = new List<Arquivo>();
        private string _url = "https://cpom.prefeitura.sp.gov.br/prestador/SituacaoCadastral/LayoutTransmissaoArquivo?arquivo=TXT";
        private string nomeDoArquivo = MontarNomeArquivo();
        public void Download()
        {
            var wc = new WebClient();
            Uri link = new Uri(_url);
            //wc.DownloadFileAsync(link, nomeDoArquivo);
            Console.WriteLine("Download....");
            wc.DownloadFile(link, nomeDoArquivo);
            Console.WriteLine("Download concluido");
        }
        public static string MontarNomeArquivo()
        {
            // O nome do arquivo segue o seguinte formato "ArquivoPrestadorYYYYmmdd.txt" 
            var data = DateTime.Now.ToString("yyyyMMdd");
            return "ArquivoPrestador" + data + ".txt";
        }

        public List<Arquivo> LerArquivo()
        {
            //implementar logica de leitura do arquivo usando threads
            StreamReader leitor = new StreamReader(nomeDoArquivo); //caminho relativo na pasta bin do projeto
            //File.Exists(nomeDoArquivo);
            
            while (!leitor.EndOfStream)
            {
                Arquivo arquivo = new Arquivo();

                var teste = leitor.ReadLine().Split('|');
                if (teste.Length > 1) //Verificando se a linha não está em branco 
                {
                    var codigoAtividade = teste[2].Split('-');
                    arquivo.Cnpj = teste[0].ToString();
                    arquivo.CodigoDaAtividade = codigoAtividade[0].ToString().Trim();
                }
                else continue; //linha em branco

                listaDeArquivos.Add(arquivo);
            }
            leitor.Close();
            leitor.Dispose();
            return listaDeArquivos;
        }

        public void GravarArquivoNoBanco(List<Arquivo> ListaArquivos)
        {
            LimparRegistros();
            var conn = new SqlConnection(DbHelper.connectionString);
            conn.Open();
            var count = 1;
            //var command = new SqlCommand("GRAVAR_ARQUIVO", conn);
            var command = new SqlCommand("importacao_cpom_salvar", conn);
            command.CommandType = CommandType.StoredProcedure;
            foreach (var arquivo in ListaArquivos)
            {
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@P_CNPJ", arquivo.Cnpj);
                command.Parameters.AddWithValue("@P_CODIGOATIVIDADE", arquivo.CodigoDaAtividade);
                command.ExecuteNonQuery();
                Console.WriteLine("gravei o registro " + count);
                count++;
            }
            conn.Close();
            if (File.Exists(nomeDoArquivo))
            {
                File.Delete(nomeDoArquivo);
            }
        }

        public void LimparRegistros()
        {
            DbHelper.ExecuteNonQuery("importacao_cpom_excluir");
        }
    }
}
