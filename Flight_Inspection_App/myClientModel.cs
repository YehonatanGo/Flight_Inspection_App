using System.ComponentModel;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading;

namespace Flight_Inspection_App
{
    class myClientModel : IClientModel
    {
        public event PropertyChangedEventHandler PropertyChanged;

        volatile private CsvHandler csv_handler;
        volatile private NetworkStream ns;
        volatile private TcpClient client;
        volatile private Thread sending_lines_thread;
        volatile private bool isAlreadyStarted;
  
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

        private int running_line;
        public int RunningLine
        {
            set
            {
                running_line = value;
                NotifyPropertyChanged("running_line");
            }
            get
            {
                return running_line;
            }
        }

        volatile private bool play;
        public bool Play { get
            {
                return play;
            }
            set
            {
                play = value;

                if(play)
                {
                    if(isAlreadyStarted == false)
                    {
                        isAlreadyStarted = true;
                        start();

                    }
                    else
                    {
                        // resume playing by waking up sending lines thread
                        sending_lines_thread.Resume();
                    }
                } else 
                // play = false
                {
                    sending_lines_thread.Suspend();
                }
            }
         }
        
        public double PlaySpeed { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        
    
        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        public myClientModel()
        {
            Path = "";
            running_line = 0;
        }

        public void connectFlightGear()
        {

            csv_handler = new CsvHandler(path);

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
            Thread.Sleep(40000);

            //setting up a Tcp connection
            this.client = new TcpClient("localhost", 5400);
            this.ns = client.GetStream();        
        }

        // creating thread and runs sendLines method.
        public void start()
        {
            sending_lines_thread = new Thread(new ThreadStart(sendLines));
            sending_lines_thread.Start();
            /*sending_lines_thread.Join();*/

        }

        //sending the csv data from the csvHandler to the FG.
        public void sendLines()
        {
            string line;
            while (running_line < csv_handler.getRowCount())
            {
                line = csv_handler.getLine(running_line);
                line += "\r\n";
                ns.Write(System.Text.Encoding.ASCII.GetBytes(line));
                ns.Flush();
                Thread.Sleep(10);
                running_line++;
                Trace.WriteLine(line);
            }
        }

        //closing all the connections
        public void disconnect()
        {
            ns.Close();
            client.Close();
        }

    }
}
