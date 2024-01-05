using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KIT206Spring.Entities;

namespace KIT206Spring.View
{
    internal class PublicationView
    {
        public static void PrintAllPublication(Researcher Res)
        {
            Console.WriteLine("---\t---\t---\t We are in the Publicaion View \t---\t---\t---");
            foreach (Publication publication in Res.Pubs)
            {
                Console.WriteLine("Publicaiton Name: " + publication.Title);
            }
            Console.WriteLine("pause");
        }
    }
}
