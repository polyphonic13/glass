﻿using UnityEngine;
using System;
using Polyworks;

public class Puzzle : MonoBehaviour
{
    public const string UNLOCK_EVENT = "unlock_puzzle";
    public const string ACTIVATE_EVENT = "activate_puzzle";
    public const string SOLVED_EVENT = "puzzle_solved";
    public const string COMPLETED_EVENT = "puzzle_completed";

    public const string SOLVED_MESSAGE = "The puzzle has been solved";

    public string activateValue;
    public GameObject mainCollider;

    public PuzzleChild[] children;

    public string[] removeOnDeactivateItemPaths;
    protected bool isSolved = false;
    protected bool isCompleted = false;

    public bool isLogOn = false;

    public bool isNoteAddedOnSolved = true;
    public string noteMessage = "";

    private bool _isActive = false;


    #region eventhandlers
    public void OnStringEvent(string type, string value)
    {
        Log("Puzzle[" + this.name + "]/OnStringEvent, type = " + type + ", value = " + value + ", activateValue = " + activateValue);
        if (type == Puzzle.ACTIVATE_EVENT)
        {
            processActivateEvent(value);
            return;
        }

        if (type == Puzzle.COMPLETED_EVENT)
        {
            Log(" it is the completed event, setting isCompleted to true");
            isCompleted = true;
        }
    }

    public void OnChangeContext(InputContext context)
    {
        Log("Puzzle[ " + this.name + " ]/OnChangeContext, context = " + context);
        if (context == InputContext.PUZZLE)
        {
            return;
        }
        _toggleActive(false);
    }
    #endregion

    #region public methods
    public virtual void Init()
    {
        // Log("Puzzle["+this.name+"]/Init");
        InitChildren();
        _toggleActive(false);
    }

    public virtual void Enable()
    {
        Log("Puzzle[" + this.name + "]/Enable");
        EventCenter ec = EventCenter.Instance;
        ec.OnStringEvent += this.OnStringEvent;
    }

    public virtual void Disable()
    {
        // Log("Puzzle[" + this.name + "]/Disable");
        EventCenter ec = EventCenter.Instance;
        if (ec != null)
        {
            ec.OnStringEvent -= this.OnStringEvent;
        }
    }

    public virtual void InitChildren()
    {
        for (int i = 0, l = children.Length; i < l; i++)
        {
            Item item = children[i].gameObject.GetComponent<Item>();
            if (item != null)
            {
                children[i].item = item;
            }

            if (children[i].isDeactivatedOnInit)
            {
                ToggleChildActive(children[i], false);
            }
        }
    }

    public virtual void ToggleChildActive(PuzzleChild child, bool isActivated)
    {
        // Log("Puzzle["+this.name+"]/ToggleChildActve, child = " + child.gameObject.name + ", isActivated = " + isActivated);
        child.isActive = isActivated;
        child.gameObject.SetActive(isActivated);

        if (child.item != null)
        {
            child.item.isEnabled = isActivated;
        }
    }

    public virtual void Activate()
    {
        Debug.Log("--------------- Puzzle[" + this.name + "]/Activate");
        EventCenter.Instance.ChangeContext(InputContext.PUZZLE, this.name);
        _toggleActive(true);
    }

    public virtual void Deactivate()
    {
        _toggleActive(false);

        Log("Puzzle[" + this.name + "]/Deactivate, isSolved = " + this.isSolved);
        if (removeOnDeactivateItemPaths.Length > 0 && !isSolved)
        {
            foreach (string path in removeOnDeactivateItemPaths)
            {
                Log("  adding " + path);
                Inventory inventory = Game.Instance.GetPlayerInventory();
                inventory.AddFromPrefabPath(path);
            }
        }
    }

    public virtual void Solve()
    {
        // Log("Puzzle["+this.name+"]/Solve");
        _toggleChildrenOnSolved();
        EventCenter.Instance.InvokeStringEvent(Puzzle.SOLVED_EVENT, this.name);
        if (isNoteAddedOnSolved)
        {
            string message = (noteMessage != "") ? noteMessage : SOLVED_MESSAGE;
            EventCenter.Instance.AddNote(message);
        }
    }

    public void Log(string message)
    {
        if (!isLogOn)
        {
            return;
        }
        Debug.Log(message);
    }
    #endregion

    private void processActivateEvent(string value)
    {
        if (value == activateValue)
        {
            Log(" type and value match");
            Activate();
            return;
        }

        if (!_isActive)
        {
            return;
        }
        Deactivate();
    }

    private void _toggleActive(bool isActivated)
    {
        // Log ("Puzzle[" + this.name + "]/_toggleActive, isActivated = " + isActivated);
        _isActive = isActivated;
        mainCollider.SetActive(!_isActive);

        foreach (PuzzleChild child in children)
        {
            if (isActivated)
            {
                if (child.isActivatedOnActivate)
                {
                    // Log(" child[" + child.gameObject.name + "].isActivatedOnActivate = " + child.isActivatedOnActivate);
                    ToggleChildActive(child, true);
                }
                else if (child.isDeactivatedOnActivate)
                {
                    ToggleChildActive(child, false);
                }
            }
            else if (!isSolved)
            {
                // Log(" child[" + child.gameObject.name + "], isActivatedOnDeactive = " + child.isActivatedOnDeactivate + ", isDeactivatedOnDeactivate = " + child.isDeactivatedOnDeactivate);
                if (child.isActivatedOnDeactivate)
                {
                    ToggleChildActive(child, true);
                }
                else if (child.isDeactivatedOnDeactivate)
                {
                    ToggleChildActive(child, false);
                }
            }
        }
    }

    private void _toggleChildrenOnSolved()
    {
        for (int i = 0, l = children.Length; i < l; i++)
        {
            if (children[i].isActivatedOnSolved)
            {
                ToggleChildActive(children[i], true);
            }
            else if (children[i].isDeactivatedOnSolved)
            {
                Log(" toggling child active: " + children[i].gameObject.name);
                ToggleChildActive(children[i], false);
            }
        }
    }

    private void Awake()
    {
        Init();
    }

    private void OnDestroy()
    {
        EventCenter ec = EventCenter.Instance;
        if (ec != null)
        {
            ec.OnStringEvent -= this.OnStringEvent;
        }
    }

}

[Serializable]
public struct PuzzleChild
{
    public GameObject gameObject;
    public Item item;
    public bool isActive;
    public bool isDeactivatedOnInit;
    public bool isActivatedOnSolved;
    public bool isDeactivatedOnSolved;
    public bool isActivatedOnActivate;
    public bool isDeactivatedOnActivate;
    public bool isActivatedOnDeactivate;
    public bool isDeactivatedOnDeactivate;
}
