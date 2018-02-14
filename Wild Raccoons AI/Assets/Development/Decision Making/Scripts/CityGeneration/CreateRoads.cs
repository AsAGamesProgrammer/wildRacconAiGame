using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateRoads : MonoBehaviour {

    public PF_Grid gridScript;
    public GameObject roadTile;

    bool crossroadsInstantiated = false;
    bool roadsInstantiated = false;
    public int crossroadNumber = 10;

	// Use this for initialization
	void Start ()
    {
        gridScript = GameObject.Find("A* Manager").GetComponent<PF_Grid>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        Queue<Vector2> crossroadQ = new Queue<Vector2>();

        //CROSSROADS
        if (!crossroadsInstantiated)
        {
            for (int i = 0; i < crossroadNumber; i++)
            {
                //Choose random coordinates
                int randomX = Random.Range(0, gridScript.gridSizeX-1);
                int randomY = Random.Range(0, gridScript.gridSizeY - 1);

                //Add point to q
                crossroadQ.Enqueue(new Vector2(randomX, randomY));

                //Get world position
                var nodePos = gridScript.grid[randomX, randomY].worldPos;
                InstantiateRoadTile(nodePos);
            }

            crossroadsInstantiated = true;
        }

        //ROADS
        if (!roadsInstantiated)
        {
            //If queue is not empty
            while (crossroadQ.Count != 0)
            {
                //Deque next crossroad and draw a road
                drawRoad(crossroadQ.Dequeue());
            }

            //Set flag to true
            roadsInstantiated = true;
        }
 
	}

    //Instantiate a road tile prefab in the given world position
    void InstantiateRoadTile(Vector3 worldPosition)
    {
        //Instantoate tile
        GameObject emptyObject = new GameObject();

        emptyObject.transform.rotation = roadTile.transform.rotation;
        emptyObject.transform.localScale = new Vector3(gridScript.nodeRadius, roadTile.transform.localScale.y, gridScript.nodeRadius);
        emptyObject.transform.position = worldPosition;

        Instantiate(roadTile, emptyObject.transform);
    }

    //Draw road to all directions from crossroad grid position
    void drawRoad(Vector2 tileGridCoordinates)
    {
        int currentX = (int)tileGridCoordinates.x;
        int currentY = (int)tileGridCoordinates.y;

       // if (currentX > gridScript.gridSizeX - currentX)
       // {
            //Draw road LEFT
            while (currentX > 0)
            {
                currentX--;
                InstantiateRoadTile(gridScript.grid[currentX, currentY].worldPos);
            }
            //Reset
            currentX = (int)tileGridCoordinates.x;
       // }
       // else
       // {
            //Draw road RIGHT
            while (currentX < gridScript.gridSizeX - 1)
            {
                currentX++;
                InstantiateRoadTile(gridScript.grid[currentX, currentY].worldPos);
            }
            //Reset
            currentX = (int)tileGridCoordinates.x;
       // }


        int randomValue = Random.Range(0, 2);

        if (randomValue == 0)
        {
            //Draw road DOWN
            while (currentY > 0)
            {
                currentY--;
                InstantiateRoadTile(gridScript.grid[currentX, currentY].worldPos);
            }
            //Reset
            currentY = (int)tileGridCoordinates.y;
        }
        else
        {

            //Draw road UP
            while (currentY < gridScript.gridSizeY - 1)
            {
                currentY++;
                InstantiateRoadTile(gridScript.grid[currentX, currentY].worldPos);
            }
            //Reset
            currentY = (int)tileGridCoordinates.y;
        }
    }

}
