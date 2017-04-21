using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaCreator : MonoBehaviour
{
    public Material texture;

    private List<Vector2> points;

    private void Start()
    {
        points = new List<Vector2>();

        InitializePizza();
        DrawPizza();
    }

    private void InitializePizza()
    {
        for(var i = 0.0f; Mathf.Abs(i - 2 * Mathf.PI) > 0.1f; i += Mathf.PI / 6.0f)
        {
            var y = Mathf.Cos(i);
            var x = Mathf.Sqrt(1 - y * y);
            x *= i > Mathf.PI ? -1.0f : 1.0f;
            points.Add(new Vector2(x, y));
            Debug.Log("(" + x + ", " + y + ")");
        }
    }

    private void DrawPizza()
    {
        var triangulator = new Triangulator(points.ToArray());
        var indices = triangulator.Triangulate();

        var vertices2D = points.ToArray();
        // Create the Vector3 vertices
        Vector3[] vertices = new Vector3[vertices2D.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = new Vector3(vertices2D[i].x, vertices2D[i].y, 0);
        }

        // Create the mesh
        Mesh msh = new Mesh();
        msh.vertices = vertices;
        msh.triangles = indices;
        msh.RecalculateNormals();
        msh.RecalculateBounds();

        // Set up game object with mesh;
        var meshRenderer = gameObject.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
        var filter = gameObject.AddComponent(typeof(MeshFilter)) as MeshFilter;
        filter.mesh = msh;
        meshRenderer.material = texture;
    }
}
