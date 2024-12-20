﻿namespace Atata.IntegrationTests;

public sealed class FakeSession2 : AtataSession
{
    protected internal override Task StartAsync(CancellationToken cancellationToken = default) =>
        Task.CompletedTask;
}