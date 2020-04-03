using System;
using System.Xml;

namespace DeploymentHelper
{
    namespace Steps
    {
        public class BuildDocStep : ExecutableDeploymentStep
        {
            private readonly string pathToDoxygenConfFile;

            public BuildDocStep(Step parent, XmlNode node) : base(parent)
            {
                foreach (var childIt in node.ChildNodes)
                {
                    if (childIt.GetType().Equals(typeof(XmlElement)))
                    {
                        var fileNode = (XmlElement)childIt;

                        pathToDoxygenConfFile = PathHelper.GetAbsolutePathFromFileNode(fileNode).Item1 ? PathHelper.GetAbsolutePathFromFileNode(fileNode).Item2 : "";
                        break;
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
                return "doxygen " + pathToDoxygenConfFile;
            }
        }
    }
}