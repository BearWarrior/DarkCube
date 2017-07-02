using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionChoose : MonoBehaviour
{
    private int activeMission = 0;
    public MenuUnivers menuUnivers;

    public List<Text> textNameMissions;
    public List<String> nameMission;

    private const float delayDoubleClick = 0.4f;
    private float doubleClickStart = -1;
    private int lastChildNumber;

    // Use this for initialization
    void Start()
    {
        nameMission = new List<String>();
        hide();
    }

    //TODO From 0 to 1 slowwwwlllyyy
    public void missionOnHoverEnter(int childNumber)
    {
        if (childNumber != activeMission)
        {
            GameObject mission = gameObject.transform.GetChild(childNumber).gameObject;
            GameObject image = mission.transform.GetChild(mission.transform.childCount - 1).gameObject;
            image.GetComponent<Image>().fillAmount = 1;
        }
    }

    //TODO From 1 to 0 slowwwwlllyyy
    public void missionOnHoverExit(int childNumber)
    {
        if (childNumber != activeMission)
        {
            GameObject mission = gameObject.transform.GetChild(childNumber).gameObject;
            GameObject image = mission.transform.GetChild(mission.transform.childCount - 1).gameObject;
            image.GetComponent<Image>().fillAmount = 0;
        }
    }

    //TODO From 1 to 0 slowwwwlllyyy
    public void missionClic(int childNumber)
    {
        if (doubleClickStart > 0 && (Time.time - doubleClickStart) < delayDoubleClick) //double click
        {
            if (childNumber == lastChildNumber) //Si le clic se fait au même endroit que le précédent
                menuUnivers.oK();
            doubleClickStart = -1;
        }
        else // premier click ou click trop tard, on considere ce clic comme le nouveau premier click
        {
            doubleClickStart = Time.time;
            lastChildNumber = childNumber;
            unclicAllMission();
            activeMission = childNumber;
            GameObject mission = gameObject.transform.GetChild(childNumber).gameObject;
            GameObject image = mission.transform.GetChild(mission.transform.childCount - 1).gameObject;
            image.GetComponent<Image>().fillAmount = 1;
            menuUnivers.setMissionChosen(activeMission);
        }

        
    }

    private void unclicAllMission()
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            GameObject mission = gameObject.transform.GetChild(i).gameObject;
            GameObject image = mission.transform.GetChild(mission.transform.childCount - 1).gameObject;
            image.GetComponent<Image>().fillAmount = 0;
        }
    }

    public void setMissionDetails(List<string> names)
    {
        nameMission = names;
        for(int i = 0; i < textNameMissions.Count; i++)
            textNameMissions[i].text = nameMission[i];
        missionClic(0); //We choose the first mission
    }

    public void hide()
    {
        for(int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(false);
    }

    public void show()
    {
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(true);
    }
}


