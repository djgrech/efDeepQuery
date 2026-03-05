using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace ServiceDefaults
{
    public static class ServiceCollectionExtensions
    {
        public static IHostApplicationBuilder ConfigureOpenTelemetry(this IHostApplicationBuilder builder)
        {
            builder.Logging.AddOpenTelemetry(x =>
            {
                x.IncludeFormattedMessage = true;
                x.IncludeScopes = true;
                x.ParseStateValues = true;
            });

            Sdk.SetDefaultTextMapPropagator(new CompositeTextMapPropagator(new List<TextMapPropagator>
            {
                new TraceContextPropagator(),
                new BaggagePropagator()
            }));

            var openTelemetry = builder.Services.AddOpenTelemetry()
                    .ConfigureResource(resource =>
                        resource.AddService(
                            serviceName: builder.Environment.ApplicationName,
                            serviceVersion: "1.0.0"))
                    .WithMetrics(metrics => metrics.AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddRuntimeInstrumentation().
                AddMeter(
                    "Microsoft.AspNetCore.Hosting",
                    "Microsoft.AspNetCore.Server.Kestrel",
                    "System.Net.Http",
                    "Microsoft.Orleans.Runtime",
                    "Microsoft.Orleans.Application"
                ))
                .WithTracing(x =>
                {
                    if (builder.Environment.IsDevelopment())
                        x.SetSampler<AlwaysOnSampler>();
                    x
                    .AddSource("Microsoft.Orleans.Runtime")
                    .AddSource("Microsoft.Orleans.Application")
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddSqlClientInstrumentation()
                    //.AddRedisInstrumentation()
                    ;
                })
                ;

            var useOtlpExplorer = !string.IsNullOrWhiteSpace(builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"]);

            if (useOtlpExplorer)
            {
                openTelemetry.UseOtlpExporter();
            }
          //  builder.AddOpenTelemetryExporter();



            return builder;

        }

        private static IHostApplicationBuilder AddOpenTelemetryExporter(this IHostApplicationBuilder builder)
        {
            var useOtlpExplorer = !string.IsNullOrWhiteSpace(builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"]);

            if (useOtlpExplorer)
            {
                /*builder.Services.Configure<OpenTelemetryLoggerOptions>(logging => logging.AddOtlpExporter());
                builder.Services.ConfigureOpenTelemetryMeterProvider(m => m.AddOtlpExporter());
                builder.Services.ConfigureOpenTelemetryTracerProvider(m => m.AddOtlpExporter());*/
                builder.Services.AddOpenTelemetry().UseOtlpExporter();
            }

            // builder.Services.AddOpenTelemetry().WithMetrics(x=>x.AddPrometheusExplorer);
            return builder;
        }
    }
}
