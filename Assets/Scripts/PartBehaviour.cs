using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartBehaviour : MonoBehaviour
{
    private float maxDistance = 1.0f;
    private float currentDistance = 0.0f;
    private float speed = 0.5f;

    private void Update()
    {
        if(currentDistance <= maxDistance)
        {
            currentDistance += speed * Time.deltaTime;
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
