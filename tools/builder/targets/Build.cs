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
	}
}
