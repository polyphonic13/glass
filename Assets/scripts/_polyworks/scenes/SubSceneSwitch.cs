using UnityEngine;

namespace Polyworks
{
    public class SubSceneSwitch : Switch
    {
        public SceneType targetScene;
        public int targetSection = -1;

        private bool isActive { get; set; }

        public void SetActive(bool active)
        {
            isActive = active;
        }

        public override void Actuate()
        {
            // TODO: some other event needs to be triggered in order to ensure housekeeping done prior to change
            EventCenter.Instance.TriggerChangeScene(targetScene, false);
        }
    }
}
