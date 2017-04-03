using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuMain : MonoBehaviour
{
    public GameObject panelSubMenuPlay;
    public GameObject panelSubMenuConfig;
    public GameObject panelConfigGraphic;
    public GameObject panelConfigInputs;
    public GameObject panelConfigAudio;
    [Space(15)]
    public Dropdown graphicDropDownScreen;
    public Dropdown graphicDropDownResolution;
    [Space(15)]
    public List<Button> inputsListButton;
    public GameObject inputsContent;
    [Space(15)]

    private Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();


    private bool fullScreen;
    private int resW = 1600;
    private int resH = 900;

    private GameObject changingKey;

    public GameObject player { get; set; }
    public GameObject inputsLoader { get; set; }

    public GameObject exitLevelButton;

    // Use this for initialization
    void Start()
    {
        if (exitLevelButton != null)
        {
            if (SceneManager.GetActiveScene().name == "ProceduralRoom")
                exitLevelButton.SetActive(true);
            else
                exitLevelButton.SetActive(false);
        }

        inputsLoader = GameObject.FindWithTag("InputsLoader");

        changingKey = null;

        panelSubMenuPlay.SetActive(false);
        panelSubMenuConfig.SetActive(false);
        panelConfigGraphic.SetActive(false);
        panelConfigInputs.SetActive(false);
        panelConfigAudio.SetActive(false);

        graphicDropDownResolution.GetComponent<Dropdown>().options.Clear();
        List<Dropdown.OptionData> listReso = new List<Dropdown.OptionData>
        {
            new Dropdown.OptionData("1920/1080 (16/9)"),
            new Dropdown.OptionData("1600/1200 (4/3)"),
            new Dropdown.OptionData("1600/900 (16/9)"),
            new Dropdown.OptionData("1280/720 (16/9)"),
            new Dropdown.OptionData("1024/768 (4/3)")
        };
        graphicDropDownResolution.GetComponent<Dropdown>().options.AddRange(listReso);
        graphicDropDownResolution.GetComponent<Dropdown>().value = 0;
        graphicDropDownResolution.GetComponent<Dropdown>().captionText = graphicDropDownResolution.GetComponent<Dropdown>().captionText;

        graphicDropDownScreen.GetComponent<Dropdown>().options.Clear();
        List<Dropdown.OptionData> listScreen = new List<Dropdown.OptionData>
        {
            new Dropdown.OptionData("Plein écran"),
            new Dropdown.OptionData("Fenêtré")
        };
        graphicDropDownScreen.GetComponent<Dropdown>().options.AddRange(listScreen);
        graphicDropDownScreen.GetComponent<Dropdown>().value = 0;
        graphicDropDownScreen.GetComponent<Dropdown>().captionText = graphicDropDownScreen.GetComponent<Dropdown>().captionText;

        for (int i = 0; i < inputsListButton.Count; i++)
        {
            inputsListButton[i].transform.GetChild(0).GetComponent<Text>().text = keys[inputsListButton[i].name].ToString();
        }
    }

    void Awake()
    {
        //PlayerPrefs.DeleteKey("Inputs");
        load();
    }

    void OnGUI()
    {
        if (changingKey != null)
        {
            Event e = Event.current;
            if (e.isKey)
            {
                if (e.keyCode == KeyCode.Backspace)
                {
                    changingKey = null;
                }
                else
                {
                    keys[changingKey.name] = e.keyCode;
                    changingKey.transform.GetChild(0).gameObject.GetComponent<Text>().text = e.keyCode.ToString();
                    changingKey = null;
                    save();
                }
            }
            else if (e.isMouse)
            {
                keys[changingKey.name] = (KeyCode)System.Enum.Parse(typeof(KeyCode), "mouse" + e.button, true);
                changingKey.transform.GetChild(0).gameObject.GetComponent<Text>().text = keys[changingKey.name].ToString();
                changingKey = null;
                save();
            }
            else if(e.shift)
            {
                if(Input.GetKey(KeyCode.LeftShift))
                {
                    keys[changingKey.name] = KeyCode.LeftShift;
                    changingKey.transform.GetChild(0).gameObject.GetComponent<Text>().text = keys[changingKey.name].ToString();
                    changingKey = null;
                    save();
                }
                if (Input.GetKey(KeyCode.RightShift))
                {
                    keys[changingKey.name] = KeyCode.RightShift;
                    changingKey.transform.GetChild(0).gameObject.GetComponent<Text>().text = keys[changingKey.name].ToString();
                    changingKey = null;
                    save();
                }
            }
        }
    }

    //______________________SAVE____________________//
    public void save()
    {
        //Graphics
        PlayerPrefs.SetString("Graphics", fullScreen + "/" + resW + "/" + resW);
        //Inputs
        string inputs = "";
        foreach (KeyValuePair<string, KeyCode> entry in keys)
            inputs += entry.Key + ";" + entry.Value + "/";
        inputs = inputs.Substring(0, inputs.Length - 1);
        Debug.Log("save : " + inputs);
        PlayerPrefs.SetString("Inputs", inputs);
    }

    public void load()
    {
        //GRAPHICS
        string data = PlayerPrefs.GetString("Graphics", "none");
        if (data != "none")
        {
            string[] array = data.Split('/');
            //FULLSCREEN OR NO
            if (array[0] == "1")
                fullScreen = true;
            else
                fullScreen = false;
            //RESOLUTION
            if (!int.TryParse(array[1], out resW))
                resW = 1600;
            if (!int.TryParse(array[2], out resH))
                resW = 900;
        }
        else
        {
            fullScreen = false;
            resW = 1600;
            resW = 900;
        }

        //INPUTS
        data = PlayerPrefs.GetString("Inputs", "none");
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
            keys.Add("Zoom+", KeyCode.KeypadPlus);
            keys.Add("Zoom-", KeyCode.KeypadMinus);
        }
    }
    //______________________PLAY____________________//
    public void Play()
    {
        panelSubMenuPlay.SetActive(true);
        panelSubMenuConfig.SetActive(false);
        panelConfigGraphic.SetActive(false);
        panelConfigInputs.SetActive(false);
        panelConfigAudio.SetActive(false);
    }

    public void Continu()
    {
        SceneManager.LoadScene("RoomMaintenance");
    }

    public void NewGame()
    {

    }

    //______________________CONFIGURATION____________________//
    public void Configuration()
    {
        panelSubMenuPlay.SetActive(false);
        panelSubMenuConfig.SetActive(true);
        panelConfigGraphic.SetActive(false);
        panelConfigInputs.SetActive(false);
        panelConfigAudio.SetActive(false);
    }

    public void Graphics()
    {
        panelConfigGraphic.SetActive(true);
        panelConfigInputs.SetActive(false);
        panelConfigAudio.SetActive(false);
    }

    public void Inputs()
    {
        panelConfigGraphic.SetActive(false);
        panelConfigInputs.SetActive(true);
        panelConfigAudio.SetActive(false);
    }

    public void Audio()
    {
        panelConfigGraphic.SetActive(false);
        panelConfigInputs.SetActive(false);
        panelConfigAudio.SetActive(true);
    }

    public void GraphicsScreenChanged()
    {
        switch (graphicDropDownScreen.GetComponent<Dropdown>().value)
        {
            case 0:
                fullScreen = true;
                Screen.fullScreen = true;
                break;

            case 1:
                fullScreen = false;
                Screen.fullScreen = false;
                break;
        }
    }

    public void GraphicsResolutionChanged()
    {
        switch (graphicDropDownResolution.GetComponent<Dropdown>().value)
        {
            case 0: //1920/1080 (16/9)
                Screen.SetResolution(1920, 1080, fullScreen);
                break;
            case 1: //1600/1200 (4/3)
                Screen.SetResolution(1600, 1200, fullScreen);
                break;
            case 2: //1600/900 (16/9)
                Screen.SetResolution(1600, 900, fullScreen);
                break;
            case 3: //1280/720 (16/9)
                Screen.SetResolution(1280, 720, fullScreen);
                break;
            case 4: //1024/768 (4/3)
                Screen.SetResolution(1024, 768, fullScreen);
                break;
        }
    }

    public void InputsButtonClic(GameObject p_changingKey)
    {
        changingKey = p_changingKey;
    }
    
    //____________________________OTHER_________________________________//
    public void Exit()
    {
        Application.Quit();
    }

    //____________________________MENU PAUSE_____________________________//
    public void Retour()
    {
        Time.timeScale = 1;
        inputsLoader.GetComponent<InputsLoader>().keysChanged();
        player.GetComponent<PlayerController>().exitMenuPause();
    }

    public void ExitLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("RoomMaintenance");
    }
}


/*

     switch(index)
        {
            case 0: //Forward
                //inputsListButton[index]
                break;
            case 1: //Backward
                break;
            case 2: //Left
                break;
            case 3: //Right
                break;
            case 4: //Shoot
                break;
            case 5: //Face 1
                break;
            case 6: //Face 2 
                break;
            case 7: //Face 3
                break;
            case 8: //Face 4 
                break;
            case 9: //Face 5
                break;
            case 10: //Face 6
                break;
            case 11: //Interact
                break;
            case 12: //Jump
                break;
            case 13: //FreeCam
                break;
            case 14: //FollowCam
                break;
            case 15: //Zoom+
                break;
            case 16: //Zoom-
                break;
        }


    */
