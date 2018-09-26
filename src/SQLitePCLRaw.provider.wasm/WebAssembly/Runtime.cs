using System;
using System.Text;

namespace WebAssembly
{
	internal sealed class Runtime
	{
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.InternalCall)]
		private static extern string InvokeJS(string str, out int exceptional_result);

		internal static string InvokeJS(string str)
		{
			var escaped = str;

			var r = InvokeJS(escaped, out var exceptionResult);
			if (exceptionResult != 0)
			{
				Console.Error.WriteLine($"Error #{exceptionResult} \"{r}\" executing javascript: \"{str}\"");
			}
			else
			{
				// Console.WriteLine($"InvokeJS: [{str}]: {r}");
			}
			return r;
		}

		public static string EscapeJs(string s)
		{
			if (s == null)
			{
				return "";
			}

			bool NeedsEscape(string s2)
			{
				for (int i = 0; i < s2.Length; i++)
				{
					var c = s2[i];

					if (
						c > 255
						|| c < 32
						|| c == '\\'
						|| c == '"'
						|| c == '\r'
						|| c == '\n'
						|| c == '\t'
					)
					{
						return true;
					}
				}

				return false;
			}

			if (NeedsEscape(s))
			{
				var r = new StringBuilder(s.Length);

				foreach (var c in s)
				{
					switch (c)
					{
						case '\\':
							r.Append("\\\\");
							continue;
						case '"':
							r.Append("\\\"");
							continue;
						case '\r':
							continue;
						case '\n':
							r.Append("\\n");
							continue;
						case '\t':
							r.Append("\\t");
							continue;
					}

					if (c < 32)
					{
						continue; // not displayable
					}

					if (c <= 255)
					{
						r.Append(c);
					}
					else
					{
						r.Append("\\u");
						r.Append(((ushort)c).ToString("X4"));
					}
				}

				return r.ToString();
			}
			else
			{
				return s;
			}
		}

	}
}
