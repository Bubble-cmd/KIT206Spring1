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
using KIT206Spring.Entities;
using KIT206Spring.Spring_RAP.View

namespace KIT206Spring.Spring_RAP.Database
{

    class DBAdapter
    {
        //Note that ordinarily these would (1) be stored in a settings file and (2) have some basic encryption applied
        private const string db = "kit206";
        private const string user = "kit206";
        private const string pass = "kit206";
        private const string server = "alacritas.cis.utas.edu.au";

        private MySqlConnection conn;

        public DBAdapter()
        {
            /*
             * Create the connection object (does not actually make the connection yet)
             * Note that the RAP case study database has the same values for its name, user name and password (to keep things simple)
             */
            string connectionString = String.Format("Database={0};Data Source={1};User Id={2};Password={3}", db, server, user, pass);
            conn = new MySqlConnection(connectionString);
        }

        // gets the publications for any one researcher
        public static List<Publication> GetPubs(Researcher rs)
        {
            MySqlDataReader rdr = null;
            DBAdapter demo = new DBAdapter();

            // get list of DOI strings which match resercher_id
            List<String> DOIS = new List<String>();
            try
            {
                // Open the connection
                demo.conn.Open();
                // 1. Instantiate a new command with a query and connection
                MySqlCommand cmd = new MySqlCommand("select doi from researcher_publication where researcher_id = @id", demo.conn);
                cmd.Parameters.AddWithValue("@id", rs.ID.ToString());

                // 2. Call Execute reader to get query results
                rdr = cmd.ExecuteReader();
                // print the CategoryName of each record
                while (rdr.Read())
                {
                    var DOI = rdr.GetString("doi");
                    Console.WriteLine("adding " + DOI);
                    DOIS.Add(DOI);
                    //Console.WriteLine(rdr.GetInt32("researcher_id"));
                }
            }
            finally
            {
                // close the reader
                if (rdr != null)
                {
                    rdr.Close();
                }

                // Close the connection
                if (demo.conn != null)
                {
                    demo.conn.Close();
                }
            }
            Console.WriteLine("PAUSE;");
            return GetPublications(rs, DOIS);
        }

        public static List<Publication> GetPublications(Researcher Res, List<string> TheDOIS)

        {
            MySqlDataReader rdr = null;
            DBAdapter demo = new DBAdapter();
            List<Publication> publications = new List<Publication>();

            foreach (string doi in TheDOIS)
            {
                try
                {
                    Console.WriteLine("the doi is " + doi);

                    demo.conn.Open();
                    // 1. Instantiate a new command with a query and connection
                    MySqlCommand cmd = new MySqlCommand("select * from publication where doi = @doi", demo.conn);
                    cmd.Parameters.AddWithValue("@doi", doi);
                    rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        var title = rdr.GetString("title");
                        var authors = rdr.GetString("authors"); // thisis just a string of authors, not researcchers
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
                finally
                {
                    // close the reader
                    if (rdr != null)
                    {
                        rdr.Close();
                    }

                    // Close the connection
                    if (demo.conn != null)
                    {
                        demo.conn.Close();
                    }

                }
            }

            return publications;
            // used for debugging
            /*
            foreach(Publication pub in pubs)
            {
                Console.WriteLine("pub name is " + pub.Title);
            }
            Console.WriteLine("PAUSE;");
            */
        }

        //gets supervisions delete this
        public static void GetSupervisions(Staff Stf)
        {
            MySqlDataReader rdr = null;
            DBAdapter demo = new DBAdapter();

            try
            {
                // Open the connection
                demo.conn.Open();
                // 1. Instantiate a new command with a query and connection
                MySqlCommand cmd = new MySqlCommand("select  * from researcher where supervisor_id=@id", demo.conn);
                cmd.Parameters.AddWithValue("@id", Stf.ID.ToString());
                // 2. Call Execute reader to get query results
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
                    //Stf.Supervisions.Add(rs);
                }
            }
            finally
            {
                // close the reader
                if (rdr != null)
                {
                    rdr.Close();
                }

                // Close the connection
                if (demo.conn != null)
                {
                    demo.conn.Close();
                }
            }
        }


        public static List<Researcher> GetResearcher()
        {
            MySqlDataReader rdr = null;
            DBAdapter demo = new DBAdapter();

            List<Researcher> Researchers = new List<Researcher>();
            try
            {
                // Open the connection
                demo.conn.Open();
                // 1. Instantiate a new command with a query and connection
                MySqlCommand cmd = new MySqlCommand("select  * from researcher", demo.conn);
                // 2. Call Execute reader to get query results
                rdr = cmd.ExecuteReader();
                // print the CategoryName of each record

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
                        //GetPubs(Stu);
                        Researchers.Add(Stu);
                    }
                    else
                    {
                        var lev = rdr.GetString("level");
                        Staff sta = new Staff(id, type, firstName, lastName, title, unit, campus, email, photo, lev, utas_start, cur_start);
                        //GetPubs(sta);
                        Researchers.Add(sta);

                    }
                }
            }
            finally
            {
                // close the reader
                if (rdr != null)
                {
                    rdr.Close();
                }

                // Close the connection
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

                // Open the connection
                demo.conn.Open();

                // 1. Instantiate a new command with a query and connection
                MySqlCommand cmd = new MySqlCommand("select  * from researcher", demo.conn);

                // 2. Call Execute reader to get query results
                rdr = cmd.ExecuteReader();
                // print the CategoryName of each record
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
                // close the reader
                if (rdr != null)
                {
                    rdr.Close();
                }

                // Close the connection
                if (demo.conn != null)
                {
                    demo.conn.Close();
                }
            }
            return Staff;
        }


        /*
         * Using the ExecuteReader method to select from a single table
         */
        public static Student GetStudent(int ID)
        {
            Student Stu;
            MySqlDataReader rdr = null;
            DBAdapter demo = new DBAdapter();
            try
            {
                // Open the connection
                demo.conn.Open();

                // 1. Instantiate a new command with a query and connection
                MySqlCommand cmd = new MySqlCommand("select * from researcher where id = @id", demo.conn);
                cmd.Parameters.AddWithValue("@id", ID);

                // 2. Call Execute reader to get query results
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Console.WriteLine("the individual is" + rdr.GetString("given_name"));
                    // coud just access the fields we don't currently have in the researcher class
                    // and coppy the fields which already exist?

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
                    // close the reader
                    if (rdr != null)
                    {
                        rdr.Close();
                    }

                    // Close the connection
                    if (demo.conn != null)
                    {
                        demo.conn.Close();
                    }
                    return Stu;
                }
            }

            finally
            {
                // close the reader
                if (rdr != null)
                {
                    rdr.Close();
                }

                // Close the connection
                if (demo.conn != null)
                {
                    demo.conn.Close();
                }
            }
            // would rather return Stu here, how ever was not sure how
            return null;
        }

        /*
        * Using the ExecuteReader method to select from a single table
        */
        public static Staff GetStaff(int ID)
        {
            Staff Sta;
            MySqlDataReader rdr = null;
            DBAdapter demo = new DBAdapter();
            try
            {
                // Open the connection
                demo.conn.Open();

                // 1. Instantiate a new command with a query and connection
                MySqlCommand cmd = new MySqlCommand("select * from researcher where id = @id", demo.conn);
                cmd.Parameters.AddWithValue("@id", ID);

                // 2. Call Execute reader to get query results
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Console.WriteLine("the individual is" + rdr.GetString("given_name"));
                    // coud just access the fields we don't currently have in the researcher class
                    // and coppy the fields which already exist?

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
                    // close the reader
                    if (rdr != null)
                    {
                        rdr.Close();
                    }

                    // Close the connection
                    if (demo.conn != null)
                    {
                        demo.conn.Close();
                    }
                    return Sta;
                }
            }

            finally
            {
                // close the reader
                if (rdr != null)
                {
                    rdr.Close();
                }

                // Close the connection
                if (demo.conn != null)
                {
                    demo.conn.Close();
                }
            }
            return null;
        }

        public static void GetPositions(Staff Sta)
        {

            MySqlDataReader rdr = null;
            DBAdapter demo = new DBAdapter();
            // get list of DOI strings which match resercher_id

            try
            {
                // Open the connection
                demo.conn.Open();

                // 1. Instantiate a new command with a query and connection
                MySqlCommand cmd = new MySqlCommand("select * from position where id = @id", demo.conn);
                cmd.Parameters.AddWithValue("@id", Sta.ID);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var level = rdr.GetString("level");
                    var start = rdr.GetDateTime("start"); // thisis just a string of authors, not researcchers
                    DateTime? end = null;

                    if (!rdr.IsDBNull(rdr.GetOrdinal("end"))) // check if value is not null
                    {
                        end = rdr.GetDateTime("end"); // assign value to end variable
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
                // close the reader
                if (rdr != null)
                {
                    rdr.Close();
                }

                // Close the connection
                if (demo.conn != null)
                {
                    demo.conn.Close();
                }
            }
            Console.WriteLine("PAUSE;");
        }


        /*
         * Using the ExecuteReader method to select from a single table */
        public void ReadIntoDataSet()
        {
            try
            {
                var researcherDataSet = new DataSet();
                var researcherAdapter = new MySqlDataAdapter("select * from researcher", conn);
                researcherAdapter.Fill(researcherDataSet, "researcher");

                foreach (DataRow row in researcherDataSet.Tables["researcher"].Rows)
                {
                    //Again illustrating that indexer (based on column name) gives access to whatever data
                    //type was obtained from a given column, but can call ToString() on an entry if needed.
                    Console.WriteLine("Name: {0} {1}", row["given_name"], row["family_name"].ToString());
                }
            }
            finally
            {
                // Close the connection
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }


        /*
         * Using the ExecuteScalar method
         * returns number of records
         */
        public int GetNumberOfRecords()
        {
            int count = -1;
            try
            {
                // Open the connection
                conn.Open();

                // 1. Instantiate a new command
                MySqlCommand cmd = new MySqlCommand("select COUNT(*) from researcher", conn);

                // 2. Call ExecuteScalar to send command
                // This convoluted approach is safe since cannot predict actual return type
                count = int.Parse(cmd.ExecuteScalar().ToString());
            }
            finally
            {
                // Close the connection
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return count;
        }

    }
}
