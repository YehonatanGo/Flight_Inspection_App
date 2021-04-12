using OxyPlot;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading;
using System.Runtime.InteropServices;
using System;
using System.IO;
using System.Linq;
using OxyPlot.Wpf;
using System.Reflection;

namespace Flight_Inspection_App
{
    public class myClientModel : IClientModel
    {

        public event PropertyChangedEventHandler PropertyChanged;
        ManualResetEvent manualResetEvent;
        volatile private CsvHandler csv_handler;
        volatile private NetworkStream ns;
        volatile private TcpClient client;
        volatile private Thread sending_lines_thread;
        volatile private bool isAlreadyStarted;


        // *********************dashboard controller*********************
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

        private double yaw;
        public double Yaw
        {
            get
            {
                return this.yaw;
            }
            set
            {
                this.yaw = value;
                NotifyPropertyChanged("Yaw");
            }
        }

        private double roll;
        public double Roll
        {
            get
            {
                return this.roll;
            }
            set
            {
                this.roll = value;
                NotifyPropertyChanged("Roll");
            }
        }

        private double pitch;
        public double Pitch
        {
            get
            {
                return this.pitch;
            }
            set
            {
                this.pitch = value;
                NotifyPropertyChanged("Pitch");
            }
        }

        // **************************************************************


        private string testPath;
        public string TestPath
        {
            set
            {
                testPath = value;
                NotifyPropertyChanged("TestPath");
            }
            get
            {
                return testPath;
            }
        }

        private string trainPath;
        public string TrainPath
        {
            get
            {
                return trainPath;
            }
            set
            {
                trainPath = value;
                NotifyPropertyChanged("trainPath");
            }
        }

        private string dllPath;
        public string DllPath
        {
            get => dllPath;
            set
            {
                dllPath = value;
            }
        }
        // *********************playing controller*********************

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
                if (Math.Abs(value) > 1 && !play)
                {
                    sendOneLine();
                }
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
        volatile private int sleepTime;
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

        // **************************************************************


        // ************************graphs controller*********************
        private string displayedFeature;
        public string DisplayedFeature
        {
            get { return displayedFeature; }
            set
            {
                this.displayedFeature = value;
                CorrelatedFeature = getCorrealtedFeature(displayedFeature);
                if (isAlreadyStarted && !Play)
                {
                    sendOneLine();
                }
                NotifyPropertyChanged("displayedFeature");

                string name = displayedFeature + "+" + correlatedFeature;
                if (anomaliesPlot != null)
                {
                    Annotation annotation = (Annotation)getAnnotation.Invoke(anomalyDetector, new object[] { name });
                    if (anomaliesPlot.Annotations.Count > 1)
                    {
                        anomaliesPlot.Annotations.RemoveAt(anomaliesPlot.Annotations.Count - 1);
                    }
                    anomaliesPlot.Annotations.Add(annotation);
                }

                if (anomalyDetector != null)
                {

                    List<DataPoint> anmlsPts = (List<DataPoint>)getAnomalies.Invoke(anomalyDetector, new object[] { name });
                    AnomaliesPoints = anmlsPts;
                }

                if (getAnomaliesTS != null)
                {
                    List<int> anomaliesTS = (List<int>)getAnomaliesTS.Invoke(anomalyDetector, new object[] { name });
                    AnomaliesTSList = anomaliesTS;
                }
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

        private string correlatedFeature;
        public string CorrelatedFeature
        {
            get { return correlatedFeature; }
            set
            {
                correlatedFeature = value;
                NotifyPropertyChanged("CorrelatedFeature");
            }
        }


        private volatile List<DataPoint> correlatedDataPoints;
        public List<DataPoint> CorrelatedDataPoints
        {
            get { return correlatedDataPoints; }
            set
            {
                this.correlatedDataPoints = value;
                NotifyPropertyChanged("correlatedDataPoints");
            }
        }

        private List<string> featuresList;
        public List<string> FeaturesList
        {
            get { return featuresList; }
            set
            {
                this.featuresList = value;
                NotifyPropertyChanged("featuresList");
            }
        }

        private volatile Dictionary<string, string> correlatedFeatures;

        private Dictionary<string, Line> linearRegressions;

        private Line linearRegression;

        public Line LinearRegression
        {
            set
            {
                this.linearRegression = value;
                NotifyPropertyChanged("LineRegA");
                NotifyPropertyChanged("LineRegB");
            }
        }

        public float LineRegA
        {
            get
            {
                return linearRegression.A;
            }
        }
        public float LineRegB
        {
            get
            {
                return linearRegression.B;
            }
        }

        private List<DataPoint> cfPoints;
        public List<DataPoint> CFPoints
        {
            get => cfPoints;
            set
            {
                cfPoints = value;
                NotifyPropertyChanged("CFPoints");
            }
        }

        private List<DataPoint> lastPoints;
        public List<DataPoint> LastPoints
        {
            get => lastPoints;
            set
            {
                lastPoints = value;
                NotifyPropertyChanged("lastPoints");
            }
        }


        private Plot anomaliesPlot;
        public Plot AnomaliesPlot { get => anomaliesPlot; set { anomaliesPlot = value; } }

        private List<DataPoint> anomaliesPoints;
        public List<DataPoint> AnomaliesPoints
        {
            get => anomaliesPoints;
            set
            {
                anomaliesPoints = value;
                NotifyPropertyChanged("AnomaliesPoints");
            }
        }

        private List<int> anomaliesTSList;
        public List<int> AnomaliesTSList
        {
            get => anomaliesTSList;
            set
            {
                anomaliesTSList = value;
                NotifyPropertyChanged("anomaliesTSList");
            }
        }

        // **************************************************************

        MethodInfo? learnAndDetect;
        MethodInfo? getAnnotation;
        MethodInfo? getAnomalies;
        MethodInfo? getAnomaliesTS;
        object? anomalyDetector;


        public myClientModel()
        {
            TestPath = "";
            running_line = 0;
            PlaySpeed = 1;
            elevator = 125;
            aileron = 125;
            airspeed = 0;
            heading = 0;
            altitude_hundreds = 0;
            altitude_thousands = 0;
            altitude_dozens = 0;
            correlatedFeatures = new Dictionary<string, string>();
            CorrelatedFeature = "";
            DisplayedFeature = "";
            linearRegressions = new Dictionary<string, Line>();
            linearRegression = new Line();
            cfPoints = new List<DataPoint>();
            lastPoints = new List<DataPoint>();
            manualResetEvent = new ManualResetEvent(true);
            anomaliesTSList = new List<int>();
        }

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));

        }

        public void connectFlightGear()
        {
            var assembly = Assembly.LoadFile(DllPath);
            var type2 = assembly.GetType("Anomaly_Detecton_Algorithm.AnomalyDetector");
            anomalyDetector = Activator.CreateInstance(type2);
            learnAndDetect = type2.GetMethod("learnAndDetect");
            learnAndDetect.Invoke(anomalyDetector, new object[]
            {
               trainPath,
               testPath
            });

            getAnnotation = type2.GetMethod("GetAnnotation");
            getAnomalies = type2.GetMethod("getAnomalies");
            getAnomaliesTS = type2.GetMethod("getAnomaliesTimeSteps");




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


            csv_handler = new CsvHandler(testPath);
            numOfLines = csv_handler.getRowCount();
            FeaturesList = csv_handler.getFeaturesNamesList();
            findCorrelatedFeatures(csv_handler.FeaturesDict);
            DisplayedFeature = featuresList[0];

            // wait till FG starts
            Thread.Sleep(10000);

            //setting up a Tcp connection
            this.client = new TcpClient("localhost", 5400);
            this.ns = client.GetStream();
        }

        // creating thread and runs sendLines method.
        public void start()
        {
            correlatedFeature = getCorrealtedFeature(DisplayedFeature);
            sending_lines_thread = new Thread(new ThreadStart(sendLines));
            sending_lines_thread.SetApartmentState(ApartmentState.STA);
            sending_lines_thread.Start();
        }

        //sending the csv data from the csvHandler to the FG.
        public void sendLines()
        {
            string line;
            // TODO: change bound. currently, when reaching the last line flight isn't displayed anymore. Maybe put thread asleep until something happens. 
            while (running_line < numOfLines)
            {
                // wait for pause/play signal if needed
                manualResetEvent.WaitOne();

                line = csv_handler.getLine(running_line);
                updateDashboard();
                updateGraphs();

                // send the line to FG
                line += "\r\n";
                try
                {

                    ns.Write(System.Text.Encoding.ASCII.GetBytes(line));
                }
                catch
                {
                    Console.WriteLine("sending error");
                }
                ns.Flush();
                Thread.Sleep(sleepTime);
                RunningLine++;
            }
        }

        private void updateGraphs()
        {
            bool hasCorrelated = !correlatedFeature.Equals("none");
            updateFeaturesPlots(hasCorrelated);
            updateLinearRegression(hasCorrelated);
        }

        private void updateLinearRegression(bool hasCorrelated)
        {
            Line line_reg = new Line();
            if (hasCorrelated)
            {
                line_reg = findLinearRegression(displayedFeature, correlatedFeature);
            }
            LinearRegression = line_reg;


            // show all data points and update last 30 seconds points to be displayed on linear regression plot (if exists)
            var newCFPointsList = new List<DataPoint>();
            var newLastPoints = new List<DataPoint>();
            if (hasCorrelated)
            {
                for (int i = 0; i <= running_line; i++)
                {
                    if (running_line - i < 300)
                    {
                        newLastPoints.Add(new DataPoint(csv_handler.getFeatureByLine(displayedFeature, i), csv_handler.getFeatureByLine(correlatedFeature, i)));
                    }
                    else
                    {

                        newCFPointsList.Add(new DataPoint(csv_handler.getFeatureByLine(displayedFeature, i), csv_handler.getFeatureByLine(correlatedFeature, i)));
                    }
                }
            }
            CFPoints = newCFPointsList;
            LastPoints = newLastPoints;
        }

        private void updateFeaturesPlots(bool hasCorrelated)
        {
            var dataNewList = new List<DataPoint>();
            var correlatedNewList = new List<DataPoint>();
            // update displayed feature plot, and correlated feature plot (if exists)
            for (int i = 0; i <= running_line; i++)
            {
                float x = csv_handler.getFeatureByLine(displayedFeature, i);
                dataNewList.Add(new DataPoint(i, x));

                if (hasCorrelated)
                {
                    x = csv_handler.getFeatureByLine(correlatedFeature, i);
                    correlatedNewList.Add(new DataPoint(i, x));
                }
            }
            DataPoints = dataNewList;
            CorrelatedDataPoints = correlatedNewList;
        }

        private void updateDashboard()
        {
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

            // update Yaw, Roll, Pitch
            Yaw = csv_handler.getFeatureByLine("side-slip-deg", running_line);
            Roll = csv_handler.getFeatureByLine("roll-deg", running_line);
            Pitch = csv_handler.getFeatureByLine("pitch-deg", running_line);
        }

        private Line findLinearRegression(string f1, string f2)
        {
            return linearRegressions[f1 + "+" + f2];
        }

        private string getCorrealtedFeature(string feature)
        {
            if (correlatedFeatures.ContainsKey(feature))
            {
                return correlatedFeatures[feature];
            }
            // if it's not a key, then it's a (unique) value, return it's key
            return correlatedFeatures.FirstOrDefault(x => x.Value == feature).Key;
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

        // sending one line without incremeting RunningLine, needed when we only want to display current line without further playing
        public void sendOneLine()
        {
            string line = csv_handler.getLine(running_line);
            line += "\r\n";
            ns.Write(System.Text.Encoding.ASCII.GetBytes(line));
            ns.Flush();
            Thread.Sleep(sleepTime);

            updateDashboard();
            updateGraphs();
        }
        public void findCorrelatedFeatures(Dictionary<string, List<float>> features)
        {
            foreach (var feature1 in features)
            {
                float maxCorrelation = 0;
                string mostCorrelated = "none";
                foreach (var feature2 in features)
                {
                    if (feature1.Key.Equals(feature2.Key))
                        continue;

                    float pears = Math.Abs(anomaly_detection_util.pearson(features[feature1.Key], features[feature2.Key], feature1.Value.Count));
                    if (pears >= maxCorrelation)
                    {
                        maxCorrelation = pears;
                        mostCorrelated = feature2.Key;
                    }
                }
                correlatedFeatures.Add(feature1.Key, mostCorrelated);
                if (!mostCorrelated.Equals("none"))
                {
                    Line line_reg = anomaly_detection_util.linear_reg(anomaly_detection_util.toPoints(features[feature1.Key], features[mostCorrelated]), feature1.Value.Count);
                    linearRegressions.Add(feature1.Key + "+" + mostCorrelated, line_reg);
                }
            }
        }
    }
}
