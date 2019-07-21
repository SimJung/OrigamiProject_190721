using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickPoint : MonoBehaviour
{
    public InputField angle;

    Vector3 from;
    Vector3 to;
    Vector3 dest;
    private bool fromChk = false, toChk = false, destChk = false;
    float distance = 3;

    public void Fold()
    {
        StartCoroutine("getMousePoint");
    }

    public void Reset()
    {
        CutManager.Reset();
    }

    IEnumerator getMousePoint()
    {
        Debug.Log("getMouseStart");
        while (true)
        {
            fromChk = true;
            yield return new WaitForSeconds(0.1f);
            if (!fromChk)
                break;
        }
        Debug.Log("end From");
        while (true)
        {
            toChk = true;
            yield return new WaitForSeconds(0.1f);
            if (!toChk)
                break;
        }
        Debug.Log("end To");
        while (true)
        {
            destChk = true;
            yield return new WaitForSeconds(0.1f);
            if (!destChk)
                break;
        }
        Debug.Log("end Dest");

        //여기서 접기 실행
        float a = (to.y - from.y) / (to.x - from.x);
        float b = from.y - a * from.x;

        float xm = (-0.5f - b) / a;
        float xp = (0.5f - b) / a;
        float ym = a / -2 + b;
        float yp = a / 2 + b;

        Vector2 p1 = new Vector2(xm, -0.5f);
        Vector2 p2 = new Vector2(xp, 0.5f);
        Vector2 p3 = new Vector2(-0.5f, ym);
        Vector2 p4 = new Vector2(0.5f, yp);

        Vector2 f2 = Vector2.zero;
        Vector2 t2 = Vector2.zero;

        Debug.Log(p1 + " " + p2 + " " + p3 + " " + p4);
        if (a > 0)
        {
            f2 = (xm > -0.5) ? p1 : p3;
            t2 = (xp < 0.5) ? p2 : p4;
        }
        else
        {
            f2 = (xp > -0.5) ? p2 : p3;
            t2 = (xm < 0.5) ? p1 : p4;
        }
        from.x = f2.x;  from.y = f2.y;
        to.x = t2.x;    to.y = t2.y;

        Debug.Log("from : " + from + "to : " + to);
        
        float fangle = float.Parse(angle.text);
        if(from.x < to.x)
            CutManager.fold(from, to, dest, fangle);
        else
            CutManager.fold(to, from, dest, fangle);
    }

    private void Update()
    {
        if (fromChk)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 Pos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
                from = new Vector3((Pos.x - Screen.width / 2f) / 172f, (Pos.y - Screen.height / 2f) / 172f, 0);

                fromChk = false;
            }
        }
        if (toChk)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 Pos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
                to = new Vector3((Pos.x - Screen.width / 2f) / 172f, (Pos.y - Screen.height / 2f) / 172f, 0);
                
                toChk = false;
            }
        }
        if (destChk)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 Pos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
                dest = new Vector3((Pos.x - Screen.width / 2f) / 172f, (Pos.y - Screen.height / 2f) / 172f, 0);
                destChk = false;
            }
        }
    }
}
