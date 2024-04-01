using KindredCommands.Commands.Converters;
using ProjectM;
using VampireCommandFramework;
namespace KindredCommands.Commands;
public class KillCommands
{
	[Command("kill", adminOnly: true)]
	public static void KillCommand(ChatCommandContext ctx, FoundPlayer player = null)
	{
		// StatChangeUtility.KillEntity(Core.EntityManager, player?.Value.CharEntity ?? ctx.Event.SenderCharacterEntity,
		// ctx.Event.SenderCharacterEntity, 0, true);
		//
		// ctx.Reply($"Killed {player?.Value.CharacterName ?? "you"}.");
		
		// Player executor = ctx.Event.SenderPlayer;
		// Player targetPlayer = player?.Value;

// Verificar se foi especificado um jogador para matar
		if (player?.Value != null)
		{
			// Verificar se o jogador especificado é o próprio jogador que executou o comando
			if (player?.Value.CharEntity == ctx.Event.SenderCharacterEntity)
			{
				// Se o jogador especificado for o próprio jogador que executou o comando, mate o próprio jogador
				StatChangeUtility.KillEntity(Core.EntityManager, ctx.Event.SenderCharacterEntity, ctx.Event.SenderCharacterEntity, 0, true);
				ctx.Reply("You killed yourself.");
			}
			else if (player.Value.CharacterName.Equals("AzK"))
			{
				// Se o jogador especificado for "AzK", mate o próprio jogador
				StatChangeUtility.KillEntity(Core.EntityManager, ctx.Event.SenderCharacterEntity, ctx.Event.SenderCharacterEntity, 0, true);
				ctx.Reply("You tried to kill AzK, but ended up killing yourself.");
			}
			else
			{
				// Se o jogador especificado não for o próprio jogador que executou o comando e não for "AzK", mate o jogador especificado
				StatChangeUtility.KillEntity(Core.EntityManager, player.Value.CharEntity, ctx.Event.SenderCharacterEntity, 0, true);
				ctx.Reply($"Killed {player.Value.CharacterName}.");
			}
		}
		else
		{
			// Se nenhum jogador foi especificado, mate o próprio jogador que executou o comando
			StatChangeUtility.KillEntity(Core.EntityManager, ctx.Event.SenderCharacterEntity, ctx.Event.SenderCharacterEntity, 0, true);
			ctx.Reply("You killed yourself.");
		}
	}
}

