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
        volatile private int sleepTime;


        private double elevator;
        public double Elevator
        {
            get
            {
                return this.elevator;
            }
            set
            {

                this.elevator = value;
                NotifyPropertyChanged("Elevator");
            }
        }

        private double aileron;

        public double Aileron
        {
            get
            {
                return this.aileron;
            }
            set
            {
                this.aileron = value;
                NotifyPropertyChanged("Aileron");
            }
        }

        private double rudder;

        public double Rudder
        {
            get
            {
                return this.rudder;
            }
            set
            {
                this.rudder = value;
                NotifyPropertyChanged("Rudder");
            }
        }

        private double throttle;
        public double Throttle
        {
            get
            {
                return this.throttle;
            }
            set
            {
                this.throttle = value;
                NotifyPropertyChanged("Throttle");
            }
        }

        ManualResetEvent mre = new ManualResetEvent(true);

        private string path;
        public string Path
        {
            set
            {
                path = value;
                NotifyPropertyChanged("Path");
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
                if (value < 0)
                {
                    value = 0;
                }
                if (value > csv_handler.getRowCount())
                {
                    value = csv_handler.getRowCount();
                }
                running_line = value;
                NotifyPropertyChanged("Running_line");
            }
            get
            {
                return running_line;
            }
        }

        volatile private bool play;
        public bool Play
        {
            get
            {
                return play;
            }
            set
            {
                play = value;

                if (play)
                {
                    // first time played is pressed - start the sending thread
                    if (isAlreadyStarted == false)
                    {
                        isAlreadyStarted = true;
                        start();

                    }
                    // play after pausing - wake up sending thread
                    else
                    {
                        mre.Set();
                    }
                }
                else
                // play = false - put sending thread asleep
                {
                    mre.Reset();
                }
            }
        }

        private double playSpeed;
        public double PlaySpeed
        {
            get
            {
                return playSpeed;
            }
            set
            {
                playSpeed = value;
                sleepTime = (int)((1 / playSpeed) * 160);
            }
        }

        private int numOfLines;
        public int NumOfLines { get { return numOfLines; } set { numOfLines = value; } }

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        public myClientModel()
        {
            Path = "";
            running_line = 0;
            PlaySpeed = 1;
            elevator = 125;
            aileron = 125;
        }

        public void connectFlightGear()
        {
            csv_handler = new CsvHandler(path);
            numOfLines = csv_handler.getRowCount();

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

        }

        //sending the csv data from the csvHandler to the FG.
        public void sendLines()
        {
            double current_aileron_value, current_elevator_value;
            string line;
            while (running_line < csv_handler.getRowCount())
            {
                // wait fot pause/play signal
                mre.WaitOne();
                line = csv_handler.getLine(running_line);
                double[] doubles = System.Array.ConvertAll(line.Split(','), double.Parse);
                current_aileron_value = doubles[0];
                current_elevator_value = doubles[1];
                calculateAileron(current_aileron_value);
                calculateElevator(current_elevator_value);
                line += "\r\n";
                ns.Write(System.Text.Encoding.ASCII.GetBytes(line));
                ns.Flush();
                Thread.Sleep(sleepTime);
                RunningLine++;
            }
        }

        private void calculateAileron(double current)
        {
            Aileron = current * 60 + 125;
        }

        private void calculateElevator(double current)
        {
            Elevator = current * 60 + 125;
        }

        //closing all the connections
        public void disconnect()
        {
            ns.Close();
            client.Close();
        }

        // return to the begining of the flight & pause
        public void stop()
        {
            RunningLine = 0;
            // send first line to bring the picture back to the begining
            sendOneLine();
            Play = false;
        }

        public void sendOneLine()
        {
            string line = csv_handler.getLine(running_line);
            line += "\r\n";
            ns.Write(System.Text.Encoding.ASCII.GetBytes(line));
            ns.Flush();
            Thread.Sleep(sleepTime);
        }
    }
}
