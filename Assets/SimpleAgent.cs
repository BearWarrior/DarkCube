using UnityEngine;
using System.Collections;

public class SimpleAgent : MonoBehaviour
{
    public GameObject target;
    NavMeshAgent agent;

	// Use this for initialization
	void Start ()
    {
        agent = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 agentPos = new Vector3(this.transform.position.x, 0, this.transform.position.z);
        Vector3 playerPos = new Vector3(target.transform.position.x, 0, target.transform.position.z);
        Vector3 destination = new Vector3();

        //destination

        if (Vector3.Distance(agentPos, playerPos) > 20)
        {
            agent.Resume();
            agent.SetDestination(target.transform.position);
        }
        else
        {
            agent.Stop();
            this.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        }

        Debug.Log(Vector3.Distance(agentPos, playerPos));
	}
}
