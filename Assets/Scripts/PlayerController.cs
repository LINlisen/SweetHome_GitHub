﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TouchControlsKit;
using Hashtable = ExitGames.Client.Photon.Hashtable;
public class PlayerController : MonoBehaviour
{
	[SerializeField] GameObject camerHolder;
    [SerializeField] float mouseSensitivity, walkSpeed, smoothTime;
	[SerializeField] Item[] items;
    Animator playerAni;
	int itemIndex;
	int previousItemIndex = -1;
    public GameObject rotation_Wall;
    public GameObject RedDoor;
    public GameObject BlueDoor;
    float verticalLookRotation;
    float rotation;
    bool grounded;
    Vector3 smoothMoveVelocity;
    Vector3 moveAmount;

    bool _bIsDash=false;
    private float dashTime=0.0f;
    private Vector3 directionXOZ;
    public float dashDuration;// 控制冲刺时间
    public float dashSpeed;// 冲刺速度
    // Start is called before the first frame update

    public CharacterController playerController;
    Rigidbody rb;
	PhotonView PV;
    GameObject UpInformation;
    /*Organ*/
    //[SerializeField] private GameObject SeesawSet;

    //[SerializeField] private float Speed = 5;
    //boost speed var
    private float normalSpeed;
    public float boostedSpeed;
    public float speedCooldown;

    private float angle = 20.0f;

    private bool playerOnLeftSeesaw;
    private bool playerOnRightSeesaw;
    public float maxAngle;
    public float minAngle;

    private bool animated;//trueR falseL can be trigger

    //treasure
    [SerializeField] private GameObject treasure;
    

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerController = GetComponent<CharacterController>();
		PV = GetComponent<PhotonView>();
        UpInformation = GameObject.Find("UpInformationCanvas");
        playerAni = GetComponent<Animator>();
        Debug.Log(playerAni.name);

        //organ-treasure animate
        //boxAnim = treasure.GetComponent<Animator>();

    }

    void Start()
    {
        Debug.Log(playerController.name);
        if (PV.IsMine)
        {
			//EquipItem(0); 
        }
        else
        {
			Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(playerController);
            Destroy(rb);
        }
        GameObject.Find("_TCKCanvas").gameObject.transform.GetChild(5).gameObject.SetActive(false);
        /*Orgna*/
        normalSpeed = walkSpeed;

        //seesaw init
        playerOnLeftSeesaw = false;
        playerOnRightSeesaw = false;

        animated = true;
    }
    public void Dash()
    {
        _bIsDash = true;
        playerAni.SetBool("Dash", true);
        //Vector3 i = new Vector3(10f, 0f, 10f);
        //playerController.Move(i);
        Debug.Log("DASH");
        Debug.Log(_bIsDash);
        Debug.Log("DASH");
        //Debug.Log(_bIsDash);
        //Debug.Log(dashTime);
        //Debug.Log(dashDuration);
        directionXOZ.y = 0f;// 只做平面的上下移动和水平移动，不做高度上的上下移动
        directionXOZ = playerController.transform.right;// forward 指向物体当前的前方
    }
    private void Update()
    {
        if (TCKInput.GetAction("dashBtn", EActionEvent.Down))
        {
            Dash();
        }
        if (_bIsDash == true)
        {
            Debug.Log(dashTime);
            Debug.Log(dashDuration);
            if (dashTime <= dashDuration)
            {
                dashTime += Time.deltaTime;
                //Debug.Log(directionXOZ);
                Debug.Log("trueinside");
                //Debug.Log(dashSpeed);
                playerController.Move(-directionXOZ * dashTime * dashSpeed);
            }
            else
            {
                _bIsDash = false;
                dashTime = 0.0f;
                playerAni.SetBool("Dash", false);
            }
            
        }
        if (!PV.IsMine)
			return;
        //Move();
        Vector2 look = TCKInput.GetAxis("Touchpad");
        PlayerRotation(look.x, look.y);


        /*Organ*/
        //Debug.Log(SeesawSet.transform.localRotation.eulerAngles.z);
        //if (playerOnRightSeesaw == true)
        //{
        //    if (SeesawSet.transform.localRotation.eulerAngles.z > maxAngle)
        //    {
                //SeesawSet.transform.RotateAround(SeesawSet.transform.position, SeesawSet.transform.parent.forward, angle*Time.deltaTime);

                //SeesawSet.transform.Rotate(Vector3.forward, angle * Time.deltaTime);
        //    }
        //}
        //if (playerOnLeftSeesaw == true)
        //{
        //    SeesawSet.transform.Rotate(Vector3.forward, -angle * Time.deltaTime);
        //    if (SeesawSet.transform.localRotation.eulerAngles.z == minAngle)
        //    {
        //        playerOnLeftSeesaw = false;
                //SeesawSet.transform.RotateAround(SeesawSet.transform.position, SeesawSet.transform.parent.forward, angle*Time.deltaTime);


        //    }
        //}
    }
    private void PlayerMovement(float horizontal, float vertical)
    {
        //bool grounded = controller.isGrounded;

        Vector3 moveDirection = playerController.transform.forward * horizontal;
        moveDirection -= playerController.transform.right * vertical ;

        //Vector3 moveDirection = rb.transform.forward * vertical;
        //moveDirection += rb.transform.right * horizontal;
        moveDirection.y = -1.0f;



        playerController.Move(moveDirection * Time.fixedDeltaTime * walkSpeed);
        //rb.MovePosition(moveDirection * Time.fixedDeltaTime * 10.0f);


    }

    // PlayerRotation
    public void PlayerRotation(float horizontal, float vertical)
    {
        playerController.transform.Rotate(0f, horizontal * 12f, 0f);
        //rb.transform.Rotate(0f, horizontal * 12f, 0f);
        rotation += vertical * 12f;
        rotation = Mathf.Clamp(rotation, -60f, 60f);
        camerHolder.transform.localEulerAngles = new Vector3(-rotation, camerHolder.transform.localEulerAngles.y, 0f);
    }

    //void EquipItem(int _index)
    //{

    //    if (_index == previousItemIndex)
    //        return;

    //    itemIndex = _index;

    //    items[itemIndex].itemGameObject.SetActive(true);

    //    if (previousItemIndex != -1)
    //    {
    //        items[previousItemIndex].itemGameObject.SetActive(false);
    //    }

    //    previousItemIndex = itemIndex;
    //}


    public void SetGroundedState(bool _grounded)
    {
		grounded = _grounded;
    }

    private void FixedUpdate()
    {
		if (!PV.IsMine)
			return;
		//playerController.MovePosition(playerController.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
        Vector2 move = TCKInput.GetAxis("Joystick"); // NEW func since ver 1.5.5
        if (move.x != 0 || move.y != 0)
        {
            playerAni.SetFloat("Speed",5);
        }
        else
        {
            playerAni.SetFloat("Speed", 0);
            //Debug.Log("not Walk");
        }
        
        PlayerMovement(move.x, move.y);
        /*Dash*/
     
  
    }
    float rotate = 0;

    void OnTriggerStay(Collider other)
    {
        
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //toast
        if (other.gameObject.name == "toast")
        {
            GameObject.Find("_TCKCanvas").gameObject.transform.GetChild(5).gameObject.SetActive(true);
        }

        /*Organ*/
        //booster
        if (other.CompareTag("SpeedBooster"))
        {
            walkSpeed = boostedSpeed;
            StartCoroutine("BoostDuration");
        }
        if (other.CompareTag("SlowDowner"))
        {
            walkSpeed = walkSpeed / 2;
            StartCoroutine("BoostDuration");
        }


        //seesaw set not used
        //Debug.Log(SeesawSet.transform.localRotation.eulerAngles.z);
        //if (other.CompareTag("RSeesaw"))
        //{
           
        //    playerOnLeftSeesaw = true;
        //}
        //if (other.CompareTag("LSeesaw"))
        //{
        //    Debug.Log("l");
        //    playerOnLeftSeesaw = true;
        //}


        //animated seesaw
        if (other.tag == "AnimRSeesaw")
        {

            if (animated == true)
            {
                Debug.Log("touched");
                Animator anim = other.GetComponentInParent<Animator>();
                anim.SetTrigger("moveOC2");
                animated = false;
            }
        }
        if (other.tag == "AnimLSeesaw")
        {
            if (animated == false)
            {
                Animator anim = other.GetComponentInParent<Animator>();
                anim.SetTrigger("moveOC");
                animated = true;
            }
        }

        //treasure
        if (other.tag == "TreasureNormal")
        {
            
            Animator boxAnim = treasure.GetComponent<Animator>();
            boxAnim.SetBool("openbox", true);
        }
        //easter
        if (other.tag == "TreasureDeath")
        {
            Debug.Log("Die");
            Animator diebox = other.GetComponentInParent<Animator>();
            diebox.SetBool("openbox", true);
        }

        /*PotionGet*/
        if (other.gameObject.tag == "Potion")
        {
            Debug.Log("take"+other.gameObject.name);
            Hashtable team = PhotonNetwork.LocalPlayer.CustomProperties;
            PhotonView photonView = PhotonView.Get(UpInformation);
            photonView.RPC("getPoint", RpcTarget.All, (int)team["WhichTeam"]);
            other.GetComponent<RaiseEvent>().getPotion(other.gameObject);
            

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "toast")
        {
            GameObject.Find("_TCKCanvas").gameObject.transform.GetChild(5).gameObject.SetActive(false);
        }

        /*Organ*/
        //seesaw set
        //if (other.CompareTag("RSeesaw"))
        //{
        //    playerOnLeftSeesaw = false;
        //}
        //if (other.CompareTag("LSeesaw"))
        //{
        //    playerOnLeftSeesaw = false;
        //}
    }

    /*Organ*/
    IEnumerator BoostDuration()
    {
        //boost cooldown
        yield return new WaitForSeconds(speedCooldown);
        walkSpeed = normalSpeed;

    }
}




