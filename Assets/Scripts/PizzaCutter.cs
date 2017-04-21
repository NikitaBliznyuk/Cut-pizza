using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaCutter : MonoBehaviour
{
    private Mesh mesh;
    private MeshRenderer meshRenderer;

    private void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        meshRenderer = GetComponent<MeshRenderer>();

        CutPizza(new List<Vector3>());
    }

    public void CutPizza(List<Vector3> points)
    {
        var vertices = mesh.vertices;
        var triangles = mesh.triangles;

        var newTriangles = new int[triangles.Length - 9];

        for(int i = 0; i < newTriangles.Length; i++)
        {
            newTriangles[i] = triangles[i];
        }

        mesh.triangles = newTriangles;
    }
}
