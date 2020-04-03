using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace DeploymentHelper
{
    public class DeploymentStep
    {
        public EDeploymentStepsType Type { get; }
        public List<Step> Steps { get; private set; }

        public DeploymentStep(XmlElement stepsNode)
        {
            Type = (stepsNode.GetAttribute("type")) switch
            {
                "qt-cpp" => EDeploymentStepsType.QT_CPP,
                "cs" => EDeploymentStepsType.CS,
                _ => EDeploymentStepsType.NONE,
            };

            Console.WriteLine($"New step-list added of type {Type}");

            Steps = new List<Step>();
            foreach (var step in stepsNode.ChildNodes)
            {
                if (step.GetType().Equals(typeof(XmlElement)))
                    Steps.Add(new Step((XmlElement)step));
                else
                    Console.WriteLine($"Different step type found: {stepsNode.GetType()}");
            }
        }
    }
}