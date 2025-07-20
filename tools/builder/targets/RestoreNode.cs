using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Xunit.BuildTools.Models;

namespace Xunit.BuildTools.Targets;

[Target(BuildTarget.RestoreNode)]
public static partial class RestoreNode
{
	public static async Task OnExecute(BuildContext context)
	{
		context.BuildStep("Restoring NodeJS build environment");

		if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			await context.Exec("cmd", "/c npm install --no-fund");
		else
			await context.Exec("npm", "install --no-fund");
	}
}
