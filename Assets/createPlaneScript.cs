using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class createPlaneScript : MonoBehaviour
{
    public float vertWidthAway;

    public int width;

    public int height;
    public MeshFilter mF;

    private void Awake()
    {
        //mF = GetComponent<MeshFilter>();
    }

    public void GenerateMesh()
    {
        Vector3[] vertices = new Vector3[width * height];
        int[] triangles = new int[(width - 1) * (height - 1) * 6];
        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                vertices[y * width + x] = new Vector3(x * vertWidthAway, y * vertWidthAway,0);
            }
        }

        int triIndex = 0;
        for (var x = 0; x < width - 1; x++)
        {
            for (var y = 0; y < height - 1; y++)
            {
                int i = y * width + x;
                triangles[triIndex] = i + width;
                triangles[triIndex + 1] = i + 1;
                triangles[triIndex + 2] = i;
                triangles[triIndex + 3] = i + width;
                triangles[triIndex + 4] = i + width + 1;
                triangles[triIndex + 5] = i + 1;
                triIndex += 6;
            }
        }
        Mesh mesh = new Mesh();
        mesh.SetVertices(vertices.ToList());
        mesh.SetTriangles(triangles, 0);

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
       
        mF.mesh = mesh;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
