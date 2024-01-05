using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KIT206Spring.Spring_RAP.Entities
using KIT206Spring.Spring_RAP.Controllers

namespace KIT206Spring.Spring_RAP.View
{
    internal class PerformaceDetailsView
    {
        public static void PrintPerformanceView(Researcher Res)
        {
            Console.WriteLine("\t#####\t#####\t WELCOME TO PERFORMANCE WIEW \t########\t#######");
            Console.WriteLine("Performance for :" + Res.LastName);
            Console.WriteLine("Q1 performance is ... un-available at the moment" + "%");
            Console.WriteLine("The three year average is... " + Researcher.AveragePublicationsPerYear(Res));
            Console.WriteLine("The performance by Publication is " + Researcher.CalculatePerformanceByPublication(Res));
            Console.WriteLine("Pause");
        }
    }
}
