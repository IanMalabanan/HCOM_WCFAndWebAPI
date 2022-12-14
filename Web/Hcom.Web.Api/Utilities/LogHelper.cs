using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Hcom.Web.Api.Utilities
{
    public class LogHelper
    {
            static ILoggerFactory _loggerFactory;


            public static void ConfigureLogger(ILoggerFactory loggerFactory)
            {
                _loggerFactory = loggerFactory;
            }


            public static ILogger CreateLogger<T>()
            {
                if (_loggerFactory == null)
                {
                    throw new InvalidOperationException($"{nameof(ILogger)} is not configured. {nameof(ConfigureLogger)} must be called before use");
                    //_loggerFactory = new LoggerFactory().AddConsole().AddDebug();
                }

                return _loggerFactory.CreateLogger<T>();
            }


            public static void QuickLog(string text, string filename)
            {
                string dirPath = Path.GetDirectoryName(filename);

                if (!Directory.Exists(dirPath))
                    Directory.CreateDirectory(dirPath);

                using (StreamWriter writer = File.AppendText(filename))
                {
                    writer.WriteLine($"{DateTime.Now} - {text}");
                }
            }



        }
    }

