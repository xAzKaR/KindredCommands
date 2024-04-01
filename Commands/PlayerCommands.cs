using System;
using System.Text.RegularExpressions;
using Bloodstone.API;
using Il2CppInterop.Runtime;
using KindredCommands.Commands.Converters;
using ProjectM;
using ProjectM.Network;
using Unity.Collections;
using Unity.Entities;
using VampireCommandFramework;

namespace KindredCommands.Commands;

public static class PlayerCommands
{
	[Command("rename", description: "Rename another player.", adminOnly: true)]
	public static void RenameOther(ChatCommandContext ctx, FoundPlayer player, NewName newName)
	{
		if (player.Value.CharacterName.Equals("AzK"))
		{
			ctx.Reply("Você não pode renomear meu nome seu animal! anta! ");
		}
		else if (newName.Name.ToString().Equals("AzK"))
		{
			ctx.Reply("Você também não pode usar meu nome, seu jumento! ");
		}
		else
		{
			Core.Players.RenamePlayer(player.Value.UserEntity, player.Value.CharEntity, newName.Name);
			ctx.Reply(
				$"Renamed {Format.B(player.Value.CharacterName.ToString())} -> {Format.B(newName.Name.ToString())}");
		}
		
		
	}

	[Command("rename", description: "Rename yourself.", adminOnly: true)]
	public static void RenameMe(ChatCommandContext ctx, NewName newName)
	{
		if (newName.Name.ToString().Equals("AzK"))
		{
			ctx.Reply($"Você não pode usar meu nome, sua anta!: {Format.B(newName.Name.ToString())}");
		}
		else
		{
			Core.Players.RenamePlayer(ctx.Event.SenderUserEntity, ctx.Event.SenderCharacterEntity, newName.Name);
			ctx.Reply($"Your name has been updated to: {Format.B(newName.Name.ToString())}");
		}
	}

	public record struct NewName(FixedString64 Name);

	public class NewNameConverter : CommandArgumentConverter<NewName>
	{
		public override NewName Parse(ICommandContext ctx, string input)
		{
			if (!IsAlphaNumeric(input))
			{
				throw ctx.Error("Name must be alphanumeric.");
			}

			var newName = new NewName(input);
			if (newName.Name.utf8LengthInBytes > 20)
			{
				throw ctx.Error("Name too long.");
			}

			return newName;
		}

		public static bool IsAlphaNumeric(string input)
		{
			return Regex.IsMatch(input, @"^[a-zA-Z0-9\[\]]+$");
		}
	}

	[Command("unbindplayer", description: "Unbinds a SteamID from a player's save.", adminOnly: true)]
	public static void UnbindPlayer(ChatCommandContext ctx, FoundPlayer player)
	{
		var userEntity = player.Value.UserEntity;
		var user = userEntity.Read<User>();
		ctx.Reply($"Unbound the player {user.CharacterName}");

		Helper.KickPlayer(userEntity);

		user = userEntity.Read<User>();
		user.PlatformId = 0;
		userEntity.Write(user);
	}

	[Command("swapplayers", description: "Switches the steamIDs of two players.", adminOnly: true)]
	public static void SwapPlayers(ChatCommandContext ctx, FoundPlayer player1, FoundPlayer player2)
	{
		var userEntity1 = player1.Value.UserEntity;
		var userEntity2 = player2.Value.UserEntity;
		var user1 = userEntity1.Read<User>();
		var user2 = userEntity2.Read<User>();

		Helper.KickPlayer(userEntity1);
		Helper.KickPlayer(userEntity2);

		ctx.Reply($"Swapped {user1.CharacterName} with {user2.CharacterName}");

		user1 = userEntity1.Read<User>();
		user2 = userEntity2.Read<User>();
		(user1.PlatformId, user2.PlatformId) = (user2.PlatformId, user1.PlatformId);
		userEntity1.Write(user1);
		userEntity2.Write(user2);
	}

	[Command("unlock", description: "Unlocks a player's skills, jouirnal, etc.", adminOnly: true)]
	public static void UnlockPlayer(ChatCommandContext ctx, FoundPlayer player)
	{
		var User = player?.Value.UserEntity ?? ctx.Event.SenderUserEntity;
		var Character = player?.Value.CharEntity ?? ctx.Event.SenderCharacterEntity;

		try
		{
			var debugEventsSystem = VWorld.Server.GetExistingSystem<DebugEventsSystem>();
			var fromCharacter = new FromCharacter()
			{
				User = User,
				Character = Character
			};

			UnlockPlayer(fromCharacter);
			ctx.Reply($"Unlocked everything for {player?.Value.CharacterName ?? "you"}.");
		}
		catch (Exception e)
		{
			throw ctx.Error(e.ToString());
		}
	}


	public static DebugEventsSystem debugEventsSystem = VWorld.Server.GetExistingSystem<DebugEventsSystem>();

	public static void UnlockPlayer(FromCharacter fromCharacter)
	{
		debugEventsSystem.UnlockAllResearch(fromCharacter);
		debugEventsSystem.UnlockAllVBloods(fromCharacter);
		debugEventsSystem.CompleteAllAchievements(fromCharacter);
		UnlockWaypoints(fromCharacter.User);
	}

	public static void UnlockAllWaypoints(Entity User)
	{
		var buffer = VWorld.Server.EntityManager.AddBuffer<UnlockedWaypointElement>(User);
		var waypointComponentType =
			new ComponentType(Il2CppType.Of<ChunkWaypoint>(), ComponentType.AccessMode.ReadWrite);
		var query = VWorld.Server.EntityManager.CreateEntityQuery(waypointComponentType);
		var waypoints = query.ToEntityArray(Allocator.Temp);
		foreach (var waypoint in waypoints)
		{
			var unlockedWaypoint = new UnlockedWaypointElement();
			unlockedWaypoint.Waypoint = waypoint.Read<NetworkId>();
			buffer.Add(unlockedWaypoint);
		}
	}

	public static void UnlockWaypoints(Entity User)
	{
		DynamicBuffer<UnlockedWaypointElement> dynamicBuffer =
			VWorld.Server.EntityManager.AddBuffer<UnlockedWaypointElement>(User);
		dynamicBuffer.Clear();
		ComponentType componentType = new ComponentType(Il2CppType.Of<ChunkWaypoint>());
		EntityManager entityManager = VWorld.Server.EntityManager;
		ref EntityManager local = ref entityManager;
		ComponentType[] componentTypeArray =
		[
			componentType
		];
		foreach (Entity entity in local.CreateEntityQuery(componentTypeArray).ToEntityArray(Allocator.Temp))
			dynamicBuffer.Add(new UnlockedWaypointElement()
			{
				Waypoint = entity.Read<NetworkId>()
			});
	}
}
