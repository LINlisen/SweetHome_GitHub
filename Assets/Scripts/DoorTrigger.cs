using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField]

    public GameObject Door;

    public float maxOpen = 5f;//max door height
    public float maxClose = 0f;
    public float doorSpeed = 5f;

    bool playerHere;
    bool isOpened;

    //float countTime=10;
    //float count=0;

    private void Start()
    {
        playerHere = false;
        isOpened = false;

    }
    private void Update()
    {
        if (playerHere)
        {
            if (Door.transform.position.y < maxOpen)//move LeftRight
            {
                //Debug.Log(playerHere);
                Door.transform.Translate( 0f, doorSpeed * Time.deltaTime, 0f);
            }
        }
        else
        {
            if (Door.transform.position.y > maxClose)
            {
                //Debug.Log(playerHere);
                Door.transform.Translate( 0f, -doorSpeed * Time.deltaTime, 0f);
            }
        }
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            playerHere = true;
        }
    }
    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            playerHere = false;
        }
    }
    //private void OnTriggerStay(Collider col)
    //{
    //    if (isOpened == false)
    //    {
    //        
    //        if(count!=500)
    //        {
    //            count ++;
    //            door.transform.position += new Vector3(0, 1, 0)*Time.deltaTime;
    //            
    //            Debug.Log(door.transform.position);
    //            Debug.Log(Time.deltaTime);
    //            Debug.Log(count);
    //        }
    //        count = 0;
    //    }
    //}


}
