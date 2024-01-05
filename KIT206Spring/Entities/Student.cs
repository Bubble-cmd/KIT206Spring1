using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KIT206Spring.Entities
{
    public class Student : Researcher
    {
        public int Supervisor { get; set; }   // This is also staff
        public string Degree { get; set; }
        public string SupervisorName { get; set; }

        // campHouse to campus, curretn_start to current_start, lev to lvl, photURL to photoURL
        public Student(int id, string type, string firstName, string lastName, string title, string schoolUnit, string campus, string email, string photoURL, int supervisorID, string degree,
            DateTime utas_start, DateTime current_start, String lvl)
            : base(id, type, firstName, lastName, title, schoolUnit, campus, email, photoURL, utas_start, current_start, lvl)
        {
            Supervisor = supervisorID;
            Degree = degree;
        }
    }
}
