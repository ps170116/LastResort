using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckWall : MonoBehaviour
{
    public bool onWall;

    PlayerController playerController;

    public Vector3 poc;

    public List<Collider> cols = new List<Collider>();


    MeshCollider m;

    public Vector3 pos;


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 7)
        {

            onWall = true;

            Vector3 temp = other.ClosestPoint(transform.position);
            pos = temp;
            cols.Add(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer == 7)
        {
            onWall = false;
            cols.Remove(other);
        }
    }
}
