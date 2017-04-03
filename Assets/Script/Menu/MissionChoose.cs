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
        unclicAllMission();
        activeMission = childNumber;
        GameObject mission = gameObject.transform.GetChild(childNumber).gameObject;
        GameObject image = mission.transform.GetChild(mission.transform.childCount - 1).gameObject;
        image.GetComponent<Image>().fillAmount = 1;
        menuUnivers.setMissionChosen(activeMission);
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


