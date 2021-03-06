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
            Dictionary<string, int> features_names_to_idx = new Dictionary<string, int>();

            using (var reader = new StreamReader(path))
            {
                // extract features names from the first line of the csv file 
                var featuresLine = reader.ReadLine();
                var featuresNames = featuresLine.Split(",");
                for (int i = 0; i < featuresNames.Length; i++)
                {
                    string aFeatureName = featuresNames[i];
                    if (features_names_to_idx.ContainsKey(aFeatureName))
                    {
                        aFeatureName += "2";
                    }
                    features_names_to_idx.Add(aFeatureName, i);
                }

                // add a pair of (feature, values-list) to the features dictionary
                foreach (var feature in features_names_to_idx)
                {
                    featuresDict.Add(feature.Key, new List<float>());
                }

                // fill features' values lists with data from the csv file
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    // split each line by ',' to get specific features values
                    float[] floats = System.Array.ConvertAll(line.Split(','), float.Parse);
                    // add each value in doubles list to the matching feature's values list in the dictionary
                    foreach (var feature in featuresDict)
                    {
                        feature.Value.Add(floats[features_names_to_idx[feature.Key]]);
                    }
                    if (line == null) continue;
                    lines_list.Add(line);
                    row_count++;
                }
            }
            string flightContent = File.ReadAllText(path);
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
            if (index < lines_list.Count)
            {
                return lines_list[index];
            }
            return lines_list[0];
        }

        public float getFeatureByLine(string feature, int line)
        {
            if (feature.Equals("none"))
            {
                return 0;
            }
            return featuresDict[feature][line];
        }

    }
}
