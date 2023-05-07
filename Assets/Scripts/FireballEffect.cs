using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballEffect : MonoBehaviour
{
    public float disableTime = 0.5f;
	// Use this for initialization
	void OnEnable ()
    {
        Invoke("Deactivate", disableTime);
	}
	
	// Update is called once per frame
	void Deactivate ()
    {
        gameObject.SetActive(false);
	}
}
