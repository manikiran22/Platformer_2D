using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]

public class Player : MonoBehaviour
{
    public float jumpHeight = 4;
    public float jumpApex = 0.4f; //time it takes for the player to reach the max height point while jumping

    float accTimeAir = .2f;
    float accTimeGround = .1f;//these determine the horizontal movement while in air and while in ground

    float moveSpeed = 10;
    float gravity;
    float jumpVelocity;

    Vector3 velocity;

    float velocityXSmoothing;

    Controller2D control;

    // Start is called before the first frame update
    void Start()
    {
        control = GetComponent<Controller2D>();

        gravity = -(2 * jumpHeight) / Mathf.Pow(jumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * jumpApex;

    }

    //so since gravity and jumpvelocity are not so confident values we are determing jumpheight
    //and jumpapex to convert gravity and jumpvelocity in terms of the jumhei & jumapx
    //we need two formulas 1. delta movement(calculation for gravity) 2.final velocity (calculation for jumpvelocity) 

        /*
         * deltaMovement = initialvelocity * time + (acc * time^2 / 2)
         * we have to write it interms of gravity
         * acc = gravity
         * time = jumpapex
         * deltamovement = jumpheight
         * since initial velocity = 0.
         * gravity = (2 * jumpheight / jumpapex^2)
         */

        /*
         * calculating for final velocity
         * finalvel = initialvel + acc * time
         * since initialvel will be 0
         * jumpvel = gravity * jumpapex
         */

    // Update is called once per frame
    void Update()
    {
        //
        if (control.collisions.above || control.collisions.below)
        {
            velocity.y = 0;
        }
        
        //
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        //
        if (Input.GetKeyDown(KeyCode.Space) && control.collisions.below)
        {
            velocity.y = jumpVelocity;
        }

        //this is to have smooth left and right shifts while moving
        float targetVelocityX = input.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, control.collisions.below ? accTimeGround : accTimeAir);

        //
        velocity.y += gravity * Time.deltaTime;
        control.Move(velocity * Time.deltaTime);
    }
}
