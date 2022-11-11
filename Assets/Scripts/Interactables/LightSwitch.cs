using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : Interactable
{

    [SerializeField] Light lightSource;
    bool isOn = false;


    private void Start()
    {
        if(isOn)
        {
            lightSource.enabled = true;
            interactableName = "Turn light Off";
        }
        else
        {
            lightSource.enabled = false;
            interactableName = "Turn light On";
        }
    }

    public override void Interact()
    {
        base.Interact();

        if(isOn)
        {
            interactableName = "Turn light On";
            isOn = false;
            lightSource.enabled = false;
        }
        else if(!isOn)
        {
            interactableName = "Turn light Off";
            isOn= true;
            lightSource.enabled = true;
        }
    }
}
