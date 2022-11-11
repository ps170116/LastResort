using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Interact : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI itemText;
    [SerializeField] float interactRange;
    [HideInInspector] public GameObject currentItem;
    public LayerMask itemLayer;
    [SerializeField] Camera cam;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, interactRange, itemLayer))
        {
            if (hit.collider.gameObject.GetComponent<Interactable>() != null)
            {
                Interactable interactable = hit.collider.gameObject.GetComponent<Interactable>();
                itemText.text = interactable.interactableName;
                itemText.gameObject.SetActive(true);
                if (Input.GetButtonDown("Interact"))
                {
                    interactable.Interact();
                }
            }
        }
        else
        {
            itemText.gameObject.SetActive(false);
        }
    }


}
