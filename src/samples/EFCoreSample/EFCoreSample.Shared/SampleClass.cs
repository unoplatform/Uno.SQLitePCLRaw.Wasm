using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EFCoreSample
{
	public class SampleClass
	{
		public static async Task Run()
		{
			using (var db = new BloggingContext())
			{
				db.Database.EnsureCreated();

				Console.WriteLine("Database created");

				db.Blogs.Add(new Blog { Url = "http://blogs.msdn.com/adonet" });
				var count = await db.SaveChangesAsync(CancellationToken.None);

				Console.WriteLine("{0} records saved to database", count);

				Console.WriteLine();
				Console.WriteLine("All blogs in database:");
				foreach (var blog in db.Blogs)
				{
					Console.WriteLine(" - {0}", blog.Url);
				}
			}
		}
	}

	public class Blog
	{
		public int BlogId { get; set; }
		public string Url { get; set; }

		public List<Post> Posts { get; set; }
	}

	public class Post
	{
		public int PostId { get; set; }
		public string Title { get; set; }
		public string Content { get; set; }

		public int BlogId { get; set; }
		public Blog Blog { get; set; }
	}

	public class BloggingContext : DbContext
	{
		public DbSet<Blog> Blogs { get; set; }
		public DbSet<Post> Posts { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			// Uncomment those to enable logging
			// optionsBuilder.UseLoggerFactory(LogExtensionPoint.AmbientLoggerFactory);
			// optionsBuilder.EnableSensitiveDataLogging(true);

			optionsBuilder.UseSqlite($"data source=local.db");
		}
	}
}
