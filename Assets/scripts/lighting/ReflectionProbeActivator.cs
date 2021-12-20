namespace Polyworks
{
    using UnityEngine;

    public class ReflectionProbeActivator : MonoBehaviour
    {
        public ReflectionProbe Probe;
        public Color EnabledColor = Color.white;

        public void Enable(bool isEnabled)
        {
            Debug.Log("ReflectionProbeActivator/ToggleEnabled, isEnabled = " + isEnabled);
            Color color = (isEnabled) ? EnabledColor : Color.black;
            Probe.backgroundColor = color;
        }
    }
}

