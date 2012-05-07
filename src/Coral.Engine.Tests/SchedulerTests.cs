using System;
using Coral.Engine.Scheduler;
using Coral.Engine.Tests.Fakes;
using Xunit;

namespace Coral.Engine.Tests
{
    public class SchedulerTests
    {
        [Fact]
        public void ItShouldStartAndStopWithoutExceptions()
        {
            var scheduler = new DefaultScheduler(new FakeLoggerFactory());
            scheduler.Start();
            var stopped = scheduler.Stop().Wait(4000);
            Assert.True(stopped);
        }

        [Fact]
        public void PostShouldThrowExceptionBeforeStart()
        {
            var scheduler = new DefaultScheduler(new FakeLoggerFactory());
            Assert.Throws<InvalidOperationException>(() => scheduler.Post(() => { }));
        }

        [Fact]
        public void PostShouldThrowExceptionAfterStop()
        {
            var scheduler = new DefaultScheduler(new FakeLoggerFactory());
            scheduler.Start();
            scheduler.Stop();
            Assert.Throws<InvalidOperationException>(() => scheduler.Post(() => { }));
        }

    }
}
