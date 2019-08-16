using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Threading;

namespace ArquivoPrestador
{
    class Program
    {
        static void Main(string[] args)
        {
            Utilitarios utilitarios = new Utilitarios();
            utilitarios.Download();
            List<Arquivo> listaDeArquivos = utilitarios.LerArquivo();
            utilitarios.GravarArquivoNoBanco(listaDeArquivos);
            Console.WriteLine("Procedimento finalizado.");
            //Console.ReadKey();
            
            
            
        }
    }
}
