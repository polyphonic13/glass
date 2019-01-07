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

    private LightPuzzleLightGroup[] _lightGroups;

    private string _actuateEvent;

    public void OnIntEvent(string type, int value)
    {
        if(type == _actuateEvent)
        {
            Log("LightPuzzle[" + this.name + "]/OnInEvent, type = " + type + ", value = " + value + ", _actuateEvent = " + _actuateEvent);
            IncrementCurrent(value);
        }

    }

    public override void Solve()
    {
        Log("LightPuzzle[" + this.name + "]/Solve");
        base.Solve();
    }

    public override void Activate() 
    {
        Log("LightPuzzle[" + this.name + "]/Activate");
        base.Activate();
        EventCenter.Instance.OnIntEvent += OnIntEvent;

        foreach(LightPuzzleLightGroup group in _lightGroups)
        {
            group.Activate();            
        }

    }

    public override void Deactivate() 
    {
        Log("LightPuzzle[" + this.name + "]/Deactivate");
        base.Deactivate();
        EventCenter.Instance.OnIntEvent -= OnIntEvent;

        foreach(LightPuzzleLightGroup group in _lightGroups)
        {
            group.Deactivate();            
        }
    }
    
    private void Awake() 
    {
        _actuateEvent = this.name + "ButtonClicked";
        _lightGroups = GetComponentsInChildren<LightPuzzleLightGroup>();
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
        Log("LightPuzzle[" + this.name + "]/IncrementCurrent, current[" + index + "] = " + current[index]);
        this.isSolved = CheckIsSolved();
        Log("  isSolved = " + this.isSolved);
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
                Log("solution[" + i + "]: " + solution[i] + ", current = " + current[i]);
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
