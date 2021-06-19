namespace Polyworks
{
    using System;
    using System.Collections;

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
    public struct SceneInfo
    {
        public string name;
        public bool isPlayerScene;
        public string[] subScenes;
        public Section[] sections;
    }

    [Serializable]
    public struct GameJSON
    {
        public SceneInfo[] scenes;
    }

    [Serializable]
    public class GameData
    {
        public string currentScene;
        public int targetSection = -1;
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
