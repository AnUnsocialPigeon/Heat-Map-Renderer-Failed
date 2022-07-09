using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heat_Map_Renderer {
    internal class TemperatureNodes {
        public TemperatureNodes() {
            Temperatures = new List<TemperatureNode>();
        }
        public TemperatureNodes(IEnumerable<TemperatureNode> nodes) {
            Temperatures = nodes;
        }


        public void AddTemperatureNode(int x, int y, double temp) {
            AddTemperatureNode(new TemperatureNode(x, y, temp));
        }
        public void AddTemperatureNode(TemperatureNode node) {
            var t = Temperatures.ToList();
            t.Add(node);
            Temperatures = t;
        }
        public void AddRandomTemperatureNodes(int count, double temp_Low, double temp_High, int x_Max, int y_Max) {
            for (int i = 0; i < count; i++) {
                int x = random.Next(x_Max);
                int y = random.Next(y_Max);
                if (FindNode(x, y) == null) {
                    AddTemperatureNode(x, y, (random.NextDouble() * temp_High) + temp_Low);
                }
            }
        }

        public double AverageTemperature() {
            return Temperatures.Average(x => x.Temperature);
        }

        public TemperatureNode? FindNode(int x, int y) {
            foreach (TemperatureNode node in Temperatures) {
                if (node.X == x && node.Y == y) return node;
            }
            return null;
        }

        public IEnumerable<TemperatureNode> Temperatures { get; set; }
        private readonly Random random = new();
    }


    internal class TemperatureNode {
        public TemperatureNode() { }
        public TemperatureNode(int x, int y, double temp) {
            X = x;
            Y = y;
            Temperature = temp;
        }

        public int X;
        public int Y;
        public double Temperature;

    }

    internal class NodeMaths {
        public static double GetDistance(TemperatureNode node, int x, int y) => GetDistance(node.X, x, node.Y, y);
        public static double GetDistance(int x1, int x2, int y1, int y2) {
            return Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
        }

        public static Color GetColour(double c, double min, double max) {
            double range = max - min;
            double val = (c - min) / range;

            int r = Math.Clamp((int)(Math.Sin((Math.Pow(Math.Log2(val * 1000), 0.07f) * 1.57f)) * 255), 0, 255);
            //int r = Math.Clamp((int)(Math.Cos(val * 1.57f) * 255), 0, 255);
            int b = Math.Clamp((int)(Math.Cos(Math.Cos(val * 1.57f) / val) * 255), 0, 255);
            //int g = Math.Clamp((int)((0.4f * Math.Cos(Math.Abs((r - (val * b)) / 2f)) + 0.2f) * 128), 0, 255);
            int g = Math.Clamp((int)(Math.Tan((b * Math.PI / 2) - (Math.PI / 4))), 0, 255);

            return Color.FromArgb(255, r, g, b);
        }

        public static (double, double) GetMinMax(double[,] data) {
            double min = data[0, 0];
            double max = data[0, 0];
            foreach (var item in data) {
                if (item < min) { min = item; }
                if (item > max) { max = item; }
            }
            return (min, max);
        }
    }
}
