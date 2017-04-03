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

    void Start()
    {
        StartCoroutine(seekPlayer());
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
    }

    public void show()
    {
        GameObject.FindWithTag("Player").GetComponent<PlayerController>().showCanvas(false);
    }
}
