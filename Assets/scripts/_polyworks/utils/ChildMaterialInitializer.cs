namespace Polyworks {
	using UnityEngine;
	using System.Collections;

	public class ChildMaterialInitializer : MonoBehaviour
	{
		public Material material;

		public string path = ""; 
		public bool isRecursive; 

		private void Awake() {
			if (path != "") {
				Transform target = transform.Find (path);
				if (target != null) {
					_setMaterialOnTransformChildren (target);
				}
			}
		}

		private void _setMaterialOnTransformChildren(Transform target) {
			Debug.Log (" _setMaterialOnTransformChildren, target = " + target.gameObject.name);
			foreach (Transform child in target) {
				_setMaterialOnTransform (child);

				if (isRecursive) {
					_setMaterialOnTransformChildren (child);
				}
			}
		}

		private void _setMaterialOnTransform(Transform child) {
			Debug.Log ("  _setMaterialOnTransform, child = " + child.gameObject.name);
			GameObject go = child.gameObject;
			if (go != null) {
				Renderer renderer = go.GetComponent<Renderer> ();
				if (renderer != null) {
					renderer.material = material;
				}
			}
		}
	}
}