using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;

namespace DeploymentHelper
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("╔════════════════════════════════════════╗");
            Console.WriteLine($"║      deploymentHelper    v{Assembly.GetExecutingAssembly().GetName().Version.ToString()}      ║");
            Console.WriteLine("╚════════════════════════════════════════╝\n");

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

            var startTime = DateTime.Now;
            Console.WriteLine($"\n\n\nExecution started at: {startTime.ToLongTimeString()}");
            foreach (DeploymentStep stepList in steps)
            {
                foreach (Step step in stepList.Steps)
                {
                    step.ExecuteStep();
                }
            }

            var endtime = DateTime.Now;
            Console.WriteLine($"\n\n\nExecution finished at: {endtime.ToLongTimeString()}");
            Console.WriteLine($"The execution of all steps took {(endtime - startTime).TotalMinutes} minutes and {(endtime - startTime).TotalMinutes} seconds in total.");
        }

        private static void PrintErrorAndExit(string errorDescr = "")
        {
            Console.WriteLine(errorDescr);
            Console.ReadKey();
            Environment.Exit(0);
        }
    }
}