using UnityEngine;
using System.Collections;

public class SimpleAgent : MonoBehaviour
{
   
    public float distPlayer;
    public float distDetection;

    public  GameObject target;
    private Vector3 posDepart;
    private bool playerDetected;
    private NavMeshAgent agent;
    

    // Use this for initialization
    void Start()
    {
        posDepart = transform.position;
        agent = GetComponent<NavMeshAgent>();
        if (GameObject.FindWithTag("Player") != null)
        {
            target = GameObject.FindWithTag("Player");
        }
        playerDetected = false;

        
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
                    if(hit.transform.root.tag == "Player")
                        playerDetected = true;       
            }
        }
        else
        {
            //destination
            if (Vector3.Distance(agentPos, playerPos) > distPlayer)
            {
                agent.Resume();
                agent.SetDestination(target.transform.position);
                //agent.enabled = true;
            }
            else
            {
                agent.Stop();
                //agent.enabled = false;
                agent.transform.LookAt(target.transform);
            }

            
        }

        //Debug.Log(Vector3.Distance(agentPos, playerPos));
    }
}
