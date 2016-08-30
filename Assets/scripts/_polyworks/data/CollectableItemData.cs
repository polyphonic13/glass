using System;

namespace Polyworks 
{
	[Serializable]
	public class UsableRange {
		public string target1;
		public string target2;
		public float distance;
	}

	[Serializable]
	public class CollectableItemData 
	{
		public bool isCollected = false;
		public bool isDroppable = false; 
		public bool isUsable = false;
		public bool isDestroyedOnUse = false; 

		public int count = 0;

		public string thumbnail = ""; 

		public string name { get; set; }
		public string displayName { get; set; }
		public string description { get; set; }
		public string prefabPath { get; set; }

		public UsableRange usableRange; 
	}
}

