using UnityEngine;
using System.Collections;

public class DroneAgent : MonoBehaviour
{
    public float distPlayer;
    public float distDetection;

    public  GameObject target;
    public bool playerDetected;
    private bool isPlayerDead = false;
    private UnityEngine.AI.NavMeshAgent agent;

    private float timeBetweenShots;
    private float timeBetweenShotsAct;

    private Animator animator;

    private CapsuleCollider capsulecollider;
    private float colliderMin;
    private float colliderMax;

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();

        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (GameObject.FindWithTag("Player") != null)
        {
            target = GameObject.FindWithTag("Player");
        }
        playerDetected = false;

        timeBetweenShots = 2;
        timeBetweenShotsAct = 0;

        capsulecollider = GetComponent<CapsuleCollider>();
        colliderMin = 0.5f;
        colliderMax = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPlayerDead)
        {
            if (animator.GetBool("CanMove"))
                capsulecollider.height = colliderMin;
            else
                capsulecollider.height = colliderMax;


            Vector3 agentPos = new Vector3(this.transform.position.x, 0, this.transform.position.z);
            Vector3 playerPos = new Vector3(target.transform.position.x, 0, target.transform.position.z);

            if (!playerDetected)
            {
                //Si le joueur est à moins de distDetection et que y a une vue dégagé avec le joueur
                if (Vector3.Distance(agentPos, playerPos) < distDetection)
                {
                    RaycastHit hit;
                    Ray ray = new Ray(this.transform.position, (target.transform.position - this.transform.position));
                    if (Physics.Raycast(ray, out hit, Vector3.Distance(this.transform.position, target.transform.position)))
                    {
                        if (hit.transform.tag == "Player")
                            playerDetected = true;
                    }
                }
            }
            else //playerDetected
            {
                //destination
                if (Vector3.Distance(agentPos, playerPos) > distPlayer) //trop loin
                {
                    animator.SetInteger("WeaponsState", 1);
                    if (animator.GetBool("CanMove"))
                    {
                        agent.Resume();
                        agent.SetDestination(target.transform.position);
                    }
                }
                else //A portée
                {
                    agent.Stop();
                    agent.transform.LookAt(target.transform);
                    animator.SetInteger("WeaponsState", 2);

                    RaycastHit hit;
                    Ray ray = new Ray(this.transform.position, (target.transform.position - this.transform.position));
                    if (Physics.Raycast(ray, out hit, Vector3.Distance(this.transform.position, target.transform.position)))
                    {
                        if (hit.transform.tag == "Player")
                        {
                            tryShot(hit);
                        }
                    }
                }
            }
        }
    }

    private void tryShot(RaycastHit hit)
    {
        if(timeBetweenShotsAct > timeBetweenShots)
        {
            GetComponent<Enemy>().shoot(hit);
            timeBetweenShotsAct = 0;
        }
        else
        {
            timeBetweenShotsAct += Time.deltaTime;
        }
    }

    public void playerIsDead()
    {
        isPlayerDead = true;
    }
}
