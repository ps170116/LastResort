using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public string interactableName;
    public virtual void Interact()
    {
        //this function does not contain any code because its only use
        //is to be overwritten by another class
        //this is done so that you only need to check for this specific class
    }
}
