using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItems : MonoBehaviour
{
    public GameObject[] items;
    int randomInt;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Drop()
    {
        //randomInt = Random.Range(0, items.Length);
        //Instantiate(items[randomInt], new Vector3(transform.position.x, transform.position.z + 4f, transform.position.z), Quaternion.identity);
    }
}
