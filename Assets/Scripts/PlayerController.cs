using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TouchControlsKit;

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
    private bool isLock = true;
    bool grounded;
    Vector3 smoothMoveVelocity;
    Vector3 moveAmount;

    private bool _bIsDash;
    private float dashTime=0;
    private Vector3 directionXOZ;
    public float dashDuration;// 控制冲刺时间
    public float dashSpeed;// 冲刺速度
    // Start is called before the first frame update

    public CharacterController playerController;
    Rigidbody rb;
	PhotonView PV;
 
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerController = GetComponent<CharacterController>();
		PV = GetComponent<PhotonView>();
        playerAni = GetComponent<Animator>();
        Debug.Log(playerAni.name);

    }

    void Start()
    {
        _bIsDash = false;
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
    }
    public void Dash()
    {
        _bIsDash = true;
        //Vector3 i = new Vector3(10f, 0f, 10f);
        //playerController.Move(i);
        Debug.Log("dash");
        Debug.Log(_bIsDash);
        Debug.Log(dashTime);
        Debug.Log(dashDuration);
        directionXOZ.y = 0f;// 只做平面的上下移动和水平移动，不做高度上的上下移动
        directionXOZ = playerController.transform.right;// forward 指向物体当前的前方
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isLock = false;
        }
        if (Input.GetMouseButtonUp(1))
        {
            isLock = true;
        }

        if (!PV.IsMine)
			return;
        //      if (!isLock)
        //      {
        //          Look();
        //      }

        //Move();
        Vector2 look = TCKInput.GetAxis("Touchpad");
        PlayerRotation(look.x, look.y);
    //    for (int i = 0; i < items.Length; i++)
    //    {
    //        if (Input.GetKeyDown((i + 1).ToString()))
    //        {
				//EquipItem(i);
				//break;
    //        }
    //    }
        if (_bIsDash)
        {
            if (dashTime <= dashDuration)
            {
                dashTime += Time.deltaTime;
                Debug.Log(directionXOZ);
                Debug.Log(dashTime);
                Debug.Log(dashSpeed);
                playerController.Move(directionXOZ * dashTime * dashSpeed);
            }
            else
            {
                _bIsDash = false;
                dashTime = 0f;
            }
        }
    
    }
    private void PlayerMovement(float horizontal, float vertical)
    {
        //bool grounded = controller.isGrounded;

        Vector3 moveDirection = playerController.transform.forward * horizontal;
        moveDirection -= playerController.transform.right * vertical ;

        //Vector3 moveDirection = rb.transform.forward * vertical;
        //moveDirection += rb.transform.right * horizontal;
        moveDirection.y = -1.0f;



        playerController.Move(moveDirection * Time.fixedDeltaTime * 10.0f);
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
            playerAni.SetBool("Walk",true);
            Debug.Log(playerAni.GetBool("Walk"));
            //Debug.Log(move.x);
            //Debug.Log(move.y);
        }
        else
        {
            playerAni.SetBool("Walk", false);
            //Debug.Log("not Walk");
        }
        PlayerMovement(move.x, move.y);
        /*Dash*/
        
    }
    float rotate = 0;

    void OnTriggerStay(Collider other)
    {
        /*Rotation_Wall_Setting*/
        if (other.gameObject.name == "RotationDoorR")
        {
            Debug.Log(other.gameObject.name);
            rotate -= 2;
           
            
            other.gameObject.transform.rotation = Quaternion.Euler(0f, rotate, 0f);
        }
        if (other.gameObject.name == "RotationDoorL")
        {
            Debug.Log(other.gameObject.name);
            rotate += 2;
            other.gameObject.transform.rotation = Quaternion.Euler(0f, rotate, 0f);
        }
        if (other.gameObject.name == "RotationDoor")
        {
            Debug.Log(other.gameObject.name);
           
            other.gameObject.transform.rotation = Quaternion.Euler(0f, rotate, 0f);
        }
        /*Color_Door_Setting*/
        if (other.gameObject.name == "BlueDoor")//only PlayerId==odd can pass
        {

            if (this.GetComponent<PhotonView>().ViewID % 2 == 0)
            {
                // other.gameObject.SetActive(true);
                
                other.gameObject.transform.GetChild(3).gameObject.SetActive(true);
            }
            else
            {
                //other.gameObject.SetActive(false);
                other.gameObject.transform.GetChild(3).gameObject.SetActive(false);
            }
        }
        if (other.gameObject.name == "RedDoor")//only PlayerId==even can pass
        {

            if (this.GetComponent<PhotonView>().ViewID % 2 == 0)
            {
                //other.gameObject.SetActive(false);
                
                other.gameObject.transform.GetChild(3).gameObject.SetActive(false);
            }
            else
            {
                //other.gameObject.SetActive(true);
                other.gameObject.transform.GetChild(3).gameObject.SetActive(true);
            }
        }
        if (other.gameObject.name == "Cookie3_cell" || other.gameObject.name == "Cookie3_cell_001" || other.gameObject.name == "Cookie3_cell_002" || other.gameObject.name == "Cookie3_cell_003" || other.gameObject.name == "Cookie3_cell_004")
        {
            this.transform.position += new Vector3(0, 10f * Time.deltaTime * 50.0f, 0);
        }

        rotation_Wall.transform.rotation = Quaternion.Euler(0f, rotate, 0f);

        /*PotionGet*/
        if (other.gameObject.name == "Potion(Clone)")
        {
            GameObject.Find("TimeController").SendMessage("AddPotions");
            other.GetComponent<RaiseEvent>().getPotion();
            //Destroy(other.gameObject);
            //Debug.Log("get");
        }

    }
}


