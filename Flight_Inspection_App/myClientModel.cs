using System.ComponentModel;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading;

namespace Flight_Inspection_App
{
    class myClientModel : IClientModel
    {
        public event PropertyChangedEventHandler PropertyChanged;



        private string path;
        public string Path
        {
            set
            {
                path = value;
                NotifyPropertyChanged("path");
            }
            get
            {
                return path;
            }
        }

        public bool Play { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public double PlaySpeed { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public int RunningLine { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        public myClientModel()
        {
            Path = "";
        }


        public void connectFlightGear()
        {
            // cmd process
            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.UseShellExecute = false;
            cmd.StartInfo.WorkingDirectory = @"C:\Program Files\FlightGear 2020.3.6\bin";
            cmd.StartInfo.RedirectStandardOutput = false;
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.Start();

            // run FlightGear
            cmd.StandardInput.WriteLine(@"fgfs --generic=socket,in,10,127.0.0.1,5400,tcp,playback_small --fdm=null");

            // wait till FG starts
            Thread.Sleep(100000);

            var client = new TcpClient("localhost", 5400);
            NetworkStream ns = client.GetStream();
            var file = new System.IO.StreamReader(path);
            string line;

            //send csv file to FlightGear, line after line
            while ((line = file.ReadLine()) != null)
            {
                line += "\r\n";
                ns.Write(System.Text.Encoding.ASCII.GetBytes(line));
                ns.Flush();
                Thread.Sleep(100);
            }

            file.Close();
            ns.Close();
            client.Close();
        }

        public void connect(string ip, int port)
        {
            throw new System.NotImplementedException();
        }

        public void start()
        {
            throw new System.NotImplementedException();
        }

        public void loadFileToMap()
        {
            throw new System.NotImplementedException();
        }

        public void disconnect()
        {
            throw new System.NotImplementedException();
        }

        public void connectFlightGear(string ip, int port)
        {
            throw new System.NotImplementedException();
        }

        public void sendLine()
        {
            throw new System.NotImplementedException();
        }
    }
}
