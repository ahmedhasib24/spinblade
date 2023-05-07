using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public Transform target;
    public Vector3 distance;

    public float minDistanceZ;
    public float maxDistanceZ;
    public bool collisionHappened = false;

    //Components
    private Transform myTransform;

    public float goAwayTime = 1.5f;
    float currentTime = 0;

    private void Awake()
    {
        myTransform = GetComponent<Transform>();
    }

    private void LateUpdate()
    {
        //if (GameSceneController.Instance.isGameStarted == false || GameSceneController.Instance.isPaused == true)
        //{
        //    return;
        //}
        //distance.z = Vector3.Lerp();
        if(target != null)
        {
            if(collisionHappened == true)
            {
                float y = 15f;
                distance.z = 5.5f;
                myTransform.position = Vector3.Lerp(myTransform.position, new Vector3(target.position.x, target.position.y + y, target.position.z - distance.z), 0.1f);
                //StartCoroutine("DeactivateCollision");
                currentTime += Time.deltaTime;
                if(currentTime >= goAwayTime)
                {
                    currentTime = 0;
                    collisionHappened = false;
                }
            }
            else
            {
                float y = 27.5f;
                distance.z = 12.3f;
                myTransform.position = Vector3.Lerp(myTransform.position, new Vector3(target.position.x, target.position.y + y, target.position.z - distance.z), 0.05f);
            }
        }
    }

    IEnumerator DeactivateCollision()
    {
        yield return new WaitForSeconds(1.5f);
        collisionHappened = false;
    }
}
