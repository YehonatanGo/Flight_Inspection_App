using OxyPlot;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading;

namespace Flight_Inspection_App
{
    class myClientModel : IClientModel
    {
        public myClientModel()
        {
            Path = "";
            running_line = 0;
            PlaySpeed = 1;
            elevator = 125;
            aileron = 125;
            airspeed = 0;
            heading = 0;
            data_points = new List<DataPoint>
                              {
                                  new DataPoint(0, 4),
                                  new DataPoint(10, 13),
                                  new DataPoint(20, 15),
                                  new DataPoint(30, 16),
                                  new DataPoint(40, 12),
                                  new DataPoint(50, 12)
                              };
        }

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

        private double airspeed;
        public double Airspeed
        {
            get
            {
                return this.airspeed;
            }
            set
            {
                this.airspeed = value;
                NotifyPropertyChanged("Airspeed");
            }
        }

        private double heading;
        public double Heading
        {
            get
            {
                return this.heading;
            }
            set
            {
                this.heading = value;
                NotifyPropertyChanged("Heading");
            }
        }

        ManualResetEvent manualResetEvent = new ManualResetEvent(true);

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
                // check edge cases - less than 0, more than all the rows
                if (value < 0)
                {
                    value = 0;
                }
                if (value > numOfLines)
                {
                    value = numOfLines;
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
                        manualResetEvent.Set();
                    }
                }
                else
                // play = false - put sending thread asleep
                {
                    manualResetEvent.Reset();
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
        public int NumOfLines
        {
            get
            {
                return numOfLines;
            }
            set
            {
                numOfLines = value;
            }
        }

        private List<DataPoint> data_points;
        public List<DataPoint> DataPoints
        {
            get
            {
                return data_points;
            }
            set
            {
                this.data_points = value;
                NotifyPropertyChanged("data_points");
            }
        }

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
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
            string line;
            while (running_line < numOfLines)
            {
                // wait for pause/play signal if needed
                manualResetEvent.WaitOne();
                // send line to FG
                line = csv_handler.getLine(running_line);
                //update the joystick according to Aileron and Elevator values
                calculateAileron(csv_handler.getFeatureByLine("aileron", running_line));
                calculateElevator(csv_handler.getFeatureByLine("elevator", running_line));
                // update throttle and rudder sliders
                Throttle = csv_handler.getFeatureByLine("throttle", running_line);
                Rudder = csv_handler.getFeatureByLine("rudder", running_line);
                //update airspeed radial gauge
                Airspeed = csv_handler.getFeatureByLine("airspeed-kt", running_line);
                //
                Heading = csv_handler.getFeatureByLine("heading-deg", running_line);


                /*List<DataPoint> newList = new List<DataPoint>(data_points);
                newList.Add(new DataPoint(running_line, running_line * 0.5));
                DataPoints = newList;*/

                line += "\r\n";
                ns.Write(System.Text.Encoding.ASCII.GetBytes(line));
                ns.Flush();
                Thread.Sleep(sleepTime);
                RunningLine++;
            }
        }

        /**
         * joystick center value is (125, 125)
         * Aileron contorls X-axis, Elevator controls Y-axis.
         * Both in range [65, 185] - up to 60 from the center.
         * real Aileron and Elevator values are in range [-1,1]
         */

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

        // sending one line, needed when we only want to display current line without further playing
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
