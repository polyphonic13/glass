using UnityEngine;
using System.Collections;

namespace Polyworks {
	[System.Serializable]
	public class GameData {
		public string currentScene;
		public int targetSection = -1;

		public int count = 0;

		public Hashtable tasks;
		public Hashtable items;

		public Hashtable clearedScenes;
		public LevelData[] levels; 

		public PlayerData playerData;

		public Flag[] flags;

		/*
		 * tasks = {
		 * 	sceneA: {
		 * 		sceneTaskData: {
		 * 			intTaskData[],
		 * 			floatTaskData[],
		 * 			stringTaskData[]
		 * 		}
		 * 	}
		 * }
		 */
	}

}

