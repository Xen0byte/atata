﻿using Atata.Xunit;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;
using Xunit.Extensions.AssemblyFixture;

namespace Atata.IntegrationTests.Xunit;

[Collection(SomeCollection.Name)]
public sealed class WithCollectionFixtureTests :
    AtataTestSuite,
    IAssemblyFixture<GlobalFixture>
{
    public WithCollectionFixtureTests(ITestOutputHelper output)
        : base(output)
    {
    }

    [Fact]
    public void Context_ParentContext() =>
        Context.ParentContext.Test.Should().Be(
            new TestInfo(typeof(SomeCollectionFixture), suiteGroupName: SomeCollection.Name));

    [Fact]
    public void Context_ParentContext_ParentContext() =>
        Context.ParentContext.ParentContext.Should().NotBeNull().And.Be(
            AtataContext.Global);

    [Fact]
    public void Context_Variables() =>
        Context.Variables[nameof(SomeCollectionFixture)].Should().Be(true);

    [Fact]
    public void Context_Artifacts() =>
        Context.Artifacts.FullName.Value
            .Replace(AtataContext.GlobalProperties.ArtifactsRootPath, null)
            .Should().Be(@$"{Path.DirectorySeparatorChar}{nameof(WithCollectionFixtureTests)}{Path.DirectorySeparatorChar}{nameof(Context_Artifacts)}");
}