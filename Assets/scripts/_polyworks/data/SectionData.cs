using System;
using System.Collections;
using UnityEngine;

namespace Polyworks {

	[Serializable]
	public class SectionData
	{
		public string name;
		public PlayerLocation playerLocation;
	}

	[Serializable]
	public struct PlayerLocation {
		public Vector3 position;
		public Quaternion rotation;
	}

}

