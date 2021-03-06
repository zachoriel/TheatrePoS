﻿using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

[System.Serializable]
public struct LoginData
{
    public Dictionary<string, string> userRegistry;
    public int registeredUsers;

    public LoginData(Dictionary<string, string> _userRegistry, int _registeredUsers)
    {
        userRegistry = _userRegistry;
        registeredUsers = _registeredUsers;
    }
}

public class LoginManager : MonoBehaviour
{
    [SerializeField] TMP_InputField newID, newPassword, idInput, passwordInput, deleteID, deletePassword;
    [SerializeField] TextMeshProUGUI mainStatusText;
    [SerializeField] TextMeshProUGUI loginStatusText;

    Dictionary<string, string> userRegistry = new Dictionary<string, string>();
    int registeredUsers = 0;

    
    void Awake()
    {
        string fileDestination = Application.persistentDataPath + "\\AccountInfo.dat";
        FileStream file;

        if (File.Exists(fileDestination))
        {
            file = File.OpenRead(fileDestination);
        }
        else
        {
            return;
        }

        BinaryFormatter bf = new BinaryFormatter();
        LoginData loginData = (LoginData)bf.Deserialize(file);
        file.Close();

        userRegistry = loginData.userRegistry;
        registeredUsers = loginData.registeredUsers;

        if (registeredUsers == 1)
        {
            mainStatusText.text = "There is " + registeredUsers + " registered user on this machine.";
        }
        else
        {
            mainStatusText.text = "There are " + registeredUsers + " registered users on this machine.";
        }
    }

    public void CreateNewUser()
    {
        string userID = newID.text;
        string userPassword = newPassword.text;

        if (!userRegistry.ContainsKey(userID))
        {
            userRegistry.Add(userID, userPassword);
            registeredUsers++;

            string fileDestination = Application.persistentDataPath + "\\AccountInfo.dat";
            FileStream file;

            if (File.Exists(fileDestination))
            {
                file = File.OpenWrite(fileDestination);
            }
            else
            {
                file = File.Create(fileDestination);
            }

            LoginData loginData = new LoginData(userRegistry, registeredUsers);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(file, loginData);
            file.Close();

            if (registeredUsers == 1)
            {
                mainStatusText.text = "There is " + registeredUsers + " registered user on this machine.";
            }
            else
            {
                mainStatusText.text = "There are " + registeredUsers + " registered users on this machine.";
            }
        }
        else
        {
            Debug.LogError("Account Name Already Exists!");
        }

        newID.text = null;
        newPassword.text = null;
    }

    public void DeleteUser()
    {
        string userID = deleteID.text;

        if (userRegistry.ContainsKey(userID))
        {
            userRegistry.Remove(userID);
            registeredUsers--;

            string fileDestination = Application.persistentDataPath + "\\AccountInfo.dat";
            FileStream file;

            file = File.OpenWrite(fileDestination);

            LoginData loginData = new LoginData(userRegistry, registeredUsers);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(file, loginData);
            file.Close();

            if (registeredUsers == 1)
            {
                mainStatusText.text = "There is " + registeredUsers + " registered user on this machine.";
            }
            else
            {
                mainStatusText.text = "There are " + registeredUsers + " registered users on this machine.";
            }
        }
        else
        {
            Debug.LogError("User doesn't exist!");
        }

        deleteID.text = null;
    }

    public void SignIn()
    {
        string userID = idInput.text;
        string userPassword = passwordInput.text;

        if (userRegistry.ContainsKey(userID))
        {
            if (IsCorrectPassword(userID, userPassword))
            {
                UserTracker.instance.currentUser = userID;
                SceneManager.LoadScene("PointOfSaleScene");
            }
            else
            {
                loginStatusText.text = "Invalid Password";
                loginStatusText.GetComponent<Animator>().SetTrigger("InvalidEntry");
            }
        }
        else
        {
            loginStatusText.text = "User ID Not Found";
            loginStatusText.GetComponent<Animator>().SetTrigger("InvalidEntry");
        }

        idInput.text = null;
        passwordInput.text = null;
    }

    bool IsCorrectPassword(string username, string password)
    {
        string value;
        userRegistry.TryGetValue(username, out value);

        if (password == value)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Exit()
    {
        Application.Quit();
    }
}
