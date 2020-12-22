using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

[RequireComponent(typeof(InputField))]
public class MenuSetUsername : MonoBehaviour
{
    private static string prefKeyUsername = "player_username";
    private static InputField inputField;

    private void Awake()
    {
        inputField = GetComponent<InputField>();
        
        if (PlayerPrefs.HasKey(prefKeyUsername))
        {
            string _savedUsername = PlayerPrefs.GetString(prefKeyUsername);
            inputField.text = _savedUsername;
            PhotonNetwork.NickName = _savedUsername;
        }
        else
        {
            SetRandomUsername();
        }
    }

    public void SetUsername(string username)
    {
        if (inputField.text == string.Empty)
        {
            SetRandomUsername();
            return;
        }
        
        PlayerPrefs.SetString(prefKeyUsername, username);
        PhotonNetwork.NickName = username;
    }

    public void SetRandomUsername()
    {
        PhotonNetwork.NickName = $"Player#{Random.Range(1000, 10000)}";
    }
}
