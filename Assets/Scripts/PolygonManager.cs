using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PolygonManager : MonoBehaviour
{
    public GameObject poly;
    public InputField angle;

    Vector3 from;
    Vector3 to;
    private bool fromChk = false, toChk = false;

    public void create()
    {

        //if (Polygon.floorArray.Count <= 0)
        //{
        //    _obj = Instantiate(poly) as GameObject;

        //    float[] arr_x = { -1, 1, 1, -1 };
        //    float[] arr_y = { 1, 1, -1, -1 };

        //    for (int i = 0; i < arr_x.Length; i++)
        //    {
        //        Point temp = new Point(arr_x[i], arr_y[i]);
        //    }
        //}else{
        //    _obj = Instantiate(Polygon.objArray[0]) as GameObject;
        //}
        poly.GetComponent<Polygon>().createMesh();
    }

    public void delete()
    {
        
        //Point from = new Point(float.Parse(fromx.text), float.Parse(fromy.text));
        //Point to = new Point(float.Parse(tox.text), float.Parse(toy.text));

        StartCoroutine("getMousePoint");
    }

    IEnumerator getMousePoint()
    {
        Debug.Log("getMouseStart");
        while(true)
        {
            fromChk = true;
            yield return new WaitForSeconds(0.1f);
            if (!fromChk)
                break;
        }
        Debug.Log("end From");
        while(true)
        {
            toChk = true;
            yield return new WaitForSeconds(0.1f);
            if (!toChk)
                break;
        }
        Debug.Log("end To");

        Debug.Log(from.x+","+from.y +"   ,   "+ to.x + "," + to.y);
        Point f = new Point(from.x, from.y);
        Point t = new Point(to.x, to.y);
        Polygon.PolygonFold(f, t, float.Parse(angle.text));
    }

    private void Update()
    {
        if (fromChk)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 Pos = Input.mousePosition;
                from = Camera.main.ScreenToWorldPoint(Pos);
                if (from.x > 1) from.x = 1;
                else if (from.x < -1) from.x = -1;
                if (from.y > 1) from.y = 1;
                else if (from.y < -1) from.y = -1;

                fromChk = false;
            }
        }
        if (toChk)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 Pos = Input.mousePosition;
                to = Camera.main.ScreenToWorldPoint(Pos);
                if (to.x > 1) to.x = 1;
                else if (to.x < -1) to.x = -1;
                if (to.y > 1) to.y = 1;
                else if (to.y < -1) to.y = -1;

                toChk = false;
            }
        }
    }
}
