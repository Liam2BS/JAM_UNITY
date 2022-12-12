using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Top-down 3rd person shooter movement.
    //Camera will not affect the player's direction.
    //ZQSD to move the character.
    //Mouse to aim, alter direction and look around.
    //Space to jump.
    //Shift to sprint.
    //Camera will use cinemachine.

    //BASE PARAMETERS.

    //Movement variables.
    public float speed = 6f;
    public float sprintSpeed = 9f;
    private float actuelSpeed;
    private float h; //Horizontal Axis
    private float v; //Vertical Axis
    private Vector3 movementDirection; //Direction to go to

    //Stamina
    public float stamina = 100;
    public float staminaLost = 10;
    public float staminaGain = 5;

    //Rotation variables.
    public float lookAtSpeed = 10f;
    private Vector3 mousePos;
    private Vector3 lookAtDirection; //Direction to look at

    //Components.
    private Camera cam;
    private CharacterController cc;

    void Start()
    {
        cam = Camera.main;
        cc = gameObject.GetComponent<CharacterController>();
    }

    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        mousePos = Input.mousePosition;

        //Movement
        movementDirection = new Vector3(h, 0, v);
        float movementLength = Mathf.Clamp(movementDirection.magnitude, 0f, 1f);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            actuelSpeed = sprintSpeed;
            stamina -= staminaLost * Time.deltaTime;
            if(stamina <= 0) { stamina = 0; }
        }
        else
        {
            actuelSpeed = speed;
            stamina += staminaGain * Time.deltaTime;
            if (stamina >= 100) { stamina = 100; }
        }

        movementDirection = transform.forward * movementLength * Time.deltaTime * actuelSpeed;

        if(!cc.isGrounded)
        {
            movementDirection.y -= 9f * Time.deltaTime;
        }

        //transform.Translate(transform.forward * movementLength * Time.deltaTime * actuelSpeed);
        cc.Move(movementDirection);

        //transform.position += transform.forward * movementLength * Time.deltaTime * actuelSpeed;

        //Rotation : Raycast from the mouse position to return a Vector 3 to look at, which will be our mouse's position.
        Ray ray = cam.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance: 300f))
        {
            //Get the position to look at.
            lookAtDirection = hitInfo.point;
            lookAtDirection.y = transform.position.y;

            //Create a rotation from the previously mentioned point.
            Vector3 dir = lookAtDirection - transform.position;
            Quaternion toRotation = Quaternion.LookRotation(dir);

            //Slerp to actuel rotation to the new one.
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, Time.deltaTime * lookAtSpeed);
        }
    }
}
