using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Collections;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using System.Runtime.Versioning;
using System.CodeDom.Compiler;
using System.Windows.Navigation;
using static System.Windows.Forms.AxHost;
using KIT206Spring.Spring_RAP.DataBase
using KIT206Spring.Spring_RAP.DataSource
using KIT206Spring.Spring_RAP.Entities
using KIT206Spring.Spring_RAP.View
using KIT206Spring.Spring_RAP

namespace KIT206Spring.Spring_RAP.Controllers
{
    internal class ResearcherControl
    {
        public static List<Researcher> Researchers { get; private set; }
        public static Researcher CurrentResearcher { get; private set; }
        public static List<Researcher> FetchResearchers()
        {
            List<Researcher> ResearcherList = DBAdapter.GetResearcher();
            return ResearcherList;
        }
        public static void DisplayResearchers()
        {
            List<Researcher> ResearcherList = DBAdapter.GetResearcher();
            List<Student> StuList = ResearcherList.OfType<Student>().ToList();
            ResearcherView.PrintAllResearchers(ResearcherList);
        }

        public static void DisplayResearcherDetails(Researcher rs, List<Researcher> ResearcherList)
        {
            Console.WriteLine("in display Researcher Details");
            if (rs is Staff staff)
            {
                DBAdapter.GetPositions(staff);

                findSupervisions(ResearcherList, staff);
            }
            if (rs is Student stu)
            {
                Console.WriteLine("rs is student");
            }
            ResearcherDetailsView.DisplayResearcherDetails(rs);
        }

        public static void DisplayPerformanceDetails(Researcher rs)
        {

            DBAdapter.GetPubs(rs);


            PerformaceDetailsView.PrintPerformanceView(rs);
        }


        public static List<Researcher> FilterLevel(string lvl, ObservableCollection<Researcher> rs)
        {
            if (lvl == "All levels")
            {
                return rs.OrderBy(x => x.LastName).ToList();
            }
            else
            {
                Level level = (Level)Enum.Parse(typeof(Level), lvl);
                var filteredResearchers =
                    from entry in rs
                    where entry.PositionLevel == level
                    select entry;

                return filteredResearchers.OrderBy(x => x.LastName).ToList();
            }
        }


        public static List<Researcher> FilterList(ObservableCollection<Researcher> ResearcherList, string searchText)
        {
            List<Researcher> filteredList = new List<Researcher>();
            string tempFirstName;
            string tempLastName;
            searchText = searchText.ToLower();

            foreach (Researcher researcher in ResearcherList)
            {
                tempFirstName = researcher.FirstName.ToLower();
                tempLastName = researcher.LastName.ToLower();

                if (tempFirstName.Contains(searchText) || tempLastName.Contains(searchText))
                {
                    filteredList.Add(researcher);
                }
            }
            return filteredList;
        }

        public static void DetailsController(Researcher researcher)
        {
            Researcher.Q1PercentageCalc(researcher);

            if (researcher.Type == Researcher.ResearcherType.Staff)
            {
                Staff staff = (Staff)researcher;
                Staff.AverageThreeYear(staff);
                Staff.PerfByPub(staff);
                staff.FundingRecieved = XMLAdapter.LoadFunding(staff);
                Staff.PerfByFund(staff);
            }
        }

        public static string findSupervisions(List<Researcher> researchers, Staff stf)
        {
            Console.WriteLine("finding supervisions");
            int supervisorID = stf.ID; 
            var matchingStudents = researchers
                .Where(rs => rs is Student)
                .Cast<Student>()
                .Where(stu => stu.Supervisor == supervisorID);
            string studentNames = string.Join(", ", matchingStudents.Select(stu => stu.FirstName + " " + stu.LastName));
            foreach (var student in matchingStudents)
            {
                // Access the properties of each matching student
                Console.WriteLine("Matching student found: " + student.FirstName + " " + student.LastName);
            }
            stf.SuperCount = matchingStudents.Count();
            stf.StudentsSupervised = studentNames;

            Console.WriteLine("the student names are: ");
            Console.WriteLine(studentNames);

            return studentNames;
        }


        public static List<List<Staff>> SortReport(ObservableCollection<Researcher> researchers)
        {
            List<Staff> Poor = new List<Staff>();
            List<Staff> B_Expect = new List<Staff>();
            List<Staff> M_Min = new List<Staff>();
            List<Staff> Stars = new List<Staff>();
            List<List<Staff>> results = new List<List<Staff>>();
            Staff staff;                                        // Conversion for Researcher -> Staff

            // Loops Full list
            foreach (Researcher rs in researchers)
            {
                // Matches Staff types
                if (rs.Type == Researcher.ResearcherType.Staff)
                {
                    //casts each Researcher of type Staff to a Staff object
                    staff = (Staff)rs;
                    /*
                    fetches the publications related to the Staff object from a database using DBAdapter.GetPubs(staff)
                    calculates the three-year average of their performance, calculates their performance by publication
                    loads their received funding data from an XML file
                    */
                    staff.Pubs = DBAdapter.GetPubs(staff);
                    Staff.AverageThreeYear(staff);
                    Staff.PerfByPub(staff);
                    staff.FundingRecieved = XMLAdapter.LoadFunding(staff);

                    //converts the PerformanceByPublication property (which is a percentage string) into a double data type for mathematical comparisons
                    double.TryParse(staff.PerformanceByPublication.Replace("%", ""), out double result);

                    //categorizes Staff objects into different lists: Poor, B_Expect, M_Min, and Stars. The categorization is based on the performance
                    if (result <= 70.0)
                    {
                        Poor.Add(staff);
                    }
                    else if (result > 70.0 && result < 110.0)
                    {
                        B_Expect.Add(staff);
                    }
                    else if (result >= 110.0 && result < 200.0)
                    {
                        M_Min.Add(staff);
                    }
                    else
                    {
                        Stars.Add(staff);
                    }
                }
            }
            results.Add(Poor);
            results.Add(B_Expect);
            results.Add(M_Min);
            results.Add(Stars);
            return results;
        }

        public static List<Staff> GenReport(string lvl, List<List<Staff>> data)
        {
            switch (lvl)
            {
                case "Poor":
                    return data[0].OrderBy(x => x.PerformanceByPublication).ToList();
                    break;
                case "Below Expectations":
                    return data[1].OrderBy(x => x.PerformanceByPublication).ToList();
                    break;
                case "Meeting Minimum":
                    return data[2].OrderByDescending(x => x.PerformanceByPublication).ToList();
                    break;
                case "Star Performers":
                    return data[3].OrderByDescending(x => x.PerformanceByPublication).ToList();
                    break;
            }
            return null;
        }

    }

}