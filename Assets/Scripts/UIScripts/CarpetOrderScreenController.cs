using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarpetOrderScreenController : MonoBehaviour
{
    public Carpet Carpet { get; set; }
    [SerializeField] private Text carpetNameTxt;
    void Start()
    {
        
    }


    public void FillContent()
    {
        carpetNameTxt.text = Carpet.serial_number;

    }
}
