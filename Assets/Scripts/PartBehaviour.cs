using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartBehaviour : MonoBehaviour
{
    private float maxDistance = 5.0f;
    private float currentDistance = 0.0f;
    private float speed = 2.0f;

    public Vector3 Direction { get; set; }

    private void Update()
    {
        if(currentDistance <= maxDistance)
        {
            currentDistance += speed * Time.deltaTime;
            transform.Translate(Direction * speed * Time.deltaTime);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
