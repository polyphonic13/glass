using UnityEngine;
using System.Collections;

public class ArmatureParent : MonoBehaviour {
	
	public delegate void AnimationHandler(Transform bone);
	
	public event AnimationHandler onAnimationPlayed;
	
	public AnimationClip defaultAnimation; 
	
	public Animation animation { get; set; }
	
	public virtual void playAnimation(string clip, Transform bone = null) {
		Debug.Log("ArmatureParent[ " + this.name + " ]/playAnimation, clip = " + clip + ", bone = " + bone.name);
		animateArmatureBone(clip, bone);
//		if(bone != null) {
//			this.animationPlayed(bone);
//		}
	}
	
	public void animateArmatureBone(string clip, Transform bone = null) {
		Debug.Log("  animateArmatureBone, clip = " + clip);
		if(bone != null) {
			this.animation [clip].AddMixingTransform(bone);
		}
		this.animation [clip].wrapMode = WrapMode.Once;
		this.animation.Play(clip);
	}

	public virtual void init() {
		this.animation = GetComponent<Animation>();
		Debug.Log("ArmatureParent[ " + this.name + " ]/init, animation = " + this.animation);
		playDefaultAnimation();

	}
	
	public void playDefaultAnimation() {
//		gameObject.SetActive(false);

		if(defaultAnimation != null) {
//			Debug.Log("ArmatureParent/Start, defaultAnimation = " + defaultAnimation.name);
			this.animation [defaultAnimation.name].layer = 0;
			this.animation[defaultAnimation.name].wrapMode = WrapMode.Once;
			this.animation.Play(defaultAnimation.name);
		}
	}
	
	public void animationPlayed(Transform bone = null) {
		Debug.Log("ArmatureParent[ " + this.name + " ]/animationPlayed, bone = " + bone);
		if(onAnimationPlayed != null) {
			onAnimationPlayed(bone);
		}
	}
	
	void Awake() {
		init();
	}
	
}
