using System;
using VampireCommandFramework;

namespace KindredCommands.Commands;

internal static class DiscordCommands
{
	[Command("discord", shortHand: "dc", description: "Shows discord server.")]
	public static void PingCommand(ChatCommandContext ctx, string mode = "")
	{
		if (mode is null)
		{
			throw new ArgumentNullException(nameof(mode));
		}

		var discord = "asdfasdf";
		ctx.Reply($"The server discord: {discord}");
	}
}
