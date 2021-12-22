namespace Polyworks
{
    using UnityEngine;

    public class SectionGameObjectEnabler : SectionAgent
    {
        public override void ToggleEnabled(bool isEnabled)
        {
            Debug.Log("SectionGameObjectEnabler/ToggleEnabled, isEnabled = " + isEnabled);
            gameObject.SetActive(isEnabled);
        }
    }
}

