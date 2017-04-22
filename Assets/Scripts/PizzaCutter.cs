using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaCutter : MonoBehaviour
{
    public GameObject pizzaPart;

    private MeshFilter meshFilter;

    private InputManager manager;

    private int vertexCount;
    private int trianglesCount;

    private int indexOfCenter = 2; // hardcoded index by Blender :|

    private float percents = 100.0f;

    public float Percents { get { return percents; } }

    private void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        manager = GetComponent<InputManager>();

        vertexCount = meshFilter.mesh.vertexCount;
        trianglesCount = meshFilter.mesh.triangles.Length;
    }

    private void Update()
    {
        if(manager.HaveInput) // if player entered some input, then we need to cut pizza
        {
            CutPizza(manager.MouseInput);
        }

        CheckPizzaState(); // check, if our pizza done
    }

    /** Ckecks, if amount of percent is equal to zero.
     */
    private void CheckPizzaState()
    {
        if ((int)percents == 0)
        {
            Destroy(gameObject);
        }
    }
    
    /** Cut pizza with inputs.
     */
    public void CutPizza(List<Vector3> input)
    {
        var vertices = meshFilter.mesh.vertices;
        var triangles = meshFilter.mesh.triangles;

        var nearestPoints = GetNearPoints(input, vertices);

        if (nearestPoints[0] == nearestPoints[1]) // to prevent tapping on screen
        {
            return;
        }

        if (nearestPoints[0] > nearestPoints[1]) // sort inputs (smaller to right)
        {
            var temp = nearestPoints[0];
            nearestPoints[0] = nearestPoints[1];
            nearestPoints[1] = temp;
        }

        var delta = Mathf.Abs(nearestPoints[1] - nearestPoints[0]);
        var startIndex = delta > (vertexCount - delta) ? nearestPoints[1] : nearestPoints[0]; // choosing direction of moving
        var endIndex = startIndex == nearestPoints[0] ? nearestPoints[1] : nearestPoints[0];
        var startTriangleIndex = 0; // triangle index, that starts cutted part
        var endTriangleIndex = 0; // triangle index, that ends cutted part

        for (int i = 0; i < triangles.Length; i += 3) // finding those indeces
        {
            if (triangles[i] == startIndex)
                startTriangleIndex = i;
            else if (triangles[i + 1] == endIndex)
                endTriangleIndex = i;
        }

        List<int> newTriangles, cuttedTriangles;
        SplitMesh(triangles, startTriangleIndex, endTriangleIndex, out newTriangles, out cuttedTriangles);

        percents = newTriangles.Count / (float)trianglesCount * 100.0f; // updating count of percents left

        meshFilter.mesh.triangles = newTriangles.ToArray(); // updating current pizza mesh

        CreateCuttedPart(cuttedTriangles.ToArray(), ((input[0] + input[1]) / 2).normalized);
    }

    /** Split triangles by 2 started from startIndex and ended at endIndex. 
    */
    private void SplitMesh(int[] triangles, int startIndex, int endIndex, out List<int> firstPart, out List<int> secondPart)
    {
        firstPart = new List<int>();
        secondPart = new List<int>();

        for (int i = 0; i < triangles.Length; i += 3)
        {
            if (startIndex < endIndex)
            {
                if (i >= startIndex && (i + 1) <= endIndex)
                {
                    secondPart.Add(triangles[i]);
                    secondPart.Add(triangles[i + 1]);
                    secondPart.Add(triangles[i + 2]);
                }
                else
                {
                    firstPart.Add(triangles[i]);
                    firstPart.Add(triangles[i + 1]);
                    firstPart.Add(triangles[i + 2]);
                }
            }
            else
            {
                if (i >= endIndex && (i + 1) <= startIndex)
                {
                    firstPart.Add(triangles[i]);
                    firstPart.Add(triangles[i + 1]);
                    firstPart.Add(triangles[i + 2]);
                }
                else
                {
                    secondPart.Add(triangles[i]);
                    secondPart.Add(triangles[i + 1]);
                    secondPart.Add(triangles[i + 2]);
                }
            }
        }
    }

    /** Creates cutted part with moving direction.
     */
    private void CreateCuttedPart(int[] triangles, Vector3 direction)
    {
        var cuttedPart = Instantiate(pizzaPart); // instantiating cutted part
        var cuttedFilter = cuttedPart.GetComponent<MeshFilter>();
        cuttedPart.GetComponent<PartBehaviour>().Direction = direction;

        cuttedFilter.mesh.triangles = triangles;
    }

    /** Calculating nearest points of mesh to inputs. 
     */
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
