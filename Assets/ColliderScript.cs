using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ColliderScript : MonoBehaviour
{
    private GameObject collidingObject = null;
    [SerializeField] GameObject planepanel;
    [SerializeField] GameObject lookingForObject;
    [SerializeField] GameObject lookingATObject;
    private TextMeshPro text = null;

    private int counter = 0;
    private string str;

    private void Start()
    {
        text = planepanel.GetComponent<TextMeshPro>();
    }
    private void OnTriggerEnter(Collider other)
    {
        counter++;
        str = " " + counter;
        if (other.name.Contains("pathTrack"))
        {
            lookingATObject = other.gameObject;
            if(lookingForObject.tag == lookingATObject.tag)
            {
                text.SetText("Found");
            }
            else
            {
                text.SetText("Not Found");
            }
        }
    }
}
