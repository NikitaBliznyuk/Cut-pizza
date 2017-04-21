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

        CutPizza(new List<Vector3>() { new Vector3(0.1f, 0.5f), new Vector3(0.45f, 0.2f) });
    }

    public void CutPizza(List<Vector3> input)
    {
        var vertices = meshFilter.mesh.vertices;
        var triangles = meshFilter.mesh.triangles;

        var nearestPoints = GetNearPoints(input, vertices);

        if (nearestPoints[0] == nearestPoints[1]) // to prevent tapping on screen
        {
            Debug.Log("Try again!");
            return;
        }

        if (nearestPoints[0] > nearestPoints[1])
        {
            var temp = nearestPoints[0];
            nearestPoints[0] = nearestPoints[1];
            nearestPoints[1] = temp;
        }

        var delta = Mathf.Abs(nearestPoints[1] - nearestPoints[0]);
        var startIndex = delta > (vertexCount - delta) ? nearestPoints[1] : nearestPoints[0];
        var endIndex = startIndex == nearestPoints[0] ? nearestPoints[1] : nearestPoints[0];
        var startTriangleIndex = 0;
        var endTriangleIndex = 0;

        for (int i = 0; i < triangles.Length; i += 3)
        {
            if (triangles[i] == startIndex)
                startTriangleIndex = i;
            else if (triangles[i + 1] == endIndex)
                endTriangleIndex = i;
        }

        Debug.Log("Start indexes: " + startIndex + " " + endIndex);
        Debug.Log("Start triangle indexes: " + startTriangleIndex + " " + endTriangleIndex
            + "  VALUES: " + triangles[startTriangleIndex] + " " + triangles[endTriangleIndex + 1]);

        var newTriangles = new List<int>();
        var cuttedTriangles = new List<int>();

        for (int i = 0; i < triangles.Length; i += 3)
        {
            if(startTriangleIndex < endTriangleIndex)
            {
                if(i >= startTriangleIndex && (i + 1) <= endTriangleIndex)
                {
                    cuttedTriangles.Add(triangles[i]);
                    cuttedTriangles.Add(triangles[i + 1]);
                    cuttedTriangles.Add(triangles[i + 2]);
                }
                else
                {
                    newTriangles.Add(triangles[i]);
                    newTriangles.Add(triangles[i + 1]);
                    newTriangles.Add(triangles[i + 2]);
                }
            }
            else
            {
                if (i >= endTriangleIndex && (i + 1) <= startTriangleIndex)
                {
                    newTriangles.Add(triangles[i]);
                    newTriangles.Add(triangles[i + 1]);
                    newTriangles.Add(triangles[i + 2]);
                }
                else
                {
                    cuttedTriangles.Add(triangles[i]);
                    cuttedTriangles.Add(triangles[i + 1]);
                    cuttedTriangles.Add(triangles[i + 2]);
                }
            }
        }

        if (cuttedTriangles.Count == 0)
        {
            Debug.Log("Try again!");
            return;
        }

        Debug.Log(newTriangles.Count);

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

        for (int i = 0; i < input.Count; i++)
        {
            float minDistance = (input[i] - vertices[0]).magnitude;
            int index = 0;

            for (int j = 1; j < vertices.Length; j++)
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
