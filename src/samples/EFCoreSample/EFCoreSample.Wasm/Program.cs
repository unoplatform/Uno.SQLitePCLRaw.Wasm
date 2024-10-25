// #define ENABLE_EXCEPTIONS_LOGGING

using System;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using Uno;
using Uno.Extensions;
using Microsoft.UI.Xaml;
using System.Runtime.InteropServices;

namespace EFCoreSample.Wasm
{
	public class Program
	{
		private static App _app;

		static void Main(string[] args)
        {
#if ENABLE_EXCEPTIONS_LOGGING
            MonoInternals.mono_trace_enable(1);
			MonoInternals.mono_trace_set_options("E:all");
#endif

#if DEBUG
            InitializeLogging();
#endif  
            SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_sqlite3());

            Microsoft.UI.Xaml.Application.Start(_ => _app = new App());
		}
        /// <summary>
        /// Configures global Uno Platform logging
        /// </summary>
        private static void InitializeLogging()
        {
            var factory = LoggerFactory.Create(builder =>
            {
#if __WASM__
                builder.AddProvider(new global::Uno.Extensions.Logging.WebAssembly.WebAssemblyConsoleLoggerProvider());
#elif __IOS__
                builder.AddProvider(new global::Uno.Extensions.Logging.OSLogLoggerProvider());
#elif NETFX_CORE
                builder.AddDebug();
#else
                builder.AddConsole();
#endif

                // Exclude logs below this level
                builder.SetMinimumLevel(LogLevel.Trace);

                // Default filters for Uno Platform namespaces
                builder.AddFilter("Uno", LogLevel.Trace);
                builder.AddFilter("Windows", LogLevel.Trace);
                builder.AddFilter("Microsoft", LogLevel.Trace);

                // Generic Xaml events
                // builder.AddFilter("Microsoft.UI.Xaml", LogLevel.Debug );
                // builder.AddFilter("Microsoft.UI.Xaml.VisualStateGroup", LogLevel.Debug );
                // builder.AddFilter("Microsoft.UI.Xaml.StateTriggerBase", LogLevel.Debug );
                // builder.AddFilter("Microsoft.UI.Xaml.UIElement", LogLevel.Debug );
                // builder.AddFilter("Microsoft.UI.Xaml.FrameworkElement", LogLevel.Trace );

                // Layouter specific messages
                // builder.AddFilter("Microsoft.UI.Xaml.Controls", LogLevel.Debug );
                // builder.AddFilter("Microsoft.UI.Xaml.Controls.Layouter", LogLevel.Debug );
                // builder.AddFilter("Microsoft.UI.Xaml.Controls.Panel", LogLevel.Debug );

                // builder.AddFilter("Windows.Storage", LogLevel.Debug );

                // Binding related messages
                // builder.AddFilter("Microsoft.UI.Xaml.Data", LogLevel.Debug );
                // builder.AddFilter("Microsoft.UI.Xaml.Data", LogLevel.Debug );

                // Binder memory references tracking
                // builder.AddFilter("Uno.UI.DataBinding.BinderReferenceHolder", LogLevel.Debug );

                // RemoteControl and HotReload related
                // builder.AddFilter("Uno.UI.RemoteControl", LogLevel.Information);

                // Debug JS interop
                // builder.AddFilter("Uno.Foundation.WebAssemblyRuntime", LogLevel.Debug );
            });

            global::Uno.Extensions.LogExtensionPoint.AmbientLoggerFactory = factory;
        }
    }

    static class MonoInternals
    {
        [DllImport("__Native")]
        internal static extern void mono_trace_enable(int enable);

        [DllImport("__Native")]
        internal static extern int mono_trace_set_options(string options);
    }
}
