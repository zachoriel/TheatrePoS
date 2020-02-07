using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserTracker : MonoBehaviour
{
    #region Singleton
    public static UserTracker instance;
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }
    #endregion

    [HideInInspector] public string currentUser;
}
