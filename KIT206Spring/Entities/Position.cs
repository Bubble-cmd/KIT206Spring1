using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KIT206Spring.Entities
{
    public class Position
    {
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; } // The end date is nullable for ongoing positions
        public Level Level { get; set; }

        public Position(DateTime startDate, DateTime? endDate, string level)
        {
            StartDate = startDate;
            EndDate = endDate;
            Level = CalcPosLevel(level);
        }

        // !changed from lev to lvl
        public Level CalcPosLevel(string lvl)
        {
            if (lvl.Equals("A"))
            {
                return Level.A;
            }
            else if (lvl.Equals("B"))
            {
                return Level.B;
            }
            else if (lvl.Equals("C"))
            {
                return Level.C;
            }
            else if (lvl.Equals("D"))
            {
                return Level.D;
            }
            else
            {
                return Level.E;
            }

        }
    }
}
