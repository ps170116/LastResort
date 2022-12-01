using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField] float range;
    [SerializeField] float damage;


    Camera cam;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponentInParent<Camera>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(Shoot());

            
        }
    }

    IEnumerator Shoot()
    {
        animator.SetBool("isShooting", true);
        RaycastHit hit;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range))
        {

        }

        yield return new WaitForSeconds(0.5f);
        animator.SetBool("isShooting", false);

    }

}
