using UnityEngine;
using System.Collections;

public class SimpleAgent : MonoBehaviour
{
   
    public float distPlayer;
    public float distDetection;

    public  GameObject target;
    public bool playerDetected;
    private NavMeshAgent agent;

    private float timeBetweenShots;
    private float timeBetweenShotsAct;
    

    // Use this for initialization
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (GameObject.FindWithTag("Player") != null)
        {
            target = GameObject.FindWithTag("Player");
        }
        playerDetected = false;

        timeBetweenShots = 2;
        timeBetweenShotsAct = 0;
    }

    // Update is called once per frame
    void Update()
    {
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
                agent.Resume();
                agent.SetDestination(target.transform.position);
            }
            else //A portée
            {
                agent.Stop();
                agent.transform.LookAt(target.transform);

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
}
