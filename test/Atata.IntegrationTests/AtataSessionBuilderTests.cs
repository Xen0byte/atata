﻿namespace Atata.IntegrationTests;

#pragma warning disable S6966 // Awaitable method should be used

public static class AtataSessionBuilderTests
{
    public sealed class Build
    {
        [Test]
        public async Task WithoutContext()
        {
            AtataSession session = await new FakeSessionBuilder().BuildAsync();
            AtataContext context = null;

            await using (session)
            {
                context = session.Context;
                context.IsActive.Should().BeTrue();
                context.Test.Should().Be(new TestInfo(null));
            }

            session.Context.Should().BeNull();
            context.IsActive.Should().BeFalse();
        }

        [Test]
        public async Task WithinCurrentContext_ThenDisposeSession()
        {
            using AtataContext context = AtataContext.CreateDefaultNonScopedBuilder().Build();
            AtataSession session = await new FakeSessionBuilder().BuildAsync();

            await using (session)
            {
                session.Context.Should().Be(context);
                context.Sessions.Should().BeEquivalentTo([session]);
            }

            session.Context.Should().BeNull();
            context.IsActive.Should().BeTrue();
            context.Sessions.Should().BeEmpty();
        }

        [Test]
        public async Task WithinCurrentContext_ThenDisposeContext()
        {
            AtataContext context = AtataContext.CreateDefaultNonScopedBuilder().Build();
            AtataSession session = null;

            await using (context)
            {
                session = await new FakeSessionBuilder().BuildAsync();

                session.Context.Should().Be(context);
                context.Sessions.Should().BeEquivalentTo([session]);
            }

            session.Context.Should().BeNull();
            session.IsActive.Should().BeFalse();
            context.Sessions.Should().BeEmpty();
        }

        [Test]
        public async Task WithinContext_ThenDisposeSession()
        {
            await using AtataContext context = await AtataContext.CreateDefaultNonScopedBuilder()
                .Sessions.Add<FakeSessionBuilder>()
                .BuildAsync();

            var session = context.Sessions.Get<FakeSession>();

            await using (session)
            {
                session.Context.Should().Be(context);
                context.Sessions.Should().BeEquivalentTo([session]);
            }

            session.Context.Should().BeNull();
            context.IsActive.Should().BeTrue();
            context.Sessions.Should().BeEmpty();
        }

        [Test]
        public async Task WithinContext_ThenDisposeContext()
        {
            AtataContext context = await AtataContext.CreateDefaultNonScopedBuilder()
                .Sessions.Add<FakeSessionBuilder>()
                .BuildAsync();

            var session = context.Sessions.Get<FakeSession>();

            await using (context)
            {
                session.Context.Should().Be(context);
                context.Sessions.Should().BeEquivalentTo([session]);
            }

            session.Context.Should().BeNull();
            session.IsActive.Should().BeFalse();
            context.Sessions.Should().BeEmpty();
        }

        [Test]
        public async Task AfterAddToContextSessions()
        {
            await using AtataContext context = await AtataContext.CreateDefaultNonScopedBuilder().BuildAsync();
            var sessionBuidler = context.Sessions.Add<FakeSessionBuilder>();

            await using AtataContext noiseContext = await AtataContext.CreateDefaultNonScopedBuilder().BuildAsync();

            await using AtataSession session = await sessionBuidler.BuildAsync();

            session.Context.Should().Be(context);
            context.Sessions.Should().BeEquivalentTo([session]);
        }
    }
}

#pragma warning restore S6966 // Awaitable method should be used
