using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] GameObject node;

    [SerializeField] bool open = false;
    
    // Update is called once per frame
    void Update()
    {
        if (open)
        {
            node.GetComponent<Pathnode>().nodeActive = true;
            GetComponentInChildren<MeshRenderer>().enabled = false;
        }
        else
        {
            node.GetComponent<Pathnode>().nodeActive = false;
            GetComponentInChildren<MeshRenderer>().enabled = true;
        }
    }
}
