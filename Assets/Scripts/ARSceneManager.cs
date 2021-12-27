using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ARSceneManager : MonoBehaviour
{
    [SerializeField]
    public Text CarpetInfoTxt;
    void Start()
    {
        string carpetName = PlayerPrefs.GetString("carpet_name");
        string carpetPrice = PlayerPrefs.GetString("collection_price");
        CarpetInfoTxt.text = $"Name: {carpetName}\nPrice: {carpetPrice} UZS";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            //SceneManager.LoadScene(0);
        }
    }
}
