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
    public struct ItemInspectionScale
    {
        public string name;
        public Coords scale;
    }

    [Serializable]
    public struct PrefabData
    {
        public string name;
        public string path;
        public string addTo;
        public Coords position;
        public Coords rotation;
    }

    [Serializable]
    public struct Section
    {
        public string name;
        public PrefabData[] prefabs;
    }
    [Serializable]
    public struct SubSceneData
    {
        public string name;
        public bool isPlayerScene;
        public string[] siblingScenes;
        public Section[] sections;
    }

    [Serializable]
    public struct GameJSON
    {
        public SubSceneData[] subScenes;
        public Flag[] flags;
        public ItemInspectionScale[] itemInspectionScales;
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
