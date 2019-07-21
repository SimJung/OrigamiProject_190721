using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoldManager : MonoBehaviour
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
        GameObject gobj = Instantiate(Resources.Load("Prefabs/Paper") as GameObject);
        temp.Add(gobj);
        Destroy(gobj.gameObject);
        steps.Add(temp);
    }

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
            if(isTemp)
            {
                for(int i=0; i<frameObjs.Count; i++)
                {
                    Destroy(frameObjs[i]);
                }
                frameObjs.Clear();
                isTemp = false;
            }

            for(int i=0; i<steps[now].Count; i++)
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
            for(int i=0; i<steps[now].Count; i++)
            {
                _obj = steps[now][i];
                Material material = _obj.GetComponent<MeshRenderer>().material;
                angle.z = 45;
                isTemp = true;
                GameObject[] temp;
                //temp = MeshCut.Cut(_obj, mid, diff, material);
                //frameObjs.Add(temp[0]);
                //frameObjs.Add(temp[1]);
            }
        }
    }
}
