using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using Hashtable = ExitGames.Client.Photon.Hashtable;


public class PlayerManager : MonoBehaviour
{
    PhotonView PV;
    Hashtable team = PhotonNetwork.LocalPlayer.CustomProperties;
    void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    void Start()
    {
        if (PV.IsMine)
        {
            CreateController();
        }
    }

    void CreateController()
    {
        int i = UnityEngine.Random.Range(0, 50);
        if ((int)team["WhichTeam"] == 1)
        {
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "candywalk"), new Vector3(i, 20, 0), Quaternion.identity);
        }
        else 
        {
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "candywalkblue"), new Vector3(i, 20, 0), Quaternion.identity);
        }
    }
}
