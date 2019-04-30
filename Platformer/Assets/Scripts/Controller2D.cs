using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Controller2D : MonoBehaviour
{
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
    }

    //step-4
    private void Update()
    {
        UpdateRaycastOrigins();
        CalculateRaySpacing();

        for (int i = 0;i < verticalRayCount; i++)
        {
            Debug.DrawRay(raycastOrigin.botLeft + Vector2.right * verticalRaySpacing * i, Vector2.up * -2, Color.red);
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
