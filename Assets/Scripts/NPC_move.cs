using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_move : MonoBehaviour
{
    public float walkingSpeed;
    private Vector3 moveDir;
    private int currDirIndex;
    // initialize the array with four directions
    private Vector3[] directions = new Vector3[] { Vector3.up, Vector3.left, Vector3.down, Vector3.right };
    private float stuckTime = 0;
    private float startMovingTime = 0.0f;
    private float changeDirTimer = 0.0f;
    private bool active = true;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer spr;
 


    // Start is called before the first frame update
    void Start()
    {
        // on initialize, give NPC a random direction
        // probably will add more constrains based on their spawn location.
        currDirIndex = Random.Range(0, 4);
        moveDir = directions[currDirIndex];
        // set the walking speed
        walkingSpeed = 3.5f;

        // initialize rigidbody and animator
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spr = GetComponent<SpriteRenderer>();
    }


    // Update is called once per frame
    void Update()
    {
        UpdateAnimation();
        AutoChangeDirection();
    }

    private void FixedUpdate()
    {
        startMovingTime += Time.fixedDeltaTime;
        if (startMovingTime < 0.5f)
        {
            anim.SetBool("isWalking", false);
            return;
        }
        rb.MovePosition(transform.position + moveDir * walkingSpeed * Time.deltaTime);

    }

    public void FreezePosition_DisableCollision()
    {
        changeDirTimer = 0;
        moveDir = Vector3.zero;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        active = false;
    }


    private void SetOppositeDirection()
    {
        if (currDirIndex >= 0 && currDirIndex <= 1)
        {
            currDirIndex += 2;
        }
        else if (currDirIndex >= 2 && currDirIndex <= 3)
        {
            currDirIndex -= 2;
        }
        moveDir = directions[currDirIndex];
    }

    private void SetRandomDirection()
    {
        int oldDir = currDirIndex;
        do
        {
            currDirIndex = Random.Range(0, 4);
        } while (oldDir == currDirIndex);
        // assign the new direction
        moveDir = directions[currDirIndex];
    }

    private void AutoChangeDirection()
    {
        if (active)
        {
            changeDirTimer += Time.deltaTime;
            if (changeDirTimer > 2)
            {
                changeDirTimer = 0;
                SetRandomDirection();
            }
        }

    }

    // change the NPC moving direction if the NPC collides with margin or surface
    // but won't change direction if collides with the player.
    void OnCollisionEnter2D(Collision2D col)
    {
        stuckTime = 0;
        changeDirTimer = 0;
        // change to a random direction when colliding with surface
        if (col.collider.tag.Equals("Surface"))
        {
            SetRandomDirection();
        }

        // change to opposite direction when colliding with NPC or Margin
        if (col.collider.tag.Equals("Margin") || col.collider.tag.Equals("NPC"))
        {
            SetOppositeDirection();
        }
    }


    private void OnCollisionStay2D(Collision2D col)
    {
        stuckTime += Time.deltaTime;
        if(stuckTime > 1)
        {
            stuckTime = 0;
            SetOppositeDirection();
        }
    }



    private void UpdateAnimation() 
    {
        // update the face direction
        if(moveDir == Vector3.left)
        {
            spr.flipX = true;
        }
        else if(moveDir == Vector3.right)
        {
            spr.flipX = false;
        }

        // update animator condition variable
        if(moveDir == Vector3.zero)
        {
            anim.SetBool("isWalking", false);
        }
        else
        {
            anim.SetBool("isWalking", true);
        }
    }
}
