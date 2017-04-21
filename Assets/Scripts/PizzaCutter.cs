using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaCutter : MonoBehaviour
{
    private Mesh mesh;

    private void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;

        CutPizza(new List<Vector3>());
    }

    public void CutPizza(List<Vector3> points)
    {
        var vertices = mesh.vertices;

        foreach(var vertice in vertices)
        {
            Debug.Log(vertice);
        }
    }
}
