﻿namespace Objectivity.AutoFixture.XUnit2.AutoMocking.Core.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Common;
    using Customizations;
    using MemberData;
    using Ploeh.AutoFixture;
    using Providers;
    using Xunit;
    using Xunit.Sdk;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    [DataDiscoverer("Xunit.Sdk.MemberDataDiscoverer", "xunit.core")]
    public abstract class MemberAutoMockingDataAttribute : MemberDataAttributeBase
    {
        protected MemberAutoMockingDataAttribute(string memberName, params object[] parameters)
            : this(new Fixture(), memberName.NotNull(nameof(memberName)), parameters)
        {
        }

        protected MemberAutoMockingDataAttribute(IFixture fixture, string memberName, params object[] parameters)
            : base(memberName.NotNull(nameof(memberName)), parameters)
        {
            this.Fixture = fixture.NotNull(nameof(fixture));
        }

        public IFixture Fixture { get; }

        /// <summary>
        /// Gets or sets a value indicating whether virtual members should be ignored during object creation.
        /// </summary>
        public bool IgnoreVirtualMembers { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether to share a fixture across all data items; true by default.
        /// </summary>
        public bool ShareFixture { get; set; } = true;

        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            // Customize shared fixture
            this.CustomizeFixture(this.Fixture);

            return base.GetData(testMethod);
        }

        protected override object[] ConvertDataItem(MethodInfo testMethod, object item)
        {
            var fixture = this.ShareFixture ? this.Fixture : this.CustomizeFixture(new Fixture());

            var converter = new MemberAutoDataItemConverter(fixture, new InlineAutoDataAttributeProvider());

            return converter.Convert(testMethod, item, this.MemberName, this.MemberType);
        }

        protected abstract IAutoMockingDataCustomization GenerateAutoMockingDataCustomization();

        private IFixture CustomizeFixture(IFixture fixture)
        {
            fixture.Customize(this.GenerateAutoMockingDataCustomization());
            fixture.Customize(new IgnoreVirtualMembersCustomization(this.IgnoreVirtualMembers));

            return fixture;
        }
    }
}