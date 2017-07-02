using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputsLoader : MonoBehaviour
{
    private Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();

    public List<GameObject> listInputsUser;

    void Awake()
    {
        if (listInputsUser == null)
            listInputsUser = new List<GameObject>();

        loadKeys();
    }

    public void loadKeys()
    {
        string data = PlayerPrefs.GetString("Inputs", "none");
        if (data != "none")
        {
            string[] array = data.Split('/');
            for (int i = 0; i < array.Length; i++)
            {
                string[] pair = array[i].Split(';');
                string key = pair[0];
                KeyCode value = (KeyCode)System.Enum.Parse(typeof(KeyCode), pair[1]);
                keys.Add(key, value);
            }
        }
        else
        {
            keys.Add("Forward", KeyCode.Z);
            keys.Add("Backward", KeyCode.S);
            keys.Add("Left", KeyCode.Q);
            keys.Add("Right", KeyCode.D);
            keys.Add("Shoot", KeyCode.Mouse0);
            keys.Add("Face1", KeyCode.Alpha1);
            keys.Add("Face2", KeyCode.Alpha2);
            keys.Add("Face3", KeyCode.Alpha3);
            keys.Add("Face4", KeyCode.Alpha4);
            keys.Add("Face5", KeyCode.Alpha5);
            keys.Add("Face6", KeyCode.Alpha6);
            keys.Add("Interact", KeyCode.E);
            keys.Add("Jump", KeyCode.Space);
            keys.Add("FreeCam", KeyCode.Escape);
            keys.Add("FollowCam", KeyCode.Mouse1);
            keys.Add("Zoom+", KeyCode.Mouse3);
            keys.Add("Zoom-", KeyCode.Mouse4);
        }
    }

	public Dictionary<string, KeyCode> getInputs()
    {
        return keys;
    }

    public Dictionary<string, KeyCode> lookAtInputs(GameObject go)
    {
        if (listInputsUser == null)
            listInputsUser = new List<GameObject>();
        listInputsUser.Add(go);
        return keys;
    }

    public void keysChanged()
    {
        keys.Clear();
        loadKeys();
        foreach (GameObject go in listInputsUser)
        {
            go.SendMessage("keysChanged", keys, SendMessageOptions.DontRequireReceiver);
        }
    }
}
