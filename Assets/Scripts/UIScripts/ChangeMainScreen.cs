using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        TapToBackButton();
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
            if(item.name != "FeaturedScreen")
                item.SetActive(false);
        }
    }

    public void LoadCategoriesFromServer(string categoryType)
    {

        
    }
    public void LoadGoogleMap()
    {
        Application.OpenURL("https://www.google.com/maps/d/edit?mid=1g9Q3KOPBWNBOzICGMdeOL6Fjwk1tJ0G0&usp=sharing");
      
    }


    public void TapToBackButton()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (ActionScreensGo[3].activeInHierarchy)    /// When now open carpet details screen
            {
                ActionScreensGo[3].SetActive(false);
            }
            else if (ActionScreensGo[6].activeInHierarchy)    /// When now open collection list screen
            {
                ActionScreensGo[6].SetActive(false);
            }
            else if(ActionScreensGo[5].activeInHierarchy)    /// When now open carpet list screen
            {
                ActionScreensGo[5].SetActive(false);
            }
            else if(ActionScreensGo[1].activeInHierarchy)   /// When now open by room screen
            {
                ActionScreensGo[1].SetActive(false);
                //ActionScreensGo[0].SetActive(true);
            }
            else if (ActionScreensGo[2].activeInHierarchy)  /// When now open by style screen
            {
                ActionScreensGo[2].SetActive(false);
                //ActionScreensGo[0].SetActive(true);
            }
            else
            {
                Application.Quit();
            }
        }
    }
}
