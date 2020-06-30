using System;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using Uno.Extensions;
using Windows.UI.Xaml;

namespace EFCoreSample.Wasm
{
	public class Program
	{
		private static App _app;

		static void Main(string[] args)
		{
#if DEBUG
            ConfigureFilters(LogExtensionPoint.AmbientLoggerFactory);
#endif
			SQLitePCL.Batteries.Init();
			
			Windows.UI.Xaml.Application.Start(_ => _app = new App());
		}

		static void ConfigureFilters(ILoggerFactory factory)
		{
			factory
				.WithFilter(new FilterLoggerSettings
					{
						{ "Uno", LogLevel.Warning },
						{ "Windows", LogLevel.Warning },
						{ "Windows.ApplicationModel.Resources.ResourceLoader", LogLevel.Debug },

						// Generic Xaml events
						//{ "Windows.UI.Xaml", LogLevel.Debug },
						// { "Windows.UI.Xaml.Shapes", LogLevel.Debug },
						//{ "Windows.UI.Xaml.VisualStateGroup", LogLevel.Debug },
						//{ "Windows.UI.Xaml.StateTriggerBase", LogLevel.Debug },
						// { "Windows.UI.Xaml.UIElement", LogLevel.Debug },
						// { "Windows.UI.Xaml.Setter", LogLevel.Debug },
						   
						// Layouter specific messages
						// { "Windows.UI.Xaml.Controls", LogLevel.Debug },
						//{ "Windows.UI.Xaml.Controls.Layouter", LogLevel.Debug },
						//{ "Windows.UI.Xaml.Controls.Panel", LogLevel.Debug },
						   
						// Binding related messages
						// { "Windows.UI.Xaml.Data", LogLevel.Debug },
						// { "Windows.UI.Xaml.Data", LogLevel.Debug },
						   
						//  Binder memory references tracking
						// { "ReferenceHolder", LogLevel.Debug },
					}
				)
				.AddConsole(LogLevel.Debug);
		}
	}
}
