using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace DataAccess.EFCore
{
	public static class ConfigurationHelpers
	{
		public static string GetConnectionString()
		{
			var builder = new ConfigurationBuilder();
			var path = Path.Combine(Directory.GetCurrentDirectory(), "dbconfig.json");
			builder.AddJsonFile(path);

			var config = builder.Build();

			return config.GetConnectionString("DefaultConnection");
		}
	}
}
