using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace BlazorTestingsSystem.Logging
{
    public class FileLogger : ILogger
    {
        private readonly FileLoggerProvider _fileLoggerProvider;
        private int _fileCounter = 1;
        private static object _lock = new object();
        public FileLogger(FileLoggerProvider fileLoggerProvider)
        {
            _fileLoggerProvider = fileLoggerProvider;
        }
        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel != LogLevel.None;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
                return;

            var fileName = GenetateFileName();

            var fi = new FileInfo(fileName);
            if (fi.Exists && fi.Length > _fileLoggerProvider.Options.MaxByteSize)
            {
                _fileCounter++;
                fileName = GenetateFileName();
            }

            fileName = Path.Combine(_fileLoggerProvider.Options.FolderPath, _fileLoggerProvider.Options.FilePath
                .Replace("[timestamp]", DateTime.Now.ToString("yyyyMMdd"))
                .Replace("[counter]", _fileCounter.ToString()));

            var threadId = Thread.CurrentThread.ManagedThreadId;
            var logRecord = string.Format("[{0}] [{1}] [{2}] - {3}", DateTime.Now.ToString("[yyyy.MM.dd HH:mm]"), logLevel.ToString(), threadId, formatter(state, exception));
            
            lock (_lock)
            {
                using (var sw = new StreamWriter(fileName, true, System.Text.Encoding.UTF8))
                {
                    sw.WriteLine(logRecord);
                }
            }
        }

        private string GenetateFileName()
        {
            return Path.Combine(_fileLoggerProvider.Options.FolderPath, _fileLoggerProvider.Options.FilePath
                   .Replace("[timestamp]", DateTime.Now.ToString("yyyyMMdd"))
                   .Replace("[counter]", _fileCounter.ToString()));

        }
    }
}
