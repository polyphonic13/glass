using UnityEngine;
using System.Collections;

namespace Polyworks
{
    public class SceneSwitch : Switch
    {
        public bool IsFadedOut = true;
        public SceneType targetScene { get; set; }
        public int targetSection { get; set; }

        private bool isActive { get; set; }

        public void SetActive(bool active)
        {
            isActive = active;
        }

        public override void Actuate()
        {
            Debug.Log("SceneSwitch[" + this.name + "]/Actuate, targetScene = " + targetScene + ", targetSection = " + targetSection);
            // EventCenter.Instance.StartSceneChange(targetScene, targetSection);
            EventCenter.Instance.TriggerChangeScene(targetScene, targetSection, IsFadedOut);
        }
    }
}
