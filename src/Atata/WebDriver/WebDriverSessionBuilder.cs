﻿using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Safari;

namespace Atata;

public class WebDriverSessionBuilder : WebSessionBuilder<WebDriverSession, WebDriverSessionBuilder>
{
    /// <summary>
    /// Gets the driver factories.
    /// </summary>
    public List<IWebDriverFactory> DriverFactories { get; private set; } = [];

    /// <summary>
    /// Gets the driver factory to use.
    /// </summary>
    public IWebDriverFactory DriverFactoryToUse { get; private set; }

    /// <summary>
    /// Gets or sets a value indicating whether to dispose the <see cref="WebDriverSession.Driver"/>
    /// when <see cref="WebDriverSession.Dispose()"/> method is invoked.
    /// The default value is <see langword="true"/>.
    /// </summary>
    public bool DisposeDriver { get; set; } = true;

    /// <summary>
    /// Gets a value indicating whether it uses a local browser.
    /// Basically, determines whether <see cref="DriverFactoryToUse"/> is <see cref="IUsesLocalBrowser"/>.
    /// </summary>
    public bool UsesLocalBrowser =>
        DriverFactoryToUse is IUsesLocalBrowser;

    /// <summary>
    /// Gets the name of the local browser to use or <see langword="null"/>.
    /// Returns <see cref="IUsesLocalBrowser.BrowserName"/> value
    /// if <see cref="DriverFactoryToUse"/> is <see cref="IUsesLocalBrowser"/>.
    /// </summary>
    public string LocalBrowserToUseName =>
        (DriverFactoryToUse as IUsesLocalBrowser)?.BrowserName;

    /// <summary>
    /// Gets the names of local browsers that this instance uses.
    /// Distinctly returns <see cref="IUsesLocalBrowser.BrowserName"/> values of all
    /// <see cref="DriverFactories"/> that are <see cref="IUsesLocalBrowser"/>.
    /// </summary>
    public IEnumerable<string> ConfiguredLocalBrowserNames =>
        DriverFactories.OfType<IUsesLocalBrowser>().Select(x => x.BrowserName).Distinct();

    /// <summary>
    /// Gets or sets the default control visibility.
    /// The default value is <see cref="Visibility.Any"/>.
    /// </summary>
    public Visibility DefaultControlVisibility { get; set; }

    /// <summary>
    /// Gets the screenshots configuration options.
    /// </summary>
    public ScreenshotsWebDriverSessionOptions Screenshots { get; private set; } = new();

    /// <summary>
    /// Gets the page snapshots configuration options.
    /// </summary>
    public PageSnapshotsWebDriverSessionOptions PageSnapshots { get; private set; } = new();

    /// <summary>
    /// Gets the driver factory by the specified alias.
    /// </summary>
    /// <param name="alias">The alias of the driver factory.</param>
    /// <returns>The driver factory or <see langword="null"/>.</returns>
    public IWebDriverFactory GetDriverFactory(string alias)
    {
        alias.CheckNotNullOrWhitespace(nameof(alias));

        return DriverFactories.LastOrDefault(x => alias.Equals(x.Alias, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Returns an existing or creates a new builder for <typeparamref name="TDriverBuilder"/> by the specified alias.
    /// </summary>
    /// <typeparam name="TDriverBuilder">The type of the driver builder.</typeparam>
    /// <param name="alias">The driver alias.</param>
    /// <param name="driverBuilderCreator">The function that creates a driver builder.</param>
    /// <returns>The <typeparamref name="TDriverBuilder"/> instance.</returns>
    public TDriverBuilder ConfigureDriver<TDriverBuilder>(string alias, Func<TDriverBuilder> driverBuilderCreator)
        where TDriverBuilder : WebDriverBuilder<TDriverBuilder>
    {
        alias.CheckNotNullOrWhitespace(nameof(alias));
        driverBuilderCreator.CheckNotNull(nameof(driverBuilderCreator));

        var driverFactory = GetDriverFactory(alias);

        if (driverFactory is null)
        {
            driverFactory = driverBuilderCreator.Invoke();
            DriverFactories.Add(driverFactory);
        }
        else if (driverFactory is not TDriverBuilder)
        {
            throw new ArgumentException(
                $"""
                Existing driver with "{alias}" alias has other factory type in {nameof(AtataContextBuilder)}.
                Expected: {typeof(TDriverBuilder).FullName}
                Actual: {driverFactory.GetType().FullName}
                """,
                nameof(alias));
        }

        return (TDriverBuilder)driverFactory;
    }

    /// <summary>
    /// Use the driver builder.
    /// </summary>
    /// <typeparam name="TDriverBuilder">The type of the driver builder.</typeparam>
    /// <returns>The <typeparamref name="TDriverBuilder"/> instance.</returns>
    public TDriverBuilder UseDriver<TDriverBuilder>()
        where TDriverBuilder : WebDriverBuilder<TDriverBuilder>, new() =>
        UseDriver(new TDriverBuilder());

    /// <summary>
    /// Use the driver builder.
    /// </summary>
    /// <typeparam name="TDriverBuilder">The type of the driver builder.</typeparam>
    /// <param name="driverBuilder">The driver builder.</param>
    /// <returns>The <typeparamref name="TDriverBuilder"/> instance.</returns>
    public TDriverBuilder UseDriver<TDriverBuilder>(TDriverBuilder driverBuilder)
        where TDriverBuilder : WebDriverBuilder<TDriverBuilder>
    {
        driverBuilder.CheckNotNull(nameof(driverBuilder));

        DriverFactories.Add(driverBuilder);
        DriverFactoryToUse = driverBuilder;

        return driverBuilder;
    }

    /// <summary>
    /// Sets the alias of the driver to use.
    /// </summary>
    /// <param name="alias">The alias of the driver.</param>
    /// <returns>The same <see cref="WebDriverSessionBuilder"/> instance.</returns>
    public WebDriverSessionBuilder UseDriver(string alias)
    {
        alias.CheckNotNullOrWhitespace(nameof(alias));

        IWebDriverFactory driverFactory = GetDriverFactory(alias);

        if (driverFactory != null)
            DriverFactoryToUse = driverFactory;
        else if (UsePredefinedDriver(alias) == null)
            throw new ArgumentException($"No driver with \"{alias}\" alias defined.", nameof(alias));

        return this;
    }

    /// <summary>
    /// Use the specified driver instance.
    /// </summary>
    /// <param name="driver">The driver to use.</param>
    /// <returns>The <see cref="CustomWebDriverBuilder"/> instance.</returns>
    public CustomWebDriverBuilder UseDriver(IWebDriver driver)
    {
        driver.CheckNotNull(nameof(driver));

        return UseDriver(() => driver);
    }

    /// <summary>
    /// Use the custom driver factory method.
    /// </summary>
    /// <param name="driverFactory">The driver factory method.</param>
    /// <returns>The <see cref="CustomWebDriverBuilder"/> instance.</returns>
    public CustomWebDriverBuilder UseDriver(Func<IWebDriver> driverFactory)
    {
        driverFactory.CheckNotNull(nameof(driverFactory));

        return UseDriver(new CustomWebDriverBuilder(driverFactory));
    }

    private IWebDriverFactory UsePredefinedDriver(string alias) =>
        alias.ToLowerInvariant() switch
        {
            WebDriverAliases.Chrome => UseChrome(),
            WebDriverAliases.Firefox => UseFirefox(),
            WebDriverAliases.InternetExplorer => UseInternetExplorer(),
            WebDriverAliases.Safari => UseSafari(),
            WebDriverAliases.Edge => UseEdge(),
            _ => null
        };

    /// <summary>
    /// Sets a value indicating whether to dispose the <see cref="WebDriverSession.Driver"/>
    /// when <see cref="WebDriverSession.Dispose()"/> method is invoked.
    /// The default value is <see langword="true"/>.
    /// </summary>
    /// <param name="disposeDriver">Whether to dispose driver.</param>
    /// <returns>The same <see cref="WebDriverSessionBuilder"/> instance.</returns>
    public WebDriverSessionBuilder UseDisposeDriver(bool disposeDriver)
    {
        DisposeDriver = disposeDriver;
        return this;
    }

    /// <summary>
    /// Creates and returns a new builder for <see cref="ChromeDriver"/>
    /// with default <see cref="WebDriverAliases.Chrome"/> alias.
    /// Sets this builder as a one to use for a driver creation.
    /// </summary>
    /// <returns>The <see cref="ChromeDriverBuilder"/> instance.</returns>
    public ChromeDriverBuilder UseChrome() =>
        UseDriver(new ChromeDriverBuilder());

    /// <summary>
    /// Creates and returns a new builder for <see cref="FirefoxDriver"/>
    /// with default <see cref="WebDriverAliases.Firefox"/> alias.
    /// Sets this builder as a one to use for a driver creation.
    /// </summary>
    /// <returns>The <see cref="FirefoxDriverBuilder"/> instance.</returns>
    public FirefoxDriverBuilder UseFirefox() =>
        UseDriver(new FirefoxDriverBuilder());

    /// <summary>
    /// Creates and returns a new builder for <see cref="InternetExplorerDriver"/>
    /// with default <see cref="WebDriverAliases.InternetExplorer"/> alias.
    /// Sets this builder as a one to use for a driver creation.
    /// </summary>
    /// <returns>The <see cref="InternetExplorerDriverBuilder"/> instance.</returns>
    public InternetExplorerDriverBuilder UseInternetExplorer() =>
        UseDriver(new InternetExplorerDriverBuilder());

    /// <summary>
    /// Creates and returns a new builder for <see cref="EdgeDriver"/>
    /// with default <see cref="WebDriverAliases.Edge"/> alias.
    /// Sets this builder as a one to use for a driver creation.
    /// </summary>
    /// <returns>The <see cref="EdgeDriverBuilder"/> instance.</returns>
    public EdgeDriverBuilder UseEdge() =>
        UseDriver(new EdgeDriverBuilder());

    /// <summary>
    /// Creates and returns a new builder for <see cref="SafariDriver"/>
    /// with default <see cref="WebDriverAliases.Safari"/> alias.
    /// Sets this builder as a one to use for a driver creation.
    /// </summary>
    /// <returns>The <see cref="SafariDriverBuilder"/> instance.</returns>
    public SafariDriverBuilder UseSafari() =>
        UseDriver(new SafariDriverBuilder());

    /// <summary>
    /// Creates and returns a new builder for <see cref="RemoteWebDriver"/>
    /// with default <see cref="WebDriverAliases.Remote"/> alias.
    /// Sets this builder as a one to use for a driver creation.
    /// </summary>
    /// <returns>The <see cref="RemoteDriverBuilder"/> instance.</returns>
    public RemoteDriverBuilder UseRemoteDriver() =>
        UseDriver(new RemoteDriverBuilder());

    /// <summary>
    /// Returns an existing or creates a new builder for <see cref="ChromeDriver"/> by the specified alias.
    /// </summary>
    /// <param name="alias">
    /// The driver alias.
    /// The default value is <see cref="WebDriverAliases.Chrome"/>.
    /// </param>
    /// <returns>The <see cref="ChromeDriverBuilder"/> instance.</returns>
    public ChromeDriverBuilder ConfigureChrome(string alias = WebDriverAliases.Chrome) =>
        ConfigureDriver(
            alias,
            () => new ChromeDriverBuilder().WithAlias(alias));

    /// <summary>
    /// Returns an existing or creates a new builder for <see cref="FirefoxDriver"/> by the specified alias.
    /// </summary>
    /// <param name="alias">
    /// The driver alias.
    /// The default value is <see cref="WebDriverAliases.Firefox"/>.
    /// </param>
    /// <returns>The <see cref="FirefoxDriverBuilder"/> instance.</returns>
    public FirefoxDriverBuilder ConfigureFirefox(string alias = WebDriverAliases.Firefox) =>
        ConfigureDriver(
            alias,
            () => new FirefoxDriverBuilder().WithAlias(alias));

    /// <summary>
    /// Returns an existing or creates a new builder for <see cref="InternetExplorerDriver"/> by the specified alias.
    /// </summary>
    /// <param name="alias">
    /// The driver alias.
    /// The default value is <see cref="WebDriverAliases.InternetExplorer"/>.
    /// </param>
    /// <returns>The <see cref="InternetExplorerDriverBuilder"/> instance.</returns>
    public InternetExplorerDriverBuilder ConfigureInternetExplorer(string alias = WebDriverAliases.InternetExplorer) =>
        ConfigureDriver(
            alias,
            () => new InternetExplorerDriverBuilder().WithAlias(alias));

    /// <summary>
    /// Returns an existing or creates a new builder for <see cref="EdgeDriver"/> by the specified alias.
    /// </summary>
    /// <param name="alias">
    /// The driver alias.
    /// The default value is <see cref="WebDriverAliases.Edge"/>.
    /// </param>
    /// <returns>The <see cref="EdgeDriverBuilder"/> instance.</returns>
    public EdgeDriverBuilder ConfigureEdge(string alias = WebDriverAliases.Edge) =>
        ConfigureDriver(
            alias,
            () => new EdgeDriverBuilder().WithAlias(alias));

    /// <summary>
    /// Returns an existing or creates a new builder for <see cref="SafariDriver"/> by the specified alias.
    /// </summary>
    /// <param name="alias">
    /// The driver alias.
    /// The default value is <see cref="WebDriverAliases.Safari"/>.
    /// </param>
    /// <returns>The <see cref="SafariDriverBuilder"/> instance.</returns>
    public SafariDriverBuilder ConfigureSafari(string alias = WebDriverAliases.Safari) =>
        ConfigureDriver(
            alias,
            () => new SafariDriverBuilder().WithAlias(alias));

    /// <summary>
    /// Returns an existing or creates a new builder for <see cref="RemoteWebDriver"/> by the specified alias.
    /// </summary>
    /// <param name="alias">
    /// The driver alias.
    /// The default value is <see cref="WebDriverAliases.Remote"/>.
    /// </param>
    /// <returns>The <see cref="RemoteDriverBuilder"/> instance.</returns>
    public RemoteDriverBuilder ConfigureRemoteDriver(string alias = WebDriverAliases.Remote) =>
        ConfigureDriver(
            alias,
            () => new RemoteDriverBuilder().WithAlias(alias));

    /// <summary>
    /// Sets the default control visibility.
    /// The default value is <see cref="Visibility.Any"/>.
    /// </summary>
    /// <param name="visibility">The visibility.</param>
    /// <returns>The same <see cref="WebDriverSessionBuilder"/> instance.</returns>
    public WebDriverSessionBuilder UseDefaultControlVisibility(Visibility visibility)
    {
        DefaultControlVisibility = visibility;
        return this;
    }

    protected override void ConfigureSession(WebDriverSession session, AtataContext context)
    {
        base.ConfigureSession(session, context);

        session.DriverFactory = DriverFactoryToUse ?? DriverFactories.LastOrDefault();
        session.DisposeDriver = DisposeDriver;

        session.DefaultControlVisibility = DefaultControlVisibility;
    }

    protected override IScreenshotTaker CreateScreenshotTaker(WebDriverSession session) =>
        new ScreenshotTaker<WebDriverSession>(
            Screenshots.Strategy,
            WebDriverViewportScreenshotStrategy.Instance,
            FullPageOrViewportScreenshotStrategy.Instance,
            Screenshots.FileNameTemplate,
            session);

    protected override IPageSnapshotTaker CreatePageSnapshotTaker(WebDriverSession session) =>
        new PageSnapshotTaker<WebDriverSession>(
            PageSnapshots.Strategy,
            PageSnapshots.FileNameTemplate,
            session);

    protected override void OnClone(WebDriverSessionBuilder copy)
    {
        base.OnClone(copy);

        copy.DriverFactories = [.. DriverFactories];
        copy.Screenshots = Screenshots.Clone();
        copy.PageSnapshots = PageSnapshots.Clone();
    }
}