namespace Tools
{
	using System;
	using System.Diagnostics;
	using System.Text;

	/// <summary>
	/// The trace class for the application.
	/// </summary>
	public sealed class Trace
	{
		/// <summary>
		/// Synchronization object.
		/// </summary>
		private static readonly object SyncRoot = new object();

		/// <summary>
		/// The single instance.
		/// </summary>
		private static volatile Trace instance;

		/// <summary>
		/// The trace source.
		/// </summary>
		private readonly TraceSource traceSource = new TraceSource(nameof(Trace));

		/// <summary>
		/// Indicates whether initial environment info was traced already.
		/// </summary>
		private bool initialInfoTraced;

		/// <summary>
		/// Prevents a default instance of the <see cref="Trace"/> class from being created.
		/// </summary>
		private Trace()
		{
		}

		/// <summary>
		/// Gets the single instance.
		/// </summary>
		public static Trace Instance
		{
			get
			{
				if (instance == null)
				{
					lock (SyncRoot)
					{
						if (instance == null)
						{
							instance = new Trace { IsClosed = false };
						}
					}
				}

				return instance;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the <see cref="Trace"/> is closed or not.
		/// </summary>
		public bool IsClosed
		{
			get;
			set;
		}

		/// <summary>
		/// Traces the warning message.
		/// </summary>
		/// <param name="message">The message.</param>
		public void TraceWarning(string message)
		{
			this.DoTrace(TraceEventType.Warning, message);
		}

		/// <summary>
		/// Traces the information message.
		/// </summary>
		/// <param name="message">The message.</param>
		public void TraceInformation(string message)
		{
			this.DoTrace(TraceEventType.Information, message);
		}

		/// <summary>
		/// Traces the error message.
		/// </summary>
		/// <param name="message">The message.</param>
		public void TraceError(string message)
		{
			this.DoTrace(TraceEventType.Error, message);
		}

		/// <summary>
		/// Traces the critical error message.
		/// </summary>
		/// <param name="exception">The traced <see cref="Exception"/></param>
		/// <param name="additionExceptionInfo">The additional message which will appear after time stamp.</param>
		/// <returns>The traced information.</returns>
		public string TraceError(Exception exception, string additionExceptionInfo = @"") => this.TraceException(exception, TraceEventType.Error, additionExceptionInfo);

		/// <summary>
		/// Traces the critical error message.
		/// </summary>
		/// <param name="message">The message.</param>
		public void TraceCritical(string message)
		{
			this.DoTrace(TraceEventType.Critical, message);
		}

		/// <summary>
		/// Traces the debug message.
		/// </summary>
		/// <param name="message">The message.</param>
		public void TraceDebug(string message)
		{
			this.DoTrace(TraceEventType.Verbose, message);
		}

		/// <summary>
		/// Traces the critical error message.
		/// </summary>
		/// <param name="exception">The traced <see cref="Exception"/></param>
		/// <param name="additionExceptionInfo">The additional message which will appear after time stamp.</param>
		/// <returns>The traced information.</returns>
		public string TraceCritical(Exception exception, string additionExceptionInfo = @"") => this.TraceException(exception, TraceEventType.Critical, additionExceptionInfo);

		/// <summary>
		/// The unified method of tracing exceptions.
		/// </summary>
		/// <param name="exception">The traced <see cref="Exception"/>.</param>
		/// <param name="eventType">The event type.</param>
		/// <param name="additionExceptionInfo">The additional message which will appear after time stamp.</param>
		/// <returns>The traced information.</returns>
		private string TraceException(Exception exception, TraceEventType eventType, string additionExceptionInfo = @"")
		{
			DateTime timeStamp = DateTime.Now;
			StringBuilder builder = new StringBuilder(2);
			string traceMessage;

			builder.Append($"{timeStamp:yyyy-MM-dd HH:mm:ss} - ");
			if (additionExceptionInfo.Length > 0)
			{
				builder.Append($"{additionExceptionInfo} - ");
			}

			builder.AppendLine($"{exception.GetType()} - {exception.Message}{exception.StackTrace}");

			if (exception.InnerException != null)
			{
				builder.AppendLine($"Inner exception: {exception.InnerException.GetType()} - {exception.InnerException.Message}{exception.InnerException.StackTrace}");
			}

			traceMessage = builder.ToString();

			this.DoTrace(eventType, traceMessage);
			return traceMessage;
		}

		/// <summary>
		/// Writes the information to the trace.
		/// </summary>
		/// <param name="eventType">The event type.</param>
		/// <param name="message">The traced message.</param>
		private void DoTrace(TraceEventType eventType, string message)
		{
			lock (SyncRoot)
			{
				if ((this.traceSource != null) && !this.IsClosed)
				{
					if (this.traceSource.Switch.ShouldTrace(eventType))
					{
						if (!this.initialInfoTraced)
						{
							this.TraceInitialInformation();
						}

						this.traceSource.TraceEvent(eventType, Environment.TickCount, message);
					}
				}
			}
		}

		/// <summary>
		/// Traces the initial information about environment.
		/// </summary>
		private void TraceInitialInformation()
		{
			DateTime timeStamp = DateTime.Now;
			SourceLevels configuredLevel = this.traceSource.Switch.Level;
			this.traceSource.Switch.Level = SourceLevels.Information;
			this.traceSource.TraceEvent(TraceEventType.Information, Environment.TickCount, $"Trace begins at: {timeStamp:yyyy-MM-dd HH:mm:ss},{timeStamp.Millisecond}{Environment.NewLine}{EnvironmentInfoProvider.EnvironmentInfo}");
			this.traceSource.Switch.Level = configuredLevel;
			this.initialInfoTraced = true;
		}
	}
}