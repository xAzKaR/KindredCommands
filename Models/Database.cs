using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Il2CppSystem;
using ProjectM.Network;
using Unity.Entities;

namespace KindredCommands.Models;

public readonly struct Database
{
	private static readonly string CONFIG_PATH = Path.Combine(BepInEx.Paths.ConfigPath, "KindredCommands");
	private static readonly string STAFF_PATH = Path.Combine(CONFIG_PATH, "staff.json");
	private static readonly string NOSPAWN_PATH = Path.Combine(CONFIG_PATH, "nospawn.json");
	private static readonly string VIP_PATH = Path.Combine(CONFIG_PATH, "vip.json");

	public static void InitConfig()
	{
		string json;
		Dictionary<string, string> dict;
		Dictionary<string, Dictionary<string, string>> vip;

		STAFF.Clear();
		NOSPAWN.Clear();
		VIP.Clear();

		if (File.Exists(STAFF_PATH))
		{
			json = File.ReadAllText(STAFF_PATH);
			dict = JsonSerializer.Deserialize<Dictionary<string, string>>(json);

			foreach (var kvp in dict)
			{
				STAFF[kvp.Key] = kvp.Value;
			}
		}
		else
		{
			SaveStaff();
		}

		if (File.Exists(VIP_PATH))
		{
			json = File.ReadAllText(VIP_PATH);
			vip = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string>>>(json);

			foreach (var kvp in vip)
			{
				VIP[kvp.Key] = kvp.Value;
			}
		}
		else
		{
			SaveVip();
		}

		if (File.Exists(NOSPAWN_PATH))
		{
			json = File.ReadAllText(NOSPAWN_PATH);
			dict = JsonSerializer.Deserialize<Dictionary<string, string>>(json);

			foreach (var kvp in dict)
			{
				NOSPAWN[kvp.Key] = kvp.Value;
			}
		}
		else
		{
			NOSPAWN["CHAR_VampireMale"] = "it causes corruption to the save file.";
			NOSPAWN["CHAR_Mount_Horse_Gloomrot"] = "it causes an instant server crash.";
			NOSPAWN["CHAR_Mount_Horse_Vampire"] = "it causes an instant server crash.";
			SaveNoSpawn();
		}
	}

	static void WriteConfig(string path, Dictionary<string, string> dict)
	{
		if (!Directory.Exists(CONFIG_PATH)) Directory.CreateDirectory(CONFIG_PATH);
		var json = JsonSerializer.Serialize(dict, new JsonSerializerOptions { WriteIndented = true });
		File.WriteAllText(path, json);
	}

	static void WriteConfigVip(string path, Dictionary<string, Dictionary<string, string>> vip)
	{
		if (!Directory.Exists(CONFIG_PATH)) Directory.CreateDirectory(CONFIG_PATH);
		var json = JsonSerializer.Serialize(vip, new JsonSerializerOptions { WriteIndented = true });
		File.WriteAllText(path, json);
	}

	static public void SaveStaff()
	{
		WriteConfig(STAFF_PATH, STAFF);
	}

	static public void SaveVip()
	{
		WriteConfigVip(VIP_PATH, VIP);
	}

	static public void SaveNoSpawn()
	{
		WriteConfig(NOSPAWN_PATH, NOSPAWN);
	}

	static public void SetStaff(Entity userEntity, string rank)
	{
		var user = userEntity.Read<User>();
		STAFF[user.PlatformId.ToString()] = rank;
		SaveStaff();
		Core.Log.LogWarning($"User {user.CharacterName} added to staff config as {rank}.");
	}

	static public void SetVip(Entity userEntity, string rank, string steamId)
	{
		Player player = new(userEntity);
		var user = userEntity.Read<User>();

		DateTime time = DateTime.Now;

		//Verificar se existe
		if (!VIP.ContainsKey(steamId))
		{
			VIP[steamId] = new Dictionary<string, string>();
		}

		VIP[player.SteamID.ToString()]["Rank"] = user.PlatformId.ToString();

		SaveVip();
		Core.Log.LogWarning($"User {user.CharacterName} added to Vip config as {rank}, até a data {time}.");
	}

	static public void SetNoSpawn(string prefabName, string reason)
	{
		NOSPAWN[prefabName] = reason;
		SaveNoSpawn();
		Core.Log.LogWarning($"NPC {prefabName} is banned from spawning because {reason}.");
	}

	static public bool IsSpawnBanned(string prefabName, out string reason)
	{
		return NOSPAWN.TryGetValue(prefabName, out reason);
	}

	private static readonly Dictionary<string, string> STAFF = new()
	{
		{ "SteamID1", "[Rank]" },
		{ "SteamID2", "[Rank]" }
	};

	private static readonly Dictionary<string, string> NOSPAWN = new()
	{
		{ "PrefabGUID", "Reason" }
	};

	private static readonly Dictionary<string, Dictionary<string, string>> VIP = new()
	{
		{ "StreamID1", new Dictionary<string, string> { { "Rank", "[Rank]" }, { "Month", "[Month]" } } },
		{ "StreamID2", new Dictionary<string, string> { { "Rank", "[Rank]" }, { "Month", "[Month]" } } }
	};

	public static Dictionary<string, string> GetStaff()
	{
		return STAFF;
	}

	public static Dictionary<string, Dictionary<string, string>> GetVip()
	{
		return VIP;
	}

	public static bool RemoveStaff(Entity userEntity)
	{
		var removed = STAFF.Remove(userEntity.Read<User>().PlatformId.ToString());
		if (removed)
		{
			SaveStaff();
			Core.Log.LogWarning($"User {userEntity.Read<User>().CharacterName} removed from staff config.");
		}
		else
		{
			Core.Log.LogInfo(
				$"User {userEntity.Read<User>().CharacterName} attempted to be removed from staff config but wasn't there.");
		}

		return removed;
	}

	public static bool RemoveVip(Entity userEntity)
	{
		var removed = VIP.Remove(userEntity.Read<User>().PlatformId.ToString());
		if (removed)
		{
			SaveVip();
			Core.Log.LogWarning($"User {userEntity.Read<User>().CharacterName} Removed from vip config.");
		}
		else
		{
			Core.Log.LogInfo(
				$"User {userEntity.Read<User>().CharacterName} attempted to be removed from vip config but wasn't there.");
		}

		return removed;
	}
}
