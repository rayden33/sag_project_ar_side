using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarpetManager : MonoBehaviour
{
    public bool isPlaced = false;
    private bool isMovingUp = true;
    private int movingDirection = 1;
    [SerializeField] private float maxHeight = .1f;
    [SerializeField] private float minHeight = 0;
    [SerializeField] private float movingSpeed = 25.0f;

    void Start()
    {

    }


    void Update()
    {
        if (!isPlaced)
        {
            if (transform.localPosition.y > maxHeight ) 
                isMovingUp = false;
            if (transform.localPosition.y < minHeight)
                isMovingUp = true;

            if (isMovingUp)
                transform.Translate(Vector3.up * Time.deltaTime  * (movingSpeed / 100.0f));
            else
                transform.Translate(Vector3.down * Time.deltaTime * (movingSpeed / 100.0f));
        }
    }

    public void PlaceObject()
    {
        //transform.parent.transform.position += transform.parent.transform.up * -.1f;
        transform.localPosition.Set(0, 0, 0);
        isPlaced = true;
    }
    public void ChangePlacedStatus()
    {
        isPlaced = false;
    }
}
