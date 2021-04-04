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
        private List<double> aileron_list = new List<double>();
        private List<double> elevator_list = new List<double>(); 
        private List<double> Rudder_list= new List<double>(); 
        private List<double> Throttle_list = new List<double>(); 


        public CsvHandler(string path)
        {
            this.path = path;
            row_count = 0;
            parseCsv();
            
        }

        private void parseCsv()
        {
            using (var reader = new StreamReader(path))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    double[] doubles = System.Array.ConvertAll(line.Split(','), double.Parse);
                    aileron_list.Add(doubles[0]);
                    elevator_list.Add(doubles[1]);
                    Rudder_list.Add(doubles[2]);
                    Throttle_list.Add(doubles[6]);
                    if (line == null) continue;
                    my_list.Add(line);
                    row_count++;
                }
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

        public List<string> getList()
        {
            return this.my_list;
        }

        public string getLine(int index)
        {
            return my_list[index];
        }

        public double getAileronByLine(int line)
        {
            return aileron_list[line];
        }
        public double getElevatorByLine(int line)
        {
            return elevator_list[line];
        }
        public double getRudderByLine(int line)
        {
            return Rudder_list[line];
        }
        public double getThrottleByLine(int line)
        {
            return Throttle_list[line];
        }
        
        public int getRowCount()
        {
            return row_count;
        }
    }
}
