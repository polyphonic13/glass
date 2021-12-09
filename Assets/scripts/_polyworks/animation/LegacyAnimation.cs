namespace Polyworks
{
    using UnityEngine;

    [System.Serializable]
    public struct AnimationBone
    {
        public string animation;
        public Transform bone;
    }

    [System.Serializable]
    public class AnimationBoneCollection
    {
        public AnimationBone[] animationBones;

        public static Transform GetBone(string name, AnimationBone[] bones)
        {
            Transform bone = null;
            for (int i = 0; i < bones.Length; i++)
            {
                if (name == bones[i].animation)
                {
                    bone = bones[i].bone;
                    break;
                }
            }
            return bone;
        }
    }

    public class LegacyAnimation : AnimationAgent
    {

        public AnimationClip[] animationClips;
        public AnimationBoneCollection bones;

        public bool isAutoStart = false;
        public bool isAutoAdvance = true;
        public bool isLoopPlayback = false;

        private const float PLAY_SPEED = 1f;
        private const float PAUSE_SPEED = 0f;

        private int _currentAnimation = 0;

        private Animation _animation;
        private bool _isPlaying = false;

        public override void Play(string clip = "")
        {
            Actuate(clip);
        }

        public override void Pause()
        {
            _adjustSpeed(PAUSE_SPEED);
        }

        public override void Resume()
        {
            _adjustSpeed(PLAY_SPEED);
        }

        public override bool GetIsActive()
        {
            if (_animation == null)
            {
                return false;
            }
            return _animation.isPlaying;
        }

        public void Actuate(string clip = "")
        {
            string clipName = (clip != "") ? clip : animationClips[_currentAnimation].name;
            // Debug.Log("LegacyAnimation[" + this.name + "]/Actuate, clip = " + clip + ", _currentAnimation[ " + _currentAnimation + "] = " + animationClips[_currentAnimation] + "clipName = " + clipName);

            if (clip == "")
            {
                _incrementCurrentAnimation();
            }

            Transform bone = AnimationBoneCollection.GetBone(clipName, bones.animationBones);

            if (bone != null)
            {
                // Debug.Log (" THERE IS A BONE: " + bone);
                _animation[clipName].AddMixingTransform(bone);
            }

            // Debug.Log(" about to call Play on _animation = " + _animation + ", clip = " + _animation[clipName] + ", clipName = " + clipName);

            if (_animation[clipName] == null)
            {
                return;
            }
            _animation[clipName].wrapMode = WrapMode.Once;
            _animation[clipName].speed = PLAY_SPEED;
            _animation.Play(clipName);
            _isPlaying = true;
        }

        private string getClipName()
        {
            return "";
        }

        private void Awake()
        {
            _animation = GetComponent<Animation>();
            // Debug.Log("LegacyAnimation[ " + this.name + " ]/Awake, got " + _animation);
            if (isAutoStart)
            {
                Actuate();
            }
        }

        private void _incrementCurrentAnimation()
        {
            if (_currentAnimation < animationClips.Length - 1)
            {
                _currentAnimation++;
            }
            else
            {
                _currentAnimation = 0;
            }
        }

        private void _adjustSpeed(float speed)
        {
            // Debug.Log ("LegacyAnimation[" + this.name + "]/_adjustSpeed, speed = " + speed);
            string clip = animationClips[_currentAnimation].name;
            _animation[clip].speed = speed;
        }
    }
}
