using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DeveloperConsole;

public class Health : MonoBehaviour
{

    public float health;

    private void Awake()
    {
    }

    private void Update()
    {
        
    }

    [ConCommand("hurtme", "hurt")]
    public  void Testing()
    {
        health -= 10;
        Debug.Log(health.ToString());
    }


}
