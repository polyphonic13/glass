using UnityEngine;
using System.Collections;

namespace Polyworks {
	public class SceneController : Singleton<SceneController>
	{
		[SerializeField] public ScenePrefabData prefabs;

		private SceneData sceneData;

		public void Init(SceneData data) {
			sceneData = data;
		}

		void Start ()
		{

		}

		void Update ()
		{

		}
	}
}

