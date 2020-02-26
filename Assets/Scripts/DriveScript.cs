using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriveScript : MonoBehaviour
{
    public WheelCollider[] wheelCollider;
    public GameObject[] wheels;
    public float torque = 200;
    public float maxSteerAngle = 30;
    public float maxBrakeForce = 500;
    
    
    void Start()
    {
        //wheelCollider = this.GetComponent<WheelCollider>();
    }

    void Go(float accelarator,float steerAngle,float brakeForce)
    {
        accelarator = Mathf.Clamp(accelarator, -1, 1);
        steerAngle = Mathf.Clamp(steerAngle, -1, 1) * maxSteerAngle;
        brakeForce = Mathf.Clamp(brakeForce, 0, 1) * maxBrakeForce;
        float thurstTorque = accelarator * torque;
        for(int i = 0; i < wheelCollider.Length; i++)
        {
            wheelCollider[i].motorTorque = thurstTorque;
            if(i < 2)
            {
                wheelCollider[i].steerAngle = steerAngle;
            }
            else
            {
                wheelCollider[i].brakeTorque = brakeForce;
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
        float brake = Input.GetAxis("Jump");
        Go(accelarator,steerAngle,brake);
    }
}
