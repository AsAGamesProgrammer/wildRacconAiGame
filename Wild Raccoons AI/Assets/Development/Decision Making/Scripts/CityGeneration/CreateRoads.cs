using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateRoads : MonoBehaviour {

    public PF_Grid gridScript;
    public GameObject roadTile;

	// Use this for initialization
	void Start ()
    {
        gridScript = GameObject.Find("A* Manager").GetComponent<PF_Grid>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        var nodePos = gridScript.grid[1, 1].worldPos;
        GameObject emptyObject = new GameObject();
        emptyObject.transform.rotation = roadTile.transform.rotation;
        emptyObject.transform.localScale = roadTile.transform.localScale;
        emptyObject.transform.position = nodePos;

        Instantiate(roadTile, emptyObject.transform);
		
	}
}
