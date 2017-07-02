namespace Polyworks {
	using UnityEngine;
	using System.Collections;

	public class ActuateMessageAgent : Item
	{
		public override void Actuate ()
		{
			SendMessage ("Actuate", null, SendMessageOptions.DontRequireReceiver);
		}
	}
}