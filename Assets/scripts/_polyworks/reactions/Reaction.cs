using UnityEngine;
using System.Collections;

namespace Polyworks {
	public class Reaction : MonoBehaviour, IReactable
	{
		public virtual void Execute() {
			this.gameObject.SendMessage ("Actuate", null, SendMessageOptions.DontRequireReceiver);
		}
	}
}

