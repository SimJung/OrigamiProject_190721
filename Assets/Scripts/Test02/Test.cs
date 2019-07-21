using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Vector3 f;
    public Vector3 t;
    public Vector3 pivot;
    public float rotationSpeed = 10;
    RectTransform tr;
    void Start()
    {
        tr = GetComponent<RectTransform>();
        tr.pivot = pivot;
        tr.position += (tr.rotation * tr.pivot);
        
        //tr.rotation = Quaternion.AngleAxis(angle, t - f);

        tr.position -= (tr.rotation * tr.pivot);
    }

    private void Update()
    {
        tr.position += (tr.rotation * tr.pivot);
        tr.rotation *= Quaternion.AngleAxis(rotationSpeed * Time.deltaTime, t - f);
        tr.position -= (tr.rotation * tr.pivot);
    }
}
