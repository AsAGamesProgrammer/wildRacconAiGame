using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateRoads : MonoBehaviour {

    public PF_Grid gridScript;
    public GameObject roadTile;

    bool crossroadsInstantiated = false;
    int crossroadNumber = 10;

	// Use this for initialization
	void Start ()
    {
        gridScript = GameObject.Find("A* Manager").GetComponent<PF_Grid>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        //Put crossroads
        if (!crossroadsInstantiated)
        {
            for (int i = 0; i < crossroadNumber; i++)
            {
                //Choose random coordinates
                int randomX = Random.Range(0, gridScript.gridSizeX-1);
                int randomY = Random.Range(0, gridScript.gridSizeY - 1);

                //Get world position
                var nodePos = gridScript.grid[randomX, randomY].worldPos;
                InstantiateRoadTile(nodePos);
            }

            crossroadsInstantiated = true;
        }
 

		
	}

    void InstantiateRoadTile(Vector3 worldPosition)
    {
        //Instantoate tile
        GameObject emptyObject = new GameObject();

        emptyObject.transform.rotation = roadTile.transform.rotation;
        emptyObject.transform.localScale = new Vector3(gridScript.nodeRadius, roadTile.transform.localScale.y, gridScript.nodeRadius);
        emptyObject.transform.position = worldPosition;

        Instantiate(roadTile, emptyObject.transform);
    }
}
