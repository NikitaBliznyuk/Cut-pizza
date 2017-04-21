using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaCutter : MonoBehaviour
{
    public GameObject pizzaPart;

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    private void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();

        CutPizza(new List<Vector3>());
    }

    public void CutPizza(List<Vector3> input)
    {
        var vertices = meshFilter.mesh.vertices;
        var triangles = meshFilter.mesh.triangles;

        var newTriangles = new int[triangles.Length - 30];
        var cuttedTriangles = new int[30];

        var i = 0;
        for(; i < newTriangles.Length; i++)
        {
            newTriangles[i] = triangles[i];
        }
        for(var j = 0; i < triangles.Length && j < cuttedTriangles.Length; i++, j++)
        {
            cuttedTriangles[j] = triangles[i];
        }

        meshFilter.mesh.triangles = newTriangles;

        var cuttedPart = Instantiate(pizzaPart);
        cuttedPart.transform.position = transform.position;
        var cuttedFilter = cuttedPart.GetComponent<MeshFilter>();
        var cuttedRenderer = cuttedPart.GetComponent<MeshRenderer>();
        
        cuttedFilter.mesh.triangles = cuttedTriangles;
    }

    private List<Vector3> GetNearPoints(List<Vector3> input)
    {
        var points = new List<Vector3>();

        foreach(var inputPoint in points)
        {

        }

        return points;
    }
}
