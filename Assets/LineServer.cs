using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class LineServer : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
 
{
    public bool isOver = false;
    private int lineNumber;

    public void setLineNumber(int p_lineNumber)
    {
        lineNumber = p_lineNumber;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameObject.Find("MenuManager").GetComponent<MenuManager>().highlightLine(lineNumber);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameObject.Find("MenuManager").GetComponent<MenuManager>().deselectgLines();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject.Find("MenuManager").GetComponent<MenuManager>().clicLine(lineNumber);
    }

    //IPointerUpHandler
   /* void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("Pointer Up");
    }*/

    //IPointerDownHandler
    /*public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Pointer Down");
    }*/

    //ISelectHandler
    /*public void OnSelect(BaseEventData eventData)
    {
        Debug.Log("Select");
    }*/


}