using UnityEngine;
using System;

/** 
 * component to dyamically add AnimationEvents to clips from AnimationEventClipData
 */
namespace Polyworks {
	public class AnimationEventAgent : MonoBehaviour
	{
		public AnimationClipEventData[] clipData;
			
		public AnimationEvent Create(AnimationEventData data) {
			var evt = new AnimationEvent ();
			evt.functionName = data.method;
			evt.time = data.time;
			evt.intParameter = data.intParam;
			evt.floatParameter = data.floatParam;
			evt.stringParameter = data.stringParam;
			return evt;
		}

		private void Awake() {
			for (int i = 0; i < clipData.Length; i++) {
				AnimationClip clip = clipData [i].clip;
				for (int j = 0; j < clipData [i].events.Length; j++) {
					AnimationEvent evt = Create (clipData [i].events [j]);
					clip.AddEvent (evt);
				}
			}
		}
	}

	[Serializable]
	public struct AnimationClipEventData {
		public AnimationClip clip;
		public AnimationEventData[] events;
	}

	[Serializable]
	public struct AnimationEventData {
		public string method; 
		public float time;
		public int intParam;
		public float floatParam; 
		public string stringParam;
	}
}

