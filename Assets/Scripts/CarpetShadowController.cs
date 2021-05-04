using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarpetShadowController : MonoBehaviour
{
    private void Start()
    {
        transform.localPosition += transform.up * -.1f;
    }
    public void PlaceCarpet()
    {
        transform.GetComponent<Renderer>().enabled = false;
    }
    public void UnPlaceCarpet()
    {
        transform.GetComponent<Renderer>().enabled = true;
    }
}
