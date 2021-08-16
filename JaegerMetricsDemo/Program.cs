using Jaeger;
using Jaeger.Samplers;
using Jaeger.Senders;
using Jaeger.Senders.Grpc;
using Microsoft.Extensions.Logging;
using System;

namespace JaegerMetricsDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                throw new ArgumentException("Expecting one argument");
            }

            using (var loggerFactory = LoggerFactory.Create((ILoggingBuilder ilb) => { ilb.AddConsole(); }))
            {
                var helloTo = args[0];
                using (var tracer = InitTracer("hello-world", loggerFactory))
                {
                    // new Hello(GlobalTracer.Instance, loggerFactory).SayHello(helloTo);
                    new Hello(tracer, loggerFactory).SayHello(helloTo);
                }
            }
        }

        private static Tracer InitTracer(string serviceName, ILoggerFactory loggerFactory)
        {
            Configuration.SenderConfiguration.DefaultSenderResolver = new SenderResolver(loggerFactory)
                // You can add other ISenderFactory instances too
                .RegisterSenderFactory<GrpcSenderFactory>();

            var samplerConfiguration = new Configuration.SamplerConfiguration(loggerFactory)
                .WithType(ConstSampler.Type)
                .WithParam(1);

            var sender = new Configuration.SenderConfiguration(loggerFactory)
                .WithEndpoint("127.0.0.1:14250");

            var reporterConfiguration = new Configuration.ReporterConfiguration(loggerFactory)
                .WithLogSpans(true)
                .WithSender(sender);

            return (Tracer)new Configuration(serviceName, loggerFactory)
                .WithSampler(samplerConfiguration)
                .WithReporter(reporterConfiguration)
                .GetTracer();
        }
    }
}
