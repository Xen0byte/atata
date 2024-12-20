﻿namespace Atata.Xunit;

public abstract class AtataGlobalFixture : AtataFixture
{
    protected AtataGlobalFixture()
        : base(AtataContextScope.Global)
    {
    }

    public override async Task InitializeAsync()
    {
        AtataContext.GlobalProperties.ModeOfCurrent = AtataContextModeOfCurrent.AsyncLocalBoxed;
        ConfigureAtataContextBaseConfiguration(AtataContext.BaseConfiguration);

        await base.InitializeAsync().ConfigureAwait(false);
    }

    protected virtual void ConfigureAtataContextBaseConfiguration(AtataContextBuilder builder)
    {
    }

    protected override void ConfigureAtataContext(AtataContextBuilder builder) =>
        builder.UseTestSuiteType(GetType());
}
