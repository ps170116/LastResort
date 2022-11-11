using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushBall : Interactable
{

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override void Interact()
    {
        base.Interact();
        rb.AddForce(transform.forward * 5, ForceMode.Force);
    }
}
