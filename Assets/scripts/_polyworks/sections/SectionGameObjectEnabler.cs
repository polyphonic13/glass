namespace Polyworks
{
    using UnityEngine;

    public class SectionGameObjectEnabler : SectionAgent
    {
        public override void ToggleEnabled(bool isEnabled)
        {
            gameObject.SetActive(isEnabled);
            gameObject.SendMessage("Enable", isEnabled, SendMessageOptions.DontRequireReceiver);
        }
    }
}

