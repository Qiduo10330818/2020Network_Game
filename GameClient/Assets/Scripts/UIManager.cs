using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject startMenu;
    public Text ScoreText;
    public InputField usernameField;
    public InputField IPField;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            ScoreText.enabled = false;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    public void ConnectToServer()
    {
        startMenu.SetActive(false);
        ScoreText.enabled = true;
        ScoreText.text = "Score: 0";
        usernameField.interactable = false;
        IPField.interactable = false;
        Client.instance.ip = IPField.text;
        Client.instance.ConnectToServer();
    }
}