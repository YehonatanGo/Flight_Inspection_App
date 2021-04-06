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
            altitude_hundreds = 0;
            altitude_thousands = 0;
            altitude_dozens = 0;

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

        private double altitude_hundreds;
        public double Altitude_hundreds
        {
            get
            {
                return this.altitude_hundreds;
            }
            set
            {
                this.altitude_hundreds = value;
                NotifyPropertyChanged("Altitude_hundreds");
            }
        }

        private double altitude_thousands;
        public double Altitude_thousands
        {
            get
            {
                return this.altitude_thousands;
            }
            set
            {
                this.altitude_thousands = value;
                NotifyPropertyChanged("Altitude_thousands");
            }
        }

        private double altitude_dozens;
        public double Altitude_dozens
        {
            get
            {
                return this.altitude_dozens;
            }
            set
            {
                this.altitude_dozens = value;
                NotifyPropertyChanged("Altitude_dozens");
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

        // graphs contoroller

        private string displayedFeature;
        public string DisplayedFeature { 
            get { return displayedFeature; }
            set
            {
                this.displayedFeature = value;
            }
        }

        private volatile List<DataPoint> data_points;
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

        private List<string> featuresList;
        public List<string> FeaturesList {
            get { return featuresList; }
            set {
                this.featuresList = value;
                NotifyPropertyChanged("featuresList");
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
            FeaturesList = csv_handler.getFeaturesNamesList();

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
                
                line = csv_handler.getLine(running_line);

                //update the joystick according to Aileron and Elevator values
                CalculateAileron(csv_handler.getFeatureByLine("aileron", running_line));
                CalculateElevator(csv_handler.getFeatureByLine("elevator", running_line));
                // update throttle and rudder sliders
                Throttle = csv_handler.getFeatureByLine("throttle", running_line);
                Rudder = csv_handler.getFeatureByLine("rudder", running_line);
                // update airspeed: radial gauge
                Airspeed = csv_handler.getFeatureByLine("airspeed-kt", running_line);
                // update heading: compass
                Heading = csv_handler.getFeatureByLine("heading-deg", running_line);
                // update altitude: altimeter
                CalculateAltitude(csv_handler.getFeatureByLine("altitude-ft", running_line));

                var newList = new List<DataPoint>();
                for(int i =0; i <= running_line; i++)
                {
                    newList.Add(new DataPoint(i, csv_handler.getFeatureByLine(displayedFeature, i)));
                }
                DataPoints = newList;

                // send the line to FG
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

        private void CalculateAileron(double current)
        {
            Aileron = current * 60 + 125;
        }

        private void CalculateElevator(double current)
        {
            Elevator = current * 60 + 125;
        }

        private void CalculateAltitude(double current)
        {
            Altitude_hundreds = (current % 1000) / 100.0;
            Altitude_thousands = (current % 10000) / 1000.0;
            Altitude_dozens = current / 10000.0;
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
