using System;
using UnityEngine;

public abstract class DialogTriggerEvent : MonoBehaviour
{
    private DialogTrigger _dialogTrigger;


    private void Start()
    {
        _dialogTrigger = GetComponent<DialogTrigger>();
    }

    private void FixedUpdate()
    {
        if (_dialogTrigger.status)
        {
            DoEvent();
        }
    }

    protected abstract void DoEvent();
}