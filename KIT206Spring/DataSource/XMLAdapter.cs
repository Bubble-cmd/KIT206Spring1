using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Navigation;
using System.Xml;
using System.Xml.Linq;
using KIT206Spring.Entities;

namespace KIT206Spring.DataSource
{
    public static class XMLAdapter
    {
        private static string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\DataSource\\Fundings_Rankings.xml");


        public static int LoadFunding(Staff staff)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(filePath);
            int funding = 0;

            XmlNodeList projectNode = xml.SelectNodes("/Projects/Project");

            foreach (XmlNode nodeProject in projectNode)
            {
                XmlNode researchersNode = nodeProject.SelectSingleNode("Researchers");

                XmlNodeList staffNodes = researchersNode.SelectNodes("staff_id");
                foreach (XmlNode nodeStaff in staffNodes)
                {
                    if (nodeStaff.InnerText == Convert.ToString(staff.ID))
                    {
                        XmlNode fundingNode = nodeProject.SelectSingleNode("Funding");
                        int fundingAmount = int.Parse(fundingNode.InnerText);
                        funding += fundingAmount;
                    }
                }
            }
            return funding;
        }
    }
}
