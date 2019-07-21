using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeBar : MonoBehaviour
{
    Renderer renderer;
    private void Start()
    {
        renderer = GetComponent<Renderer>();
    }
    void Update()
    {
        renderer.material.SetFloat("_Cutoff", Mathf.InverseLerp(0, Screen.width, Input.mousePosition.x));
    }
}
