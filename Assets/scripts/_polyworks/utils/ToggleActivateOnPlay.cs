using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ActivateTarget
{
    public GameObject gameObject;
    public bool isActivateOnPlay;
}

public class ToggleActivateOnPlay : MonoBehaviour
{

    public ActivateTarget[] targets;

    private void Awake()
    {
        for (int i = 0; i < targets.Length; i++)
        {
            setActive(targets[i]);
        }
    }

    private void setActive(ActivateTarget target)
    {
        if (target.gameObject == null)
        {
            return;
        }
        target.gameObject.SetActive(target.isActivateOnPlay);
    }

}
