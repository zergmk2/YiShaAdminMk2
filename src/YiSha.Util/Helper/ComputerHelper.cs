using System;
using System.Runtime.InteropServices;

namespace YiSha.Util.Helper
{
    public class ComputerHelper
    {
        public static ComputerInfo GetComputerInfo()
        {
            var computerInfo = new ComputerInfo();
            try
            {
                var client = new MemoryMetricsClient();
                var memoryMetrics = client.GetMetrics();
                computerInfo.TotalRAM = Math.Ceiling(memoryMetrics.Total / 1024) + " GB";
                computerInfo.RAMRate = Math.Ceiling(100 * memoryMetrics.Used / memoryMetrics.Total);
                computerInfo.CPURate = Math.Ceiling(GetCPURate().ParseToDouble());
                computerInfo.RunTime = GetRunTime();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }

            return computerInfo;
        }

        public static bool IsUnix()
        {
            var isUnix = RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ||
                         RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
            return isUnix;
        }

        public static string GetCPURate()
        {
            var cpuRate = string.Empty;
            if (IsUnix())
            {
                var output = ShellHelper.Bash("top -b -n1 | grep \"Cpu(s)\" | awk '{print $2 + $4}'");
                cpuRate = output.Trim();
            }
            else
            {
                var output = ShellHelper.Cmd("wmic", "cpu get LoadPercentage");
                cpuRate = output.Replace("LoadPercentage", string.Empty).Trim();
            }

            return cpuRate;
        }

        public static string GetRunTime()
        {
            var runTime = string.Empty;
            try
            {
                if (IsUnix())
                {
                    var output = ShellHelper.Bash("uptime -s");
                    output = output.Trim();
                    runTime = DateTimeHelper.FormatTime(
                        (DateTime.Now - output.ParseToDateTime()).TotalMilliseconds.ToString().Split('.')[0]
                        .ParseToLong());
                }
                else
                {
                    var output = ShellHelper.Cmd("wmic", "OS get LastBootUpTime/Value");
                    var outputArr = output.Split("=", StringSplitOptions.RemoveEmptyEntries);
                    if (outputArr.Length == 2)
                        runTime = DateTimeHelper.FormatTime(
                            (DateTime.Now - outputArr[1].Split('.')[0].ParseToDateTime()).TotalMilliseconds.ToString()
                            .Split('.')[0].ParseToLong());
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }

            return runTime;
        }
    }

    public class MemoryMetrics
    {
        public double Total { get; set; }
        public double Used { get; set; }
        public double Free { get; set; }
    }

    public class MemoryMetricsClient
    {
        public MemoryMetrics GetMetrics()
        {
            if (ComputerHelper.IsUnix()) return GetUnixMetrics();
            return GetWindowsMetrics();
        }

        private MemoryMetrics GetWindowsMetrics()
        {
            var output = ShellHelper.Cmd("wmic", "OS get FreePhysicalMemory,TotalVisibleMemorySize /Value");

            var lines = output.Trim().Split("\n");
            var freeMemoryParts = lines[0].Split("=", StringSplitOptions.RemoveEmptyEntries);
            var totalMemoryParts = lines[1].Split("=", StringSplitOptions.RemoveEmptyEntries);

            var metrics = new MemoryMetrics();
            metrics.Total = Math.Round(double.Parse(totalMemoryParts[1]) / 1024, 0);
            metrics.Free = Math.Round(double.Parse(freeMemoryParts[1]) / 1024, 0);
            metrics.Used = metrics.Total - metrics.Free;

            return metrics;
        }

        private MemoryMetrics GetUnixMetrics()
        {
            var output = ShellHelper.Bash("free -m");

            var lines = output.Split("\n");
            var memory = lines[1].Split(" ", StringSplitOptions.RemoveEmptyEntries);

            var metrics = new MemoryMetrics();
            metrics.Total = double.Parse(memory[1]);
            metrics.Used = double.Parse(memory[2]);
            metrics.Free = double.Parse(memory[3]);

            return metrics;
        }
    }

    public class ComputerInfo
    {
        /// <summary>
        ///     CPU使用率
        /// </summary>
        public double CPURate { get; set; }

        /// <summary>
        ///     总内存
        /// </summary>
        public string TotalRAM { get; set; }

        /// <summary>
        ///     内存使用率
        /// </summary>
        public double RAMRate { get; set; }

        /// <summary>
        ///     系统运行时间
        /// </summary>
        public string RunTime { get; set; }
    }
}
