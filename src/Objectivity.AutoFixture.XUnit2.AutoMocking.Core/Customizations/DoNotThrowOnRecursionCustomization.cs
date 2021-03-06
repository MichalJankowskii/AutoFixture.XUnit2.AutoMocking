﻿namespace Objectivity.AutoFixture.XUnit2.AutoMocking.Core.Customizations
{
    using System.Linq;
    using Common;
    using Ploeh.AutoFixture;

    public class DoNotThrowOnRecursionCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            // Do not throw exception on circular references.
            fixture.NotNull(nameof(fixture))
                .Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));
        }
    }
}