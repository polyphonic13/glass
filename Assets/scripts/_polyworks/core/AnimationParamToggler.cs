namespace Polyworks
{
    using UnityEngine;

    public class AnimationParamToggler : Toggler
    {
        public Animator Target;
        public string Param;

        public override void Toggle()
        {
            Log("AnimationParamToggler[" + this.name + "]/Toggle, isOn = " + isOn + ", Target = " + Target);

            if (Target == null)
            {
                Log(" ERROR: Target is null");
            }
            base.Toggle();
            Target.SetBool(Param, this.isOn);
        }
    }
}
