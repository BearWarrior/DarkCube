using UnityEngine;
using UnityEngine.EventSystems;

public class TestHandler : MonoBehaviour, IPointerUpHandler
{
    public void OnPointerUp(PointerEventData data)
    {
        Debug.Log("Button " + data.pointerId + " up");
    }
}