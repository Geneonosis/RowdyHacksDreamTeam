using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomObjects : MonoBehaviour
{
    private GameObject RandomItem = null;
    [SerializeField] GameObject[] objs;

    // Start is called before the first frame update
    void Start()
    {
        int index = Random.Range(0, objs.Length-1);
        RandomItem = objs[index];

        Debug.Log(index);
        this.gameObject.GetComponent<MeshFilter>().mesh = RandomItem.GetComponent<MeshFilter>().mesh;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
