using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Polygon : MonoBehaviour
{
    List<Point> polygonPointArray = new List<Point>();  // 종이 경계의 모서리
    Point parentPolygon;    // 부모 폴리곤
    List<Polygon> polygonLineChild = new List<Polygon>(); // 각 선의 접힌 다른 면에 대한 정보
    public bool HT = true;    // 앞면이면 1 뒷면이면 0
    public static List<Polygon> floorArray = new List<Polygon>();    // 종이가 몇 번째 면인지 나타내기 위한 Z축 index 구조
    public static List<GameObject> objArray = new List<GameObject>();
    static List<GameObject> objs = new List<GameObject>();
    static int planeNum = 0;
    public static void PolygonFold(Point from, Point to, float angle)
    {
        /*
        // from과 to가 모서리와의 교점인지 확인
        for(int i=0; i<floorArray.Count; i++)
        {
            bool leftside = false;
            bool rightside = false;
            for(int j=0; j<floorArray[i].polygonPointArray.Count; j++)
            {
                if(floorArray[i].polygonPointArray[j] <= from && floorArray[i].polygonPointArray[j] <= to)
                    leftside = true;
                else if(floorArray[i].polygonPointArray[j] >= from && floorArray[i].polygonPointArray[j] >= to)
                    rightside = true;
                else
                    leftside = rightside = true;
                
                if(leftside && rightside)
                    break;
            }

            float max_x = 0, max_y = 0, min_x = 0, min_y = 0;

            max_x = floorArray[i].polygonPointArray[0].x;
            max_y = floorArray[i].polygonPointArray[0].y;
            min_x = floorArray[i].polygonPointArray[0].x;
            min_y = floorArray[i].polygonPointArray[0].y;

            for (int k = 1; k < floorArray[i].polygonPointArray.Count; k++)
            {
                max_x = max_x < floorArray[i].polygonPointArray[k].x ? floorArray[i].polygonPointArray[k].x : max_x;
                max_y = max_y < floorArray[i].polygonPointArray[k].y ? floorArray[i].polygonPointArray[k].y : max_y;
                min_x = min_x > floorArray[i].polygonPointArray[k].x ? floorArray[i].polygonPointArray[k].x : min_x;
                min_y = min_y > floorArray[i].polygonPointArray[k].y ? floorArray[i].polygonPointArray[k].y : min_y;
            }

            for (int j = 0; j < floorArray[i].polygonPointArray.Count; j++)
            {
                float a = (to.y - from.y) / (to.x - from.x);
                float b = to.y - a * to.x;

                float x = floorArray[i].polygonPointArray[j].x;
                float y = floorArray[i].polygonPointArray[j].y;
                float px = (x - a * x - 2 * a * b + 2 * a * y) / (a - 1);
                float py = x + px + 2 * b - y;

                if (to.x - from.x == 0)
                {
                    px = x + (to.x - min_x) + (to.x - max_x);
                    py = y;
                }
                else if (to.y - from.y == 0)
                {
                    px = x;
                    py = y + (to.y - min_y) + (to.y - max_y);
                }
                
                if (leftside && rightside)
                {
                    Polygon poly = new Polygon();
                    poly.polygonPointArray.Add(from);
                    //if (floorArray[i].polygonPointArray[j] <= from && floorArray[i].polygonPointArray[j] <= to)
                        
                }
                else if (leftside)
                {
                    floorArray[i].polygonPointArray[j].x = px;
                    floorArray[i].polygonPointArray[j].y = py;
                    floorArray[i].switchHT();
                }
            }

            if(leftside)
            {
                Destroy(objArray[i]);
                objArray[i].GetComponent<Polygon>().createMesh();
                Instantiate(objArray[i]);
            }
        }
        */

        
        Vector3 from3 = new Vector3(from.x, 0, from.y);
        Vector3 to3 = new Vector3(to.x, 0, to.y);
        Vector3 mid = (from3 + to3)/2;
        Vector3 toto = mid;
        if (from3.x == to3.x)
            toto.x = 1;
        if (from3.z == to3.z)
            toto.z = 1;

        if(from3.x != to3.x && from3.z != to3.z)
        {
            float a = (to.y - from.y) / (to.x - from.x);
            float b = to.y - a * to.x;

            float m = -1 / a;

            float yy = (from.x * (from.x - 2) + from.y * from.y - to.x * (to.x - 2) - to.y * to.y) / (2 * (from.y - to.y));
            float n = yy + m;

            toto.x = 1;
            toto.z = yy;
        }
        int cntt = objArray.Count, delnum = 0;
        List<int> delList = new List<int>();
        Debug.Log(cntt);
        Quaternion qAngle = Quaternion.Euler(new Vector3(0, angle, 0));
        for (int i = 0; i < cntt; i++)
        {
            if (lineTouchChk(from3, to3, objArray[i].transform.GetChild(0).gameObject)) 
            {
                Debug.Log(i + "번 째 포문은 성공");
                GameObject[] temp = PolygonApplyMeshCut.ChildCut(objArray[i].transform.GetChild(0).gameObject, mid, toto, qAngle);
                temp[1].transform.name = "plane" + planeNum;
                planeNum++;

                temp[0].transform.name = "plane" + planeNum;
                planeNum++;
                delList.Add(i);
                
                objs.Add(temp[1]);
                objs.Add(temp[0]);

                objArray[i].gameObject.SetActive(false);
                objs[i * 2].gameObject.SetActive(false);
                objs[i * 2 + 1].gameObject.SetActive(false);
            }
        }
        for(int i=0; i<delList.Count; i++)
        {
            objArray.RemoveAt(delList[i]-delnum);
            delnum++;
            Debug.Log(i + "번째 gameobject objArray에서 삭제");
        }
        
        int objcnt = objs.Count;
        Debug.Log("objs count " + objs.Count);
        for (int i=0; i<objs.Count; i++)
        {
            objs[i].SetActive(true);
            objArray.Add(objs[i]);
        }
        objs.Clear();
    }
    static void Init()
    {

    }
    public void createMesh()
    {
        /*
        int sz = polygonPointArray.Count;

        Vector3[] vertices = new Vector3[sz];
        int[] triangles = new int[sz*3];
        
        for(int i=0; i<sz; i++)
        {
            Vector3 v = new Vector3(
                polygonPointArray[i].x,
                polygonPointArray[i].y,
                0);
            vertices[i] = v;
        }

        for(int i=0; i<sz-2; i++)
        {
            triangles[i*3    ] = 0;
            triangles[i*3 + 1] = i+1;
            triangles[i*3 + 2] = i+2;
        }
        Mesh mesh = new Mesh();

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        GetComponent<MeshFilter>().mesh = mesh;
        switchHT();

        createShader();
        */
        GameObject _obj = Instantiate(Resources.Load("Prefabs/PolygonObject") as GameObject);
        objArray.Add(_obj);
        createShader(_obj);
    }

    public void createShader(GameObject obj)
    {
        if (HT)
            obj.GetComponentInChildren<MeshRenderer>().material.color = Color.red;
        else
            obj.GetComponentInChildren<MeshRenderer>().material.color = Color.green;
    }

    public int getNumofPoints()
    {
        return polygonPointArray.Count;
    }

    public Point getPoint(int idx)
    {
        return polygonPointArray[idx];
    }

    public void switchHT()
    {
        HT = HT ? false : true;
    }

    public bool getHT()
    {
        return HT;
    }

    public void addPoint(Point p)
    {
        polygonPointArray.Add(p);
    }

    /*
    private bool IntersectTriangle(Vector3 RayOrigin, Vector3 RayDirection, Vector3 V0, Vector3 V1, Vector3 V2)
    {
        Vector3 edge1 = V1 - V0;
        Vector3 edge2 = V2 - V0;

        Vector3 pvec = Vector3.Cross(RayDirection, edge2);

        dot = Vector3.Dot(edge1, pvec);

        Vector3 tvec;
        if (dot > 0) tvec = RayOrigin - V0;
        else
        {
            tvec = V0 - RayOrigin;
            dot = -dot;
        }

        if (dot < 0.0001f)
            return false;

        u = Vector3.Dot(tvec, pvec);
        // if (u < 0.0f || u > dot)
        // return false;

        Vector3 qvec = Vector3.Cross(tvec, edge1);

        v = Vector3.Dot(RayDirection, qvec);
        // if (v < 0.0f || u + v > dot) return false;

        t = Vector3.Dot(edge2, qvec);
        float flnvDet = 1.0f / dot;

        t *= flnvDet;
        u *= flnvDet;
        v *= flnvDet;

        return true;
    }
    */

    private static bool lineTouchChk(Vector3 from, Vector3 to, GameObject obj)
    {
        RaycastHit hit;
        Ray ray;
        Vector3 startv = from;
        startv.y = 1;
        int touchk = 0;
        for(int i=0; i<=20; i++)
        {
            startv.x += (to.x - from.x) / 20;
            startv.z += (to.z - from.z) / 20;
            ray = new Ray(startv, Vector3.down);
            if (Physics.Raycast(ray, out hit, 2))
            {
                Debug.Log(hit.transform.parent.name +"."+hit.transform.name);
                if (hit.transform.gameObject == obj)
                {
                    touchk++;
                }
                if (touchk >= 2)
                    return true;
            }
        }
        return false;
    }
    
}
