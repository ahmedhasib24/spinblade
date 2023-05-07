using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abilities : MonoBehaviour
{
    public float shieldTime;
    public float attackTime;
    public bool isSkillAvailable = false;
    public bool isAttackActive = false;
    public bool isShieldActive = false;

    public GameObject fireball;
    public ParticleSystem shieldParticle;
    public AIController target;

    private void Start()
    {
        shieldParticle.Stop();
    }

    private void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            ActivateShield();
        }
        
    }

    void ThrowFireball()
    {
        var heading = target.myTransform.position - transform.position;
        var distance = heading.magnitude;
        var direction = heading / distance;
        //Debug.DrawLine(transform.position, transform.position + direction * 10, Color.red, Mathf.Infinity);
        fireball.transform.position = transform.position + new Vector3(0f, 2f, 0f);
        fireball.GetComponent<Rigidbody>().velocity = Vector3.zero;
        fireball.GetComponent<Rigidbody>().AddForce(direction * 1000f);
    }

    void ActivateShield()
    {
        if(isShieldActive == false)
        {
            isShieldActive = true;
            shieldParticle.Play();
        }
    }

    void AttackOpponent()
    {
        if (isAttackActive == false)
        {
            isAttackActive = true;
            target.isAttacked = true;
        }
    }
}
