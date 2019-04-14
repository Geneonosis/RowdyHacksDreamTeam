using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomObjects : MonoBehaviour
{
    private GameObject RandomItem = null;
    private MeshFilter[] filters = null;
    private GameObject childOBJ = null;
    [SerializeField] GameObject[] objs;
    private Vector3 coords;
    private string tagStr = null;

    // Start is called before the first frame update
    void Start()
    {
        int index = Random.Range(0, objs.Length-1);
        RandomItem = objs[index];

        Debug.Log(index);

        //filters = this.gameObject.GetComponentsInChildren<MeshFilter>();
        foreach(Transform child in transform)
        {
            //filter.mesh = RandomItem.GetComponent<MeshFilter>().mesh;
            coords = child.transform.position;
            //tagStr = child.tag;
            Destroy(child.gameObject);
            RandomItem.layer = 11;
            Instantiate(RandomItem);
            RandomItem.transform.position = coords;
            //RandomItem.tag = tagStr;


            index = Random.Range(0, objs.Length - 1);
            RandomItem = objs[index];
        }
        //this.gameObject.GetComponent<MeshFilter>().mesh = RandomItem.GetComponent<MeshFilter>().mesh;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
