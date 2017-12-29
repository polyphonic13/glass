namespace Polyworks {
	using UnityEngine;

	public class FlagConditionalAgent: Item {

		public string flag;
		public ActuateAgent target;
		public bool isAddNoteOnFalse;
		public string message; 

		public override void Actuate() {
			bool isFlagOn = Game.Instance.GetFlag (flag);
			Debug.Log ("FlagConditionalAgent[" + this.name + "]/Actuate, flag = " + flag + ", value = " + isFlagOn);

			if (isFlagOn) {
				target.Actuate ();
			} else if (isAddNoteOnFalse) {
				EventCenter.Instance.AddNote (message);
			}
		}

		public override void Use() {
			Actuate ();
		}
	}
}
