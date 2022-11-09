using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeTest : Interactable
{
    MeshRenderer mesh;

    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public override void Interact()
    {
        base.Interact();
        mesh.material.color = Color.red;
    }
}
