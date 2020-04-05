using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace DeploymentHelper.Steps
{
    public class CleanStep : ExecutableDeploymentStep
    {
        private readonly List<string> directories;

        public CleanStep(Step parent, XmlNode node) : base(parent)
        {
            directories = new List<string>();
            foreach (var childIt in node.ChildNodes)
            {
                if (childIt.GetType().Equals(typeof(XmlElement)))
                {
                    var child = (XmlElement)childIt;

                    if (child.Name.Equals("dir"))
                    {
                        var _ = PathHelper.GetAbsolutePathFromDirNode(child);
                        if (_.Item1)
                            directories.Add(_.Item2);
                    }
                }
#if DEBUG
                else
                {
                    Console.WriteLine($"Different Type found: {childIt.GetType()}");
                }
#endif
            }

            Console.WriteLine($"\t\t{ToString()}");
        }

        public bool ExecuteStep()
        {
            foreach (var dir in directories)
            {
                try
                {
                    Directory.Delete(dir, true);
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return true;
        }

        public override string ToString()
        {
            var str = "";
            str += "To be cleaned:\n";
            foreach (var dir in directories)
            {
                str += $"\t\t\t- {dir}\n";
            }
            if (str.EndsWith("\n"))
                return str.Substring(0, str.Length - 2);

            return str;
        }
    }
}