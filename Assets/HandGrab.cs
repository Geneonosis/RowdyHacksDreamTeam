using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandGrab : MonoBehaviour
{
    public GameObject CollidingObject;
    public GameObject objectInHand;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger) > 0.2 && CollidingObject)
        {
            GrabObject();
        }
        if (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger) < 0.2 && objectInHand)
        {
            ReleaseObject();
        }
    }

    private void ReleaseObject()
    {
        objectInHand.GetComponent<Rigidbody>().isKinematic = false;
        objectInHand.transform.SetParent(null);
        objectInHand = null;
    }

    private void GrabObject()
    {
        objectInHand = CollidingObject;
        objectInHand.transform.SetParent(this.transform);
        objectInHand.GetComponent<Rigidbody>().isKinematic = true;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Collider>() && other.gameObject.tag != "Player")
        {
            CollidingObject = other.gameObject;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        CollidingObject = null;
    }
}
