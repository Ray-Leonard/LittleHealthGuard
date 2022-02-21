using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{

    [SerializeField] private Transform target;

    private float min_x;
    private float min_y;
    private float max_x;
    private float max_y;

    void Start()
    {
        // initialize camera position constrains
        min_x = min_y = -2.95f;
        max_x = max_y = 2.95f;
    }


    // Update is called once per frame
    void Update()
    {
        if(target != null)
        {
            // player position
            float target_x = target.position.x;
            float target_y = target.position.y;

            // camera new position
            float new_x = target_x;
            float new_y = target_y;
            if(new_x < min_x)
            {
                new_x = min_x;
            }
            else if(new_x > max_x)
            {
                new_x = max_x;
            }

            if(new_y < min_y)
            {
                new_y = min_y;
            }
            else if(new_y > max_y)
            {
                new_y = max_y;
            }
            
            transform.position = new Vector3(new_x, new_y, transform.position.z);
        }
    }
}
