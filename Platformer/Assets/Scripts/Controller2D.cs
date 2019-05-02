using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Controller2D : MonoBehaviour
{
    public LayerMask collisionMask;

    float skinWidth = 0.15f;
    RaycastOrigins raycastOrigin;
    public int horizontalRayCount;
    public int verticalRayCount;
    float horizontalRaySpacing;
    float verticalRaySpacing;


    BoxCollider2D collide;

     void Start()
    {
        collide = GetComponent<BoxCollider2D>();
        CalculateRaySpacing();
    }
 
    //step-4 check for the reference below for better understanding in the player script
    public void Move(Vector3 velocity)
    {
        UpdateRaycastOrigins();

        if (velocity.x != 0) {
            HorizontalCollisions(ref velocity);
                }

        if (velocity.y != 0)
        {
            VerticalCollision(ref velocity);
        }
        transform.Translate(velocity);
    }

    //step-5 since we are not using rigidbody we are trying to calculate stuff using rays
    //so when the ray detects an obstacle using collision mask as it gets near to the object a condition is return in such a way that the velocity is reduced accordingly
    //ray origin - if falling down then bot if we are jumping up then topleft
    //rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x); we did + velocity.x cause we have to check for vertical collision while moving left and right as well.
    void VerticalCollision(ref Vector3 velocity)
    {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;

        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigin.botLeft : raycastOrigin.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

            //Debug.DrawRay(raycastOrigin.botLeft + Vector2.right * verticalRaySpacing * i, Vector2.up * -2, Color.red);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

            if (hit)
            {
                velocity.y = (hit.distance - skinWidth) * directionY;
                Debug.Log(hit.distance);
                rayLength = hit.distance;
            }            
        }

    }


    //Step-6 same as the vertical collision checking collisions for x axis

    void HorizontalCollisions(ref Vector3 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + skinWidth;

        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigin.botLeft : raycastOrigin.botRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            //Debug.DrawRay(raycastOrigin.botLeft + Vector2.right * verticalRaySpacing * i, Vector2.up * -2, Color.red);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

            if (hit)
            {
                velocity.x = (hit.distance - skinWidth) * directionX;
                Debug.Log(hit.distance);
                rayLength = hit.distance;
            }
        }

    }

    //Step-2 creating bounds and assigning bounds wrt to each corner of the box.
    void UpdateRaycastOrigins()
    {
        Bounds bound = collide.bounds;
        bound.Expand(skinWidth * -1);

        raycastOrigin.botLeft = new Vector2(bound.min.x, bound.min.y);
        raycastOrigin.botRight = new Vector2(bound.max.x, bound.min.y);
        raycastOrigin.topLeft = new Vector2(bound.min.x, bound.max.y);
        raycastOrigin.topRight = new Vector2(bound.max.x, bound.max.y);

    }

    //step-3 calculate the raycount and rayspacing for the bounds
    void CalculateRaySpacing()
    {
        Bounds bound = collide.bounds;
        bound.Expand(skinWidth * -1);

        //making sure horizontal / veritical ray count is >= 2 since we atleast rays from each edge
        //so we clamp the ray counts between 2 and maxvalue clamp(max,min,upperboundvalue)
        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        horizontalRaySpacing = bound.size.x / (horizontalRayCount - 1);
        verticalRaySpacing = bound.size.y / (verticalRayCount - 1);


    }


    //step-1 creating the ray origins that come out of the player.
    struct RaycastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 botLeft, botRight;
    }
}
