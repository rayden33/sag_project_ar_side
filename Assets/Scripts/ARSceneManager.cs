using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ARSceneManager : MonoBehaviour
{
    [SerializeField]
    public Text CarpetInfoTxt;
    void Start()
    {
        string carpetName = CustomCoreRAM.selectedCarpetDetails.serial_number;
        string carpetPrice = CustomCoreRAM.selectedCarpetDetails.price.ToString("N", CultureInfo.CreateSpecificCulture("sv-SE"));
        CarpetInfoTxt.text = $"Name: {carpetName}\nPrice: {carpetPrice} UZS";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            CustomCoreRAM.isReturnFromAR = true;
            SceneManager.LoadScene(0);
        }
    }

    public void GoToCarpetDetails()
    {
        CustomCoreRAM.isReturnFromAR = true;
        SceneManager.LoadScene(0);
    }
}
