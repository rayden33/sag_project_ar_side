using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionManager : MonoBehaviour
{
    public float checkConnectionSecondsInterval = 1.0f;
    [SerializeField] private GameObject ErrorScreenGo;

    [HideInInspector]
    public bool isOnline;

    private float tmpTimer;

    void Start()
    {
        isOnline = true;
        tmpTimer = checkConnectionSecondsInterval;
    }

    void Update()
    {
        if(tmpTimer > 0)
        {
            tmpTimer -= Time.deltaTime;
        }
        else
        {
            tmpTimer = checkConnectionSecondsInterval;
            CheckInternetConnection();
        }
        
    }

    public void CheckInternetConnection()
    {
        if (isOnline)
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                isOnline = false;
                ErrorScreenGo.SetActive(true);
            }
        }
    }
}
