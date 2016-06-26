using System;

namespace Polyworks
{
	[System.Serializable]
	public class LevelData {
		public string name;
		public bool isCleared;
		public LevelTaskData tasks;
	}

	public class LevelUtils
	{
		public static bool Has(string name, LevelData[] levels) {
			LevelData level = GetLevel (name, levels);
			if (level != null) {
				return true;
			}
			return false;
		}

		public static void SetIsCleared(string name, LevelData[] levels) {
			LevelData level = GetLevel (name, levels);
			if (level != null) {
				level.isCleared = true;
			}
		}

		public static bool GetIsCleared(string name, LevelData[] levels) {
			LevelData level = GetLevel (name, levels);
			if (level != null) {
				return level.isCleared;
			}
			return false;
		}

		public static LevelData GetLevel(string name, LevelData[] levels) {
			for (int i = 0; i < levels.Length; i++) {
				if (levels [i].name == name) {
					return levels [i];
				}
			}
			return null;
		}
	}


}

