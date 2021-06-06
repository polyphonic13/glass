using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Polyworks;

public class DirectionalCircuitPuzzle : CircuitPuzzle
{
    public int cols;
    public int rows;
    public int vRows;

    public string solvedSwitchEventValue = "";
    public string solvedSwitchEventType = "solvedSwitchThrown";

    List<List<int>> _ports;

    private bool _isSwitchThrown = false;

    public override void Init()
    {
        base.Init();
        _initPorts();
        InitPuzzleWires();
        // Log("DirectionCircuitPuzzle["+this.name+"]/Init, wireChildren.Count = " + wireChildren.Count);
    }

    private void _initPorts()
    {
        _ports = new List<List<int>>();

        int total = rows * cols;
        int row = 0;
        int col = 0;

        List<int> wires;

        for (int i = 0; i < total; i++)
        {
            wires = new List<int>();

            if (i > 0 && i % rows == 0)
            {
                row++;
                col = 0;
            }

            // horizontal
            if (col == 0)
            {
                // first col
                wires.Add(i - row);

            }
            else if (col == cols - 1)
            {
                // last col
                wires.Add(i - (row + 1));
            }
            else
            {
                // middle cols
                wires.Add(i - (row + 1));
                wires.Add(i - row);
            }

            if (row == 0)
            {
                // first row
                wires.Add(i + (vRows * rows));
            }
            else if (row == rows - 1)
            {
                // last row
                wires.Add(i + (vRows * rows) - rows);
            }
            else
            {
                wires.Add(i + (vRows * rows) - rows);
                wires.Add(i + (vRows * rows));

            }
            col++;

            _ports.Add(wires);
        }
    }

    public override void Activate()
    {
        base.Activate();
    }

    private void InitPuzzleWires()
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

    public override List<int> GetWireSiblings(int index)
    {
        List<int> siblings = new List<int>();
        foreach (List<int> port in _ports)
        {
            if (port.Contains(index))
            {
                foreach (int wire in port)
                {
                    if (wire != index)
                    {
                        siblings.Add(wire);
                    }
                }
            }
        }
        return siblings;
    }

}
