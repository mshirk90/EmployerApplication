using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;

namespace ConfigurationHelper
{
    public class Configuration
    {
        public static string GetConnectionString(string name)
        {
            string result =  string.Empty;
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string filepath = Path.Combine(baseDirectory, "DataSystems.config");
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(filepath);
            XmlNodeList nodelist = xmldoc.SelectNodes("/configuration/connectionStrings/add");
            foreach (XmlNode node in nodelist)
            {

                if (node.Attributes["name"].Value.ToString().ToUpper() == name.ToUpper())
                {
                    result = node.Attributes["connectionString"].Value;
                    break;
                }
            }
            return result;
        }
    }
}
