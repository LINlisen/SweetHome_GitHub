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
         
            object[] datas = (object[])obj.CustomData;
            bool b = (bool)datas[0];
            string PotionName = (string)datas[1];
            Debug.Log("NetWork"+ PotionName);
            GameObject.Find(PotionName).SetActive(b);
            

        }
    }

    public void getPotion(string name)
    {
        
        bool b = false;
        string PotionName = name;
        //Debug.Log(this.gameObject.name);
        //potion.SetActive(b);
        object[] datas = new object[] {b, PotionName };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(GET_POTION_EVENT,datas, raiseEventOptions, SendOptions.SendReliable);
       



    }
    public void takeToast()
    {
        //Vector3 takePos =
    }
}
