using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

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
    private bool isLock = true;
    bool grounded;
    Vector3 smoothMoveVelocity;
    Vector3 moveAmount;
    
    Rigidbody rb;

	PhotonView PV;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
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
        if (!isLock)
        {
            Look();
        }
		
		Move();

        for (int i = 0; i < items.Length; i++)
        {
            if (Input.GetKeyDown((i + 1).ToString()))
            {
				EquipItem(i);
				break;
            }
        }
	}

	void Move()
	{

		Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

		moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * walkSpeed, ref smoothMoveVelocity, smoothTime);
	}

    void Look()
    {
		transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensitivity);

		verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
		verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

		camerHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;
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
		rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
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
                this.gameObject.GetComponent<SpriteRenderer>().color = new Color(123, 250, 111, 100);
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
                this.gameObject.GetComponent<MeshRenderer>().materials = Color.red;
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
