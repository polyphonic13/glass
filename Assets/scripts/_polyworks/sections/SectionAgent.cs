namespace Polyworks
{
    using UnityEngine;

    public class SectionAgent : Reaction
    {
        public virtual void ToggleEnabled(bool isEnabled)
        {
            if (isEnabled)
            {
                // Debug.Log("SectionAgent[" + this.name + "]/ToggleEnabled, isEnabled = " + isEnabled);
                gameObject.SendMessage("Enable", null, SendMessageOptions.DontRequireReceiver);
                return;
            }
            gameObject.SendMessage("Disable", null, SendMessageOptions.DontRequireReceiver);
        }
    }
}

