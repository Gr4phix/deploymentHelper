using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace DeploymentHelper
{
    internal class Program
    {
        private static void Main(string[] args)
        {
#if DEBUG
            if (args.Length == 0)
                args = new string[]
                {
                    Directory.GetCurrentDirectory() + @"/deploy.fsd"
                };
#endif

            if (args.Length != 1)
            {
                PrintErrorAndExit("Please enter a valid deploymentfile-path as argument.");
            }

            var deploymentFilePath = args[0];
            if (!File.Exists(deploymentFilePath))
            {
                PrintErrorAndExit("The entered file does not exist.");
            }

            var deploymentFile = new XmlDocument();

            try
            {
                deploymentFile.Load(deploymentFilePath);
                Temp.DeploymentFilePath = deploymentFilePath;
            }
            catch (Exception e)
            {
                PrintErrorAndExit($"Error while reading the deployment file: {e.Message}.");
            }

            var deploymentNode = deploymentFile.DocumentElement;
            var deploymentStepsNodes = deploymentNode.ChildNodes;

            var steps = new List<DeploymentStep>();
            foreach (var stepsNodes in deploymentStepsNodes)
            {
                if (stepsNodes.GetType().Equals(typeof(XmlElement)))
                    steps.Add(new DeploymentStep((XmlElement)stepsNodes));
#if DEBUG
                else
                {
                    Console.WriteLine($"smth else found: {stepsNodes.GetType()}");
                }
#endif
            }

            Console.WriteLine("\nReading deployment file done. Do you want to execute all commands (y/n) ?");
            var res = Console.ReadLine();
            if (!res.StartsWith("y"))
            {
                PrintErrorAndExit("Commands will not be executed.");
            }

            Console.WriteLine("\n\n\nEXECUTING:");
            foreach (DeploymentStep stepList in steps)
            {
                foreach (Step step in stepList.Steps)
                {
                    step.ExecuteStep();
                }
            }
        }

        private static void PrintErrorAndExit(string errorDescr = "")
        {
            Console.WriteLine(errorDescr);
            Console.ReadKey();
            Environment.Exit(0);
        }
    }
}