using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float jumpPower;
    private Vector2 curMovementInput;
    public LayerMask groundLayerMask;

    [Header("Look")]
    public Transform cameraContainer;
    public float minXLook;
    public float maxXLook;
    private float camCurXRot;
    public float lookSensitivity;   //민감도
    private Vector2 mouseDelta;
    public bool canLook = true;

    public Action inventory;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        if (canLook)
        {
            CameraLook();
        }
    }


    void Move()
    {
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= moveSpeed;
        dir.y = _rigidbody.velocity.y;

        _rigidbody.velocity = dir;
    }

    void CameraLook() 
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        { 
            curMovementInput = Vector2.zero;
        }
        
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();

    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGrounded())
        { 
            _rigidbody.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
        }
    }

    bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down)
        };

        for (int i = 0; i < rays.Length; i++)
        {
            Debug.Log(rays[i].direction);
            if (Physics.Raycast(rays[i], 0.1f, groundLayerMask))
            { 
                return true;
            }
        }
        return false;
    }

    public void OnInventory(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            inventory?.Invoke();
            ToggleCursor();
        }
    }

    void ToggleCursor()
    {
        //커서락모드가 켜져있으면 true
        bool toggle = Cursor.lockState == CursorLockMode.Locked; 
        //커서락모드가 켜져있으면 꺼주고 커서락모드가 꺼져있으면 다시 켜준다 (true라면 꺼주고 false라면 켜준다)
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        //toggle이 true라면 커서락모드를 꺼놓도록했다. 이때는 마우스를 볼 수 있기때문에 주위를 볼 수 없도록 설정해준다.
        canLook = !toggle;
    }
}
