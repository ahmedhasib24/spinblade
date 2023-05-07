using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float forceMultiplier = 10f;
    public float maxVelocity = 5f;

    //Components
    private Rigidbody myRigidbody;

    void Awake ()
    {
        myRigidbody = GetComponent<Rigidbody>();
    }
	
	void Update ()
    {
        //Get input from axis
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 currentVelocity = myRigidbody.velocity;
        myRigidbody.AddForce(Vector3.right * h * forceMultiplier * Time.deltaTime, ForceMode.VelocityChange);
        myRigidbody.AddForce(Vector3.forward * v * forceMultiplier * Time.deltaTime, ForceMode.VelocityChange);

        myRigidbody.velocity = Vector3.ClampMagnitude(currentVelocity, maxVelocity);
    }
}
