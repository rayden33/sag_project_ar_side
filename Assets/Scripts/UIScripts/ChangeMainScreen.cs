using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMainScreen : MonoBehaviour
{
    [SerializeField] private GameObject[] ActionScreensGo;
    void Start()
    {
        hideAllActionScreens();
        changeActionScreen(0);
    }

    void Update()
    {
        
    }

    public void changeActionScreen(int screenIndex)
    {
        hideAllActionScreens();
        ActionScreensGo[screenIndex].SetActive(true);
    }

    public void hideAllActionScreens()
    {
        foreach (GameObject item in ActionScreensGo)
        {
            item.SetActive(false);
        }
    }

    public void LoadCategoriesFromServer(string categoryType)
    {

        
    }
}
