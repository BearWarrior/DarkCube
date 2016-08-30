using UnityEngine;
using System.Collections;

public class SimpleAgent : MonoBehaviour
{
    public GameObject target;
    NavMeshAgent agent;
    public float dist = 1;

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

        if (Vector3.Distance(agentPos, playerPos) > dist)
        {
            agent.Resume();
            agent.SetDestination(target.transform.position);
            agent.enabled = true;
        }
        else
        {
            agent.Stop();
            agent.enabled = false;
            agent.transform.LookAt(target.transform);
        }

        Debug.Log(Vector3.Distance(agentPos, playerPos));
	}
}
