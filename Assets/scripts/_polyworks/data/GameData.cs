﻿using UnityEngine;
using System.Collections;

namespace Polyworks {
	[System.Serializable]
	public class GameData {
		public string currentScene;

		public int count = 0;

		public Hashtable tasks;
		public Hashtable items;

		public Hashtable clearedScenes;

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

