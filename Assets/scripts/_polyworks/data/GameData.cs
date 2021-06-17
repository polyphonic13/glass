using UnityEngine;
using System;
using System.Collections;

namespace Polyworks
{
    [Serializable]
    public struct Coords
    {
        public float x;
        public float y;
        public float z;
    }

    [Serializable]
    public struct PrefabData
    {
        public string name;
        public string path;
        public string addTo;
        public Coords location;
        public Coords rotation;
    }

    [Serializable]
    public struct Section
    {
        public string name;
        public PrefabData[] prefabs;
    }
    [Serializable]
    public struct Level
    {
        public string name;
        public string[] subScenes;
        public Section[] sections;
    }

    [Serializable]
    public struct GameJSON
    {
        public Level[] levels;

    }

    [Serializable]
    public class GameData
    {
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

