using System;
using System.IO;
using System.Threading.Tasks;
using Xunit.BuildTools.Models;

namespace Xunit.BuildTools.Targets;

[Target(
	BuildTarget.Build,
	BuildTarget.RestoreDotNet
)]
public static class Build
{
	public static async Task OnExecute(BuildContext context)
	{
		context.BuildStep("Building web site");

		await context.Exec("dotnet", "docfx site/docfx.json");

		var srcFolder = Path.Combine(context.BaseFolder, "site");
		var dstFolder = Path.Combine(context.BaseFolder, ".site");
		var anyCopied = false;

		foreach (var srcFile in Directory.GetFiles(srcFolder, "*.md", SearchOption.AllDirectories))
		{
			var dstFile = srcFile.Replace(srcFolder, dstFolder);

			if (srcFile != dstFile)
			{
				var shouldCopy = false;

				if (!File.Exists(dstFile))
					shouldCopy = true;
				else
				{
					var srcDate = File.GetLastWriteTimeUtc(srcFile);
					var dstDate = File.GetLastWriteTimeUtc(dstFile);

					shouldCopy = dstDate < srcDate;
				}

				if (shouldCopy)
				{
					anyCopied = true;

					context.WriteLineColor(ConsoleColor.DarkGray, $"COPY: {srcFile[(context.BaseFolder.Length + 6)..]}");
					File.Copy(srcFile, dstFile);
				}
			}
		}

		if (anyCopied)
			Console.WriteLine();
	}
}
