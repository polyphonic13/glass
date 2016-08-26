using System;
using System.Collections;

/*
 * SceneData contains general information on scene, not stored in persistent GameData
 * LevelData contains state information for the scene, including currentSection
 */
namespace Polyworks
{
	[Serializable]
	public class SceneData
	{
		public string sceneName;

		public SectionPrefabs[] sectionPrefabs;
	}

}

