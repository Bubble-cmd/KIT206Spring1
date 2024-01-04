using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KIT206Spring.Spring_RAP.Entities
{
    public class Researcher
    {
        public int ID { get; set; }
        public ResearcherType Type { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public string SchoolUnit { get; set; }
        public Campus Camp { get; set; }
        public string Email { get; set; }
        public string PhotoURL { get; set; }
        public string CurrentJobTitle { get; private set; }
        public DateTime CommenceCurrentPosition { get; private set; }
        public DateTime CommencedWithInstitution { get; private set; }
        public Level PositionLevel { get; set; }
        public string Q1Percentage { get; private set; }
        public List<Publication> Pubs { get; set }
        public JobTitle Job_Title { get; set; }
        public double ExpectedNoPubs { get; set; }
        public double Tenure { get; private set } // Length of time in fractional years since the researcher's commencement in the institution

        // Constructors

        // iD to id, campHouse to campus, photURL to photoURL, curretn_start to current_status, lev to lvl
        public Researcher(int id, string type, string firstName, string lastName, String title, string schoolUnit, string campus, string email, string photoURL, DateTime utas_start, DateTime current_start,
            string lvl)
        {
            ID = id;
            Type = (ResearcherType)Enum.Parse(typeof(ResearcherType), type);
            FirstName = firstName;
            LastName = lastName;
            Title = title;
            SchoolUnit = schoolUnit;
            Email = email;
            PhotoURL = photoURL;
            Pubs = new List<Publication>();
            //Camp = (Campus)Enum.Parse(typeof(Campus), type); (fix)
            CommencedWithInstitution = utas_start;
            CommenceCurrentPosition = current_start;
            PositionLevel = (Level)Enum.Parse(typeof(Level), lvl);

            Console.WriteLine(firstName + "..." + PositionLevel);
            DeriveJobTitle(PositionLevel);
            CalcTenure(this, CommencedWithInstitution);

        }

        public void DeriveJobTitle(Level level)
        {
            switch (level)
            {
                case Level.A:
                    Job_Title = JobTitle.ResearchAssociate;
                    ExpectedNoPubs = 0.5;
                    break;
                case Level.B:
                    Job_Title = JobTitle.Lecturer;
                    ExpectedNoPubs = 1;
                    break;
                case Level.C:
                    Job_Title = JobTitle.AssistantProfessor;
                    ExpectedNoPubs = 2;
                    break;
                case Level.D:
                    Job_Title = CurrentJobTitle.AssociateProfessor;
                    ExpectedNoPubs = 3.2;
                    break;
                case Level.E:
                    Job_Title = JobTitle.Professor;
                    ExpectedNoPubs = 4;
                    break;
                case Level.Student:
                    Job_Title = JobTitle.Student;
                    ExpectedNoPubs = 0;
                    break;

                default:
                    throw new ArgumentException("Level invalid"); // for invalid position
            }
        }

        // add CalcPositionInfo, CalcEarliestPos, CalcTenure, CalcCommencedCurrentPosition, CalcPerformanceByPublication, AveragePublicationsPerYear, Q1PercentageCalc
        // add enum ResearcherType, Campus, JobTitle
    }
}
