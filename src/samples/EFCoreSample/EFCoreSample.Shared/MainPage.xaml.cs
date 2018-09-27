using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using EFCoreSample.Model;
using System.Threading.Tasks;
using System.Threading;
using System.Text;
using System.Reflection;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace EFCoreSample
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainPage : Page
	{
		public MainPage()
		{
			this.InitializeComponent();
			Console.SetOut(new TextBlockTextWriter(output));

			SetVersions();

#if __WASM__
			codeBlock.IsReadOnly = false;
#else
			codeBlock.Text = "/* Code edition is only available for WebAssembly */\n\n";
#endif

			var assembly = GetType().Assembly;

			var resourceName = assembly.GetManifestResourceNames().First(n => n.EndsWith("SampleClass.cs"));

			codeBlock.Text += new StreamReader(GetType().Assembly.GetManifestResourceStream(resourceName)).ReadToEnd();
		}

		private void SetVersions()
		{
			buildVersion.Text = GetAssemblyVersion(this.GetType());
#if __WASM__
			roslynVersion.Text = GetAssemblyVersion(typeof(Microsoft.CodeAnalysis.Compilation));
#else
			roslynVersion.Text = "Not used";
#endif
			efCoreVersion.Text = GetAssemblyVersion(typeof(Microsoft.EntityFrameworkCore.DbContext));
			sqLiteVersion.Text = SQLitePCL.raw.sqlite3_libversion_number().ToString();
		}

		private string GetAssemblyVersion(Type t)
			=> t.GetTypeInfo().Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? "Unkown";

		private async void Run_Click(object sender, RoutedEventArgs e)
		{
#if __WASM__
			await SampleRunner.RunSample(codeBlock.Text);
#else
			await SampleClass.Run();
#endif
		}

		private class TextBlockTextWriter : TextWriter
		{
			private readonly TextBlock _target;

			public TextBlockTextWriter(TextBlock target)
			{
				_target = target;
			}

			/// <summary>
			/// Writes a character to the text stream.
			/// </summary>
			/// <param name="value">The character to write to the text stream.</param>
			/// <exception cref="T:System.ObjectDisposedException">
			/// The <see cref="T:System.IO.TextWriter"/> is closed.
			/// </exception>
			/// <exception cref="T:System.IO.IOException">
			/// An I/O error occurs.
			/// </exception>
			public override void Write(char value)
			{
				var r = _target.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => _target.Text += value);
			}

			/// <summary>
			/// Writes a string to the text stream.
			/// </summary>
			/// <param name="value">The string to write.</param>
			/// <exception cref="T:System.ObjectDisposedException">
			/// The <see cref="T:System.IO.TextWriter"/> is closed.
			/// </exception>
			/// <exception cref="T:System.IO.IOException">
			/// An I/O error occurs.
			/// </exception>
			public override void Write(string value)
			{
				var r = _target.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => _target.Text += value);
			}

			/// <summary>
			/// Writes a string followed by a line terminator to the text stream.
			/// </summary>
			/// <param name="value">The string to write. If <paramref name="value"/> is null, only the line termination characters are written.</param>
			/// <exception cref="T:System.ObjectDisposedException">
			/// The <see cref="T:System.IO.TextWriter"/> is closed.
			/// </exception>
			/// <exception cref="T:System.IO.IOException">
			/// An I/O error occurs.
			/// </exception>
			public override void WriteLine(string value)
			{
				var r = _target.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => _target.Text += value + "\n");
			}

			/// <summary>
			/// When overridden in a derived class, returns the <see cref="T:System.Text.Encoding"/> in which the output is written.
			/// </summary>
			/// <value></value>
			/// <returns>
			/// The Encoding in which the output is written.
			/// </returns>
			public override System.Text.Encoding Encoding { get; } = new System.Text.UTF8Encoding(encoderShouldEmitUTF8Identifier: false);

		}
	}
}
