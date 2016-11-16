using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CanvasDungeonEnd : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void Quitter()
    {
        GameObject.FindWithTag("CaracSorts").GetComponent<CaracProjectiles>().sauvegarder();
        SceneManager.LoadScene("CustomRoom");
    }
}
