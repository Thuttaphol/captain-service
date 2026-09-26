namespace Captain.Extensions;

public static class LogExtension
{
  extension(ILoggingBuilder loggingBuilder)
  {
    public ILoggingBuilder AddLoggingConfiguration()
    {
      loggingBuilder.AddSimpleConsole(options => options.IncludeScopes = true);

      loggingBuilder.Configure(options =>
        options.ActivityTrackingOptions =
          ActivityTrackingOptions.TraceId | ActivityTrackingOptions.SpanId
      );

      return loggingBuilder;
    }
  }
}
