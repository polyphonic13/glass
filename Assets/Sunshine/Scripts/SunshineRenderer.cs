using UnityEngine;
using System.Collections;

/// <summary>
/// Gives granular control over a particular Renderer's shadow receieving.
/// This only works in Unity 4.1 or above since it requires Per-Material Keywords.
/// </summary>
[RequireComponent (typeof(Renderer))]
public class SunshineRenderer : MonoBehaviour
{
		private bool _receiveShadows = true;
		private bool isDirty = true;
		private static readonly string[] disabledKeywords = new string[] { "SUNSHINE_DISABLED" };
		Material[] originalSharedMaterials;
		Renderer attachedRenderer;

		void OnEnable ()
		{
				attachedRenderer = GetComponent<Renderer> ();
		}

		void Update ()
		{
				bool newReceiveShadows = attachedRenderer.receiveShadows;
				if (_receiveShadows != newReceiveShadows) {
						_receiveShadows = newReceiveShadows;
						isDirty = true;
				}
				if (isDirty) {
						if (newReceiveShadows) {
								if (originalSharedMaterials != null)
										attachedRenderer.materials = originalSharedMaterials;
						} else {
								originalSharedMaterials = attachedRenderer.sharedMaterials;
								foreach (var mat in attachedRenderer.materials)
										mat.shaderKeywords = disabledKeywords;
						}
						isDirty = false;
				}
		}
}
