using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DeveloperConsole;

public class TestCommand : MonoBehaviour
{
    public int value = 5;



    [ConCommand("testcommand", "for testing")]
    public void SetValue()
    {
        this.value = 2;
    }
}
