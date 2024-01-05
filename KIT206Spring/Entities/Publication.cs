using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KIT206Spring.Entities
{
    public class Publication
    {
        //                        Publication pub = new Publication(title, doi, authors, cite_as, available, type, ranking);

        public string Title { get; set; }
        public string DOI { get; set; }

        public string Authors { get; set; }
        public string CiteAs { get; set; }
        public RankingType Ranking { get; set; }
        public DateTime AvailabilityDate { get; set; }
        public PublicationType Type { get; set; }


        // Constructor
        public Publication(string title, string doi, string authors, string citeAs, DateTime availabilityDate, string type, string ranking)
        {
            //PublicationYear = publicationYear;
            Title = title;
            DOI = doi;
            Authors = authors;
            CiteAs = citeAs;
            AvailabilityDate = availabilityDate;
            //PageCount = pageCount;
            typeCalc(type);
            Ranking = (RankingType)Enum.Parse(typeof(RankingType), ranking);


        }

        public void typeCalc(string st)
        {
            if (st.Equals("Conference"))
            {
                Type = PublicationType.Conference;
            }
            else
            {
                Type = PublicationType.Journal;
            }

        }

    }


    public enum PublicationType
    {
        Conference,
        Journal
    }

    public enum RankingType
    {
        Q1,
        Q2,
        Q3,
        Q4
    }

}
