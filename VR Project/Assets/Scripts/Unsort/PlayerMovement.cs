using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float moveSpeed = 5;
    public GameObject cameraRig;
    public GameObject centerEye;
    private Vector2 touchPadFingerPosition;
    private Vector2 touchPadCenter;

    void Update()
    {
        Movement();
    }

    void Movement()
    {
        //Set the new center of the touch
        if (OVRInput.GetDown(OVRInput.Touch.PrimaryTouchpad))
            touchPadCenter = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad);
        //Set the center to zero
        else if (OVRInput.GetUp(OVRInput.Touch.PrimaryTouchpad))
            touchPadCenter = Vector2.zero;

        touchPadFingerPosition = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad);
        transform.eulerAngles = new Vector3(0, centerEye.transform.localEulerAngles.y, 0);
        transform.Translate(Vector3.forward * moveSpeed * (touchPadFingerPosition.y - touchPadCenter.y) * Time.deltaTime);
        transform.Translate(Vector3.right * moveSpeed * (touchPadFingerPosition.x - touchPadCenter.x) * Time.deltaTime);
        cameraRig.transform.position = transform.position;
    }
}
