using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform CameraTransform;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveCamera();
    }


    void MoveCamera()
    {
        if (Input.GetKey(KeyCode.A))
        {
            CameraTransform.position += new Vector3(-15f*Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            CameraTransform.position += new Vector3(15f*Time.deltaTime, 0, 0);
        }
    }
}
