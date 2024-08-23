import { OTLPTraceExporter } from "@opentelemetry/exporter-trace-otlp-http";
import { Resource } from "@opentelemetry/resources";
import { TracerConfig } from "@opentelemetry/sdk-trace-base";
import {
  BatchSpanProcessor,
  WebTracerProvider,
} from "@opentelemetry/sdk-trace-web";
import { SEMRESATTRS_SERVICE_NAME } from "@opentelemetry/semantic-conventions";
import { ZoneContextManager } from "@opentelemetry/context-zone";
import { registerInstrumentations } from "@opentelemetry/instrumentation";
import { getWebAutoInstrumentations } from "@opentelemetry/auto-instrumentations-web";

// pending https://github.com/dotnet/aspire/discussions/1914

export function initTracer() {
  const collectorOptions = {
    url: import.meta.env.VITE_OTEL_EXPORTER_OTLP_ENDPOINT,
  };

  console.log(
    "Initializing tracer with collector endpoint: ",
    collectorOptions.url
  );

  const providerConfig: TracerConfig = {
    resource: new Resource({
      [SEMRESATTRS_SERVICE_NAME]: "issue-tracker-ui",
    }),
  };

  const provider = new WebTracerProvider(providerConfig);
  provider.addSpanProcessor(
    new BatchSpanProcessor(new OTLPTraceExporter(collectorOptions))
  );

  provider.register({
    contextManager: new ZoneContextManager().enable(),
  });

  registerInstrumentations({
    instrumentations: [getWebAutoInstrumentations()],
  });
}
