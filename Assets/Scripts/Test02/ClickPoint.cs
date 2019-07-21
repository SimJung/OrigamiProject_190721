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
        if(to.x == from.x)
        {
            
        }else
        {
            float a = (to.y - from.y) / (to.x - from.x);
            float b = from.y - a * from.x;

            float m_val = 0.52f;

            float xm = (-m_val - b) / a;
            float xp = (m_val - b) / a;
            float ym = a * -m_val + b;
            float yp = a * m_val + b;

            Vector2 p1 = new Vector2(xm, -m_val);
            Vector2 p2 = new Vector2(xp, m_val);
            Vector2 p3 = new Vector2(-m_val, ym);
            Vector2 p4 = new Vector2(m_val, yp);

            Vector2 f2 = Vector2.zero;
            Vector2 t2 = Vector2.zero;

            Debug.Log(p1 + " " + p2 + " " + p3 + " " + p4);
            if (a > 0)
            {
                f2 = (xm > -m_val) ? p1 : p3;
                t2 = (xp < m_val) ? p2 : p4;
            }
            else
            {
                f2 = (ym < m_val) ? p3 : p2;
                t2 = (yp > -m_val) ? p4 : p1;
            }

            from.x = f2.x; from.y = f2.y;
            to.x = t2.x; to.y = t2.y;

        }

        float bound = 0.03f;

        // 좌표 자석 효과
        if (Mathf.Abs(from.x) < bound)
            from.x = 0;
        if (Mathf.Abs(from.y) < bound)
            from.y = 0;
        if (Mathf.Abs(to.x) < bound)
            to.x = 0;
        if (Mathf.Abs(to.y) < bound)
            to.y = 0;

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
            Debug.Log("X : " + (Input.mousePosition.x - Screen.width / 2f) / 172f + " Y : " + (Input.mousePosition.y - Screen.height / 2f) / 172f);
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 Pos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
                from = new Vector3((Pos.x - Screen.width / 2f) / 172f, (Pos.y - Screen.height / 2f) / 172f, 0);

                fromChk = false;
            }
        }
        if (toChk)
        {
            Debug.Log("X : " + (Input.mousePosition.x - Screen.width / 2f) / 172f + " Y : " + (Input.mousePosition.y - Screen.height / 2f) / 172f);
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 Pos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
                to = new Vector3((Pos.x - Screen.width / 2f) / 172f, (Pos.y - Screen.height / 2f) / 172f, 0);
                
                toChk = false;
            }
        }
        if (destChk)
        {
            Debug.Log("X : " + (Input.mousePosition.x - Screen.width / 2f) / 172f + " Y : " + (Input.mousePosition.y - Screen.height / 2f) / 172f);
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 Pos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
                dest = new Vector3((Pos.x - Screen.width / 2f) / 172f, (Pos.y - Screen.height / 2f) / 172f, 0);
                destChk = false;
            }
        }
    }
}
