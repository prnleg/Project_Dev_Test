using System.Diagnostics;

namespace Project_Dev_Test.Core.Handlers
{
    public class ProcessingMetrics
    {
        private Stopwatch stopwatch;
        private PerformanceCounter cpuCounter;
        private PerformanceCounter ramCounter;

        public ProcessingMetrics()
        {
            stopwatch = new Stopwatch();
            cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        }

        public void StartProcessing()
        {
            stopwatch.Start();
            cpuCounter.NextValue();
        }

        public void EndProcessing()
        {
            stopwatch.Stop();
        }

        public float GetCpuUsage()
        {
            return cpuCounter.NextValue();
        }

        public float GetRamUsageMB()
        {
            System.Diagnostics.Process p = System.Diagnostics.Process.GetCurrentProcess();
            long ram = p.WorkingSet64;
            return ram / 1024 / 1024;
        }

        public TimeSpan GetElapsedTime()
        {
            return stopwatch.Elapsed;
        }
    }
}
