#if __WASM__
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Host;

namespace EFCoreSample
{
    public class SampleRunner
    {
		public static async Task RunSample(string code)
		{
			Console.WriteLine($"Parsing tree...");
			await Task.Yield();
			var st = SyntaxFactory.ParseCompilationUnit(code);

			Compilation compilation = CSharpLanguage.Instance
			  .CreateLibraryCompilation(assemblyName: "InMemoryAssembly", enableOptimisations: false)
			  .AddSyntaxTrees(new[] { st.SyntaxTree });

			Console.WriteLine($"Got compilation");
			await Task.Yield();

			Console.WriteLine($"Emitting assembly...");
			var stream = new MemoryStream();
			var emitResult = compilation.Emit(stream);

			await Task.Yield();

			if (emitResult.Success)
			{
				Console.WriteLine($"Got binary assembly: {emitResult.Success}");

				var asm = Assembly.Load(stream.ToArray());
				if (asm
					.GetExportedTypes()
					.Where(et => et.GetMethod("Run") !=null)
					.FirstOrDefault() is Type runnerType)
				{
					Console.WriteLine("Got Runner type " + runnerType);
					if (runnerType.GetMethod("Run") is MethodInfo runMethod)
					{
						Console.WriteLine($"Running {runMethod} method");
						await Task.Yield();

						var res = runMethod.Invoke(null, null);

						switch (res)
						{
							case Task t:
								await t;
								break;
						}
					}
				}

				Console.WriteLine("Done running");
			}
			else
			{
				Console.WriteLine($"Failed to emit assembly:");

				foreach (var diagnostic in emitResult.Diagnostics)
				{
					Console.WriteLine(diagnostic);
				}
			}
		}

		public class CSharpLanguage : ILanguageService
		{
			private readonly IEnumerable<MetadataReference> _references;

			public static CSharpLanguage Instance { get; } = new CSharpLanguage();

			private CSharpLanguage()
			{
				_references = Directory.GetFiles("/managed", "*.dll")
					.Select(f => MetadataReference.CreateFromFile(f))
					.ToArray();
			}

			public Compilation CreateLibraryCompilation(string assemblyName, bool enableOptimisations)
			{
				var options = new CSharpCompilationOptions(
					OutputKind.DynamicallyLinkedLibrary,
					optimizationLevel: OptimizationLevel.Release,
					allowUnsafe: true)
					// Disabling concurrent builds allows for the emit to finish.
					.WithConcurrentBuild(false)
					;

				return CSharpCompilation.Create(assemblyName, options: options, references: _references);
			}
		}
	}
}
#endif