using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace BlazorTestingsSystem.Logging
{
    public class FileLogger : ILogger
    {
        //private string filePath;
        private int fileCounter = 1;
        private string filePath;
        private int maxByteSize = 30000;
        private static object _lock = new object();
        public FileLogger()
        {

        }
        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            filePath = $"log {DateTime.Now.ToString("yyyyMMdd")}_{fileCounter}.txt";

            var fi = new FileInfo(filePath);
            if (fi.Exists)
            {
                if (fi.Length > maxByteSize)
                {
                    fileCounter++;
                }
            }

            filePath = $"log {DateTime.Now.ToString("yyyyMMdd")}_{fileCounter}.txt";
            if (formatter != null)
            {
                lock (_lock)
                {
                    File.AppendAllText(filePath, formatter(state, exception) + Environment.NewLine);
                }
            }
        }
    }
}
