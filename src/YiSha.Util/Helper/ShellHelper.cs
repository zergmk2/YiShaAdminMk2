using System.Diagnostics;

namespace YiSha.Util.Helper
{
    public class ShellHelper
    {
        public static string Bash(string command)
        {
            var escapedArgs = command.Replace("\"", "\\\"");
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{escapedArgs}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            process.Start();
            var result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            process.Dispose();
            return result;
        }

        public static string Cmd(string fileName, string args)
        {
            var output = string.Empty;

            var info = new ProcessStartInfo();
            info.FileName = fileName;
            info.Arguments = args;
            info.RedirectStandardOutput = true;

            using (var process = Process.Start(info))
            {
                output = process.StandardOutput.ReadToEnd();
            }

            return output;
        }
    }
}
