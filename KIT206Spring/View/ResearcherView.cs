using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KIT206Spring.Entities;
using KIT206Spring.Controllers;


namespace KIT206Spring.View
{
    internal class ResearcherView
    {
        public static void PrintAllResearchers(List<Researcher> ResList)
        {
            Console.WriteLine("---\t---\tThis is the MainView\t---\t---");
            Console.WriteLine("Family Name | Given Name (title) | Employment level | student status |");
            // here we will display different info depending on if they are a student or not

            List<Staff> stfList = new List<Staff>();
            List<Student> stuList = new List<Student>();
            foreach (Researcher res in ResList)
            {
                if (res is Staff staff)
                {
                    // display specific student stuff... they have a degree and a super, but not type???
                    Console.WriteLine($"Staff {staff.FirstName} {staff.LastName}, Level {staff.level}");
                    stfList.Add(staff);

                }
                if (res is Student student)
                {
                    Console.WriteLine($"Student {student.FirstName} {student.LastName}, Supervisor {student.Supervisor}");
                    stuList.Add(student);
                }
            }

            Console.WriteLine("the students only are... ");
            List<Researcher> filtRes = FilterByStudent(ResList);
            Console.Write("A grade staff are");
            List<Staff> aGradeStaff = FilterByLevel(stfList, Level.A);
            foreach (Staff sta in aGradeStaff)
            {
                Console.WriteLine(sta.FirstName);
            }

            Console.WriteLine("Select researcher form the list wil line no");
            // get user input for the desired line number
            int selectedLine;
            while (!int.TryParse(Console.ReadLine(), out selectedLine) || selectedLine < 1 || selectedLine > ResList.Count())
            {
                Console.WriteLine($"Invalid input. Please enter a number between 1 and {ResList.Count()}.");
            }
            Researcher selectedResearcher = ResList[selectedLine - 1];
            Console.WriteLine("you sleected " + selectedLine + "which is " + selectedResearcher.FirstName);
            Console.WriteLine(selectedResearcher.ID);
        }

        // sorts a list of researchers by first name
        public static List<Researcher> SortByFirstName(List<Researcher> ResList)
        {
            ResList.Sort((x, y) => x.FirstName.CompareTo(y.FirstName));
            foreach (Researcher res in ResList)
            {
                Console.WriteLine(res.FirstName);
            }
            return ResList;
        }

        // sorts a list of researchers by Last name
        public static List<Researcher> SortByLastName(List<Researcher> ResList)
        {
            ResList.Sort((x, y) => x.LastName.CompareTo(y.LastName));
            foreach (Researcher res in ResList)
            {
                Console.WriteLine(res.LastName);
            }
            return ResList;
        }

        // reverse sort i.e. (z - a) a list of researchers by first name 
        public static List<Researcher> ReverseSortByFirstName(List<Researcher> ResList)
        {
            ResList.Sort((x, y) => y.FirstName.CompareTo(x.FirstName));
            foreach (Researcher res in ResList)
            {
                Console.WriteLine(res.FirstName);
            }
            return ResList;
        }
        // reverse sort i.e. (z - a) a list of researchers by last name 
        public static List<Researcher> ReverseSortByLastName(List<Researcher> ResList)
        {
            ResList.Sort((x, y) => y.LastName.CompareTo(x.LastName));
            foreach (Researcher res in ResList)
            {
                Console.WriteLine(res.LastName);
            }
            return ResList;
        }
        public static List<Researcher> FilterByStudent(List<Researcher> ResList)
        {
            //List <Researcher> studentResearchers = ResList.Where(r => r is Student).ToList();

            List<Researcher> stuResearchers = (
                                            from researcher in ResList
                                            where researcher is Student
                                            select researcher
                                            ).ToList();

            return stuResearchers;
        }
        public static List<Staff> FilterByLevel(List<Staff> ResList, Level level)
        {
            if (level.Equals(Level.A))
            {
                List<Staff> stuResearchers = (
                                                            from stf in ResList
                                                            where stf.level is "A"
                                                            select stf
                                                            ).ToList();
                return stuResearchers;

            }
            if (level.Equals(Level.B))
            {
                List<Staff> stuResearchers = (
                                                            from stf in ResList
                                                            where stf.level is "B"
                                                            select stf
                                                            ).ToList();
                return stuResearchers;

            }

            if (level.Equals(Level.C))
            {
                List<Staff> stuResearchers = (
                                                            from stf in ResList
                                                            where stf.level is "C"
                                                            select stf
                                                            ).ToList();
                return stuResearchers;

            }
            if (level.Equals(Level.D))
            {
                List<Staff> stuResearchers = (
                                                            from stf in ResList
                                                            where stf.level is "D"
                                                            select stf
                                                            ).ToList();
                return stuResearchers;
            }
            return null;
        }
    }
}
