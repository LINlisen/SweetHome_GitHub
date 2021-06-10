using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;
public class Data : MonoBehaviour
{
    public bool _bSeeSaw;
    Hashtable hash = new Hashtable();
    // Start is called before the first frame update
    void Start()
    {
        _bSeeSaw = true;
        hash.Add("seesawbool",_bSeeSaw);
        PhotonNetwork.CurrentRoom.SetCustomProperties(hash);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
