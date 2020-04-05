using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace DeploymentHelper.Steps
{
    public class MsBuildStep : ExecutableDeploymentStep
    {
        private readonly string pathToCsprojFile;
        private readonly string configuration;
        private readonly List<string> arguments;

        public MsBuildStep(Step parent, XmlNode node) : base(parent)
        {
            arguments = new List<string>();
            foreach (var childIt in node.ChildNodes)
            {
                if (childIt.GetType().Equals(typeof(XmlElement)))
                {
                    var child = (XmlElement)childIt;

                    if (child.Name.Equals("file"))
                    {
                        var _ = PathHelper.GetAbsolutePathFromFileNode(child);
                        if (_.Item1)
                            pathToCsprojFile = _.Item2;
                    }
                    else if (child.Name.Equals("configuration"))
                    {
                        configuration = child.InnerText;
                    }
                    else if (child.Name.Equals("argument"))
                    {
                        var tempArg = "-";
                        foreach (var item in child.ChildNodes)
                        {
                            tempArg += ((XmlText)item).InnerText.Trim() + " ";
                        }
                        tempArg = tempArg.TrimEnd();
                        arguments.Add(tempArg);
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
            return $"msbuild {pathToCsprojFile} {String.Join(" ", arguments)} -property:Configuration={configuration}";
        }
    }
}