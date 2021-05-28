using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;


public class PlayerManager : MonoBehaviour
{
    PhotonView PV;

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
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "candywalk"), new Vector3 (i,20,0), Quaternion.identity);
    }
}
