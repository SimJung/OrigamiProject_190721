using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutManager : MonoBehaviour
{
    static int now = 0;
    static List<List<GameObject>> steps = new List<List<GameObject>>();
    public static bool chkdrag = false;
    static GameObject _obj;
    // _obj = Instantiate(Resources.Load("Prefabs/Paper") as GameObject)

    static float distance = 3;
    static Vector3 firstTouchPoint = Vector3.zero;
    static Vector3 prevTouchPoint = Vector3.zero;
    static Vector3 screenMousePosition = Vector3.zero;

    static List<GameObject> frameObjs = new List<GameObject>();
    static bool isTemp = false;


    private void Start()
    {
        List<GameObject> temp = new List<GameObject>();
        GameObject gobj = Instantiate(Resources.Load("Prefabs/PaperCube") as GameObject);
        temp.Add(gobj);
        steps.Add(temp);
    }
    
    public static void Reset()
    {
        foreach (List<GameObject> l in steps)
        {
            foreach(GameObject g in l)
            {
                Destroy(g.gameObject);
            }
            l.Clear();
        }
        foreach (GameObject g in frameObjs)
        {
            Destroy(g.gameObject);
        }
        steps.Clear();
        frameObjs.Clear();
        chkdrag = false;
        isTemp = false;
        List<GameObject> temp = new List<GameObject>();
        GameObject gobj = Instantiate(Resources.Load("Prefabs/PaperCube") as GameObject);
        temp.Add(gobj);
        steps.Add(temp);
        now = 0;
    }

    public static void fold(Vector3 from, Vector3 to, Vector3 dest, float angle)
    {
        bool foldCase = true, foldNowCase = true;
        if(now != 0)
        {
            List<GameObject> lg = new List<GameObject>();
            steps.Add(lg);
            for (int i = 0; i < frameObjs.Count; i++)
            {
                steps[now].Add(frameObjs[i]);
            }
        }
        Debug.Log(steps[now].Count);
        if (frameObjs.Count > 0)
        {
            frameObjs.Clear();
        }
        Vector3 mid = new Vector3(from.x + to.x, from.y + to.y, from.z + to.z) / 2;
        Material mat = steps[now][0].GetComponent<MeshRenderer>().material;
        
        for (int i=0; i<steps[now].Count; i++)
        {
            Debug.Log("i : " + i);
            Vector3 toto = mid;
            Vector3 line = Vector3.zero;
            if (from.x == to.x)
                toto.x = 1;
            if (from.y == to.y)
                toto.y = 1;
            if (from.x != to.x && from.y != to.y)
            {
                float a = (to.y - from.y) / (to.x - from.x);
                float b = to.y - a * to.x;

                float m = -1 / a;

                if (a * dest.x - dest.y - a * from.x + from.y > 0)
                    foldCase = true;
                else
                    foldCase = false;
                Vector3 pt = steps[now][i].GetComponent<Rigidbody>().centerOfMass;
                if (a * pt.x - pt.y - a * from.x + from.y > 0)
                    foldNowCase = true;
                else
                    foldNowCase = false;
                line = new Vector3(a, -1, -a * from.x + from.y);


                if(foldCase)
                {
                    if(a>0)
                    {
                        toto.x = mid.x - 1;
                        toto.y = m * toto.x + mid.y - m * mid.x;
                    }
                    else
                    {
                        toto.x = mid.x + 1;
                        toto.y = m * toto.x + mid.y - m * mid.x;
                    }
                }
                else
                {
                    if (a > 0)
                    {
                        toto.x = mid.x + 1;
                        toto.y = m * toto.x + mid.y - m * mid.x;
                    }
                    else
                    {
                        toto.x = mid.x - 1;
                        toto.y = m * toto.x + mid.y - m * mid.x;
                    }
                }
            }
            
            Debug.Log("foldCase : " + foldCase);
            Debug.Log("from : (" + from.x + ", " + from.y + ", " + from.z + " to : (" + to.x + ", " + to.y + ", " + to.z + ")");
            Debug.Log("mid : ("+mid.x + ","+mid.y+","+mid.z+")"+" toto : " +toto);
            bool isCut = false;
            Vector3 startv = from;
            startv.z = startv.z - 5;
            /*
            
            Ray ray1 = new Ray(new Vector3(from.x-5, from.y-5, -0.001f), new Vector3(to.x, to.y, -0.001f));
            Ray ray2 = new Ray(new Vector3(from.x +5, from.y +5, -0.001f), new Vector3(to.x, to.y, -0.001f));
            RaycastHit hit;
            if (Physics.Raycast(ray1, out hit, 20))
            {
                Debug.Log(hit.transform.name);
                if (hit.transform.gameObject.name == steps[now][i].name)
                {
                    isCut = true;
                }
            }
            if (Physics.Raycast(ray2, out hit, 20))
            {
                Debug.Log(hit.transform.name);
                if (hit.transform.gameObject.name == steps[now][i].name)
                {
                    isCut = true;
                }
            }
            */
            Debug.Log("It has to hit with " + steps[now][i].name);
            steps[now][i].transform.position -= new Vector3(0, 0, 0.001f);
            for (int j = 0; j <= 30; j++)
            {
                startv.x += (to.x - from.x) / 30;
                startv.y += (to.y - from.y) / 30;
                Ray ray = new Ray(startv, new Vector3(0, 0, 1));
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 20))
                {
                    Debug.Log(hit.transform.name);
                    if (hit.transform.gameObject.name == steps[now][i].name)
                    {
                        isCut = true;
                        break;
                    }
                }
                
            }
            Debug.Log(isCut);
            steps[now][i].transform.position += new Vector3(0, 0, 0.001f);
            
            if (isCut)
            {
                GameObject[] temp = MeshCut.Cut(steps[now][i], mid, toto, mat, foldCase, line, to-from, angle);
                temp[0].GetComponent<RectTransform>().pivot = mid;
                temp[1].GetComponent<RectTransform>().pivot = mid;

                if(temp[0].transform.rotation == Quaternion.identity)
                {
                    temp[0].transform.position += (temp[0].transform.rotation * temp[0].GetComponent<RectTransform>().pivot);
                    temp[0].transform.rotation *= Quaternion.AngleAxis(angle, to - from);
                    temp[0].transform.position -= (temp[0].transform.rotation * temp[0].GetComponent<RectTransform>().pivot);
                }



                if (temp[0].transform.name != "emptyCut")
                {
                    temp[0].transform.name += now+1;
                    temp[0].transform.name += '_';
                    temp[0].transform.name += i;
                    frameObjs.Add(temp[0]);
                }
                    
                if (temp[1].transform.name != "emptyCut")
                {
                    temp[1].transform.name += now+1;
                    temp[1].transform.name += '_';
                    temp[1].transform.name += i;
                    frameObjs.Add(temp[1]);
                }
            }
            // 자르는 부분에 없을 때
            else if(foldCase == foldNowCase)
            {
                steps[now][i].name = now + "foldEqualblock" + (now + 1) + '_' + i;
                steps[now][i].GetComponent<RectTransform>().pivot = mid;
                frameObjs.Add(steps[now][i]);
            }
            else
            {
                steps[now][i].name = now + "block" + (now + 1) + '_' + i;
                steps[now][i].GetComponent<RectTransform>().pivot = mid;
                steps[now][i].transform.position += (steps[now][i].transform.rotation * steps[now][i].GetComponent<RectTransform>().pivot);
                steps[now][i].transform.rotation = Quaternion.AngleAxis(angle, to - from);
                steps[now][i].transform.position -= (steps[now][i].transform.rotation * steps[now][i].GetComponent<RectTransform>().pivot);
                frameObjs.Add(steps[now][i]);
            }
        }
        now++;
    }

    Quaternion getAngle(Vector3 from, Vector3 to, Vector3 direc)
    {
        Quaternion q = Quaternion.identity;
        return q;
    }

    bool IsTouchLine(Vector3 from, Vector3 to, GameObject obj)
    {
        RaycastHit hit;
        Ray ray = new Ray(from, to);
        if (Physics.Raycast(ray, out hit, 10))
        {
            if (hit.transform.name == obj.name)
                return true;
        }
        return false;
    }
    
    /*
    public static void dragAction(Vector3 nowMouse)
    {
        screenMousePosition = Camera.main.ScreenToWorldPoint(nowMouse);

        if (!chkdrag)
        {
            firstTouchPoint = screenMousePosition;
            prevTouchPoint = screenMousePosition;
            Debug.Log("첫 터치 : " + firstTouchPoint);
            chkdrag = true;
        }
        else if (prevTouchPoint != screenMousePosition)
        {
            if (isTemp)
            {
                for (int i = 0; i < frameObjs.Count; i++)
                {
                    Destroy(frameObjs[i]);
                }
                frameObjs.Clear();
                isTemp = false;
            }

            for (int i = 0; i < steps[now].Count; i++)
            {
                GameObject obj;
                obj = Instantiate(steps[now][i]);
                frameObjs.Add(obj);
            }
            Vector3 diff = screenMousePosition - firstTouchPoint;
            Vector3 mid = (prevTouchPoint + screenMousePosition) / 2;
            Debug.Log("현재 마우스 위치 : " + screenMousePosition);
            Debug.Log("차 벡터 : " + diff);
            prevTouchPoint = screenMousePosition;

            Quaternion angle = Quaternion.identity;
            for (int i = 0; i < steps[now].Count; i++)
            {
                _obj = steps[now][i];
                Material material = _obj.GetComponent<MeshRenderer>().material;
                angle.z = 45;
                isTemp = true;
                GameObject[] temp;
                temp = MeshCut.Cut(_obj, mid, diff, material);
                frameObjs.Add(temp[0]);
                frameObjs.Add(temp[1]);
            }
        }
    }*/
}
