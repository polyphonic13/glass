using System;
using UnityEngine;

namespace Polyworks
{
	[Serializable]
	public class ItemData
	{
		public string itemName;
		public string displayName;
		public string prefabPath; 
		public string thumbnail = ""; 

		public ItemData () {}
	}

}

