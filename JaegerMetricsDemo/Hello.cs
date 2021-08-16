using Microsoft.Extensions.Logging;
using OpenTracing;
using System;
using System.Collections.Generic;
using System.Text;

namespace JaegerMetricsDemo
{
    public class Hello
    {
        private readonly ITracer _tracer;
        private readonly ILogger<Hello> _logger;

        public Hello(ITracer tracer, ILoggerFactory loggerFactory)
        {
            _tracer = tracer;
            _logger = loggerFactory.CreateLogger<Hello>();
        }

        private string FormatString(ISpan rootSpan, string helloTo)
        {
            var span = _tracer.BuildSpan("format-string").AsChildOf(rootSpan).Start();
            try
            {
                var helloString = $"Hello, {helloTo}!";
                span.Log(new Dictionary<string, object>
                {
                    [LogFields.Event] = "string.Format",
                    ["value"] = helloString
                });
                return helloString;
            }
            finally
            {
                span.Finish();
            }
        }

        private void PrintHello(ISpan rootSpan, string helloString)
        {
            var span = _tracer.BuildSpan("print-hello").AsChildOf(rootSpan).Start();
            try
            {
                _logger.LogInformation(helloString);
                span.Log("WriteLine");
            }
            finally
            {
                span.Finish();
            }
        }

        public void SayHello(string helloTo)
        {
            var span = _tracer.BuildSpan("say-hello").Start();
            span.SetTag("hello-to", helloTo);
            var helloString = FormatString(span, helloTo);
            PrintHello(span, helloString);
            span.Finish();
        }
    }
}
