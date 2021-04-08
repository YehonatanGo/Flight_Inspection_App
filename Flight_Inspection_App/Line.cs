﻿using OxyPlot;
using System;

namespace Flight_Inspection_App
{
    class Line
    {
        private float a, b;
        public float A { get { return a; } set { a = value; } }
        public float B { get { return b; } set { b = value; } }
        public Line(float a, float b) {
            this.a = a;
            this.b = b;
        }

        public Line()
        {
            a = 0;
            b = 0;
        }

        public float f(float x) {
            return a * x + b;
        }
    }
}
