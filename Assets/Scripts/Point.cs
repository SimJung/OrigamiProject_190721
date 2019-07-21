using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
    public float x;
    public float y;

    public Point(float p1, float p2)
    {
        x = p1;
        y = p2;
    }

    public static bool operator >(Point p1, Point p2)
    {
        if(p1.x > p2.x)
            return true;
        else
            return false;
    }

    public static bool operator <(Point p1, Point p2)
    {
        if(p1.x < p2.x)
            return true;
        else
            return false;
    }

    public static bool operator >=(Point p1, Point p2)
    {
        if(p1.x >= p2.x)
            return true;
        else
            return false;
    }

    public static bool operator <=(Point p1, Point p2)
    {
        if(p1.x <= p2.x)
            return true;
        else
            return false;
    }
    
}
