using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KIT206Spring.Database;
using KIT206Spring.Entities;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement.Header;

namespace KIT206Spring.Controllers
{
    internal class PublicationsControl
    {
        //The function inverses the order of the elements in the PubL and returns in inversed order
        public static List<Publication> invert_sort(List<Publication> PubL)
        {
            PubL.Reverse();
            return PubL;
        }

        // The OrderByDescending method sorts the PubL in descending order based on the Availability Date
        // ThenBy method is then used to sort the items with the same year by their Title in ascending order
        // The sorted list is then converted to a List and assigned to the orderedList variable.
        public static List<Publication> sort_list(List<Publication> PubL)
        {
            List<Publication> orderedList = PubL.OrderByDescending(item => item.AvailabilityDate.Year)
                              .ThenBy(item => item.Title, StringComparer.OrdinalIgnoreCase).ToList();
            return orderedList;
        }
        // This function fetches and sorts a Researcher's Publications. If the Researcher has any Publications, it returns null.
        public static List<Publication> FetchPublications(Researcher rs)
        {
            if (rs.Pubs.Count > 0) //ideally you would actually want to select the first publication by the Researcher if count more than 0 but for now it is null(empty) on display
            {
                return null;
            }
            else
            {
                List<Publication> pubs = new List<Publication>();
                pubs = DBAdapter.GetPubs(rs);
                pubs = sort_list(pubs);
                return pubs;
            }
        }
        // This function filters the list of Publications to include only those available between two input years.
        public static List<Publication> FilterByYear(int year1, int year2, List<Publication> PubL)
        {
            int firstYear = Math.Min(year1, year2);
            int secondYear = Math.Max(year1, year2);
            List<Publication> filteredPubs = new List<Publication>();
            filteredPubs = PubL.Where(p =>
                p.AvailabilityDate.Year >= firstYear &&
                p.AvailabilityDate.Year <= secondYear).ToList();
            return filteredPubs;
        }
    }
}