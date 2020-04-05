using System;
using System.Collections.Generic;
using System.Xml;

namespace DeploymentHelper
{
    namespace Steps
    {
        public class DeployQtStep : ExecutableDeploymentStep
        {
            private readonly List<string> arguments;
            private readonly string executableFilePath;

            public DeployQtStep(Step parent, XmlNode node) : base(parent)
            {
                arguments = new List<string>();
                foreach (var childIt in node.ChildNodes)
                {
                    if (childIt.GetType().Equals(typeof(XmlElement)))
                    {
                        var child = (XmlElement)childIt;

                        if (child.Name.Equals("argument"))
                        {
                            var tempArg = "";
                            foreach (var item in child.ChildNodes)
                            {
                                if (item.GetType().Equals(typeof(XmlText)))
                                    tempArg += ((XmlText)item).InnerText.Trim() + " ";
                                else if (item.GetType().Equals(typeof(XmlElement)))
                                {
                                    tempArg += PathHelper.GetAbsolutePathFromDirNode((XmlElement)item).Item1 ? PathHelper.GetAbsolutePathFromDirNode((XmlElement)item).Item2 : "" + " ";
                                }
                            }
                            tempArg = tempArg.TrimEnd();
                            arguments.Add(tempArg);
                        }
                        else if (child.Name.Equals("file"))
                        {
                            executableFilePath = PathHelper.GetAbsolutePathFromFileNode(child).Item1 ? PathHelper.GetAbsolutePathFromFileNode(child).Item2 : "";
                        }
                    }
#if DEBUG
                    else
                    {
                        Console.WriteLine($"Different Type found: {childIt.GetType()}");
                    }
#endif
                }

                Console.WriteLine($"\t\tCommand: {ToString()}");
            }

            public override string ToString()
            {
                var command = "windeployqt";
                foreach (var arg in arguments)
                {
                    command += " --" + arg;
                }
                command += " " + executableFilePath;

                return command;
            }
        }
    }
}