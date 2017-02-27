using UnityEngine;
using System.Collections;

//TODO add more animations
//TODO make an Idle animation of 50 frame without moving to make the exit transition

public class MecaArm : MonoBehaviour
{
    private Animator animator;

    public GameObject ground;
    public GameObject lights;
    public GameObject rotateObject;

    private float startTime;

    private Vector3 groundUpPos;
    private Vector3 groundDownPos;
    private float speedG;
    private float journeyLengthG;

    private Vector3 lightsUpPos;
    private Vector3 lightsDownPos;
    private float speedL;
    private float journeyLengthL;

    // Use this for initialization
    void Start ()
    {
        animator = GetComponent<Animator>();

        groundUpPos = ground.transform.localPosition;
        //groundDownPos = new Vector3(0, -0.03f, 0);
        groundDownPos = new Vector3(0, 0.36f, 0);
        ground.transform.localPosition = groundDownPos;
        speedG = 0.5f;

        lightsUpPos = lights.transform.localPosition;
        lightsDownPos = lights.transform.localPosition - new Vector3(0, 0.5f, 0);
        speedL = 0.5f;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            animator.SetInteger("RandomAnim", Random.Range(1, 6));
        }
    }

    public IEnumerator upPlatform()
    {
        float distCovered = (Time.time - startTime) * speedG;
        float fracJourney = distCovered / journeyLengthG;
        while (fracJourney < 1)
        {
            distCovered = (Time.time - startTime) * speedG;
            fracJourney = distCovered / journeyLengthG;
            ground.transform.localPosition = Vector3.Lerp(groundDownPos, groundUpPos, fracJourney);
            yield return new WaitForEndOfFrame();
        }
    }

    public IEnumerator downPlatform()
    {
        float distCovered = (Time.time - startTime) * speedG;
        float fracJourney = distCovered / journeyLengthG;
        while (fracJourney < 1)
        {
            distCovered = (Time.time - startTime) * speedG;
            fracJourney = distCovered / journeyLengthG;
            ground.transform.localPosition = Vector3.Lerp(groundUpPos, groundDownPos, fracJourney);
            yield return new WaitForEndOfFrame();
        }
    }

    public IEnumerator downLights()
    {
        float distCovered = (Time.time - startTime) * speedL;
        float fracJourney = distCovered / journeyLengthL;
        while (fracJourney < 1)
        {
            distCovered = (Time.time - startTime) * speedG;
            fracJourney = distCovered / journeyLengthL;
            lights.transform.localPosition = Vector3.Lerp(lightsUpPos, lightsDownPos, fracJourney);
            yield return new WaitForEndOfFrame();
        }

        GetComponent<accessMenu>().setReady(true);
        animator.SetBool("Begin", true);
    }

    public IEnumerator upLights()
    {
        float distCovered = (Time.time - startTime) * speedL;
        float fracJourney = distCovered / journeyLengthL;
        while (fracJourney < 1)
        {
            distCovered = (Time.time - startTime) * speedG;
            fracJourney = distCovered / journeyLengthL;
            lights.transform.localPosition = Vector3.Lerp(lightsDownPos, lightsUpPos, fracJourney);
            yield return new WaitForEndOfFrame();
        }

        animator.SetBool("Begin", false);
    }

    public void launchMenu()
    {
        startTime = Time.time;
        journeyLengthG = Vector3.Distance(groundDownPos, groundUpPos);
        journeyLengthL = Vector3.Distance(lightsUpPos, lightsDownPos);
        //StartCoroutine(upPlatform());
        StartCoroutine(downLights());
        animator.SetBool("ExitAnim", false); //To exit the actual animation and return to Idle
    }

    public void quitMenu()
    {
        if (animator.GetBool("Begin") == true)
        {
            animator.SetBool("Begin", false);
            GetComponent<accessMenu>().setReady(false);
            startTime = Time.time;
            journeyLengthG = Vector3.Distance(groundUpPos, groundDownPos);
            journeyLengthL = Vector3.Distance(lightsDownPos, lightsUpPos);
            //StartCoroutine(downPlatform());
            StartCoroutine(upLights());
            animator.SetBool("ExitAnim", true); //To exit the actual animation and return to Idle
        }
    }
}
