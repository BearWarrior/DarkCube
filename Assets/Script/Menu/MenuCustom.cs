using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCustom : MonoBehaviour, IDisplayable
{
    private GameObject player;
    private PlayerCubeFlock playerCubeFlock;
    public List<string> listText;
    public accessMenu accessMenu;
    private bool inMenu;

    void Start()
    {
        StartCoroutine(seekPlayer());
        GetComponent<CanvasGroup>().interactable = false;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public IEnumerator seekPlayer()
    {
        while (GameObject.FindWithTag("Player") == null)
            yield return new WaitForSeconds(0.1f);
        yield return new WaitForSeconds(0.1f);
        InitMenu();
    }

    private void InitMenu()
    {
        player = GameObject.FindWithTag("Player");
        playerCubeFlock = player.GetComponent<PlayerCubeFlock>();
    }

    
    void Update()
    {
        if(Input.GetKey(KeyCode.Escape))
        {
            back();
        }
    }

    public void back()
    {
        accessMenu.exitMenu();
    }

    public void changeSkin(int index)
    {
        Texture text = Resources.Load("Player/Textures/" + listText[index]) as Texture;
        playerCubeFlock.changeSkin(text);
        player.GetComponent<Player>().setSkin(text, listText[index]);
    }

    /*public void changeCore(int index)
    {

    }*/

    public void hide()
    {
        GameObject.FindWithTag("Player").GetComponent<PlayerController>().showCanvas(true);
        GetComponent<CanvasGroup>().interactable = false;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void show()
    {
        GameObject.FindWithTag("Player").GetComponent<PlayerController>().showCanvas(false);
        GetComponent<CanvasGroup>().interactable = true;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
}
