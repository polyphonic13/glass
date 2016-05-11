using UnityEngine;

namespace AssemblyCSharp
{
	public class EventCenter: MonoBehaviour
	{
		public delegate void InventoryAdder(string item);
		public event InventoryAdder OnInventoryAdded;

		public delegate void NoteAdder(string message);
		public event NoteAdder OnNoteAdded;

		#region singleton
		private static EventCenter _instance;
		private EventCenter() {}

		public static EventCenter Instance {
			get {
				if(_instance == null) {
					_instance = GameObject.FindObjectOfType(typeof(EventCenter)) as EventCenter;      
				}
				return _instance;
			}
		}
		#endregion

		public void AddInventory(string item) {
			if (OnInventoryAdded != null) {
				OnInventoryAdded (item);
			}
		}

		public void AddNote(string message) {
			if (OnNoteAdded != null) {
				OnNoteAdded (message);
			}
		}
	}
}

