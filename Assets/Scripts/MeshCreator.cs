using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshCreator : MonoBehaviour
{
    [SerializeField] private Texture m_texture = null;
    void Start()
    {
        createPolygon();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void createPolygon()
    {
        Vector3[] vertices = new Vector3[] {
            new Vector3(-1f, 1f, 0f),   new Vector3(1f, 1f, 0f),
            new Vector3(1f, -1f, 0f),   new Vector3(-1f, -1f, 0f)
        };

        int[] triangles = new int[] {0, 1, 2, 0, 2, 3};
        Mesh mesh= new Mesh();
        Vector2[] uvs = new Vector2[] {
            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f)
        };

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        GetComponent<MeshFilter>().mesh = mesh;

        Material material = new Material(Shader.Find("Standard"));
        material.SetTexture("_MainTex", m_texture);
        GetComponent<MeshRenderer>().material = material;
    }
}
