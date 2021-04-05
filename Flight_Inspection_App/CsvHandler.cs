using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Flight_Inspection_App
{
    class CsvHandler
    {
        private string path;
        private int row_count;
        private List<string> lines_list;
        private List<double> aileron_list;
        private List<double> elevator_list;
        private List<double> Rudder_list;
        private List<double> Throttle_list;
        private Dictionary<string, List<double>> featuresDict;
        

        public CsvHandler(string path)
        {
            this.path = path;
            row_count = 0;
            lines_list = new List<string>();
            aileron_list = new List<double>();
            elevator_list = new List<double>();
            Rudder_list = new List<double>();
            Throttle_list = new List<double>();
            featuresDict = new Dictionary<string, List<double>>();
            parseCsv();
            
        }

        private void parseCsv()
        {
            using (var reader = new StreamReader(path))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    // split each line by ',' to get specific features values
                    double[] doubles = System.Array.ConvertAll(line.Split(','), double.Parse);
                    aileron_list.Add(doubles[0]);
                    elevator_list.Add(doubles[1]);
                    Rudder_list.Add(doubles[2]);
                    Throttle_list.Add(doubles[6]);
                    if (line == null) continue;
                    lines_list.Add(line);
                    row_count++;
                }
            }
        }

        // returns number of rows
        public int getRowCount()
        {
            return row_count;
        }

        // parse xml file to get features' names 
        // returns an dictionatry of FEATURE_NAME:INDEX
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

        //returns the list of lines
        public List<string> getList()
        {
            return this.lines_list;
        }
        //returns specific line by its index
        public string getLine(int index)
        {
            return lines_list[index];
        }

        // next methods - for getting spefic feature's value in a given line

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
    }
}
