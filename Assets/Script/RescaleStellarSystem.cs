using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RescaleStellarSystem : MonoBehaviour
{
    public List<GameObject> planets; //set up in inspector
    public float planetsFinalDist; //set up in inspector
    public GameObject sun; //set up in inspector
    public float sunMultiplier; //set up in inspector

    private List<Vector3> posInitPlanets; //set up in inspector
    private List<Vector3> posFinalPlanets; //calcul
    private float sunMin; //calcul
    private float sunMax; //calcul

    private List<float> journeysLength;
    private List<bool> journeysEnded;
    private float sunJourneyLength;
    private bool sunJourneyEnded;

    private float startTime;
    public float speed; //reduce and expand speed

    private bool reduceSystem = false;
    private bool expandSystem = false;

    public Light ambientLight;
    public GameObject arrow;

    void Start()
    {
        journeysEnded = new List<bool>();
        posFinalPlanets = new List<Vector3>();
        posInitPlanets = new List<Vector3>();
        journeysLength = new List<float>();
        //SUN
        sunMin = sun.transform.localScale.x;
        sun.transform.localScale *= sunMultiplier;
        sunMax = sun.transform.localScale.x;
        //PLANETS
        for(int i = 0; i < planets.Count; i++)
        {
            posInitPlanets.Add(planets[i].transform.localPosition);
            planets[i].transform.localPosition *= planetsFinalDist;
            posFinalPlanets.Add(planets[i].transform.localPosition);
            journeysEnded.Add(false);
        }
        arrow.SetActive(false);
    }

    //We use only journeysLength[0] to make them arrive at the same moment
    void Update()
    {
        //The system is selected, we want to zoom on it
        if (reduceSystem)
        {
            //When are we in the Lerp
            float distCovered = (Time.time - startTime) * speed;
            for(int i = 0; i < planets.Count; i++) //For each planet
            {
                if (!journeysEnded[i]) //If the lerp isn't finished
                {
                    float fracJourney = distCovered / journeysLength[0];
                    //We move the planet
                    planets[i].transform.localPosition = Vector3.Lerp(posFinalPlanets[i], posInitPlanets[i], fracJourney);

                    if (fracJourney > 1) //If the planet is at position
                        journeysEnded[i] = true; //stop moving this planet
                }
            }
            //Sun
            float fracJourneySun = distCovered / journeysLength[0];
            //We move the planet
            float newScale =   Mathf.Lerp(sunMax, sunMin, fracJourneySun);
            sun.transform.localScale = new Vector3(newScale, newScale, newScale);

            if (fracJourneySun > 1) //If the planet is at position
                sunJourneyEnded = true; //stop moving this planet

            //Stop lerping when all planets are in position
            bool totEnd = sunJourneyEnded;
            for(int i = 0; i < journeysEnded.Count; i++)
                totEnd = totEnd && journeysEnded[i];
            if (totEnd)
            {
                reduceSystem = false;
                for(int i =0; i < planets.Count; i++)
                {
                    journeysEnded[i] = false;
                }
            }
        }

        //Return to Galaxy mode
        if (expandSystem)
        {
            //When are we in the Lerp
            float distCovered = (Time.time - startTime) * speed;
            for (int i = 0; i < planets.Count; i++) //For each planet
            {
                if (!journeysEnded[i]) //If the lerp isn't finished
                {
                    float fracJourney = distCovered / journeysLength[0];
                    //We move the planet
                    planets[i].transform.localPosition = Vector3.Lerp(posInitPlanets[i], posFinalPlanets[i], fracJourney);

                    if (fracJourney > 1) //If the planet is at position
                        journeysEnded[i] = true; //stop moving this planet
                }
            }
            //Sun
            float fracJourneySun = distCovered / journeysLength[0];
            //We move the planet
            float newScale = Mathf.Lerp(sunMin, sunMax, fracJourneySun);
            sun.transform.localScale = new Vector3(newScale, newScale, newScale);

            if (fracJourneySun > 1) //If the planet is at position
                sunJourneyEnded = true; //stop moving this planet

            //Stop lerping when all planets are in position
            bool totEnd = sunJourneyEnded;
            for (int i = 0; i < journeysEnded.Count; i++)
                totEnd = totEnd && journeysEnded[i];
            if (totEnd)
            {
                expandSystem = false;
                for (int i = 0; i < planets.Count; i++)
                {
                    journeysEnded[i] = false;
                }
            }
        }
    }

    public void choseStellarSystem()
    {
        //Launch of the reduction of the system
        reduceSystem = true;
        //Lerp init
        startTime = Time.time; //Start time of lerping 
        for (int i = 0; i < planets.Count; i++) //For each planets
            journeysLength.Add(Vector3.Distance(posFinalPlanets[i], posInitPlanets[i])); //Calcul the distance between their pos and their destination
        sunJourneyLength = sunMax - sunMin; //Same for the sun
        //Color light to illumine the room (only when SS chosen)
        ambientLight.enabled = true;
        arrow.SetActive(true);
    }

    public void expandStellarSystem()
    {
        //Launch of the reduction of the system
        expandSystem = true;
        //Lerp init
        startTime = Time.time; //Start time of lerping 
        for (int i = 0; i < planets.Count; i++) //For each planets
            journeysLength.Add(Vector3.Distance(posInitPlanets[i], posFinalPlanets[i])); //Calcul the distance between their pos and their destination
        sunJourneyLength = sunMin - sunMax; //Same for the sun
        //Color light to illumine the room (only when SS chosen)
        ambientLight.enabled = false;
        arrow.SetActive(false);
    }
}
