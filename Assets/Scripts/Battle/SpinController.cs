using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class SpinController : MonoBehaviour
{
    //AI
    public bool isAI = false;
    public List<SpinController> targets;
    public int id = 0;

    //rotation
    public float speedMultiplier = 100f;


    //movement
    public float forceMultiplier = 20f;
    public float maxVelocity = 12f;

    //abilities
    public SpinBlade bladeData;

    private float energy;
    public float Energy
    {
        get
        {
            if (energy == 0)
            {
                energy = bladeData.initialEnergy;
            }
            return energy;
        }
        set { energy = value; }
    }

    private float health = -5f;
    public float Health
    {
        get
        {
            if (health == -5f)
            {
                health = bladeData.weightDisk.defense;
            }
            return health;
        }
        set { health = value; }
    }

    public GameObject fireball;
    public ParticleSystem shieldParticle;


    int playerRank = 0;

    //Fireball
    public int fireballCount = 0;
    public int maxFireball = 5;
    public float fireCoolDownTime = 2f;
    float time = 0;

    //Shield
    public float shieldTime = 5f;
    public bool isShieldActive = false;

    //Attack
    public float attackTime = 5f;
    public bool isAttacking = false;
    public bool isAttacked = false;

    public float targetChangeTime = 3f;
    private float currentTargetTime = 0;
    public SpinController target;

    //Components
    private Transform myTransform;
    private Rigidbody myRigidbody;

    void Awake()
    {
        myTransform = GetComponent<Transform>();
        myRigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        DeactivateShield();
        //fireball.SetActive(false);
        //myRigidbody.centerOfMass = new Vector3(0,0.1f, 0);
        target = targets[Random.Range(0, targets.Count)];
    }


    void Update()
    {
        if (GameSceneController.Instance.isGameStarted == false || GameSceneController.Instance.isPaused == true)
        {
            return;
        }

        currentTargetTime += Time.deltaTime;
        if(currentTargetTime >= targetChangeTime)
        {
            if(isAI == true)
            {
                target = targets[Random.Range(0, targets.Count)];
                currentTargetTime = 0;
            }
        }

        myTransform.Rotate(Vector3.up, bladeData.baseRing.stamina * speedMultiplier * Time.deltaTime);
        //myRigidbody.AddTorque(Vector3.up * bladeData.baseRing.stamina * 10000);
        if (GameSceneController.Instance.isGameOver == true)
        {
            return;
        }

        if (Energy < 100)
        {
            Energy += Time.deltaTime * (100f / bladeData.energyRefillTime);
        }


        //Movement
        if (isAI == false)
        {
            //if(isAttacked == true)
            //{
            //    myRigidbody.velocity = Vector3.zero;
            //}
            //else
            //{
            float h = 0;
            float v = 0;
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                h = Input.GetAxis("Horizontal");
                v = Input.GetAxis("Vertical");
            }
            else if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                h = Input.acceleration.x * 2;
                v = Input.acceleration.y * 2;
            }
            Vector3 currentVelocity = myRigidbody.velocity;
            myRigidbody.AddForce(Vector3.right * h * forceMultiplier * Time.deltaTime, ForceMode.VelocityChange);
            myRigidbody.AddForce(Vector3.forward * v * forceMultiplier * Time.deltaTime, ForceMode.VelocityChange);

            myRigidbody.velocity = Vector3.ClampMagnitude(currentVelocity, maxVelocity);
            //}
        }
        else
        {
            Vector3 targetPos = target.myTransform.position;
            Vector3 myPos = myTransform.position;
            float h = 0f;
            float v = 0f;
            if (Random.value > 0.5f)
            {
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
            }
            if (isAttacked == false)
            {
                Vector3 currentVelocity = myRigidbody.velocity;
                myRigidbody.AddForce(Vector3.right * h * forceMultiplier * Time.deltaTime, ForceMode.VelocityChange);
                myRigidbody.AddForce(Vector3.forward * v * forceMultiplier * Time.deltaTime, ForceMode.VelocityChange);

                myRigidbody.velocity = Vector3.ClampMagnitude(currentVelocity, maxVelocity);
            }
            else
            {
                myRigidbody.velocity = Vector3.zero;
            }
        }
        if (isAI == false)
        {
            if (CrossPlatformInputManager.GetButtonDown("Skill"))
            {
                if (Energy >= bladeData.skillCost)
                {
                    ThrowFireball();
                    Energy -= bladeData.skillCost;
                }
            }

            if (CrossPlatformInputManager.GetButtonDown("Attack"))
            {
                if (Energy >= bladeData.attackCost)
                {
                    AttackOpponent();
                    Energy -= bladeData.attackCost;
                }
            }

            if (CrossPlatformInputManager.GetButtonDown("Defense"))
            {
                if (Energy >= bladeData.defenseCost)
                {
                    ActivateShield();
                    Energy -= bladeData.defenseCost;
                }
            }
        }
        else
        {
            time += Time.deltaTime;
            if (Energy >= bladeData.skillCost)
            {
                if (Vector3.Distance(myTransform.position, target.myTransform.position) <= 7f)
                {
                    if (time >= fireCoolDownTime)
                    {
                        time = 0;
                        ThrowFireball();
                        Energy -= bladeData.skillCost;
                    }
                }
            }

            //if (Energy >= bladeData.attackCost)
            //{
            //    if(Random.value > 0.97f && Random.value < 0.99f)
            //    {
            //        AttackOpponent();
            //        Energy -= bladeData.attackCost;
            //    }
            //}

            //if (Energy >= bladeData.defenseCost)
            //{
            //    if(Random.value > 0.99f)
            //    {
            //        ActivateShield();
            //        Energy -= bladeData.defenseCost;
            //    }
            //}
        }
        if(myTransform.position.y < -5f)
        {
            if (GameSceneController.Instance.gameMode == GameMode.Arcade)
            {
                if (isAI == true)
                {
                    gameObject.SetActive(false);
                    GameSceneController.Instance.opponentEliminated += 1;
                    if (GameSceneController.Instance.opponentEliminated == 3)
                    {
                        playerRank = 4 - GameSceneController.Instance.opponentEliminated;
                        GameSceneController.Instance.GameOver(true, playerRank);
                    }
                }
                else
                {
                    playerRank = 4 - GameSceneController.Instance.opponentEliminated;
                    GameSceneController.Instance.GameOver(false, playerRank);
                }
            }
            else if(GameSceneController.Instance.gameMode == GameMode.Match)
            {
                if (isAI == true)
                {
                    playerRank = 1;
                    GameSceneController.Instance.GameOver(true, playerRank);
                }
                else
                {
                    playerRank = 0;
                    GameSceneController.Instance.GameOver(false, playerRank);
                }
            }
            else if (GameSceneController.Instance.gameMode == GameMode.Tournament)
            {
                if (isAI == true)
                {
                    if ((GameController.Instance.eliminatedIds.Count + 1) == 7)
                    {
                        playerRank = 8 - (GameController.Instance.eliminatedIds.Count + 1);
                    }
                    else
                    {
                        playerRank = 0;
                    }
                    GameSceneController.Instance.GameOver(true, playerRank);
                }
                else
                {
                    playerRank = 8 - (GameController.Instance.eliminatedIds.Count + 1);
                    GameSceneController.Instance.GameOver(false, playerRank);
                }
            }
        }
        
    }

    void ThrowFireball()
    {
        fireballCount -= 1;
        Vector3 direction = GetDirection(target, this);
        float angle = Mathf.Atan2(direction.z, direction.x);
        //Debug.Log(direction);
        //Debug.Log(angle);
        fireball = GameSceneController.Instance.GetFireball();
        if (fireball != null)
        {
            fireball.SetActive(true);
            fireball.transform.position = GetPositionAtDistance(myTransform.position, 1.18f, angle);
            fireball.GetComponent<Rigidbody>().velocity = Vector3.zero;
            fireball.GetComponent<Rigidbody>().AddForce(direction * 200f);
            fireball.GetComponent<Fireball>().id = id; 
            //Invoke("DeactivateFireball", 2f);
        }
    }

    void DeactivateFireball()
    {
        fireball.SetActive(false);
    }

    void AttackOpponent()
    {
        if (isAttacking == false)
        {
            isAttacking = true;
            for (int i = 0; i < targets.Count; i++)
            {
                targets[i].isAttacked = true;
            }
            StartCoroutine(DeactivateAttackRoutine());
        }
    }

    void DeactivateAttack()
    {
        isAttacking = false;
        for (int i = 0; i < targets.Count; i++)
        {
            targets[i].isAttacked = false;
        }
    }

    void ActivateShield()
    {
        if (isShieldActive == false)
        {
            isShieldActive = true;
            shieldParticle.Play();
            StartCoroutine(DeactivateShieldRoutine());
        }
    }

    void DeactivateShield()
    {
        isShieldActive = false;
        shieldParticle.Stop();
    }

    //Coroutines
    //IEnumerator GetFireballRoutine()
    //{
    //    while (true)
    //    {
    //        yield return new WaitForSeconds(fireballAvailablityTime);
    //        if (fireballCount < maxFireball)
    //        {
    //            fireballCount += 1;
    //        }
    //    }
    //}

    IEnumerator DeactivateShieldRoutine()
    {
        yield return new WaitForSeconds(shieldTime);
        DeactivateShield();
    }

    IEnumerator DeactivateAttackRoutine()
    {
        yield return new WaitForSeconds(attackTime);
        DeactivateAttack();
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (isShieldActive == true)
        {
            return;
        }
        if (collision.collider.tag == "Stadium")
        {
            //myRigidbody.AddExplosionForce(new Vector3(-myRigidbody.velocity.x * 5, 0, -myRigidbody.velocity.y * 5), collision.contacts[0].point, ForceMode.Impulse);
            myRigidbody.AddExplosionForce(350, collision.contacts[0].point, 1f);
            GameObject cEffect = GameSceneController.Instance.GetCollisionEffect();
            cEffect.transform.position = collision.contacts[0].point;
            cEffect.SetActive(true);
        }
        if (collision.collider.tag == "Spin")
        {
            //myRigidbody.AddExplosionForce(new Vector3(-myRigidbody.velocity.x * 5, 0, -myRigidbody.velocity.y * 5), collision.contacts[0].point, ForceMode.Impulse);
            myRigidbody.AddExplosionForce(700, collision.contacts[0].point, 1f);
            GameObject cEffect = GameSceneController.Instance.GetCollisionEffect();
            cEffect.transform.position = collision.contacts[0].point;
            cEffect.SetActive(true);
            Health -= collision.collider.gameObject.GetComponent<SpinController>().bladeData.attackRing.attack / 20f;
            //Debug.Log(Health);
            if (isAI == false)
            {
                //Camera effect
                GameSceneController.Instance.cameraFollower.collisionHappened = true;
                GameSceneController.Instance.UpdatePlayerHealth(Health);
            }
            else
            {
                //Health -= GameSceneController.Instance.player.attackRing.attack / 20f;
                //Debug.Log("AI = " + Health);
                GameSceneController.Instance.UpdateOpponentHealth(id, Health);
            }
        }
        if (collision.collider.tag == "Fireball")
        {
            myRigidbody.AddExplosionForce(150, collision.contacts[0].point, 1f);
            int fbId = collision.collider.gameObject.GetComponent<Fireball>().id;
            collision.collider.gameObject.SetActive(false);
            //myTransform.Find("FBEffect").GetComponent<ParticleSystem>().Play();
            //myTransform.Find("FBEffect").position = collision.contacts[0].point;
            GameObject fbEffect = GameSceneController.Instance.GetFireballEffect();
            fbEffect.transform.position = collision.contacts[0].point;
            fbEffect.SetActive(true);
            if (isAI == false)
            {
                Health -= GameSceneController.Instance.currentOpponents[fbId].attackRing.attack / 10f;
                GameSceneController.Instance.UpdatePlayerHealth(Health);
            }
            else
            {
                Health -= GameSceneController.Instance.player.attackRing.attack / 10f;
                GameSceneController.Instance.UpdateOpponentHealth(id, Health);
            }
        }
        if (GameSceneController.Instance.gameMode == GameMode.Arcade)
        {
            if (Health <= 0)
            {
                if (isAI == true)
                {
                    gameObject.SetActive(false);
                    GameSceneController.Instance.opponentEliminated += 1;
                    if (GameSceneController.Instance.opponentEliminated == 3)
                    {
                        playerRank = 4 - GameSceneController.Instance.opponentEliminated;
                        Debug.Log(playerRank);
                        GameSceneController.Instance.GameOver(true, playerRank);
                    }
                }
                else
                {
                    playerRank = 4 - GameSceneController.Instance.opponentEliminated;
                    Debug.Log(playerRank);
                    GameSceneController.Instance.GameOver(false, playerRank);
                }
            }
        }
        else if(GameSceneController.Instance.gameMode == GameMode.Match)
        {
            if (Health <= 0)
            {
                if (isAI == true)
                {
                    playerRank = 1;
                    GameSceneController.Instance.GameOver(true, playerRank);
                }
                else
                {
                    playerRank = 0;
                    GameSceneController.Instance.GameOver(false, playerRank);
                }
            }
        }
        else if (GameSceneController.Instance.gameMode == GameMode.Tournament)
        {
            if (Health <= 0)
            {
                if (isAI == true)
                {
                    if((GameController.Instance.eliminatedIds.Count + 1) == 7)
                    {
                        playerRank = 8 - (GameController.Instance.eliminatedIds.Count + 1);
                    }
                    else
                    {
                        playerRank = 0;
                    }
                    GameSceneController.Instance.GameOver(true, playerRank);
                }
                else
                {
                    playerRank = 8 - (GameController.Instance.eliminatedIds.Count + 1);
                    GameSceneController.Instance.GameOver(false, playerRank);
                }
            }
        }
    }

    //Utils
    Vector3 GetDirection(SpinController target, SpinController start)
    {
        Vector3 heading = new Vector3(myRigidbody.velocity.x, transform.position.y, myRigidbody.velocity.z) - Vector3.zero;
        //Debug.Log("X:" + myRigidbody.velocity.x);
        //Debug.Log("Z:" + myRigidbody.velocity.z);
        //Vector3 heading = target.myTransform.position - transform.position;
        float distance = heading.magnitude;
        Vector3 direction = heading / distance;
        return direction;
    }

    Vector3 GetPositionAtDistance(Vector3 center, float radius, float angle)
    {
        Vector3 pos = Vector3.zero;
        pos.x = center.x + radius * Mathf.Cos(angle);
        pos.y = center.y;
        pos.z = center.z + radius * Mathf.Sin(angle);
        return pos;
    }
}
