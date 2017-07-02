using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavingLogo : MonoBehaviour
{
    public GameObject logo;
	// Use this for initialization
	void Start ()
    {
        DontDestroyOnLoad(this);
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void DisplayLogo()
    {
        StartCoroutine(DisplayLogoTemporary());
    }

    public IEnumerator DisplayLogoTemporary()
    {
        logo.SetActive(true);
        yield return new WaitForSeconds(2);
        logo.SetActive(false);
    }
}
