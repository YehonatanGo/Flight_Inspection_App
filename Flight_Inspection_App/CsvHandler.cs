using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Flight_Inspection_App
{
    class CsvHandler
    {
        private string path;
        private int row_count;
        private List<string> my_list = new List<string>();

        public CsvHandler(string path)
        {
            this.path = path;
            parseCsv();
        }

        private void parseCsv()
        {
            using (var reader = new StreamReader(path))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (line == null) continue;
                    my_list.Add(line);
                }
                row_count++;
            }
        }

        public Dictionary<string, int> getFeaturesNames(string path)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);

            XmlNodeList features = xmlDoc.GetElementsByTagName("name");

            Dictionary<string, int> featuresDict = new Dictionary<string, int>();

            int featuresCount = 0;

            for (int i = 0; i < 42; i++)
            {
                string key = features[i].InnerText;
                if (featuresDict.ContainsKey(features[i].InnerText))
                {
                    key += "2";
                }
                featuresDict.Add(key, i);

            }
            return featuresDict;

        }
    }
}
