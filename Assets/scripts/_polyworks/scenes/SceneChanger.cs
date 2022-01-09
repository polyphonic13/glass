using UnityEngine;
using System;
using System.Collections;

namespace Polyworks
{
    [Serializable]
    public struct ScenePrepSteps
    {
        public string scene;
        public int count;
    }

    public class SceneChanger : Singleton<SceneChanger>
    {
        #region public members
        public ScenePrepSteps[] prepSteps;
        #endregion

        #region private members
        private string currentSceneName = "";
        private string targetSceneName = "";
        private int targetSection = -1;

        private ScenePrepSteps currentPrepSteps;
        private int totalPrepStepsCompleted = 0;
        private bool isListenersAdded = false;
        #endregion

        #region public methods
        public void Init(string scene)
        {
            currentSceneName = scene;
            for (int i = 0; i < prepSteps.Length; i++)
            {
                if (prepSteps[i].scene == scene)
                {
                    currentPrepSteps = prepSteps[i];
                    break;
                }
            }
            if (isListenersAdded)
            {
                return;
            }
            EventCenter eventCenter = EventCenter.Instance;
            eventCenter.OnStartSceneChange += OnStartSceneChange;
            eventCenter.OnContinueSceneChange += OnContinueSceneChange;
            isListenersAdded = true;
        }

        public void OnStartSceneChange(string sceneName, int section = -1)
        {
            targetSceneName = sceneName;
            targetSection = section;
        }

        public void OnContinueSceneChange(string sceneName, int section = -1)
        {
            totalPrepStepsCompleted++;
            Debug.Log("SceneChanger/OnContinueSceneChange, totalPrepStepsCompleted = " + totalPrepStepsCompleted + ", currentPrepSteps.count = " + currentPrepSteps.count);
            if (totalPrepStepsCompleted < currentPrepSteps.count)
            {
                return;
            }
            totalPrepStepsCompleted = 0;
            Debug.Log(" dispatching CompleteSceneChange");
            EventCenter.Instance.CompleteSceneChange(targetSceneName, targetSection);
        }
        #endregion

        private void OnDestroy()
        {
            EventCenter eventCenter = EventCenter.Instance;
            if (eventCenter == null)
            {
                return;
            }
            eventCenter.OnStartSceneChange -= OnStartSceneChange;
            eventCenter.OnContinueSceneChange -= OnStartSceneChange;
        }
    }

}


