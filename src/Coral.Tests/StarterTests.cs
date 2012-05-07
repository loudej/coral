using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shouldly;
using Xunit;

namespace Coral.Tests
{
    public class StarterTests
    {
        [Fact]
        public void CreatingDefaultContainerAndEngineShouldNotThrowException()
        {
            var container = Starter.CreateContainer();
            var engine = Starter.CreateEngine();

            container.ShouldNotBe(null);
            engine.ShouldNotBe(null);
        }
    }
}
