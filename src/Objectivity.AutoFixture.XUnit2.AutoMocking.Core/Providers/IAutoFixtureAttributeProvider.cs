﻿namespace Objectivity.AutoFixture.XUnit2.AutoMocking.Core.Providers
{
    using Ploeh.AutoFixture;
    using Xunit.Sdk;

    public interface IAutoFixtureAttributeProvider
    {
        DataAttribute GetAttribute(IFixture fixture);
    }
}