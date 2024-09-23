﻿namespace Atata.UnitTests;

public sealed class FakeSession : AtataSession
{
    protected internal override Task StartAsync(CancellationToken cancellationToken = default) =>
        Task.CompletedTask;
}
