using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class engineScript : MonoBehaviour
{
    public AudioSource highAccel;

    [SerializeField]
    public static Rigidbody carRigid;
    public static float gearLength = 3;
    public static float currentSpeed { get { return carRigid.velocity.magnitude * gearLength; } }
    public float lowPitch = 1f;
    public float highPitch = 6f;
    public int gearNumber = 5;
    float rpm;
    int currentGear = 1;
    float currentGearPerc;
    public static float maxSpeed = 200;

    void Start()
    {
        carRigid = GetComponentInChildren<Rigidbody>();
        //Debug.Log(carRigid.gameObject.name);
        
    }

    void CalculateEngineSound()
    {
        float gearPercentage = (1 / (float)gearNumber);
        float targetGearFactor = Mathf.InverseLerp(gearPercentage * currentGear, gearPercentage * (currentGear + 1),
                                 Mathf.Abs(currentSpeed / maxSpeed));
        currentGearPerc = Mathf.Lerp(currentGearPerc, targetGearFactor, Time.deltaTime * 5f);

        var gearNumFactor = currentGear / (float)gearNumber;
        rpm = Mathf.Lerp(gearNumFactor, 1, currentGearPerc);

        float speedPercantage = Mathf.Abs(currentSpeed / maxSpeed);
        float upperGearMax = (1/(float)gearNumber) * (currentGear + 1);
        float downGearMax = (1/(float)gearNumber) * currentGear;

        if (currentGear > 0 && speedPercantage < downGearMax)
            currentGear--;
        if (speedPercantage > upperGearMax && (currentGear < (gearNumber - 1)))
            currentGear++;

        float pitch = Mathf.Lerp(lowPitch, highPitch, rpm);
        highAccel.pitch = Mathf.Min(highPitch, pitch) * 0.25f;

    }

    void Update()
    {
        CalculateEngineSound();
        Debug.Log(carRigid.gameObject.name);
    }
}
