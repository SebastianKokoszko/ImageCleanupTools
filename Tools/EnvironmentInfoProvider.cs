namespace Tools
{
	using System;
	using System.Globalization;
	using System.Reflection;
	using System.Security;
	using System.Text;

	/// <summary>
	/// Class providing an information about current environment.
	/// </summary>
	public static class EnvironmentInfoProvider
	{
		/// <summary>
		///  The environment information.
		/// </summary>
		private static string environmentInfo = string.Empty;

		/// <summary>
		/// Gets the environment information. If <see cref="Initialize"/> was not called earlier returns the empty <see cref="string"/>.
		/// </summary>
		public static string EnvironmentInfo
		{
			get
			{
				return environmentInfo;
			}
		}

		/// <summary>
		/// Initializes the environment information.
		/// </summary>
		public static void Initialize()
		{
			StringBuilder builder = new StringBuilder();
			DateTime timeStamp = DateTime.Now;
			string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();

			builder.AppendLine($"Environment info collected at {timeStamp:yyyy-MM-dd HH:mm:ss},{timeStamp.Millisecond} ({Environment.TickCount})");

			builder.AppendLine($"Processor ID: {GetProcessorId()}");
			builder.AppendLine($"Processor architecture: {GetProcessorArchitecture()}");
			builder.AppendLine($"Processor count: {Environment.ProcessorCount}");

			builder.AppendLine($"CLR version: {Environment.Version}");

			builder.AppendLine($"OS version: {GetOSVersion()}");
			builder.AppendLine($"OS culture: {CultureInfo.InstalledUICulture.DisplayName}");

			builder.AppendLine($"Application culture: {CultureInfo.CurrentUICulture.DisplayName}");
			builder.AppendLine($"Application version: {version}");

			environmentInfo = builder.ToString();
		}

		/// <summary>
		/// Gets the OS information.
		/// </summary>
		/// <returns>The OS information.</returns>
		private static string GetOSVersion()
		{
			try
			{
				return Environment.OSVersion.VersionString;
			}
			catch (InvalidOperationException)
			{
				return string.Empty;
			}
		}

		/// <summary>
		/// Gets the processor architecture.
		/// </summary>
		/// <returns>The processor architecture.</returns>
		private static string GetProcessorArchitecture()
		{
			try
			{
				return System.Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE");
			}
			catch (SecurityException)
			{
				return string.Empty;
			}
		}

		/// <summary>
		/// Gets the processor identifier.
		/// </summary>
		/// <returns>The processor identifier.</returns>
		private static string GetProcessorId()
		{
			try
			{
				return System.Environment.GetEnvironmentVariable("PROCESSOR_IDENTIFIER");
			}
			catch (SecurityException)
			{
				return string.Empty;
			}
		}
	}
}
