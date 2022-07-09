using Heat_Map_Renderer;
using System.Drawing;

int width = 1500;
int height = 1500;
int temperatureNodeCount = 200;
int minTemp = 0;
int maxTemp = 100;

Random random = new Random();


Bitmap image = new Bitmap(width, height);
double[,] data = new double[width, height];
TemperatureNodes temperatureNodes = new TemperatureNodes();
temperatureNodes.AddRandomTemperatureNodes(temperatureNodeCount, minTemp, maxTemp, width, height);

for (int y = 0; y < height; y++) {
    Parallel.For(0, width, (x) => {
        //for (int x = 0; x < width; x++) {
        foreach (var node in temperatureNodes.Temperatures) {
            double temp = node.Temperature / (Math.Pow(NodeMaths.GetDistance(node, x, y), 2.5f) + 1);
            data[y, x] += temp;
        }
    });
    Console.Title = $"Progress: {Math.Round(50f * ((float)y / height))}%";
}

(double min, double max) = NodeMaths.GetMinMax(data);

for (int y = 0; y < height; y++) {
    for (int x = 0; x < width; x++) {
        image.SetPixel(x, y, NodeMaths.GetColour(data[y, x], min, max));
        //Console.Write("{0:0.00}, ", data[y, x]);
    }
    Console.Title = $"Progress: {50 + Math.Round(50f * ((float)y / height))}%";
}

image.Save(Directory.GetCurrentDirectory() + @"\Heatmap.png", System.Drawing.Imaging.ImageFormat.Png);
return;
