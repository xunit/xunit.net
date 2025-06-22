using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Xunit.BuildTools.Models;

namespace Xunit.BuildTools.Targets;

[Target(
	BuildTarget.Serve,
	BuildTarget.Restore
)]
public static class Serve
{
	public static async Task OnExecute(BuildContext context)
	{
		context.BuildStep("Building web site");

		if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			await context.Exec("cmd", "/c npm run watch");
		else
			await context.Exec("npm", "run watch");
	}
}
