using System;
using System.Collections.Generic;
using System.Text;

namespace Flight_Inspection_App
{
    class anomaly_detection_util
    {
        public anomaly_detection_util()
        {
            correlatedFeatuers = new Dictionary<string, string>();
        }

        Dictionary<string, string> correlatedFeatuers;

        public static float avg(List<float> x, int size)
        {
            float sum = 0;
            for (int i = 0; i < size; sum += x[i], i++) ;
            return sum / size;
        }

        // returns the variance of X and Y
        public static float var(List<float> x, int size)
        {
            float av = avg(x, size);
            float sum = 0;
            for (int i = 0; i < size; i++)
            {
                sum += x[i] * x[i];
            }
            return sum / size - av * av;
        }

        // returns the covariance of X and Y
        public static float cov(List<float> x, List<float> y, int size)
        {
            float sum = 0;
            for (int i = 0; i < size; i++)
            {
                sum += x[i] * y[i];
            }
            sum /= size;

            return sum - avg(x, size) * avg(y, size);
        }


        // returns the Pearson correlation coefficient of X and Y
        public static float pearson(List<float> x, List<float> y, int size)
        {
            return (float)(cov(x, y, size) / (Math.Sqrt(var(x, size)) * Math.Sqrt(var(y, size))));
        }

        // performs a linear regression and returns the line equation
        public static Line linear_reg(List<Point> points, int size)
        {
            List<float> x = new List<float>();
            List<float> y = new List<float>();
            for (int i = 0; i < size; i++)
            {
                x.Add(points[i].x);
                y.Add(points[i].y);
            }
            float a = cov(x, y, size) / var(x, size);
            float b = avg(y, size) - a * (avg(x, size));

            return new Line(a, b);
        }

        // returns the deviation between point p and the line equation of the points
        float dev(Point p, List<Point> points, int size)
        {
            Line l = linear_reg(points, size);
            return dev(p, l);
        }

        // returns the deviation between point p and the line
        float dev(Point p, Line l)
        {
            return Math.Abs(p.y - l.f(p.x));
        }

        public static List<Point> toPoints(List<float> x, List<float> y)
        {
            List<Point> points = new List<Point>();
            for (int i = 0; i < x.Count; i++)
            {
                points.Add(new Point(x[i], y[i]));
            }
            return points;
        }


        public void findCorrelatedFeatures(Dictionary<string, List<float>> features)
        {
            Dictionary<string, string> correlatedFeatures = new Dictionary<string, string>();
            foreach (var feature1 in features)
            {
                float maxCorrelation = 0;
                string mostCorrelated = "";
                foreach (var feature2 in features)
                {
                    if (feature1.Equals(feature2))
                        continue;

                    float pears = Math.Abs(pearson(features[feature1.Key], features[feature2.Key], feature1.Value.Count));
                    if (pears > maxCorrelation)
                    {
                        maxCorrelation = pears;
                        mostCorrelated = feature2.Key;
                    }
                }
                correlatedFeatuers.Add(feature1.Key, mostCorrelated);
                linear_reg(toPoints(features[feature1.Key], features[mostCorrelated]), feature1.Value.Count);
            }
        }



    }
}
