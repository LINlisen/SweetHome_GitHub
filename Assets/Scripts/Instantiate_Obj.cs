using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instantiate_Obj : MonoBehaviour
{
    public GameObject[] Potions;
    public Transform[] Points;
    public float Ins_Time = 1;
    // Start is called before the first frame update
    void Ins_objs(int size)
    {
        //int Random_Objects = Random.Range(0, Potions.Length);

        //int Random_Points = Random.Range(0, Points.Length);

        Instantiate(Potions[0], Points[size].transform.position, Points[size].transform.rotation);
    }
    void Start()
    {
        for (int i = 0; i < Points.Length; i++)
        {
            Ins_objs(i);
        }
    }
    
    // Update is called once per frame
    void Update()
    {
       
    }
}
