using UnityEngine;

public class ArmatureParent : MonoBehaviour {
	
	public delegate void AnimationHandler(Transform bone);
	
	public event AnimationHandler OnAnimationPlayed;
	
	public AnimationClip _defaultAnimation; 
	
	public Animation _animation { get; set; }

	public bool isOpen { get; set; }

	public virtual void PlayAnimation(string clip, Transform bone = null) {
//		Debug.Log("ArmatureParent[ " + name + " ]/PlayAnimation, clip = " + clip);
		isOpen = !isOpen;
		AnimateArmatureBone(clip, bone);
	}
	
	public void AnimateArmatureBone(string clip, Transform bone = null) {
//		Debug.Log("  AnimateArmatureBone, clip = " + clip);
		if(bone != null) {
			_animation [clip].AddMixingTransform(bone);
		}
		_animation [clip].wrapMode = WrapMode.Once;
		_animation.Play(clip);
	}

	public virtual void Init() {
		isOpen = false;
		_animation = GetComponent<Animation>();
//		Debug.Log("ArmatureParent[ " + name + " ]/Init, _animation = " + _animation);
		PlayDefaultAnimation();

	}
	
	public void PlayDefaultAnimation() {
		if(_defaultAnimation != null) {
//			Debug.Log("ArmatureParent["+this.name+"/Start, _defaultAnimation = " + _defaultAnimation.name);
			_animation [_defaultAnimation.name].layer = 0;
			_animation[_defaultAnimation.name].wrapMode = WrapMode.Once;
			_animation.Play(_defaultAnimation.name);
		}
	}
	
	public void AnimationPlayed(Transform bone = null) {
//		Debug.Log("ArmatureParent[ " + name + " ]/AnimationPlayed, bone = " + bone);
		if(OnAnimationPlayed != null) {
			OnAnimationPlayed(bone);
		}
	}

	public bool GetIsActive() { 
		return _animation.isPlaying;
	}

	void Awake() {
		Init();
	}
	
}
