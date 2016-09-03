using UnityEngine;
using System.Collections;

namespace Polyworks {
	public class SectionController : MonoBehaviour
	{
		public SectionData data; 

		private SectionAgent[] _agents; 
		private bool _isChildrenEnabled = false; 

		#region handlers
		public void OnSectionChanged(int currentSection) {
			_sectionTest (currentSection);
		}
		#endregion

		#region public methods
		public void Init(int currentSection) {
			_agents = GetComponentsInChildren<SectionAgent> ();
			_sectionTest (currentSection);

			EventCenter ec = EventCenter.Instance;
			if (ec != null) {
				ec.OnSectionChanged += OnSectionChanged;
			}
		}
		#endregion

		#region private methods
		private void _sectionTest(int currentSection) {
			if (_agents == null) {
				return;
			}

			if (data.section == currentSection) {
//				Debug.Log ("enabling section " + currentSection);
				_toggleEnabled (true);
			} else {
				_toggleEnabled (false);
			}
		}

		private void _toggleEnabled(bool isEnabled) {
//			Debug.Log ("SectionController[" + this.name + "]/_toggleEnabled, isEnabled = " + isEnabled);
			foreach(SectionAgent agent in _agents) {
				if (agent != null) {
//					Debug.Log (" sending " + isEnabled + " to agent[" + agent.name + "]");
					agent.ToggleEnabled (isEnabled);
				}
			}

			_isChildrenEnabled = isEnabled;
		}

		private void OnDestroy() {
			EventCenter ec = EventCenter.Instance;
			if (ec != null) {
				ec.OnSectionChanged -= OnSectionChanged;
			}
		}
		#endregion
	}
}

