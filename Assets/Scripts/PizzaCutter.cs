using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaCutter : MonoBehaviour
{
    public GameObject pizzaPart;

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    private int vertexCount;

    private int indexOfCenter = 2; // hardcoded index by Blender :|

    private void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();

        vertexCount = meshFilter.mesh.vertexCount;

        CutPizza(new List<Vector3>() { new Vector3(1.0f, -0.2f), new Vector3(0.45f, 0.2f)});
    }

    public void CutPizza(List<Vector3> input)
    {
        var vertices = meshFilter.mesh.vertices;
        var triangles = meshFilter.mesh.triangles;

        var nearestPoints = GetNearPoints(input, vertices);

        if (nearestPoints[0] == nearestPoints[1]) // to prevent tapping on screen
            return;

        var delta = Mathf.Abs(nearestPoints[0] - nearestPoints[1]);
        var startIndex = delta > (vertexCount - delta) ? nearestPoints[0] : nearestPoints[1];
        var endIndex = startIndex == nearestPoints[0] ? nearestPoints[1] : nearestPoints[0];

        var newTriangles = new List<int>();
        var cuttedTriangles = new List<int>();
        for(int k = 0, n = startIndex; k < triangles.Length; k += 3, n++)
        {
            n -= n >= vertexCount ? vertexCount : 0;
            if(triangles[k] != n && triangles[k + 1] != n && triangles[k + 2] != n)
            {
                newTriangles.Add(triangles[k]);
                newTriangles.Add(triangles[k + 1]);
                newTriangles.Add(triangles[k + 2]);

                Debug.Log("Not in!");
            }
            else
            {
                cuttedTriangles.Add(triangles[k]);
                cuttedTriangles.Add(triangles[k + 1]);
                cuttedTriangles.Add(triangles[k + 2]);

                Debug.Log("In!");
            }
        }

        meshFilter.mesh.triangles = newTriangles.ToArray();

        var cuttedPart = Instantiate(pizzaPart);
        cuttedPart.transform.position = transform.position;
        var cuttedFilter = cuttedPart.GetComponent<MeshFilter>();
        var cuttedRenderer = cuttedPart.GetComponent<MeshRenderer>();
        
        cuttedFilter.mesh.triangles = cuttedTriangles.ToArray();
    }

    private int[] GetNearPoints(List<Vector3> input, Vector3[] vertices)
    {
        var points = new int[input.Count];

        for(int i = 0; i < input.Count; i++)
        {
            float minDistance = (input[i] - vertices[0]).magnitude;
            int index = 0;

            for(int j = 1; j < vertices.Length; j++)
            {
                var distance = (input[i] - vertices[j]).magnitude;
                if (distance < minDistance && j != indexOfCenter)
                {
                    minDistance = distance;
                    index = j;
                }
            }

            points[i] = index;
        }

        return points;
    }
}
