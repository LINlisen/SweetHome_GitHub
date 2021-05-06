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

    CharacterController rb;

	PhotonView PV;

    void Awake()
    {
        rb = GetComponent<CharacterController>();
		PV = GetComponent<PhotonView>();
	}

    void Start()
    {
        if (PV.IsMine)
        {
			EquipItem(0); 
        }
        else
        {
			Destroy(GetComponentInChildren<Camera>().gameObject);
			Destroy(rb);
		}
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
        for (int i = 0; i < items.Length; i++)
        {
            if (Input.GetKeyDown((i + 1).ToString()))
            {
				EquipItem(i);
				break;
            }
        }
	}

	//void Move()
	//{

	//	Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

	//	moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * walkSpeed, ref smoothMoveVelocity, smoothTime);
	//}

 //   void Look()
 //   {
	//	transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensitivity);

	//	verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
	//	verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

	//	camerHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;
	//}
    private void PlayerMovement(float horizontal, float vertical)
    {
        //bool grounded = controller.isGrounded;

        Vector3 moveDirection =rb.transform.forward * vertical;
        moveDirection += rb.transform.right * horizontal;

        moveDirection.y = -10f;

        //if (jump)
        //{
        //    jump = false;
        //    moveDirection.y = 25f;
        //    isPorjectileCube = !isPorjectileCube;
        //}

        //if (grounded)
        //    moveDirection *= 7f;

        rb.Move(moveDirection * Time.fixedDeltaTime*10.0f);

        //if (!prevGrounded && grounded)
        //    moveDirection.y = 0f;

        //prevGrounded = grounded;
    }

    // PlayerRotation
    public void PlayerRotation(float horizontal, float vertical)
    {
        rb.transform.Rotate(0f, horizontal * 12f, 0f);
        rotation += vertical * 12f;
        rotation = Mathf.Clamp(rotation, -60f, 60f);
        camerHolder.transform.localEulerAngles = new Vector3(-rotation, camerHolder.transform.localEulerAngles.y, 0f);
    }

    void EquipItem(int _index)
    {

		if (_index == previousItemIndex)
			return;

		itemIndex = _index;

		items[itemIndex].itemGameObject.SetActive(true);

		if(previousItemIndex != -1)
        {
			items[previousItemIndex].itemGameObject.SetActive(false);
        }

		previousItemIndex = itemIndex;
    }

	public void SetGroundedState(bool _grounded)
    {
		grounded = _grounded;
    }

    private void FixedUpdate()
    {
		if (!PV.IsMine)
			return;
		//rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
        Vector2 move = TCKInput.GetAxis("Joystick"); // NEW func since ver 1.5.5
        PlayerMovement(move.x, move.y);
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



    }
}
