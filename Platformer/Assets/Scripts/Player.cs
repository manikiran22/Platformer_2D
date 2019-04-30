using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]

public class Player : MonoBehaviour
{

    Controller2D control;

    // Start is called before the first frame update
    void Start()
    {
        control = GetComponent<Controller2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
