using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public float forceMultiplier = 20f;
    public float maxVelocity = 5f;

    public Transform target;
    public bool isAttacked = false;

    private float attackTime = 0;
    
    //Components
    public Transform myTransform;
    private Rigidbody myRigidbody;

    void Awake ()
    {
        myTransform = GetComponent<Transform>();
        myRigidbody = GetComponent<Rigidbody>();
    }
	
	void Update ()
    {
        Vector3 targetPos = target.position;
        Vector3 myPos = myTransform.position;
        float h = 0f;
        float v = 0f;
        if (targetPos.x > myPos.x)
        {
            h = 1f;
        }
        else if (targetPos.x < myPos.x)
        {
            h = -1f;
        }
        if (targetPos.z > myPos.z)
        {
            v = 1f;
        }
        else if (targetPos.z < myPos.z)
        {
            v = -1f;
        }
        if(isAttacked == false)
        {
            Vector3 currentVelocity = myRigidbody.velocity;
            myRigidbody.AddForce(Vector3.right * h * forceMultiplier * Time.deltaTime, ForceMode.VelocityChange);
            myRigidbody.AddForce(Vector3.forward * v * forceMultiplier * Time.deltaTime, ForceMode.VelocityChange);

            myRigidbody.velocity = Vector3.ClampMagnitude(currentVelocity, maxVelocity);
        }
        else
        {
            myRigidbody.velocity = Vector3.zero;
            attackTime += Time.deltaTime;
            if(attackTime >= 5f)
            {
                isAttacked = false;
            }
        }
    }
}
