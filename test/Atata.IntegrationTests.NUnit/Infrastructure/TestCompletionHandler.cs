﻿using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace Atata.NUnit;

internal static class TestCompletionHandler
{
    internal static void CompleteTest(AtataContext context)
    {
        if (context is not null)
        {
            var testContext = TestContext.CurrentContext;

            if (testContext.Result.Outcome.Status == TestStatus.Failed)
                context.HandleTestResultException(testContext.Result.Message, testContext.Result.StackTrace);

            context.Dispose();
        }
    }
}
