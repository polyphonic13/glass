﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Polyworks;

[Serializable]
public struct PuzzleWire
{
    public GameObject gameObject;
    public bool isActivated;
    public List<int> siblings;
}

public class CircuitPuzzle : Puzzle
{
    public int[] solution;

    public List<PuzzleWire> wireChildren { get; set; }
    public string wiresPath = "";

    public virtual void OnIntEvent(string type, int value)
    {
        Log("CircuitPuzzle[" + this.name + "]/OnIntEvent, type = " + type + ", value = " + value);

        switch (type)
        {
            case "insert_wire":
                ToggleWireInserted(value, true);
                break;

            case "remove_wire":
                ToggleWireInserted(value, false);
                break;
        }

        PostIntEvent();
    }

    public override void Init()
    {
        base.Init();
    }

    public virtual void InitWires()
    {
        wireChildren = new List<PuzzleWire>();

        if (wiresPath != "")
        {
            Transform wireHolder = transform.Find(wiresPath);
            int count = 0;

            foreach (Transform t in wireHolder)
            {
                PuzzleWire puzzleWire = new PuzzleWire();
                puzzleWire.gameObject = t.gameObject;
                puzzleWire.isActivated = false;
                puzzleWire.gameObject.SetActive(false);
                puzzleWire.siblings = GetWireSiblings(count);
                wireChildren.Add(puzzleWire);

                count++;
            }
        }
    }

    public virtual List<int> GetWireSiblings(int index)
    {
        return new List<int>();
    }

    public override void Activate()
    {
        base.Activate();

        if (!isCompleted)
        {
            EventCenter.Instance.OnIntEvent += OnIntEvent;
        }
    }

    public override void Deactivate()
    {
        base.Deactivate();
        Log("CircuitPuzzle[" + this.name + "]/Deactivate, isCompleted = " + isCompleted + ", isSolved = " + isSolved);

        RemoveListeners();

        if (isSolved)
        {
            return;
        }
        RemoveAllWires();
    }

    public override void Solve()
    {
        base.Solve();
        RemoveListeners();
    }

    public virtual void RemoveAllWires()
    {
        for (int i = 0; i < wireChildren.Count; i++)
        {
            ToggleWireInserted(i, false);
        }
    }

    public virtual void ToggleWireInserted(int index, bool isInserted)
    {
        Log("CircuitPuzzle[" + this.name + "]/ToggleWireInserted, index = " + index + ", isInserted = " + isInserted + ", count = " + wireChildren.Count);
        PuzzleWire wire = wireChildren[index];

        if (isInserted)
        {
            Log("\twire siblings = " + wire.siblings.Count);
            foreach (int idx in wire.siblings)
            {
                Log("\twire sibling idx = " + idx);
                PuzzleWire sibling = wireChildren[idx];
                sibling.isActivated = false;
                sibling.gameObject.SetActive(false);
                wireChildren[idx] = sibling;
            }
        }

        wire.gameObject.SetActive(isInserted);
        wire.isActivated = isInserted;

        wireChildren[index] = wire;
    }

    public bool GetIsInSolution(int val)
    {
        for (int i = 0; i < solution.Length; i++)
        {
            if (solution[i] == val)
            {
                return true;
            }
        }
        return false;
    }


    public void PostIntEvent()
    {
        this.isSolved = CheckIsSolved();
        if (isSolved)
        {
            Solve();
        }
        Log("CircuitPuzzle[" + this.name + "]/PostIntEvent, isSolved = " + isSolved);
    }

    public virtual bool CheckIsSolved()
    {
        for (int i = 0; i < wireChildren.Count; i++)
        {

            if ((wireChildren[i].isActivated && !GetIsInSolution(i)) || (!wireChildren[i].isActivated) && GetIsInSolution(i))
            {
                return false;
            }
        }
        return true;
    }

    public virtual void RemoveListeners()
    {
        EventCenter ec = EventCenter.Instance;
        Debug.Log("CircuitPuzzle[ " + this.name + " ]/RemoveListeners, ec = " + ec);
        if (ec == null)
        {
            return;
        }
        ec.OnIntEvent -= OnIntEvent;
    }

    private void OnDestroy()
    {
        RemoveListeners();
    }
}

