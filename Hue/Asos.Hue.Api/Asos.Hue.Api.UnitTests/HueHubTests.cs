using System;
using Asos.Hue.Api.Options;
using NUnit.Framework;
using Should;

namespace Asos.Hue.Api.UnitTests
{
    [TestFixture]
    public class HueHubTests
    {
        private readonly HueHub _hueHub;

        public HueHubTests()
        {
            var options = new HueHubOptions
            {
                Uri = "http://abc",
                UserKey = "UserKey"
            };
            _hueHub = new HueHub(options);
        }

        [Test]
        public void Should_Return_An_Object_When_New()
        {
            var hueHub = new HueHub(null);
            hueHub.ShouldNotBeNull();
        }
        [Test]
        public void Should_Take_An_Options_When_Initialised()
        {
            var options = new HueHubOptions
            {
                Uri = "http://abc",
                UserKey = "UserKey"
            };
            var hueHub = new HueHub(options);
            hueHub.ShouldNotBeNull();
        }

    }
}
