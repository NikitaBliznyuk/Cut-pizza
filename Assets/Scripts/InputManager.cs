using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager: MonoBehaviour
{
    private bool haveInput = false;
    private List<Vector3> input = new List<Vector3>();

    public bool HaveInput { get { return haveInput; } }
    public List<Vector3> MouseInput
    {
        get
        {
            haveInput = false;
            var returnValue = new List<Vector3>(input);
            input = new List<Vector3>();
            return returnValue;
        }
    }

    private void Update()
    {
        if (!haveInput)
            GetInput();
    }

    private void GetInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var mouseInput = Input.mousePosition;
            mouseInput.z = 0;

            input.Add(Camera.main.ScreenToWorldPoint(mouseInput));
        }
        if(Input.GetMouseButtonUp(0))
        {
            var mouseInput = Input.mousePosition;
            mouseInput.z = 0;

            input.Add(Camera.main.ScreenToWorldPoint(mouseInput));
            haveInput = true;
        }
    }
}
