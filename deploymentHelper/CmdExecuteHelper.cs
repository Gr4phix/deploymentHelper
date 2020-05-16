using System;
using System.Diagnostics;
using System.Threading;

namespace DeploymentHelper
{
    public static class CmdExecuteHelper
    {
        private static string wd;

        public static void ExecuteCommand(string command, string workingDir = "")
        {
            var cmd = new Process();
            if (workingDir.Length > 0)
            {
                SetWorkingDirectory(cmd, workingDir);
            }
            cmd.OutputDataReceived += Cmd_OutputDataReceived;
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();

            cmd.StandardInput.WriteLine(command);
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();

            do
            {
                var newLine = cmd.StandardOutput.ReadLine();

                if (newLine != null && newLine.Length > 0)
                {
                    Console.WriteLine(newLine);
                    Thread.Sleep(50);
                }
            }
            while (!cmd.HasExited);

            if (workingDir.Length > 0)
            {
                ResetWorkingDorectory(cmd);
            }
        }

        private static void Cmd_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine(e.Data);
        }

        private static void SetWorkingDirectory(Process cmd, string v)
        {
            wd = cmd.StartInfo.WorkingDirectory;
            cmd.StartInfo.WorkingDirectory = v;
        }

        private static void ResetWorkingDorectory(Process cmd)
        {
            cmd.StartInfo.WorkingDirectory = wd;
        }
    }
}