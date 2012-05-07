using Coral.Options;
using Shouldly;
using Xunit;

namespace Coral.Tests
{
    public class StartOptionsTests
    {
        [Fact]
        public void SetConcurrencyShouldParseNameAndInteger()
        {
            var options = new StartOptions();
            options.SetConcurrency("alpha=5");
            options.SetConcurrency("beta=3,gamma=6");
            options.Concurrency.ShouldContainKeyAndValue("alpha", 5);
            options.Concurrency.ShouldContainKeyAndValue("beta", 3);
            options.Concurrency.ShouldContainKeyAndValue("gamma", 6);
            options.Concurrency.Count.ShouldBe(3);
        }
    }
}
