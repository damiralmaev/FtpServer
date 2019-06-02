// <copyright file="Program.cs" company="Fubar Development Junker">
// Copyright (c) Fubar Development Junker. All rights reserved.
// </copyright>

using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using NLog.Extensions.Logging;

using TestFtpServer.Configuration;

namespace TestFtpServer
{
    internal static class Program
    {
        private static async Task<int> Main(string[] args)
        {
            var hostBuilder = new HostBuilder()
               .UseConsoleLifetime()
               .ConfigureHostConfiguration(
                    configHost => { configHost.AddEnvironmentVariables("FTPSERVER_"); })
               .ConfigureAppConfiguration(
                    (hostContext, configApp) =>
                    {
                        configApp
                           .AddJsonFile("appsettings.json")
                           .AddJsonFile(
                                $"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json",
                                optional: true)
                           .AddEnvironmentVariables("FTPSERVER_")
                           .Add(new OptionsConfigSource(args));
                    })
               .ConfigureLogging(
                    (hostContext, loggingBuilder) =>
                    {
                        loggingBuilder.AddNLog(
                            new NLogProviderOptions
                            {
                                CaptureMessageTemplates = true,
                                CaptureMessageProperties = true,
                            });
                        NLog.LogManager.LoadConfiguration("NLog.config");
                    })
               .ConfigureServices(
                    (hostContext, services) =>
                    {
                        var options = hostContext.Configuration.Get<FtpOptions>();
                        options.Validate();

                        services
                           .AddLogging(cfg => cfg.SetMinimumLevel(LogLevel.Trace))
                           .AddOptions()
                           .AddFtpServices(options)
                           .AddHostedService<HostedFtpService>()
                           .AddSingleton<ServerShell>();
                    });

            try
            {
                using (var host = hostBuilder.Build())
                {
                    // Handler for auto-completion
                    var shell = host.Services.GetRequiredService<ServerShell>();

                    // Catch request to stop the application
                    var appStopCts = new CancellationTokenSource();
                    var appLifetime = host.Services.GetRequiredService<IApplicationLifetime>();
                    using (appLifetime.ApplicationStopping.Register(() => appStopCts.Cancel()))
                    {
                        // Start the host
                        await host.StartAsync(appStopCts.Token).ConfigureAwait(false);

                        // Run the shell
                        await shell.RunAsync(appStopCts.Token)
                           .ConfigureAwait(false);

                        // Stop the host
                        await host.StopAsync(CancellationToken.None).ConfigureAwait(false);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                return 1;
            }

            return 0;
        }
    }
}
