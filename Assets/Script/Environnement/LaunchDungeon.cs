using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LaunchDungeon : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameObject.FindWithTag("CaracSorts").GetComponent<CaracProjectiles>().sauvegarder();
            //GameObject.FindWithTag("CaracSort").GetComponent<CaracZones>().sauvegarder();
            SceneManager.LoadScene("TestRoom");
        }
    }
}
