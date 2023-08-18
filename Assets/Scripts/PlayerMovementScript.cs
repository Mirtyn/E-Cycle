using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovementScript : MonoBehaviour
{
    private float speed = 8f;
    private float rotateSpeed = 90f;
    private float camRotateSpeedMultiplier = 2.5f;
    //private float zoomSpeed = 12f;
    private Vector3 moveVector3 = Vector3.zero;
    private Vector2 rotateVector2 = Vector2.zero;
    [SerializeField] private GameObject mainCamera;

    private void Update()
    {
        //if (mainCamera.transform.rotation.z != 0f)
        //{
        //    mainCamera.transform.eulerAngles = new Vector3(mainCamera.transform.eulerAngles.x, mainCamera.transform.eulerAngles.y, 0f);
        //}

        if (Input.GetKey(KeyCode.Z))
        {
            moveVector3 += this.transform.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveVector3 += -this.transform.forward;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveVector3 += this.transform.right;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            moveVector3 += -this.transform.right;
        }
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.LeftControl))
        {
            if (Input.GetKey(KeyCode.Space))
            {
                moveVector3.y += 1;
            }
            if (Input.GetKey(KeyCode.LeftControl))
            {
                moveVector3.y += -1;
            }
        }
        else if (Input.mouseScrollDelta.y != 0f)
        {
            moveVector3.y = Input.mouseScrollDelta.y;
        }


        if (Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.A))
        {
            if (Input.GetKey(KeyCode.E))
            {
                rotateVector2.y += 1;
            }
            if (Input.GetKey(KeyCode.A))
            {
                rotateVector2.y += -1;
            }
        }
        else if (Input.GetMouseButton(2) && Input.GetAxis("Mouse X") != 0)
        {
            rotateVector2.y += Input.GetAxis("Mouse X") * camRotateSpeedMultiplier;
        }
        if (Input.GetMouseButton(2) && Input.GetAxis("Mouse Y") != 0)
        {
            rotateVector2.x += -Input.GetAxis("Mouse Y") * camRotateSpeedMultiplier;
        }



        float heightMultiplier = this.transform.position.y / 3f;

        if (heightMultiplier < 2.5f)
        {
            heightMultiplier = 2.5f;
        }
        else if (heightMultiplier > 15f)
        {
            heightMultiplier = 15f;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = 13f;
        }
        else
        {
            speed = 8f;
        }

        moveVector3 = moveVector3.normalized;

        this.transform.position += heightMultiplier * speed * Time.deltaTime * moveVector3;

        moveVector3 = Vector3.zero;



        Vector3 rotateVector3 = new Vector3(rotateVector2.x, rotateVector2.y, 0f);
        
        mainCamera.transform.eulerAngles += rotateSpeed * Time.deltaTime * rotateVector3;

        this.transform.rotation = Quaternion.Euler(new Vector3(this.transform.rotation.x, mainCamera.transform.eulerAngles.y, this.transform.rotation.z));

        rotateVector2 = Vector2.zero;



        //Vector3 targetRotation = new Vector3(0f, mainCamera.transform.eulerAngles.y, 0f);

        //transform.Rotate(targetRotation.x, targetRotation.y, targetRotation.z);



        ////rotateVector2 = rotateVector2.normalized;

        //Vector3 rotateVector3 = mainCamera.transform.rotation.eulerAngles + (new Vector3(rotateVector2.x, rotateVector2.y, 0) * Time.deltaTime * rotateSpeed);

        //mainCamera.transform.rotation.SetFromToRotation(mainCamera.transform.rotation.eulerAngles, rotateVector3);
    }
}
