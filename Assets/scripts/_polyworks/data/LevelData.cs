using System;
using UnityEngine;

/*
 * LevelData contains state information for the scene, including currentSection
 * SecneData contains general information on scene, not stored in persistent GameData
 */
namespace Polyworks
{
	[System.Serializable]
	public class LevelData {
		public string name = "";
		public bool isCleared = false;
		public int currentSection = 0;
		public LevelTaskData tasks;
	}

	public class LevelUtils
	{
		public static bool Has(string scene, LevelData[] levels) {
			LevelData level = GetLevel (scene, levels);
			if (level != null) {
				return true;
			}
			return false;
		}

		public static void SetIsCleared(string scene, LevelData[] levels) {
			LevelData level = GetLevel (scene, levels);
			if (level != null) {
				level.isCleared = true;
			}
		}

		public static void SetLevelData(string scene, LevelData[] levels, LevelData data) {
			LevelData level = GetLevel (scene, levels);
			if (level != null) {
				level = data;
			}
		}

		public static bool GetIsCleared(string scene, LevelData[] levels) {
			LevelData level = GetLevel (scene, levels);
			if (level != null) {
				return level.isCleared;
			}
			return false;
		}

		public static LevelData GetLevel(string scene, LevelData[] levels) {
			for (int i = 0; i < levels.Length; i++) {
				if (levels [i].name == scene) {
					return levels [i];
				}
			}
			return null;
		}
	}
}

