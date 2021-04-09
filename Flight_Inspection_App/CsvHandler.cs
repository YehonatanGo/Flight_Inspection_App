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
        private Dictionary<string, List<float>> featuresDict;
        public Dictionary<string, List<float>> FeaturesDict { get { return featuresDict; } }
        

        public CsvHandler(string path)
        {
            this.path = path;
            row_count = 0;
            lines_list = new List<string>();
            featuresDict = new Dictionary<string, List<float>>();
            parseCsv();
        }

        private void parseCsv()
        {
            Dictionary<string, int> features_names_to_idx = getFeaturesNames(@"C:\Program Files\FlightGear 2020.3.6\data\Protocol\playback_small.xml");

            using (var reader = new StreamReader(path))
            {
                // add a pair of (feature, values-list) to the features dictionary, for each feature given by the xml
                foreach(var feature in features_names_to_idx)
                {
                    featuresDict.Add(feature.Key, new List<float>());
                }

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    // split each line by ',' to get specific features values
                    float[] floats = System.Array.ConvertAll(line.Split(','), float.Parse);
                    // add each value in doubles list to the matching feature's values list in the dictionary
                    foreach(var feature in featuresDict)
                    {
                        feature.Value.Add(floats[features_names_to_idx[feature.Key]]);
                    }
                    if (line == null) continue;
                    lines_list.Add(line);
                    row_count++;
                }
            }
            string flightContent = System.IO.File.ReadAllText(path);
            List<string> featuresList = getFeaturesNamesList();
            string featuresString = concatFeaturesNames(featuresList);
            string[] lines = { featuresString, flightContent };
            File.WriteAllLines(Path.Combine(Directory.GetCurrentDirectory(), "flight.csv"), lines);
        }

        private string concatFeaturesNames(List<string> featuresList)
        {
            string result = "";
            foreach (var featureName in featuresList)
            {
                result += featureName;
                result += ",";
            }
            result = result.Remove(result.Length - 1);
            return result;
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

        public List<string> getFeaturesNamesList()
        {
            return new List<string>(featuresDict.Keys);
        }

        //returns the list of lines
        public List<string> getList()
        {
            return this.lines_list;
        }
        //returns specific line by its index
        public string getLine(int index)
        {
            if(index < lines_list.Count)
            {
            return lines_list[index];
            }
            return lines_list[0];
        }

        public float getFeatureByLine(string feature, int line)
        {
            if(feature == null)
            {
                return 0;
            }
            return featuresDict[feature][line];
        }
        
    }
}
