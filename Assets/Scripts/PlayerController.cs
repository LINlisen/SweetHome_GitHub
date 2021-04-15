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

    float verticalLookRotation;
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

		if (!PV.IsMine)
			return;
		Look();
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
}
