using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KIT206Spring.Entities;

namespace KIT206Spring.Entities
{
    public class Staff : Researcher
    {

        public int FundingRecieved { get; set; }
        public string PerformanceByFunding { get; set; }
        public double ThreeYearAverage { get; set; }
        public string PerformanceByPublication { get; set; }
        public List<Position> Positions { get; set; }
        public int SuperCount { get; set; }
        // want to make this an enum
        public string level { get; set; }
        public string StudentsSupervised { get; set; }


        // Constructor
        public Staff(int id, string type, string firstName, string lastName, string title, string schoolUnit, string campus, string email, string photoURL, string lvl, DateTime utas_start, DateTime current_status)
            : base(id, type, firstName, lastName, title, schoolUnit, campus, email, photoURL, utas_start, current_status, lvl)
        {
            level = lvl;
            Positions = new List<Position>();
        }



        public static void CalcStafPerformance(Staff Sta)
        {

        }


        // calculate performance by funding
        public double PerforByFund(double fundingRecieved)
        {
            //Performance by Funding Received is the average amount of funding per year since commencement.

            double performaceFunding = 100;
            return performaceFunding;

        }

        public static void AverageThreeYear(Staff staff)
        {
            //3-year Average is the total number of publications in the previous three whole calendar years, divided by three.
            double count = 0;
            DateTime threeYearsAgo = DateTime.Now.AddYears(-3);
            foreach (Publication publication in staff.Pubs)
            {
                if (publication.AvailabilityDate >= threeYearsAgo)
                {
                    count++;
                }
            }
            staff.ThreeYearAverage = Math.Round(count > 0 ? count / 3 : 0, 1);
        }

        public static void PerfByPub(Staff staff)
        {
            staff.PerformanceByPublication = String.Format("{0:0.0}", Math.Round(staff.ThreeYearAverage / staff.ExpectedNoPubs * 100, 1) + "%");
        }

        public static void PerfByFund(Staff staff)
        {
            staff.PerformanceByFunding = "$" + (Math.Round(staff.FundingRecieved / ((DateTime.Now - staff.CommencedWithInstitution).TotalDays / 365), 1)).ToString();
        }
        public enum ExpectedPublications
        {
            A,
            B,
            C,
            D,
            E
        }


    }
}
