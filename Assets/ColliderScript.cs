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
    [SerializeField] GameObject[] lookingForThese;
    [SerializeField] GameObject DestroyMe;
    private TextMeshPro text = null;
    GameObject newguy;
    Vector3 coord;
    int index;


    private int counter = 0;
    private string str;

    private void Start()
    {
        coord = DestroyMe.transform.position;

        index = Random.Range(0, lookingForThese.Length - 1);
        lookingForObject = lookingForThese[index];

        Destroy(DestroyMe);
        newguy = Instantiate(lookingForObject);
        newguy.layer = 0;
        newguy.layer = 12;
        newguy.transform.position = coord;
        newguy.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        newguy.transform.SetParent(this.gameObject.transform);
        Debug.Log("LOOKING FOR: " + newguy.tag);

        text = planepanel.GetComponent<TextMeshPro>();
        text.SetText(lookingForObject.tag);
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("COLIDE");
        


        lookingATObject = other.gameObject;
        if(lookingForObject.tag == lookingATObject.tag)
        {
            index = Random.Range(0, lookingForThese.Length - 1);
            lookingForObject = lookingForThese[index];
            coord = newguy.transform.position;
            text.SetText("Found");
            Destroy(newguy);

            newguy = Instantiate(lookingForObject);
            newguy.layer = 0;
            newguy.layer = 12;
            newguy.transform.position = coord;
            newguy.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            newguy.transform.SetParent(this.gameObject.transform);

        }
        else
        {
            text.SetText("Not Found");
        }
    }
}
