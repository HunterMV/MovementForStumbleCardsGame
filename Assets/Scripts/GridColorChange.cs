using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridColorChange : MonoBehaviour
{
    public Material material; // Color that object changes to; can be any color 

    private void OnTriggerEnter(Collider other) // Changes the color of the object that makes up the grid when the Player walks on it
    {
        if (other.gameObject.tag == "Player")
        {
            GetComponent<Renderer>().material = material;
        }
    }
}
