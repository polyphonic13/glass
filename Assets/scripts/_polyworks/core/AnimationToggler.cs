namespace Polyworks
{
    using UnityEngine;

    public class AnimationToggler : Toggler
    {
        public string TargetName;
        public AnimationClip OnAnimation;
        public AnimationClip OffAnimation;
        private AnimationAgent target;

        public override void Toggle()
        {
            Log("AnimationToggler[" + this.name + "]/Toggle, isOn = " + isOn + ", target = " + target);

            if (target == null)
            {
                Log(" ERROR: target is null");
            }
            base.Toggle();
            AnimationClip clip = (this.isOn) ? OnAnimation : OffAnimation;
            Log("  going to call play on target with clip " + clip.name);
            target.Play(clip.name);
        }

        private void Awake()
        {
            GameObject targetObject = GameObject.Find(TargetName);
            Log("AnimationSwitch[ " + this.name + " ]/Awake, TargetName = " + TargetName + ", targetObject = " + targetObject);
            if (targetObject == null)
            {
                return;
            }

            target = targetObject.GetComponent<AnimationAgent>();
        }
    }
}
