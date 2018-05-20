using System.Collections.Generic;
using UnityEngine;
using Polyworks;

public class LightPuzzle: Puzzle {
    public int[] solution;

    private List<int> current = new List<int> {
        0,
        0,
        0,
        0
    };

    public void OnIntEvent(string type, int value)
    {
        // Log("LightPuzzle[" + this.name + "]/OnInEvent, type = " + type + ", value = " + value);
        if(type == LightPuzzleLightGroup.ACTUATE_EVENT)
        {
            IncrementCurrent(value);
        }

    }

    public override void Solve()
    {
        Log("LightPuzzle[" + this.name + "]/Solve");
        base.Solve();
    }

    private void Awake() 
    {
        EventCenter.Instance.OnIntEvent += OnIntEvent;

    }

    private void IncrementCurrent(int index)
    {
        if(index < current.Count)
        {
            if(current[index] < LightPuzzleLightGroup.SEQUENCES.Count)
            {
                current[index]++;
            }
            else
            {
                current[index] = 0;
            }
        }
        // Log("LightPuzzle[" + this.name + "]/IncrementCurrent, current[" + index + "] = " + current[index]);
        this.isSolved = CheckIsSolved();
        if(this.isSolved)
        {
            Solve();
        }
    }

    private bool CheckIsSolved()
    {
        for(int i = 0; i < solution.Length; i++)
        {
            if(solution[i] != current[i])
            {
                // Log("solution[" + i + "]: " + solution[i] + ", current = " + current[i]);
                return false;
            }
        }
        Log("LightPuzzle["+this.name+"]/CheckIsSolved, solved = true");
        return true;
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
