using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Flight_Inspection_App
{
    class CsvHandler
    {
        private string path;
        private int row_num;
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
                row_num++;
            }
        }
    }
}
