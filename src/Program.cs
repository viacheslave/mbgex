using System;
using System.IO;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace SpringOff.MGBEx
{
  class Program
  {
    static void Main(string[] args)
    {
      if (args.Length < 2)
      {
        Console.WriteLine("Wrong parameters supplied.");
        Console.WriteLine("Usage: dotnet MGBEx.dll <email> <password>");
        Console.WriteLine(Environment.NewLine);
        return;
      }

      var login = args[0];
      var password = args[1];

      ILoggerFactory loggerFactory = new LoggerFactory();
      loggerFactory.AddNLog();
      loggerFactory.ConfigureNLog(Path.Combine(Environment.CurrentDirectory, "nlog.config"));

      ILogger logger = loggerFactory.CreateLogger<Program>();

      var apiService = new ApiService(loggerFactory);

      new ExportService(apiService, loggerFactory)
        .Dump(new LoginRequest { UserName = login, Password = password })
        .GetAwaiter()
        .GetResult();

      Console.WriteLine("All Done.");
      Console.WriteLine(Environment.NewLine);
    }
  }
}
