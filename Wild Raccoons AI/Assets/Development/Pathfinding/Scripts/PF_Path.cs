using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PF_Path {

    public readonly Vector3[] lookPoints;
    public readonly PF_Line[] turnBoundaries;
    public readonly int finishLineIndex;

    public PF_Path(Vector3[] waypoints_, Vector3 startPos_, float turnDistance_)
    {
        lookPoints = waypoints_;
        turnBoundaries = new PF_Line[lookPoints.Length];
        finishLineIndex = turnBoundaries.Length - 1;

        // Convert the vec3 to a vec2.
        Vector2 previousPoint = v3Tov2(startPos_);

        // Loop through all points, caching information about each.
        for(int i = 0; i < lookPoints.Length; i++)
        {
            Vector2 currentPoint = v3Tov2(lookPoints[i]);
            Vector2 directionToCurrentPoint = (currentPoint - previousPoint).normalized;
            Vector2 turnBoundaryPoint = new Vector2();

            if (i == finishLineIndex)
            {
                turnBoundaryPoint = currentPoint;
            }
            else
            {
                turnBoundaryPoint = currentPoint - directionToCurrentPoint * turnDistance_;
            }

            turnBoundaries[i] = new PF_Line(turnBoundaryPoint, previousPoint - directionToCurrentPoint * turnDistance_);

            previousPoint = turnBoundaryPoint;
        }
    }

    public Vector2 v3Tov2(Vector3 v3_)
    {
        // Return a vec2 using the x and z components of the original vec3.
        return new Vector2(v3_.x, v3_.z);
    }

    public void DrawWithGizmos()
    {
        Gizmos.color = Color.black;

        foreach(Vector3 p in lookPoints)
        {
            Gizmos.DrawCube(p + Vector3.up, Vector3.one);
        }

        Gizmos.color = Color.white;
        foreach(PF_Line l in turnBoundaries)
        {
            l.DrawWithGizmos(10);
        }
    }
}
