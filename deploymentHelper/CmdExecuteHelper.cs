using System;
using System.Diagnostics;
using System.Threading;

namespace DeploymentHelper
{
    public static class CmdExecuteHelper
    {
        public static void ExecuteCommand(string command)
        {
            Process cmd = new Process();
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
        }

        private static void Cmd_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine(e.Data);
        }
    }
}