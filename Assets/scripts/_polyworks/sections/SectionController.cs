using UnityEngine;
using System.Collections;

namespace Polyworks {
	public class SectionController : MonoBehaviour
	{
		public SectionData data; 

		private SectionAgent[] _agents; 
		private bool _isChildrenActive = false; 

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
				_toggleActivate (true);
			} else {
				_toggleActivate (false);
			}
		}

		private void _toggleActivate(bool isActive) {
			foreach(SectionAgent agent in _agents) {
				if (agent != null) {
					agent.ToggleActive (isActive);
				}
			}

			_isChildrenActive = isActive;
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

