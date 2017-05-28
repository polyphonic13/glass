namespace Polyworks {
	using UnityEngine;
	using System.Collections;

	public class Utils : MonoBehaviour
	{
		public static float ClampAngle(float angle, float min, float max) {
			if (angle < -360f) {
				angle += 360f;
			}
			if (angle > 360f) {
				angle -= 360f;
			}
			return Mathf.Clamp(angle, min, max);
		}
	}
}
