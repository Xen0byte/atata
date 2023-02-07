﻿using System;

namespace Atata
{
    /// <summary>
    /// Provides reporting functionality.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner.</typeparam>
    public class Report<TOwner>
    {
        private readonly TOwner _owner;

        private readonly AtataContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="Report{TOwner}"/> class.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="context">The context.</param>
        public Report(TOwner owner, AtataContext context)
        {
            _owner = owner;
            _context = context;
        }

        /// <summary>
        /// Gets the associated <see cref="AtataContext"/> instance.
        /// </summary>
        public AtataContext Context => _context;

        /// <summary>
        /// Writes the log message at the <see cref="LogLevel.Trace"/> level optionally using the specified <paramref name="args"/> as a message format parameters.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>The instance of the owner object.</returns>
        public TOwner Trace(string message, params object[] args)
        {
            _context.Log.Trace(message, args);
            return _owner;
        }

        /// <summary>
        /// Writes the log message at the <see cref="LogLevel.Debug"/> level optionally using the specified <paramref name="args"/> as a message format parameters.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>The instance of the owner object.</returns>
        public TOwner Debug(string message, params object[] args)
        {
            _context.Log.Debug(message, args);
            return _owner;
        }

        /// <summary>
        /// Writes the log message at the <see cref="LogLevel.Info"/> level optionally using the specified <paramref name="args"/> as a message format parameters.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>The instance of the owner object.</returns>
        public TOwner Info(string message, params object[] args)
        {
            _context.Log.Info(message, args);
            return _owner;
        }

        /// <summary>
        /// Writes the log message at the <see cref="LogLevel.Warn"/> level optionally using the specified <paramref name="args"/> as a message format parameters.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>The instance of the owner object.</returns>
        public TOwner Warn(string message, params object[] args)
        {
            _context.Log.Warn(message, args);
            return _owner;
        }

        /// <summary>
        /// Writes the exception at the <see cref="LogLevel.Warn"/> level.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns>The instance of the owner object.</returns>
        public TOwner Warn(Exception exception)
        {
            _context.Log.Warn(exception);
            return _owner;
        }

        /// <summary>
        /// Writes the log message and optionally exception at the <see cref="LogLevel.Warn"/> level.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        /// <returns>The instance of the owner object.</returns>
        public TOwner Warn(string message, Exception exception = null)
        {
            _context.Log.Warn(message, exception);
            return _owner;
        }

        /// <summary>
        /// Writes the exception at the <see cref="LogLevel.Error"/> level.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns>The instance of the owner object.</returns>
        public TOwner Error(Exception exception)
        {
            _context.Log.Error(exception);
            return _owner;
        }

        /// <summary>
        /// Writes the log message and optionally exception at the <see cref="LogLevel.Error"/> level.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        /// <returns>The instance of the owner object.</returns>
        public TOwner Error(string message, Exception exception = null)
        {
            _context.Log.Error(message, exception);
            return _owner;
        }

        /// <summary>
        /// Writes the log message and stack trace at the <see cref="LogLevel.Error"/> level.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="stackTrace">The stack trace.</param>
        /// <returns>The instance of the owner object.</returns>
        public TOwner Error(string message, string stackTrace)
        {
            _context.Log.Error(message, stackTrace);
            return _owner;
        }

        /// <summary>
        /// Writes the exception at the <see cref="LogLevel.Fatal"/> level.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns>The instance of the owner object.</returns>
        public TOwner Fatal(Exception exception)
        {
            _context.Log.Fatal(exception);
            return _owner;
        }

        /// <summary>
        /// Writes the log message and optionally exception at the <see cref="LogLevel.Fatal"/> level.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        /// <returns>The instance of the owner object.</returns>
        public TOwner Fatal(string message, Exception exception = null)
        {
            _context.Log.Fatal(message, exception);
            return _owner;
        }

        /// <summary>
        /// Starts the specified log section.
        /// </summary>
        /// <param name="section">The log section.</param>
        /// <returns>The instance of the owner object.</returns>
        public TOwner Start(LogSection section)
        {
            _context.Log.Start(section);
            return _owner;
        }

        /// <summary>
        /// Starts the specified log section with message.
        /// </summary>
        /// <param name="sectionMessage">The section message.</param>
        /// <returns>The instance of the owner object.</returns>
        public TOwner Start(string sectionMessage)
        {
            _context.Log.Start(sectionMessage);
            return _owner;
        }

        /// <summary>
        /// Starts the specified log section with message and log level.
        /// </summary>
        /// <param name="sectionMessage">The section message.</param>
        /// <param name="level">The level.</param>
        /// <returns>The instance of the owner object.</returns>
        public TOwner Start(string sectionMessage, LogLevel level)
        {
            _context.Log.Start(sectionMessage, level);
            return _owner;
        }

        /// <summary>
        /// Ends the latest log section.
        /// </summary>
        /// <returns>The instance of the owner object.</returns>
        public TOwner EndSection()
        {
            _context.Log.EndSection();
            return _owner;
        }

        /// <summary>
        /// Executes the specified action and represents it in a log as a setup section with the specified message.
        /// The setup action time is not counted as pure test execution time.
        /// </summary>
        /// <param name="message">The setup message.</param>
        /// <param name="action">The setup action.</param>
        /// <returns>The instance of the owner object.</returns>
        public TOwner Setup(string message, Action<TOwner> action)
        {
            message.CheckNotNullOrEmpty(nameof(message));
            action.CheckNotNull(nameof(action));

            _context.Log.ExecuteSection(message, () =>
            {
                bool shouldStopPureExecutionStopwatch = _context.PureExecutionStopwatch.IsRunning;
                if (shouldStopPureExecutionStopwatch)
                    _context.PureExecutionStopwatch.Stop();

                action.Invoke(_owner);

                if (shouldStopPureExecutionStopwatch)
                    _context.PureExecutionStopwatch.Start();
            });
            return _owner;
        }

        /// <summary>
        /// Executes the specified function and represents it in a log as a setup section with the specified message.
        /// The setup function time is not counted as pure test execution time.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="message">The setup message.</param>
        /// <param name="function">The setup function.</param>
        /// <returns>The result of the <paramref name="function"/>.</returns>
        public TResult Setup<TResult>(string message, Func<TOwner, TResult> function)
        {
            message.CheckNotNullOrEmpty(nameof(message));
            function.CheckNotNull(nameof(function));

            TResult result = default;

            _context.Log.ExecuteSection(message, () =>
            {
                bool shouldStopPureExecutionStopwatch = _context.PureExecutionStopwatch.IsRunning;
                if (shouldStopPureExecutionStopwatch)
                    _context.PureExecutionStopwatch.Stop();

                result = function.Invoke(_owner);

                if (shouldStopPureExecutionStopwatch)
                    _context.PureExecutionStopwatch.Start();
            });

            return result;
        }

        /// <summary>
        /// Executes the specified action and represents it in a log as a section with the specified message.
        /// </summary>
        /// <param name="message">The step message.</param>
        /// <param name="action">The step action.</param>
        /// <returns>The instance of the owner object.</returns>
        public TOwner Step(string message, Action<TOwner> action)
        {
            message.CheckNotNullOrEmpty(nameof(message));
            action.CheckNotNull(nameof(action));

            _context.Log.ExecuteSection(message, () => action.Invoke(_owner));
            return _owner;
        }

        /// <summary>
        /// Executes the specified function and represents it in a log as a section with the specified message.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="message">The step message.</param>
        /// <param name="function">The step function.</param>
        /// <returns>The result of the <paramref name="function"/>.</returns>
        public TResult Step<TResult>(string message, Func<TOwner, TResult> function)
        {
            message.CheckNotNullOrEmpty(nameof(message));
            function.CheckNotNull(nameof(function));

            TResult result = default;
            _context.Log.ExecuteSection(message, (Action)(() => result = function.Invoke(_owner)));

            return result;
        }

        /// <summary>
        /// Takes a screenshot with the optionally specified title.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <returns>The instance of the owner object.</returns>
        public TOwner Screenshot(string title = null)
        {
            _context.TakeScreenshot(title);
            return _owner;
        }

        /// <summary>
        /// Takes a snapshot (HTML or MHTML file) of current page with the specified title optionally.
        /// </summary>
        /// <param name="title">The title of a snapshot.</param>
        /// <returns>The instance of the owner object.</returns>
        public TOwner PageSnapshot(string title = null)
        {
            _context.TakePageSnapshot(title);
            return _owner;
        }
    }
}
