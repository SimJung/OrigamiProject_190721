using UnityEngine;
using System.Collections;

public class TouchTest : MonoBehaviour
{
    void Update()
    {
        Touch myTouch = Input.GetTouch(0);
        Vector2 posNext, posPrev;
        Touch[] myTouches = Input.touches;
        if(Input.touchCount == 2)
        {
        }
        for (int i = 0; i < Input.touchCount; i++)
        {
            //Do something with the touches
        }
    }
}