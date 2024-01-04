using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KIT206Spring.Spring_RAP.Entities
{
    public class Publication
    {
        public string DOI { get; set; }
        public string Title { get; set; }
        public string Authors { get; set; }
        public RankingType Ranking { get; set; }
        public PublicationType Type { get; set; }
        public string CiteAs { get; set; }
        public DateTime AvailabilityDate { get; set; }

        // Constructors
        public Publication(string doi, string title, string authors, string ranking, string type, string citeAs, DateTime availabilityDate)
        {
            //PublicationYear = publicationYear (fix)
            DOI = doi;
            Title = title;
            Authors = authors;
            Ranking = (RankingType)Enum.Parse(typeof(RankingType), ranking);
            typeCalc(type);
            CiteAs = citeAs;
            AvailabilityDate = availabilityDate;

        }

        public void typeCalc(string st)
        {
            if (st.Equals("Conference"))
            {
                Type = PublicationType.Conference;
            }
            else if (st.Equals("Journal"))
            {
                Type = PublicationType.Journal;
            }
            else
            {
                Type = PublicationType.Other;
            }
        }
    }

    public enum PublicationType
    {
        Conference,
        Journal,
        Other
    }

    public enum RankingType
    {
        Q1,
        Q2,
        Q3,
        Q4
    }
}
