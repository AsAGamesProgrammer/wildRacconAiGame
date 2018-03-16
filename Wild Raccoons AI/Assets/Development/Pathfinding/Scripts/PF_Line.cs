using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PF_Line {

    const float verticalLineGradient = 100000;

    float gradient;
    float y_intercept;

    Vector2 pointOnLine_1;
    Vector2 pointOnLine_2;

    float gradientPerpendicular;

    bool approachSide;

    public PF_Line(Vector2 pointOnLine_, Vector2 pointPerpendicularToLine_)
    {
        // Find the difference between the two points.
        float dx = pointOnLine_.x - pointPerpendicularToLine_.x;
        float dy = pointOnLine_.y - pointPerpendicularToLine_.y;

        if(dx == 0)
        {
            gradientPerpendicular = verticalLineGradient;
        }
        else
        {
            gradientPerpendicular = dy / dx;
        }


        // Set the line gradient.
        if(gradientPerpendicular == 0)
        {
            gradient = verticalLineGradient;
        }
        else
        {
            gradient = -1 / gradientPerpendicular;
        }

        // Calculate the y intercept.
        y_intercept = pointOnLine_.y - gradient * pointOnLine_.x;

        // Set the two points on the line.
        pointOnLine_1 = pointOnLine_;
        pointOnLine_2 = pointOnLine_ + new Vector2(1, gradient);

        // Calculate the side of approach.
        approachSide = false;
        approachSide = GetSide(pointPerpendicularToLine_);
    }

    public bool GetSide(Vector2 point_)
    {
        return (point_.x - pointOnLine_1.x) * (pointOnLine_2.y - pointOnLine_1.y) > (point_.y - pointOnLine_1.y) * (pointOnLine_2.x - pointOnLine_1.x);
    }

    public bool HasCrossedLine(Vector2 point_)
    {
        return GetSide(point_) != approachSide;
    }

    public void DrawWithGizmos(float length_)
    {
        Vector3 lineDirection = new Vector3(1, 0, gradient).normalized;
        Vector3 lineCentre = new Vector3(pointOnLine_1.x, 0, pointOnLine_1.y) + Vector3.up;

        Gizmos.DrawLine(lineCentre - lineDirection * length_ / 2f, lineCentre + lineDirection * length_ / 2f);
    }
}
