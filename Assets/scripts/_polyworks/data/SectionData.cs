using System;
using System.Collections;
using UnityEngine;

namespace Polyworks {

	[Serializable]
	public class SectionData
	{
		public int section = -1;
		public PlayerLocation playerLocation;
	}

	[Serializable]
	public struct PlayerLocation {
		public Vector3 position;
		public Vector3 rotation;
	}

}

