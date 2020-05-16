using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace DeploymentHelper
{
    namespace Steps
    {
        public class LatexCompileStep : ExecutableDeploymentStep
        {
            private readonly string pathToLatexFile;
            private readonly string destinationPath;

            public LatexCompileStep(Step parent, XmlNode node) : base(parent)
            {
                destinationPath = "";
                foreach (var childIteration in node.ChildNodes)
                {
                    if (childIteration.GetType().Equals(typeof(XmlElement)))
                    {
                        var childNode = (XmlElement)childIteration;

                        if (childNode.Name.Equals("source"))
                        {
                            foreach (var src in childNode.ChildNodes)
                            {
                                if (src.GetType().Equals(typeof(XmlElement)))
                                {
                                    if (((XmlElement)src).Name.Equals("file"))
                                    {
                                        var _ = PathHelper.GetAbsolutePathFromFileNode((XmlElement)src);
                                        if (_.Item1 && File.Exists(_.Item2))
                                            pathToLatexFile = _.Item2;
                                        else
                                        {
                                            Console.WriteLine("Given file in latexcompile step doesn't exist.");
                                        }
                                    }
                                }
                            }
                        }
                        else if (childNode.Name.Equals("destination"))
                        {
                            foreach (var dest in childNode.ChildNodes)
                            {
                                if (dest.GetType().Equals(typeof(XmlElement)))
                                {
                                    if (((XmlElement)dest).Name.Equals("file"))
                                    {
                                        var _ = PathHelper.GetAbsolutePathFromFileNode((XmlElement)dest);
                                        if (_.Item1)
                                            destinationPath = _.Item2;
                                    }
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Wrong tag in latexbuild step found.");
                        }
                    }
#if DEBUG
                    else
                    {
                        Console.WriteLine($"Different Type found: {childIteration.GetType()}");
                    }
#endif
                }

                Console.WriteLine($"\t\tCommand: {ToString()}");
                if (destinationPath.Length > 0)
                {
                    Console.WriteLine($"\t\tand copy it to {destinationPath}");
                }
            }

            public bool ExecuteStep()
            {
                try
                {
                    CmdExecuteHelper.ExecuteCommand(ToString(), Path.GetDirectoryName(pathToLatexFile));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }

                if (destinationPath.Length > 0)
                {
                    //if any destination file path is given, copy and rename the file to the destination
                    var pdfFile = Path.GetDirectoryName(pathToLatexFile) + @"/" + Path.GetFileNameWithoutExtension(pathToLatexFile) + ".pdf";
                    if (File.Exists(pdfFile))
                    {
                        if (!Directory.Exists(Path.GetDirectoryName(destinationPath)))
                            Directory.CreateDirectory(Path.GetDirectoryName(destinationPath));

                        try
                        {
                            File.Copy(pdfFile, destinationPath, true);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            return false;
                        }
                    }
                }

                return true;
            }

            public override string ToString()
            {
                return "pdflatex " + pathToLatexFile;
            }
        }
    }
}