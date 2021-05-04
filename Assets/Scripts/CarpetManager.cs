using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarpetManager : MonoBehaviour
{
    public bool isPlaced = false;
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlaceObject();
        }
        /*if (!isPlaced)
        {
            if (transform.localPosition.y > maxHeight ) 
                isMovingUp = false;
            if (transform.localPosition.y < minHeight)
                isMovingUp = true;

            if (isMovingUp)
                transform.Translate(Vector3.up * Time.deltaTime  * (movingSpeed / 100.0f));
            else
                transform.Translate(Vector3.down * Time.deltaTime * (movingSpeed / 100.0f));
        }*/
    }

    public void PlaceObject()
    {
        //transform.parent.transform.position += transform.parent.transform.up * -.1f;
        isPlaced = true;
        animator.SetTrigger("Corpet Placed");
        //transform.Translate(Vector3.down * Time.deltaTime * (movingSpeed / 100.0f));
        //transform.localPosition.Set(0, 0, 0);
        
    }
    public void ChangePlacedStatus()
    {
        isPlaced = false;
        animator.SetTrigger("Corpet Animation");
    }
}
