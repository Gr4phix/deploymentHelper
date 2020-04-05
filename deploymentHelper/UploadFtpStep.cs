using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Xml;
using WinSCP;

namespace DeploymentHelper
{
    namespace Steps
    {
        public class UploadFtpStep : ExecutableDeploymentStep
        {
            private string host;
            private string username;
            private string password;
            private readonly List<string> destinations;
            private readonly List<string> sourceFiles;
            private readonly List<string> sourceDirectories;

            public UploadFtpStep(Step parent, XmlNode node) : base(parent)
            {
                host = "";
                username = "";
                password = "";
                destinations = new List<string>();
                sourceDirectories = new List<string>();
                sourceFiles = new List<string>();

                foreach (var childIteration in node.ChildNodes)
                {
                    if (childIteration.GetType().Equals(typeof(XmlElement)))
                    {
                        var ftpNode = (XmlElement)childIteration;

                        switch (ftpNode.Name)
                        {
                            case "host":
                                host = ftpNode.InnerText;
                                break;

                            case "username":
                                username = ftpNode.InnerText;
                                break;

                            case "password":
                                password = ftpNode.InnerText;
                                break;

                            case "destination":
                                foreach (var dirs in ftpNode.ChildNodes)
                                {
                                    if (dirs.GetType().Equals(typeof(XmlElement)))
                                        destinations.Add(((XmlElement)dirs).InnerText);
                                }
                                break;

                            case "source":
                                foreach (var dirs in ftpNode.ChildNodes)
                                {
                                    if (dirs.GetType().Equals(typeof(XmlElement)))
                                    {
                                        var sourceNode = (XmlElement)dirs;

                                        if (sourceNode.Name.Equals("file"))
                                        {
                                            var _ = PathHelper.GetAbsolutePathFromFileNode(sourceNode);
                                            if (_.Item1)
                                                sourceFiles.Add(_.Item2);
                                        }
                                        else if (sourceNode.Name.Equals("dir"))
                                        {
                                            var _ = PathHelper.GetAbsolutePathFromDirNode(sourceNode);
                                            if (_.Item1)
                                                sourceDirectories.Add(_.Item2);
                                        }
                                    }
                                }
                                break;

                            default:
                                Console.WriteLine("Wrong tag in ftp-upload found.");
                                break;
                        }
                    }
#if DEBUG
                    else
                    {
                        Console.WriteLine($"Different Type found: {childIteration.GetType()}");
                    }
#endif
                }

                Console.WriteLine(ToString());
            }

            public bool ExecuteStep()
            {
                try
                {
                    if (host.Length == 0)
                    {
                        Console.WriteLine("There was no host entered to use for the ftp upload. Please enter it now:");
                        host = Console.ReadLine();
                    }
                    if (username.Length == 0)
                    {
                        Console.WriteLine("There was no username entered to use for the ftp upload. Please enter it now:");
                        username = Console.ReadLine();
                    }
                    if (password.Length == 0)
                    {
                        Console.WriteLine("There was no password entered to use for the ftp upload. Please enter it now:");
                        do
                        {
                            ConsoleKeyInfo key = Console.ReadKey(true);
                            // Backspace Should Not Work
                            if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                            {
                                password += key.KeyChar;
                                Console.Write("*");
                            }
                            else
                            {
                                if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                                {
                                    password = password.Substring(0, (password.Length - 1));
                                    Console.Write("\b \b");
                                }
                                else if (key.Key == ConsoleKey.Enter)
                                {
                                    break;
                                }
                            }
                        } while (true);
                        Console.Write("\n");
                    }

                    var sesOpt = new SessionOptions
                    {
                        Protocol = Protocol.Ftp,
                        HostName = host,
                        UserName = username,
                        Password = password
                    };

                    using (Session session = new Session())
                    {
                        session.Open(sesOpt);

                        var transOpt = new TransferOptions
                        {
                            TransferMode = TransferMode.Automatic,
                            OverwriteMode = OverwriteMode.Overwrite
                        };

                        foreach (var src in sourceDirectories)
                        {
                            foreach (var dest in destinations)
                            {
                                var transRes = session.PutFiles(src, dest, false, transOpt);
                                transRes.Check();

                                foreach (var transfer in transRes.Transfers)
                                {
                                    Console.WriteLine($"Upload of {transfer} to {dest} succeeded");
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error while uploading files: {e.Message}");
                    return false;
                }

                return true;
            }

            public override string ToString()
            {
                var str = "";

                str += $"\t\tUpload to host '{(host.Length > 0 ? host : "<No host entered>")}' " +
                    $"by using the username '{(username.Length > 0 ? username : "<No username entered>")}' " +
                    $"and password '{(password.Length > 0 ? password : "<No password entered>")}'.\n";
                str += "\t\tUpload all the following source directories/ files:\n";

                foreach (var source in sourceDirectories)
                {
                    str += $"\t\t\t- {source}\n";
                }
                foreach (var source in sourceFiles)
                {
                    str += $"\t\t\t- {source}\n";
                }

                str += "\t\tTo all the following destination directories:\n";

                foreach (var dest in destinations)
                {
                    str += $"\t\t\t- {dest}\n";
                }

                str += "\t\tusing FTP";

                return str;
            }
        }
    }
}