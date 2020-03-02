using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriveScript : MonoBehaviour
{
    public WheelCollider[] wheelCollider;

    public GameObject[] wheels;
    public GameObject brakeLight;

    public Transform skidTrailPrefab;
    Transform[] skidTrails = new Transform[4];

    public ParticleSystem smokePrefab;
    ParticleSystem[] skidSmoke = new ParticleSystem[4];

    public AudioSource skidSound;

    public float torque = 200;
    public float maxSteerAngle = 30;
    public float maxBrakeForce = 500;
    
    void Start()
    {
        brakeLight.SetActive(false);
        //wheelCollider = this.GetComponent<WheelCollider>();
        for(int i = 0; i < 4; i++)
        {
            skidSmoke[i] = Instantiate(smokePrefab);
            skidSmoke[i].Stop();
        }
    }
    public void StartSkidTrail(int i)
    {
        if (skidTrails[i] == null)
        {
            skidTrails[i] = Instantiate(skidTrailPrefab);
        }
        skidTrails[i].parent = wheelCollider[i].transform;
        skidTrails[i].localRotation = Quaternion.Euler(90, 0, 0);
        skidTrails[i].localPosition = -Vector3.up * wheelCollider[i].radius;
    }
    public void EndSkidTrail(int i)
    {
        if(skidTrails[i] == null)
        {
            return;
        }
        Transform holder = skidTrails[i];
        skidTrails[i] = null;
        holder.parent = null;
        holder.rotation = Quaternion.Euler(90, 0, 0);
        Destroy(holder.gameObject, 30);
    }

    void Go(float accelarator,float steerAngle,float brakeForce)
    {
        accelarator = Mathf.Clamp(accelarator, -1, 1);
        steerAngle = Mathf.Clamp(steerAngle, -1, 1) * maxSteerAngle;
        brakeForce = Mathf.Clamp(brakeForce, 0, 1) * maxBrakeForce;

        if(brakeForce!=0)
            brakeLight.SetActive(true);
        else
            brakeLight.SetActive(false);

        float thurstTorque = 0;
        if(engineScript.currentSpeed < engineScript.maxSpeed)
            thurstTorque = accelarator * torque;

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

    void CheckForSkid()
    {
        int skidCount = 0;
        for(int i = 0; i < wheelCollider.Length; i++)
        {
            WheelHit wheelHit;
            wheelCollider[i].GetGroundHit(out wheelHit);
            if(Mathf.Abs(wheelHit.forwardSlip) >= 0.4f|| Mathf.Abs(wheelHit.sidewaysSlip) >= 0.4f)
            {
                skidCount++;
                if (!skidSound.isPlaying)
                {
                    skidSound.Play();
                }
                StartSkidTrail(i);
                skidSmoke[i].transform.position = wheelCollider[i].transform.position - wheelCollider[i].transform.up * wheelCollider[i].radius;
                skidSmoke[i].Emit(1); 
            }
            else
            {
                EndSkidTrail(i);
            }
        }

        if(skidCount == 0 && skidSound.isPlaying)
        {
            skidSound.Stop();
        }
    }

    void Update()
    {
        float accelarator = Input.GetAxis("Vertical");
        float steerAngle = Input.GetAxis("Horizontal");
        float brake = Input.GetAxis("Jump");
        Go(accelarator,steerAngle,brake);
        CheckForSkid();
    }
}
