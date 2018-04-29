using UnityEngine;
using Polyworks;

public class LightPuzzle: Puzzle {
    public int[] solution;

    public void OnIntEvent(string type, int value)
    {

    }

    private void Awake() 
    {
        EventCenter.Instance.OnIntEvent += OnIntEvent;
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
