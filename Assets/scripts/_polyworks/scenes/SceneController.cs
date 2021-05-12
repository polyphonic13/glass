using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneType
{
    None,
    Cave1,
    HouseFloor2BathroomE,
    HouseFloor2BathroomS,
    HouseFloor2BathroomW,
    HouseFloor2BedroomNE,
    HouseFloor2BedroomNW,
    HouseFloor2BedroomE,
    HouseFloor2BedroomSE,
    HouseFloor2BedroomSW,
    HouseFloor2Halls,
    HouseFloor2Porch,
    HouseFloor2SecretRoomE,
    HouseFloor2SecretRoomS,
    HouseFloor2SecretRoomW
}

public class SceneController : MonoBehaviour
{
    public void LoadSubScene(SceneType target, System.Action<bool> callback)
    {
        StartCoroutine(loadSubScene(target, callback));
    }

    public void UnloadSubScene(SceneType target, System.Action<bool> callback)
    {
        // Debug.Log("SceneController/UnloadSubScene, target = " + target);
        StartCoroutine(unloadSubScene(target, callback));
    }

    private IEnumerator loadSubScene(SceneType target, System.Action<bool> callback)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(target.ToString("G"), LoadSceneMode.Additive);

        while (!asyncLoad.isDone)
        {
            // Debug.Log(" asyncLoad.isDone = " + asyncLoad.isDone);
            yield return null;
        }
        // Debug.Log(" asyncLoad.isDone at end of block = " + asyncLoad.isDone);
        callback(true);
    }

    private IEnumerator unloadSubScene(SceneType target, System.Action<bool> callback)
    {
        AsyncOperation asyncLoad = SceneManager.UnloadSceneAsync(target.ToString("G"));

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        // Debug.Log("  aysncLoad.isDone at end of block = " + asyncLoad.isDone);
        callback(true);
    }
}
