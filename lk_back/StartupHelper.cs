using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace lk_back
{
    public static class StartupHelper
    {
        public static string GetEnvironmentFromArgs(string[] args)
        {
            var env = "development";
            if (args.All(_ => _ != "--environment"))
            {
                var argsList = new List<string>(args);
                argsList.Add("--environment");
                argsList.Add("development");
                args = argsList.ToArray();
            }
            else
            {
                var index = -1;
                do
                {
                    index++;
                } while (args[index] != "--environment");

                env = args[index + 1];
            }

            return env;
        }

        public static IConfigurationRoot GetConfiguration(string[] args)
        {
            var env = GetEnvironmentFromArgs(args);
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddCommandLine(args)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
        }
    }
}