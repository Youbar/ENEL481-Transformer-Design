using System;
using System.IO;
using System.Xml.Linq;
using System.Numerics;

namespace ENEL481_Transformer_Design
{
    class Program
    {
        public static class Constants
        {
            public const double vacuumPerm = 4E-7 * Math.PI;
        }

        public class TransformerModel
        {
            public XMLValues _XMLValues = new XMLValues();
            public double angularFreq;
            public double currentRatingPrimary;
            public double currentRatingSecondary;
            public double windingConductorAreaPrimary;
            public double windingConductorAreaSecondary;
            public double voltagePerTurnPrimary;
            public double voltagePerTurnSecondary;
            public double primaryTurns;
            public double secondaryTurns;
            public double turnsRatio;
            public double coreArea;
            public double grossCoreArea;
            public double windingMetalAreaPrimary;
            public double windingMetalAreaSecondary;
            public double windingWindowAreaPrimary;
            public double windingWindowAreaSecondary;
            public double windowWidth;
            public double windowHeight;
            public double coreFluxPathLength;
            public double coreReluctance;
            public double coreMagnetisingReactance;
            public double coreResistivityAtOpTemp;
            public double coreSkinDepth;
            public double coreEddyCurrentResistance;
            public double coreEddyCurrentResistancePrimary;
            public double hysteresisPowerLoss;
            public double coreHysteresisLossResistance;
            public double totalCoreLossResistance;
            public double insideWindingResistivityAtOpTemp;
            public double insideWindingLengthPerTurn;
            public double insideWindingLengthTotal;
            public double insideWindingResistance;
            public double outsideWindingResistivityAtOpTemp;
            public double outsideWindingLengthPerTurn;
            public double outsideWindingLengthTotal;
            public double outsideWindingResistance;
            public double totalWindingReactance;
            public double primaryWindingReactance;
            public double secondaryWindingReactance;
            public double coreWeight;

            public class XMLValues
            {
                public double frequency { get; set; }
                public double ratedPower { get; set; }
                public double insideVoltage { get; set; }
                public double outsideVoltage { get; set; }
                public double currentDensityPrimary { get; set; }
                public double currentDensitySecondary { get; set; }
                public double voltagePerTurnFactorPrimary { get; set; }
                public double voltagePerTurnFactorSecondary { get; set; }
                public double maxFluxDensity { get; set; }
                public double stackingFactor { get; set; }
                public double spaceFactorPrimary { get; set; }
                public double spaceFactorSecondary { get; set; }
                public double windowWidthFactor { get; set; }
                public double relativePermeabilityCore { get; set; }
                public double operatingTemp { get; set; }
                public double coreThermalResistivityCoeff { get; set; }
                public double coreResistivityAt20C { get; set; }
                public double coreLaminationThickness { get; set; }
                public double hysteresisLossConstant { get; set; }
                public double fluxDensityKneePoint { get; set; }
                public double steinmetzFactor { get; set; }
                public double coreWeight { get; set; }
                public double insideWindingThermalResistivityCoeff { get; set; }
                public double insideWindingResistivityAt20C { get; set; }
                public double outsideWindingThermalResistivityCoeff { get; set; }
                public double outsideWindingResistivityAt20C { get; set; }
                public double materialDensity { get; set; }

            }

            public void calculateAngularFreq(XElement transformer)
            {
                _XMLValues.frequency = Convert.ToDouble(transformer.Element("supply").Element("frequency").Value);
                angularFreq = 2 * Math.PI * _XMLValues.frequency;
            }

            public void calculateCurrentRating(XElement transformer)
            {
                _XMLValues.ratedPower = Convert.ToDouble(transformer.Element("supply").Element("ratedPower").Value);
                _XMLValues.insideVoltage = Convert.ToDouble(transformer.Element("supply").Element("insideVoltage").Value);
                _XMLValues.outsideVoltage = Convert.ToDouble(transformer.Element("supply").Element("outsideVoltage").Value);
                currentRatingPrimary = _XMLValues.ratedPower / _XMLValues.insideVoltage;
                currentRatingSecondary = _XMLValues.ratedPower / _XMLValues.outsideVoltage;
            }

            public void calculateWindingConductorArea(XElement transformer)
            {
                _XMLValues.currentDensityPrimary = Convert.ToDouble(transformer.Element("insideWinding").Element("currentDensity").Value);
                _XMLValues.currentDensitySecondary = Convert.ToDouble(transformer.Element("insideWinding").Element("currentDensity").Value);
                windingConductorAreaPrimary = currentRatingPrimary / _XMLValues.currentDensityPrimary;
                windingConductorAreaSecondary = currentRatingSecondary / _XMLValues.currentDensitySecondary;
            }

            public void calculateVoltagePerTurn(XElement transformer)
            {
                _XMLValues.voltagePerTurnFactorPrimary = Convert.ToDouble(transformer.Element("insideWinding").Element("voltagePerTurnFactor").Value);
                _XMLValues.voltagePerTurnFactorSecondary = Convert.ToDouble(transformer.Element("outsideWinding").Element("voltagePerTurnFactor").Value);
                voltagePerTurnPrimary = Math.Sqrt(_XMLValues.ratedPower) / _XMLValues.voltagePerTurnFactorPrimary;
                voltagePerTurnSecondary = Math.Sqrt(_XMLValues.ratedPower) / _XMLValues.voltagePerTurnFactorSecondary;
            }

            public void calculateTurns(XElement transformer)
            {
                primaryTurns = _XMLValues.insideVoltage / voltagePerTurnPrimary;
                secondaryTurns = _XMLValues.outsideVoltage / voltagePerTurnSecondary;
                turnsRatio = primaryTurns / secondaryTurns;
            }

            public void calculateCoreArea(XElement transformer)
            {
                _XMLValues.maxFluxDensity = Convert.ToDouble(transformer.Element("core").Element("peakFluxDensity").Value);
                coreArea = _XMLValues.insideVoltage / (Math.Sqrt(2) * Math.PI * _XMLValues.frequency * primaryTurns * _XMLValues.maxFluxDensity);
                _XMLValues.stackingFactor = Convert.ToDouble(transformer.Element("core").Element("stackingFactor").Value);
                grossCoreArea = coreArea / _XMLValues.stackingFactor;
            }

            public void calculateWindingMetalArea(XElement transformer)
            {
                windingMetalAreaPrimary = primaryTurns * windingConductorAreaPrimary;
                windingMetalAreaSecondary = secondaryTurns * windingConductorAreaSecondary;
            }

            public void calculateWindingWindowArea(XElement transformer)
            {
                _XMLValues.spaceFactorPrimary = Convert.ToDouble(transformer.Element("insideWinding").Element("spaceFactor").Value);
                _XMLValues.spaceFactorSecondary = Convert.ToDouble(transformer.Element("outsideWinding").Element("spaceFactor").Value);
                windingWindowAreaPrimary = windingMetalAreaPrimary / _XMLValues.spaceFactorPrimary;
                windingWindowAreaSecondary = windingMetalAreaSecondary / _XMLValues.spaceFactorSecondary;
            }

            public void calculateWindowDimensions(XElement transformer)
            {
                _XMLValues.windowWidthFactor = Convert.ToDouble(transformer.Element("core").Element("windowWidthFactor").Value);
                windowWidth = Math.Sqrt((windingWindowAreaPrimary + windingWindowAreaSecondary) / _XMLValues.windowWidthFactor);
                windowHeight = _XMLValues.windowWidthFactor * windowWidth;
                coreFluxPathLength = 2 * (windowWidth + windowHeight);
            }

            public void calculateCoreReluctance(XElement transformer)
            {
                _XMLValues.relativePermeabilityCore = Convert.ToDouble(transformer.Element("core").Element("relativePermeability").Value);
                _XMLValues.materialDensity = Convert.ToDouble(transformer.Element("core").Element("materialDensity").Value);
                coreReluctance = coreFluxPathLength / (Constants.vacuumPerm * _XMLValues.relativePermeabilityCore * coreArea);
                coreWeight = coreArea * _XMLValues.materialDensity * coreFluxPathLength;
            }

            public void calculateCoreMagnetisingReactance(XElement transformer)
            {
                coreMagnetisingReactance = (angularFreq * primaryTurns * primaryTurns) / coreReluctance;
            }

            public void calculateEddyCurrentLossResistance(XElement transformer)
            {
                _XMLValues.operatingTemp = Convert.ToDouble(transformer.Element("core").Element("operatingTemp").Value);
                _XMLValues.coreThermalResistivityCoeff = Convert.ToDouble(transformer.Element("core").Element("thermalResistivityCoeff").Value);
                _XMLValues.coreResistivityAt20C = Convert.ToDouble(transformer.Element("core").Element("resistivityAt20C").Value);
                _XMLValues.coreLaminationThickness = Convert.ToDouble(transformer.Element("core").Element("laminationThickness").Value);
                coreResistivityAtOpTemp = (1 + _XMLValues.coreThermalResistivityCoeff * (_XMLValues.operatingTemp - 20)) * _XMLValues.coreResistivityAt20C;
                coreSkinDepth = Math.Sqrt((2 * coreResistivityAtOpTemp) / (Constants.vacuumPerm * _XMLValues.relativePermeabilityCore * angularFreq));

                if (coreSkinDepth < (_XMLValues.coreLaminationThickness * 0.5))
                {
                    coreEddyCurrentResistance = (12 * coreResistivityAtOpTemp * coreArea) / (4 * coreFluxPathLength * coreSkinDepth * coreSkinDepth);
                }

                else
                {
                    coreEddyCurrentResistance = (12 * coreResistivityAtOpTemp * coreArea) / (coreFluxPathLength * _XMLValues.coreLaminationThickness * _XMLValues.coreLaminationThickness);
                }

                coreEddyCurrentResistancePrimary = coreEddyCurrentResistance * primaryTurns * primaryTurns;
            }

            public void calculateHysteresisLossResistance(XElement transformer)
            {
                _XMLValues.hysteresisLossConstant = Convert.ToDouble(transformer.Element("core").Element("hysterisisLossConstant").Value);
                _XMLValues.fluxDensityKneePoint = Convert.ToDouble(transformer.Element("core").Element("peakFluxDensity").Value);
                _XMLValues.steinmetzFactor = Convert.ToDouble(transformer.Element("core").Element("steinmetzFactor").Value);
                _XMLValues.coreWeight = Convert.ToDouble(transformer.Element("core").Element("weight").Value);
                hysteresisPowerLoss = _XMLValues.hysteresisLossConstant * _XMLValues.frequency * Math.Pow(_XMLValues.fluxDensityKneePoint, _XMLValues.steinmetzFactor) * _XMLValues.insideVoltage;
                coreHysteresisLossResistance = _XMLValues.insideVoltage * _XMLValues.insideVoltage / (Math.PI * hysteresisPowerLoss);
                totalCoreLossResistance = (coreEddyCurrentResistancePrimary * coreHysteresisLossResistance) / (coreEddyCurrentResistancePrimary + coreHysteresisLossResistance);
            }

            public void calculateInsideWindingResistance(XElement transformer)
            {
                _XMLValues.insideWindingThermalResistivityCoeff = Convert.ToDouble(transformer.Element("insideWinding").Element("thermalResistivityCoeff").Value);
                _XMLValues.insideWindingResistivityAt20C = Convert.ToDouble(transformer.Element("insideWinding").Element("resistivityAt20C").Value);
                insideWindingResistivityAtOpTemp = (1 + _XMLValues.insideWindingThermalResistivityCoeff * (_XMLValues.operatingTemp - 20)) * _XMLValues.insideWindingResistivityAt20C;
                insideWindingLengthPerTurn = Math.PI * (windowWidth / 2);
                insideWindingLengthTotal = primaryTurns * insideWindingLengthPerTurn;
                insideWindingResistance = (insideWindingResistivityAtOpTemp * insideWindingLengthTotal / windingConductorAreaPrimary);
            }

            public void calculateOutsideWindingResistance(XElement transformer)
            {
                _XMLValues.outsideWindingThermalResistivityCoeff = Convert.ToDouble(transformer.Element("outsideWinding").Element("thermalResistivityCoeff").Value);
                _XMLValues.outsideWindingResistivityAt20C = Convert.ToDouble(transformer.Element("outsideWinding").Element("resistivityAt20C").Value);
                outsideWindingResistivityAtOpTemp = (1 + _XMLValues.outsideWindingThermalResistivityCoeff * (_XMLValues.operatingTemp - 20)) * _XMLValues.outsideWindingResistivityAt20C;
                outsideWindingLengthPerTurn = Math.PI * (3 * windowWidth / 2);
                outsideWindingLengthTotal = secondaryTurns * outsideWindingLengthPerTurn;
                outsideWindingResistance = (outsideWindingResistivityAtOpTemp * outsideWindingLengthTotal / windingConductorAreaSecondary);
            }

            public void calculateTotalWindingReactance(XElement transformer)
            {
                totalWindingReactance = (angularFreq * Math.PI * Constants.vacuumPerm * primaryTurns * primaryTurns * windowWidth * windowWidth) / (3 * windowHeight);
                primaryWindingReactance = totalWindingReactance / 2;
                secondaryWindingReactance = totalWindingReactance / 2;
            }

            public void calculateParameters(XElement transformer)
            {
                calculateAngularFreq(transformer);
                calculateCurrentRating(transformer);
                calculateWindingConductorArea(transformer);
                calculateVoltagePerTurn(transformer);
                calculateTurns(transformer);
                calculateCoreArea(transformer);
                calculateWindingMetalArea(transformer);
                calculateWindingWindowArea(transformer);
                calculateWindowDimensions(transformer);
                calculateCoreReluctance(transformer);
                calculateCoreMagnetisingReactance(transformer);
                calculateEddyCurrentLossResistance(transformer);
                calculateHysteresisLossResistance(transformer);
                calculateInsideWindingResistance(transformer);
                calculateOutsideWindingResistance(transformer);
                calculateTotalWindingReactance(transformer);
                Console.WriteLine("============ OPEN SPECS LOG ============");
                Console.WriteLine("Core Area:" + coreArea / 10);
                Console.WriteLine("Gross Core Area:" + grossCoreArea);
                Console.WriteLine("Core Weight:" + coreWeight);
                Console.WriteLine("Winding Metal Area:" + windingMetalAreaPrimary);
                Console.WriteLine("Gross Winding Metal Area:" + windingConductorAreaPrimary);
                Console.WriteLine("Primary Winding Weight:" + windingMetalAreaPrimary * coreFluxPathLength);
                Console.WriteLine("Secondary Winding Weight:" + windingMetalAreaPrimary * coreFluxPathLength);
                Console.WriteLine("Total Weight:" + (windingMetalAreaPrimary * coreFluxPathLength + windingMetalAreaPrimary * coreFluxPathLength + coreWeight));
                Console.WriteLine("Window Width:" + windowWidth);
                Console.WriteLine("Window Height:" + windowHeight);
                Console.WriteLine("Primary Turns:" + primaryTurns);
                Console.WriteLine("Secondary Turns:" + secondaryTurns);
                Console.WriteLine("============ END SPECS LOG ============");
                Console.WriteLine("============ OPEN PARAMS LOG ============");
                Console.WriteLine("Core Eddy Current Resistance:" + coreEddyCurrentResistancePrimary);
                Console.WriteLine("Core Hysteresis Resistance:" + coreHysteresisLossResistance);
                Console.WriteLine("Core Loss Resistance:" + totalCoreLossResistance);
                Console.WriteLine("Core Magnetizing Reactance:" + coreMagnetisingReactance);
                Console.WriteLine("Inside Winding Resistance:" + insideWindingResistance);
                Console.WriteLine("Inside Winding Leakage Reactance:" + primaryWindingReactance);
                Console.WriteLine("Outside Winding Resistance:" + outsideWindingResistance);
                Console.WriteLine("Outside Winding Leakage Reactance:" + secondaryWindingReactance);
                Console.WriteLine("============ END PARAMS LOG ============");
            }

            public void openCircuitTest()
            {
                Complex primaryImpedance = new Complex(insideWindingResistance, primaryWindingReactance);
                Complex totalCoreLossResistanceConv = new Complex(totalCoreLossResistance, 0);
                Complex coreMagnetisingReactanceConv = new Complex(0, coreMagnetisingReactance);
                Complex coreImpedance = (coreMagnetisingReactanceConv * totalCoreLossResistanceConv) / (coreMagnetisingReactanceConv + totalCoreLossResistanceConv);
                Complex impedanceTotal = primaryImpedance + coreImpedance;
                double voltageSecondary = ((coreImpedance * _XMLValues.outsideVoltage) / (primaryImpedance + coreImpedance)).Magnitude;
                double outputCurrent = (_XMLValues.outsideVoltage / coreImpedance.Magnitude);
                double powerFactor = Math.Cos((_XMLValues.insideVoltage / impedanceTotal).Phase);
                double powerTotal = _XMLValues.insideVoltage * outputCurrent;
                double powerReal = powerFactor * powerTotal;
                Console.WriteLine("============ START OC TEST LOG ============");
                Console.WriteLine("Primary Voltage:" + _XMLValues.insideVoltage);
                Console.WriteLine("Secondary Voltage:" + voltageSecondary);
                Console.WriteLine("Current:" + outputCurrent);
                Console.WriteLine("Volt-Amperes:" + powerTotal);
                Console.WriteLine("Real Power:" + powerReal);
                Console.WriteLine("Power Factor:" + powerFactor);
                Console.WriteLine("============ END OC TEST LOG ============");
            }

            public void shortCircuitTest()
            {
                Complex primaryImpedance = new Complex(insideWindingResistance, primaryWindingReactance);
                Complex totalCoreLossResistanceConv = new Complex(totalCoreLossResistance, 0);
                Complex coreMagnetisingReactanceConv = new Complex(0, coreMagnetisingReactance);
                Complex coreImpedance = (coreMagnetisingReactanceConv * totalCoreLossResistanceConv) / (coreMagnetisingReactanceConv + totalCoreLossResistanceConv);
                Complex secondaryImpedance = new Complex(outsideWindingResistance, secondaryWindingReactance);
                Complex impedanceTotal = primaryImpedance + (coreImpedance * secondaryImpedance) / (coreImpedance + secondaryImpedance);
                double outputCurrent = _XMLValues.ratedPower / _XMLValues.insideVoltage;
                double outputVoltage = outputCurrent * impedanceTotal.Magnitude;
                double powerTotal = outputVoltage * outputCurrent;
                double powerFactor = Math.Cos((outputCurrent * impedanceTotal).Phase);
                double powerReal = powerFactor * powerTotal;
                powerFactor = powerReal / powerTotal;
                Console.WriteLine("============ START SC TEST LOG ============");
                Console.WriteLine("Voltage:" + outputVoltage);
                Console.WriteLine("Current:" + outputCurrent);
                Console.WriteLine("Volt-Amperes:" + powerTotal);
                Console.WriteLine("Real Power:" + powerReal);
                Console.WriteLine("Power Factor:" + powerFactor);
                Console.WriteLine("============ END SC TEST LOG ============");
            }

            public void loadedCircuitTest()
            {
                double loadResistance = _XMLValues.insideVoltage * _XMLValues.insideVoltage / _XMLValues.ratedPower;
                Complex primaryImpedance = new Complex(insideWindingResistance, primaryWindingReactance);
                Complex totalCoreLossResistanceConv = new Complex(totalCoreLossResistance, 0);
                Complex coreMagnetisingReactanceConv = new Complex(0, coreMagnetisingReactance);
                Complex coreImpedance = (coreMagnetisingReactanceConv * totalCoreLossResistanceConv) / (coreMagnetisingReactanceConv + totalCoreLossResistanceConv);
                Complex secondaryImpedance = new Complex(outsideWindingResistance, secondaryWindingReactance);
                Complex loadImpedance = new Complex(loadResistance * turnsRatio * turnsRatio, 0);
                Complex totalImpedance = primaryImpedance + (coreImpedance * (secondaryImpedance + loadImpedance)) / (coreImpedance + (secondaryImpedance + loadImpedance));
                double outputCurrent = (_XMLValues.insideVoltage / totalImpedance.Magnitude);
                double powerFactor = Math.Cos((_XMLValues.insideVoltage / totalImpedance).Phase);
                double powerTotal = _XMLValues.insideVoltage * outputCurrent;
                double powerReal = powerFactor * powerTotal;
                powerFactor = powerReal / powerTotal;
                Console.WriteLine("============ START UPF LOAD TEST LOG ============");
                Console.WriteLine("Voltage:" + _XMLValues.insideVoltage);
                Console.WriteLine("Current:" + (outputCurrent));
                Console.WriteLine("Volt-Amperes:" + powerTotal);
                Console.WriteLine("Real Power:" + powerReal);
                Console.WriteLine("Power Factor:" + powerFactor);
                Console.WriteLine("============ END UPF LOAD TEST LOG ============");
            }
        }

        static void Main(string[] args) // Expand upon this with https://docs.microsoft.com/en-us/dotnet/standard/linq/linq-xml-overview
        {
            // Uncomment below when converting to executable
            // var filename = "TransformerData.xml";
            // var currentDirectory = Directory.GetCurrentDirectory();
            // var transformerFilepath = Path.Combine(currentDirectory, filename);
            // XElement transformer = XElement.Load(transformerFilepath);

            var transformerFilepath = @"G:\Downloads\UC\ENEL481\ENEL481-Transformer-Design\TransformerData.xml";
            XElement transformer = XElement.Load(transformerFilepath);
            XElement transAbbrev = transformer.Element("model");

            TransformerModel _transformerModel = new TransformerModel();

            Console.WriteLine(transformer.Element("specifications").Element("title").Value);
            _transformerModel.calculateParameters(transAbbrev);
            _transformerModel.openCircuitTest();
            _transformerModel.shortCircuitTest();
            _transformerModel.loadedCircuitTest();
        }
    }
}
