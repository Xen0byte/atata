﻿#warning Review AtataContextWithoutDriverTests. Change to test suite for SessionStart with custom session.

////namespace Atata.IntegrationTests.Context;

////public class AtataContextWithoutDriverTests : WebDriverSessionTestSuiteBase
////{
////    [Test]
////    public void WhenDriverInitializationStageIsNone()
////    {
////        var sut = AtataContext.Configure()
////            .UseDriverInitializationStage(AtataContextDriverInitializationStage.None);

////        sut.Build();

////        Assert.That(AtataContext.Current.Driver, Is.Null);
////    }

////    [Test]
////    public void WhenDriverInitializationStageIsBuild()
////    {
////        var sut = AtataContext.Configure()
////            .UseDriverInitializationStage(AtataContextDriverInitializationStage.Build);

////        Assert.Throws<InvalidOperationException>(() =>
////            sut.Build());
////    }

////    [Test]
////    public void WhenDriverInitializationStageIsOnDemand()
////    {
////        var sut = AtataContext.Configure()
////            .UseDriverInitializationStage(AtataContextDriverInitializationStage.OnDemand);

////        sut.Build();

////        Assert.Throws<WebDriverInitializationException>(() =>
////            _ = AtataContext.Current.Driver);
////    }
////}
