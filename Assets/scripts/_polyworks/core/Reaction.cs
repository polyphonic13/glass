using UnityEngine;
using System.Collections;

namespace Polyworks {
	public class Reaction : MonoBehaviour, IReactable
	{
		public void Execute() {
			this.gameObject.SendMessage ("Actuate");
		}
	}
}

