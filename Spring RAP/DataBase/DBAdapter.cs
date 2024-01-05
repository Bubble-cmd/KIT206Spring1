using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using MySql.Data.MySqlClient;
using KIT206Spring.Spring_RAP.Entities;
using KIT206Spring.Spring_RAP.View;

namespace KIT206Spring.Spring_RAP.Database
{
    class DBAdapter
    {
        private const string db = "kit206";
        private const string user = "kit206";
        private const string pass = "kit206";
        private const string server = "alacritas.cis.utas.edu.au";

        private MySqlConnection conn;

        public DBAdapter()
        {
            string connectionString = String.Format("Database={0};Data Source={1};User Id={2};Password={3}", db, server, user, pass);
            conn = new MySqlConnection(connectionString);
        }

        /* 
         * defines a MySqlDataReader object named rdr and a DBAdapter object named demo
         * MySqlDataReader object is used to read the data retrieved from the MySQL database
         * DBAdapter object is used to establish a connection with the database
         * DOIS is created to store the DOIs (Digital Object Identifiers) of the publications related to the researcher
        */
        public static List<Publication> GetPubs(Researcher rs)
        {
            MySqlDataReader rdr = null;
            DBAdapter demo = new DBAdapter();
            List<String> DOIS = new List<String>();
            //try block is used to handle any potential errors that can occur when connecting to the database or reading data from it
            try
            {
                demo.conn.Open();
                //creates a MySqlCommand object named cmd which contains an SQL query.
                //This query selects the DOIs from the researcher_publication table where the researcher's ID matches the one provided
                MySqlCommand cmd = new MySqlCommand("select doi from researcher_publication where researcher_id = @id", demo.conn);
                cmd.Parameters.AddWithValue("@id", rs.ID.ToString());
                //adds the researcher's ID as a parameter to the SQL command
                rdr = cmd.ExecuteReader();
                // while loop is used to read each row in the rdr object.
                // For each row, the function retrieves the DOI, prints it to the console, and adds it to the DOIS list
                while (rdr.Read())
                {
                    var DOI = rdr.GetString("doi");
                    Console.WriteLine("adding " + DOI);
                    DOIS.Add(DOI);
                }
            }
            //finally block ensures that the rdr and the database connection demo.conn are always closed, even if an error occurs during the try block
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
                if (demo.conn != null)
                {
                    demo.conn.Close();
                }
            }
            Console.WriteLine("PAUSE;");
            return GetPublications(rs, DOIS);//returns a list of Publication objects
        }
        /*
         * retrieves the details of publications associated with a specific researcher and a list of DOIs from the database. 
         * It iterates over each DOI in the provided list, executing an SQL query to fetch publication details for each DOI from the database.
        */
        public static List<Publication> GetPublications(Researcher Res, List<string> TheDOIS)
        {
            MySqlDataReader rdr = null;
            DBAdapter demo = new DBAdapter();
            List<Publication> publications = new List<Publication>();
            //For each publication found, a Publication object is created, adds it to a list of publications, and also to the researcher's list of publications.
            foreach (string doi in TheDOIS)
            {
                try
                {
                    Console.WriteLine("the doi is " + doi);

                    demo.conn.Open();
                    MySqlCommand cmd = new MySqlCommand("select * from publication where doi = @doi", demo.conn);
                    cmd.Parameters.AddWithValue("@doi", doi);
                    rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        var title = rdr.GetString("title");
                        var authors = rdr.GetString("authors");
                        var year = rdr.GetInt32("year");
                        var type = rdr.GetString("type");
                        var cite_as = rdr.GetString("cite_as");
                        var available = rdr.GetDateTime("available");
                        var ranking = rdr.GetString("ranking");
                        Publication pub = new Publication(title, doi, authors, cite_as, available, type, ranking);
                        publications.Add(pub);
                        Res.Pubs.Add(pub);
                    }
                }
                //finally block ensures the database connection is closed after each query, regardless of its success or failure.
                finally
                {
                    if (rdr != null)
                    {
                        rdr.Close();
                    }
                    if (demo.conn != null)
                    {
                        demo.conn.Close();
                    }

                }
            }
            return publications;//After all DOIs have been processed, it returns the list of publications.
        }
        //comment here
        public static void GetSupervisions(Staff Stf)
        {
            MySqlDataReader rdr = null;
            DBAdapter demo = new DBAdapter();

            try
            {
                demo.conn.Open();
                MySqlCommand cmd = new MySqlCommand("select  * from researcher where supervisor_id=@id", demo.conn);
                cmd.Parameters.AddWithValue("@id", Stf.ID.ToString());
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var id = rdr.GetInt32("id");
                    var type = rdr.GetString("type");
                    var firstName = rdr.GetString("given_name");
                    var lastName = rdr.GetString("family_name");
                    var title = rdr.GetString("title");
                    var unit = rdr.GetString("unit");
                    var campus = rdr.GetString("campus");
                    var email = rdr.GetString("email");
                    var photo = rdr.GetString("photo");
                    var utas_start = rdr.GetDateTime("utas_start");
                    var cur_start = rdr.GetDateTime("current_start");
                    var degree = rdr.GetString("degree");
                    var superID = rdr.GetInt32("supervisor_id");
                    var lev = "Student";
                    Student rs = new Student(id, type, firstName, lastName, title, unit, campus, email, photo, superID, degree, utas_start, cur_start, lev);
                }
            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
                if (demo.conn != null)
                {
                    demo.conn.Close();
                }
            }
        }

        // need comment here
        public static List<Researcher> GetResearcher()
        {
            MySqlDataReader rdr = null;
            DBAdapter demo = new DBAdapter();

            List<Researcher> Researchers = new List<Researcher>();
            try
            {
                demo.conn.Open();
                MySqlCommand cmd = new MySqlCommand("select  * from researcher", demo.conn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    var id = rdr.GetInt32("id");
                    var type = rdr.GetString("type");
                    var firstName = rdr.GetString("given_name");
                    var lastName = rdr.GetString("family_name");
                    var title = rdr.GetString("title");
                    var unit = rdr.GetString("unit");
                    var campus = rdr.GetString("campus");
                    var email = rdr.GetString("email");
                    var photo = rdr.GetString("photo");
                    var utas_start = rdr.GetDateTime("utas_start");
                    var cur_start = rdr.GetDateTime("current_start");
                    if (type.Equals("Student"))
                    {
                        var degree = rdr.GetString("degree");
                        var superID = rdr.GetInt32("supervisor_id");
                        var lev = "Student";


                        Student Stu = new Student(id, type, firstName, lastName, title, unit, campus, email, photo, superID, degree, utas_start, cur_start, lev);
                        Researchers.Add(Stu);
                    }
                    else
                    {
                        var lev = rdr.GetString("level");
                        Staff sta = new Staff(id, type, firstName, lastName, title, unit, campus, email, photo, lev, utas_start, cur_start);
                        Researchers.Add(sta);

                    }
                }
            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
                if (demo.conn != null)
                {
                    demo.conn.Close();
                }
            }
            return Researchers;
        }


        public static List<Staff> GetStaff()
        {
            MySqlDataReader rdr = null;
            DBAdapter demo = new DBAdapter();

            List<Staff> Staff = new List<Staff>();
            try
            {
                demo.conn.Open();
                MySqlCommand cmd = new MySqlCommand("select  * from researcher", demo.conn);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    string type = rdr[1].ToString();

                    if (type.Equals("Staff"))
                    {
                        var id = rdr.GetInt32("id");
                        var firstName = rdr.GetString("given_name");
                        var lastName = rdr.GetString("family_name");
                        var title = rdr.GetString("title");
                        var unit = rdr.GetString("unit");
                        var campus = rdr.GetString("campus");
                        var email = rdr.GetString("email");
                        var photo = rdr.GetString("photo");
                        var lev = rdr.GetString("level");
                        var utas_start = rdr.GetDateTime("utas_start");
                        var cur_start = rdr.GetDateTime("current_start");
                        Staff sta = new Staff(id, type, firstName, lastName, title, unit, campus, email, photo, lev, utas_start, cur_start);
                        Staff.Add(sta);
                    }
                }

                Console.WriteLine("\n\t\t StAff");
                foreach (Staff sta in Staff)
                {
                    Console.WriteLine(sta.Type + " " + sta.FirstName + " " + sta.LastName);
                }
            }

            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
                if (demo.conn != null)
                {
                    demo.conn.Close();
                }
            }
            return Staff;
        }
        // need comment here
        public static Student GetStudent(int ID)
        {
            Student Stu;
            MySqlDataReader rdr = null;
            DBAdapter demo = new DBAdapter();
            try
            {
                demo.conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from researcher where id = @id", demo.conn);
                cmd.Parameters.AddWithValue("@id", ID);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Console.WriteLine("the individual is" + rdr.GetString("given_name"));
                    var type = rdr.GetString("type");
                    var firstName = rdr.GetString("given_name");
                    var lastName = rdr.GetString("family_name");
                    var title = rdr.GetString("title");
                    var unit = rdr.GetString("unit");
                    var campus = rdr.GetString("campus");
                    var email = rdr.GetString("email");
                    var photo = rdr.GetString("photo");
                    var degree = rdr.GetString("degree");
                    var superID = rdr.GetInt32("supervisor_id");
                    var utas_start = rdr.GetDateTime("utas_start");
                    var cur_start = rdr.GetDateTime("current_start");
                    var lev = "Student";

                    Stu = new Student(ID, type, firstName, lastName, title, unit, campus, email, photo, superID, degree, utas_start, cur_start, lev);
                    if (rdr != null)
                    {
                        rdr.Close();
                    }
                    if (demo.conn != null)
                    {
                        demo.conn.Close();
                    }
                    return Stu;
                }
            }

            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
                if (demo.conn != null)
                {
                    demo.conn.Close();
                }
            }
            return null;
        }

        // need comment here
        public static Staff GetStaff(int ID)
        {
            Staff Sta;
            MySqlDataReader rdr = null;
            DBAdapter demo = new DBAdapter();
            try
            {
                demo.conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from researcher where id = @id", demo.conn);
                cmd.Parameters.AddWithValue("@id", ID);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Console.WriteLine("the individual is" + rdr.GetString("given_name"));
                    var type = rdr.GetString("type");
                    var firstName = rdr.GetString("given_name");
                    var lastName = rdr.GetString("family_name");
                    var title = rdr.GetString("title");
                    var unit = rdr.GetString("unit");
                    var campus = rdr.GetString("campus");
                    var email = rdr.GetString("email");
                    var photo = rdr.GetString("photo");
                    var level = rdr.GetString("level");
                    var utas_start = rdr.GetDateTime("utas_start");
                    var cur_start = rdr.GetDateTime("current_start");

                    Sta = new Staff(ID, type, firstName, lastName, title, unit, campus, email, photo, level, utas_start, cur_start);
                    if (rdr != null)
                    {
                        rdr.Close();
                    }
                    if (demo.conn != null)
                    {
                        demo.conn.Close();
                    }
                    return Sta;
                }
            }

            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
                if (demo.conn != null)
                {
                    demo.conn.Close();
                }
            }
            return null;
        }

        // need comment here
        public static void GetPositions(Staff Sta)
        {

            MySqlDataReader rdr = null;
            DBAdapter demo = new DBAdapter();
            try
            {
                demo.conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from position where id = @id", demo.conn);
                cmd.Parameters.AddWithValue("@id", Sta.ID);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var level = rdr.GetString("level");
                    var start = rdr.GetDateTime("start"); 
                    DateTime? end = null;

                    if (!rdr.IsDBNull(rdr.GetOrdinal("end")))
                    {
                        end = rdr.GetDateTime("end");
                    }
                    else
                    {
                        end = null;
                    }

                    Position pos = new Position(start, end, level);
                    Sta.Positions.Add(pos);
                }
            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
                if (demo.conn != null)
                {
                    demo.conn.Close();
                }
            }
            Console.WriteLine("PAUSE;");
        }

        // need comment here
        public void ReadIntoDataSet()
        {
            try
            {
                var researcherDataSet = new DataSet();
                var researcherAdapter = new MySqlDataAdapter("select * from researcher", conn);
                researcherAdapter.Fill(researcherDataSet, "researcher");

                foreach (DataRow row in researcherDataSet.Tables["researcher"].Rows)
                {
                    Console.WriteLine("Name: {0} {1}", row["given_name"], row["family_name"].ToString());
                }
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }

        // need comment here
        public int GetNumberOfRecords()
        {
            int count = -1;
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select COUNT(*) from researcher", conn);
                count = int.Parse(cmd.ExecuteScalar().ToString());
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return count;
        }

    }
}
