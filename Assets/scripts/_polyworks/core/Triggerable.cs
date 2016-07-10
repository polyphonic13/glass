using UnityEngine;
using System.Collections;

namespace Polyworks {
	public class Triggerable : MonoBehaviour, ITriggerable
	{
		public virtual void Trigger() {
		}
	}
}

