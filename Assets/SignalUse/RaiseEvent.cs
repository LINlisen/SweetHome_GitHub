using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class RaiseEvent : MonoBehaviourPun
{

    // Start is called before the first frame update
    private const byte GET_POTION_EVENT=0;
    private const byte TAKE_TOAST = 1;
    GameObject[] TakedPotion;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_EventReceived;
        Debug.Log("OnEable");
    }
    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClient_EventReceived;
        Debug.Log("OnDisEable");
    }
    private void NetworkingClient_EventReceived(EventData obj)
    {
        if(obj.Code == GET_POTION_EVENT)
        {
            Debug.Log("NetWork");
            object[] datas = (object[])obj.CustomData;
            bool b = (bool)datas[0];
            Debug.Log(TakedPotion[0].name);

            TakedPotion[0].gameObject.SetActive(b);
        }
    }

    public void getPotion(GameObject takedPotion)
    {
        
        bool b = false;
        
        //Debug.Log(this.gameObject.name);
        //potion.SetActive(b);
        object[] datas = new object[] {b };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(GET_POTION_EVENT,datas, raiseEventOptions, SendOptions.SendReliable);
        TakedPotion = new GameObject[1];
        TakedPotion[0] = takedPotion;

    }
    public void takeToast()
    {
        //Vector3 takePos =
    }
}
