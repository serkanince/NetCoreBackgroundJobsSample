using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetCoreBackgroundJobsSample.BackgroundServices;
using Polly;
using System;
using System.Collections.Concurrent;
using System.Net.Http;

using var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services
        .AddHostedService<ProducerBGService>()
        .AddHostedService<ConsumerBGService>()
        .AddSingleton((p) => { return new ConcurrentQueue<string>(); });


        var retryPolicy = Policy<HttpResponseMessage>
        .Handle<HttpRequestException>()
        .WaitAndRetryAsync(
            2,
            retryAttempt => TimeSpan.FromSeconds(Math.Pow(4, retryAttempt)),
            (exception, timeSpan, retryCount, context) =>
            {
                Console.WriteLine("Try Again :" + retryCount);
                Console.WriteLine(exception.Exception?.Message);
            }
        );

        var policyRegistry = services.AddPolicyRegistry();
        var policyKey = "policy";
        policyRegistry.Add(policyKey, retryPolicy);

        services.AddHttpClient("currencyClient")
                .AddPolicyHandlerFromRegistry(policyKey);

    })
    .Build();

await host.StartAsync();

await host.WaitForShutdownAsync();