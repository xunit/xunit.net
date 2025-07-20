using System.Threading.Tasks;
using Xunit.BuildTools.Models;

namespace Xunit.BuildTools.Targets;

[Target(BuildTarget.RestoreDotNet)]
public static partial class RestoreDotNet
{
	public static async Task OnExecute(BuildContext context)
	{
		context.BuildStep("Restoring .NET build environment");

		await context.Exec("dotnet", "tool restore");
	}
}
