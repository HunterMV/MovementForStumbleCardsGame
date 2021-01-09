using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour {

    public float x_Start, y_Start;
    public int columLenght, rowLength;
    public float x_Space, y_Space;
    

    public GameObject prefab; // Object that makes up grid; can be anything 

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < columLenght * rowLength; i++) // Creates the grind on game start, based on x_Start, y_Start, columLenght, rowLength, x_Space and y_Space
        {
            var cube = Instantiate(prefab, new Vector3(x_Start + (x_Space * (i %columLenght)), 0, y_Start +(y_Space * (i / columLenght))), Quaternion.identity);
            cube.transform.parent = transform;
        }
    }

    public void GridOnOff (bool On) // Function called to turn grid on or off
    {
        if (!On)
        {
            foreach (Transform child in transform)
            {
                child.GetComponent<Renderer>().enabled = false;
            }
        }
        else
        {
            foreach (Transform child in transform)
            {
                child.GetComponent<Renderer>().enabled = true;
            }
        }
    }
    
}
