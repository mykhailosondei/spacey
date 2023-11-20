using Serilog;
using Serilog.Formatting.Json;
using Serilog.Sinks.XUnit;
using Xunit.Sdk;

namespace Application.API.IntegrationTests.Fixtures;

public class TestOutputFixture
{
    public TestOutputFixture()
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.TestOutput(formatter: new JsonFormatter(renderMessage:true), testOutputHelper: new TestOutputHelper()).CreateLogger();
    }
}