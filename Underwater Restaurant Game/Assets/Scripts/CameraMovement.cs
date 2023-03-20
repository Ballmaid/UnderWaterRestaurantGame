using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraMovement : MonoBehaviour
{
    public Transform CameraTransform;
    public Slider CameraSlider;
    public Networking network;

    // Start is called before the first frame update
    void Start()
    {
        CameraSlider.maxValue = 40;
        InvokeRepeating("NetworkMovement", 0, 0.5f);
    }

    void NetworkMovement() {
        network.sendMessage(0, network.playerID + "," + ((int)CameraTransform.position.x).ToString() );
        network.flushBuffer();
    }

    // Update is called once per frame
    void Update()
    {
        MoveCamera();
    }

    public void SliderMove() {
        CameraTransform.position = new Vector3(CameraSlider.value, CameraTransform.position.y, CameraTransform.position.z);
    }
    
    void MoveCamera()
    {
        if (Input.GetKey(KeyCode.A))
        {
            if (CameraTransform.position.x > 0) {
                CameraTransform.position += new Vector3(-15f*Time.deltaTime, 0, 0);
                CameraSlider.value = CameraTransform.position.x;
            }
            else
            {
                CameraTransform.position = new Vector3(0, CameraTransform.position.y, CameraTransform.position.z);
            }
            
        }
        if (Input.GetKey(KeyCode.D))
        {
            if (CameraTransform.position.x < CameraSlider.maxValue) {
            CameraTransform.position += new Vector3(15f*Time.deltaTime, 0, 0);
            CameraSlider.value = CameraTransform.position.x;
            }
            else
            {
                CameraTransform.position = new Vector3(CameraSlider.maxValue, CameraTransform.position.y, CameraTransform.position.z);
            }
        }
    }
}
