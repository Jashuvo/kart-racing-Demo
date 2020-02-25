using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriveScript : MonoBehaviour
{
    public WheelCollider[] wheelCollider;
    public GameObject[] wheels;
    public float torque = 200;
    public float maxSteerAngle = 30;
    
    
    void Start()
    {
        //wheelCollider = this.GetComponent<WheelCollider>();
    }

    void Go(float accelarator,float steerAngle)
    {
        accelarator = Mathf.Clamp(accelarator, -1, 1);
        steerAngle = Mathf.Clamp(steerAngle, -1, 1) * maxSteerAngle;
        float thurstTorque = accelarator * torque;
        for(int i = 0; i < wheelCollider.Length; i++)
        {
            wheelCollider[i].motorTorque = thurstTorque;
            if(i < 2)
            {
                wheelCollider[i].steerAngle = steerAngle;
            }
            
            Quaternion rotation;
            Vector3 position;
            wheelCollider[i].GetWorldPose(out position, out rotation);
            wheels[i].transform.position = position;
            wheels[i].transform.rotation = rotation;
        }
       
    }

    
    void Update()
    {
        float accelarator = Input.GetAxis("Vertical");
        float steerAngle = Input.GetAxis("Horizontal");
        Go(accelarator,steerAngle);
    }
}
