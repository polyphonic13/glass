using System.Collections.Generic;
using UnityEngine;
using Polyworks;

public class LightPuzzleLightGroup: MonoBehaviour 
{
    public static string ACTUATE_EVENT = "actuateLightGroup";

    public GameObject[] lights;

    public Material[] materials;

    public int groupIndex;
    
    public bool isLogOn;

    private static List<int[]> SEQUENCES = new List<int[]> { 
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
    private int currentIndex;

    public void OnIntEvent(string type, int value) 
    {
        if(isLogOn) 
        {
            Debug.Log("LightPuzzleLightGroup["+this.name+"]/OnIntEvent, type = " + type + ", value = " + value);
            if(type == ACTUATE_EVENT && value == groupIndex)
            {
                IncrementSequence();
            }
        }
    }

    private void Awake() 
    {
        EventCenter.Instance.OnIntEvent += OnIntEvent;
    }

    private void IncrementSequence() 
    {
        if(currentIndex < SEQUENCES.Count) 
        {
            currentIndex++;
        }
        else
        {
            currentIndex = 0;
        } 

        int[] currentSequence = SEQUENCES[currentIndex];
    }

    private void SetLightMaterial(int[] sequence) 
    {
        GameObject gameObject;
        Material material;

        for(int i = 0; i < lights.Length; i++)
        {
            gameObject = lights[i];
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
