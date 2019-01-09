using System.Collections.Generic;
using UnityEngine;
using Polyworks;

public class LightPuzzleLightGroup: MonoBehaviour 
{
    public static List<int[]> SEQUENCES = new List<int[]> { 
        new int[4] { 0, 0, 0, 0 },
        new int[4] { 1, 0, 0, 0 },
        new int[4] { 0, 1, 0, 0 },
        new int[4] { 0, 0, 1, 0 },
        new int[4] { 0, 0, 0, 1 },
        new int[4] { 1, 1, 0, 0 },
        new int[4] { 1, 0, 1, 0 },
        new int[4] { 1, 0, 0, 1 },
        new int[4] { 0, 1, 1, 0 },
        new int[4] { 0, 1, 0, 1 },
        new int[4] { 0, 0, 1, 1 },
        new int[4] { 1, 1, 1, 0 },
        new int[4] { 1, 1, 0, 1 },
        new int[4] { 1, 0, 1, 1 },
        new int[4] { 0, 1, 1, 1 },
        new int[4] { 1, 1, 1, 1 }
    };
    // public static string ACTUATE_EVENT = "actuateLightGroup";
    public string actuateEvent = "";
    public GameObject[] lights;

    public Material[] materials;

    public int groupIndex;
    
    public bool isLogOn;

    private int currentIndex;

    public void OnIntEvent(string type, int value) 
    {
        Log("LightPuzzleLightGroup["+this.name+"]/OnIntEvent, type = " + type + ", value = " + value);
        if(type == actuateEvent && value == groupIndex)
        {
            IncrementSequence();
        }
    }

    public void Activate() 
    {
        EventCenter.Instance.OnIntEvent += OnIntEvent;
    }

    public void Deactivate()
    {
        EventCenter.Instance.OnIntEvent -= OnIntEvent;
    }

    private void Log(string message) 
    {
        if(isLogOn)
        {
            Debug.Log(message);
        }
    }
    private void Awake() 
    {
    }

    private void IncrementSequence() 
    {
        if(currentIndex < SEQUENCES.Count - 1) 
        {
            currentIndex++;
        }
        else
        {
            currentIndex = 0;
        } 

        Log("LightPuzzleGroup["+this.name+"]/IncrementSequence, new currentIndex = " + currentIndex);
        SetLightMaterial(SEQUENCES[currentIndex]);
    }

    private void SetLightMaterial(int[] sequence) 
    {
        GameObject gameObject;
        Material material;

        for(int i = 0; i < lights.Length; i++)
        {
            gameObject = lights[i];
            Log("  sequence[" + i + "] = " + sequence[i]);
            material = materials[sequence[i]];

            gameObject.GetComponent<Renderer>().material = material;
        }
    }

    private void OnDestroy() 
    {
        EventCenter ec = EventCenter.Instance;
        if(ec != null) 
        {
            ec.OnIntEvent -= OnIntEvent;
        }
    }
}
