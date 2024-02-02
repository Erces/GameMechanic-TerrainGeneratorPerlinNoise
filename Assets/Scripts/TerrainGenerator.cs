using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    Mesh mesh;

    Vector3[] vertices;
    Color[] colors;
    int[] tris;

    [Header("Specs")]
    float minHeight;
    float maxHeight;
    public Gradient gradient;
    public int xHeight;
    public int zHeight;
    public float xAmp;
    public float zAmp;
    public float perlinAmp;

    void Start()
    {
        //Null Mesh
        mesh = new Mesh();

        GetComponent<MeshFilter>().mesh = mesh;
        CreateShape();
        InvokeRepeating("UpdateMesh",.02f,.02f);
    }

    // Update is called once per frame
    public void CreateShape(){
        vertices = new Vector3[(xHeight + 1) * (zHeight + 1)];
        for (int i = 0, z = 0; z <= zHeight; z++)
        {
            for (int x = 0; x <= xHeight; x++)
            {
                float y = Mathf.PerlinNoise(x * xAmp,z * zAmp) * perlinAmp;
                vertices[i] = new Vector3(x,y,z);
                i++;
                if(y > maxHeight)
                maxHeight = y;
                if(y < minHeight)
                minHeight = y;
            }
        }
        tris = new int[xHeight * zHeight * 6];
        int vert = 0;
        int triangles = 0;

        for (int z = 0; z < zHeight; z++)
        {
            for (int x = 0; x < xHeight; x++)
        {
        
        tris[triangles + 0] = vert+ 0;
        tris[triangles +1] = vert+xHeight + 1;
        tris[triangles +2] = vert+1;
        tris[triangles +3] = vert+1;
        tris[triangles +4] = vert+xHeight +1;
        tris[triangles +5] = vert+xHeight + 2;     

        vert++;
        triangles+= 6;

        //yield return new WaitForSeconds(.02f);
        }
        vert++;
        }
        
        colors = new Color[vertices.Length];
        for (int i = 0, z = 0; z <= zHeight; z++)
        {
            for (int x = 0; x <= xHeight; x++)
            {
                float height = Mathf.InverseLerp(minHeight,maxHeight,vertices[i].y);
                colors[i] = gradient.Evaluate(height);

                i++;
            }
        }
        


    }
    void UpdateMesh(){
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = tris;
        mesh.colors = colors;
        mesh.RecalculateNormals();
    }
}
