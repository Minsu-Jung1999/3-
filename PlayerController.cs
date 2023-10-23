using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float moveSpeed = 0.0f;
    [SerializeField]
    float lookRotationSpeed= 10.0f;
    [SerializeField]
    float moveRotationSpeed = 10.0f;
    [SerializeField]
    GameObject cameraArm;
    [SerializeField]
    float cameraRotationLimitUP=90.0f;
    [SerializeField]
    float cameraRotationLimitDown=90.0f;


    private float xRotation = 0;
    private float yRotation = 0;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        bool isMove = moveInput.magnitude != 0;

        if(isMove)
        {
            Vector3 lookForward = new Vector3(cameraArm.transform.forward.x, 0f, cameraArm.transform.forward.z).normalized;
            Vector3 lookRight = new Vector3(cameraArm.transform.right.x, 0f, cameraArm.transform.right.z).normalized;
            Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;

            //transform.forward = moveDir;
            Rotate(moveDir);
            transform.position += moveDir * Time.deltaTime * moveSpeed;

        }
    }
    private void Rotate(Vector3 moveDirection)
    {
        // 원하는 회전 각도를 계산합니다.
        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);

        // 부드러운 회전을 위해 Slerp를 사용합니다.
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, moveRotationSpeed * Time.deltaTime);
    }

    void Update()
    {
        LookAround();
    }

    

    private void LookAround()
    {
        xRotation += Input.GetAxis("Mouse X");
        yRotation -= Input.GetAxis("Mouse Y");
        yRotation = Mathf.Clamp(yRotation, -cameraRotationLimitDown, cameraRotationLimitUP);

        cameraArm.transform.rotation = Quaternion.Euler(yRotation, xRotation, 0);
    }
}
