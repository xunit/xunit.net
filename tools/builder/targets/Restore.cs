using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Xunit.BuildTools.Models;

namespace Xunit.BuildTools.Targets;

[Target(BuildTarget.Restore)]
public static partial class RestoreTools
{
	public static async Task OnExecute(BuildContext context)
	{
		context.BuildStep("Restoring build environment");

		await context.Exec("dotnet", "tool restore");

		if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			await context.Exec("cmd", "/c npm install --no-fund");
		else
			await context.Exec("npm", "install --no-fund");
	}
}
