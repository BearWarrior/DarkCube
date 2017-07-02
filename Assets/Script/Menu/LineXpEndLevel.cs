using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LineXpEndLevel : MonoBehaviour
{
    public GameObject nom;
    public GameObject lvl;
    public GameObject xp;
    public GameObject progressBar;

    private GameObject caracSorts;
    private string nomPartSort;
    private int typeSort;

    private float fromXP;
    private float toXP;
    private float fromXPwon;
    private float toXPwon;
    private float xpBase = 0;
    private float filledAmountXP;
    private float xpWon;

    private float speed = 1;
    private float startTime ;
    private float journeyLength = 2;

    private float fracJourney;
    private float distCovered;

    public void display(GameObject caracSorts, string nom, string nomPart, int type)
    {
        this.caracSorts = caracSorts;
        this.nomPartSort = nomPart;
        this.typeSort = type;

        this.nom.GetComponent<Text>().text = nom;
    }

    public void animate()
    {
        if (typeSort == 1)
        {
            StartCoroutine(manageXpLine(nomPartSort));
        }
        /*else if(typeSort == 2)
        {

        }*/
    }
    
    /*
    On stocke xpPrevious
    Tant lvlPrevious < lvl
        On remplit de xpPrevious a xpActuel
        lvlPrevious++;
        xpPrevious = 0
    Fin Tant que
    On remplit de xpPrevious a xpActuel
    */
    public IEnumerator manageXpLine(string nomPart)
    {
        structSortJet structure = caracSorts.GetComponent<CaracProjectiles>().getStructFromName(nomPart);
        Debug.Log("XpTemp : " + structure.xpTemp);
        startTime = Time.time;

        int lvlPrev = structure.lvlPrevious;
        float xpPrev = structure.xpPrevious;
        this.lvl.GetComponent<Text>().text = lvlPrev.ToString();
        this.xp.GetComponent<Text>().text = structure.xpTemp.ToString();
        bool filled = false;

        while (lvlPrev < structure.lvl)
        {
            startTime = Time.time;
            journeyLength = 2;

            fromXP = xpPrev / (caracSorts.GetComponent<CaracProjectiles>().xpToLvlUp * Mathf.Pow(caracSorts.GetComponent<CaracProjectiles>().multXpByLvl, lvlPrev));
            toXP = 1;
            fromXPwon = xpBase;
            toXPwon = xpBase + (caracSorts.GetComponent<CaracProjectiles>().xpToLvlUp * Mathf.Pow(caracSorts.GetComponent<CaracProjectiles>().multXpByLvl, lvlPrev));

            //On remplit de xpPrevious a xpActuel
            while (!filled)
            {
                distCovered = (Time.time - startTime) * speed;
                fracJourney = distCovered / journeyLength;
                
                filledAmountXP = Mathf.Lerp(fromXP, toXP, fracJourney);
                this.progressBar.GetComponent<Image>().fillAmount = filledAmountXP;
                //xpBase -> xpBase + xp to lvlUp
                xpWon = Mathf.Lerp(fromXPwon, toXPwon, fracJourney);
                this.xp.GetComponent<Text>().text = xpWon.ToString("0.00");

                if (fracJourney > 1)
                    filled = true;
                yield return new WaitForEndOfFrame();
            }
            filled = false;
            lvlPrev++;
            xpBase = toXPwon;
            this.lvl.GetComponent<Text>().text = lvlPrev.ToString();
            xpPrev = 0;
        }

        startTime = Time.time;
        journeyLength = 2;
        filled = false;

        fromXP = xpPrev / (caracSorts.GetComponent<CaracProjectiles>().xpToLvlUp * Mathf.Pow(caracSorts.GetComponent<CaracProjectiles>().multXpByLvl, lvlPrev));
        toXP = structure.xpActuel / (caracSorts.GetComponent<CaracProjectiles>().xpToLvlUp * Mathf.Pow(caracSorts.GetComponent<CaracProjectiles>().multXpByLvl, lvlPrev));
        fromXPwon = xpBase;
        toXPwon = structure.xpTemp;

        //On remplit de xpPrevious a xpActuel
        while (!filled)
        {
            distCovered = (Time.time - startTime) * speed;
            fracJourney = distCovered / journeyLength;

            filledAmountXP = Mathf.Lerp(fromXP, toXP, fracJourney);
            this.progressBar.GetComponent<Image>().fillAmount = filledAmountXP;
            //xpBase -> xpBase + xp to lvlUp
            xpWon = Mathf.Lerp(fromXPwon, toXPwon, fracJourney);
            this.xp.GetComponent<Text>().text = xpWon.ToString("0.00");

            if (fracJourney > 1)
                filled = true;
            yield return new WaitForEndOfFrame();
        }
    }
}