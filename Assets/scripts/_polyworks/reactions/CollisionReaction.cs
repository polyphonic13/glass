using UnityEngine;
using System.Collections;

namespace Polyworks {
	public class CollisionReaction : Reaction
	{
		public virtual void OnCollisionEnter(Collision col) {
			Execute();
		}
	}
}

