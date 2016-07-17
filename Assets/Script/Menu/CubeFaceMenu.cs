using UnityEngine;
using System.Collections;

public class CubeFaceMenu : MonoBehaviour 
{
    protected Animator[] children;
    private float delay = 0.1f;

	// Use this for initialization
	void Start () 
    {
        children = GetComponentsInChildren<Animator>();

        StartCoroutine(ActivateInTurn());
	}
	
	// Update is called once per frame
	void Update () 
    {

	}

    public IEnumerator ActivateInTurn()
    {
        yield return new WaitForSeconds(delay);
        for (int a = 0; a < children.Length; a++)
        {
            children[a].SetBool("Shown", true);
            yield return new WaitForSeconds(delay);
        }
    }
}
