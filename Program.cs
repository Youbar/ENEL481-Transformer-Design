using System;
using System.IO;
using System.Xml.Linq;


namespace ENEL481_Transformer_Design
{
    class Program
    {
        double CalculateConductivity(float resistivity)
        {
            double conductivity = (1 / resistivity);
            return conductivity;
        }

        double CalculateResistivityByResistance(float resistance, float area, float length)
        {
            double resistivity = (resistance * area) / length;
            return resistivity;
        }

        double CalculateResistivityByTemperature(float resistivityAt20C, float thermalCoeff, float temperature)
        {
            double resistivity = resistivityAt20C * (1 + thermalCoeff * (temperature - 20));
            return resistivity;
        }

        double CalculateCapacitance(float absPerm, float relPerm, float area, float distance)
        {
            double capacitance = (absPerm * relPerm * area) / distance;
            return capacitance;
        }

        double CalculateMagneticFluxDensity(float magneticField, float relPerm = 1)
        {
            double vacuumPerm = 4E-7 * Math.PI;
            double fluxDensity = relPerm * vacuumPerm * magneticField;
            return fluxDensity;
        }

        static void Main(string[] args) // Expand upon this with https://docs.microsoft.com/en-us/dotnet/standard/linq/linq-xml-overview
        {
            // Uncomment below when converting to executable
            // var filename = "TransformerData.xml";
            // var currentDirectory = Directory.GetCurrentDirectory();
            // var transformerFilepath = Path.Combine(currentDirectory, filename);
            // XElement transformer = XElement.Load(transformerFilepath);

            var transformerFilepath = @"C:\Users\kentt\source\repos\ENEL481 Transformer Design\TransformerData.xml";
            XElement transformer = XElement.Load(transformerFilepath);

            Console.WriteLine(transformer.Element("specifications").Element("title"));
        }
    }
}
