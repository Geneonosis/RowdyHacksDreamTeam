using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class quickfix : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        foreach(Collider col in this.gameObject.GetComponentsInChildren<Collider>())
        {
            col.isTrigger = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
