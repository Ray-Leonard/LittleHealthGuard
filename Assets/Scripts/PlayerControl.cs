using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private float walkingSpeed;
    public float AmbulanceCD;
    public float AmbulanceCD_timer = 0.0f;
    [SerializeField] private int score;
    public int highScore;
    [SerializeField] private Text score_text;
    [SerializeField] private Text high_score_text;
    

    private Vector3 moveDir;
    private Rigidbody2D playerRigidBody;
    private Animator playerAnimator;

    // for UI
    [SerializeField] private GameObject prop_state_image;
    [SerializeField] private Sprite mask_spr;
    [SerializeField] private Sprite vaccine_spr;
    [SerializeField] private Sprite empty_spr;
    
    private bool isWalkingUp;
    private bool isWalkingDown;
    private bool isWalkingLeft;
    private bool isWalkingRight;

    public enum Items {mask, vaccine, none };
    public Items current_item = Items.none;

    [SerializeField] AudioSource point_sound;
    [SerializeField] AudioSource snap_sound;

    public int scoreMulp = 1;

    
    // Start is called before the first frame update
    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        //point_sound = GetComponent<AudioSource>();
        score = 0;
        highScore = 0;
    }

    // Update is called once per frame
    void Update()
    {
        updateItemUISprite();
        UpdateAmbulanceCDTimer();
        updateScore();
        updateHighScore();
        Thanos();
    }

    private void FixedUpdate()
    {
        MovePlayer();
        updateAnimation();
    }

    // the ultimate weapon
    private void Thanos()
    {
        if (Input.GetButtonDown("Thanos"))
        {
            snap_sound.PlayOneShot(snap_sound.clip);
            
            // Using thanos costs player 10 points
            score -= 10;
            // Get all NPC1
            List<NPC_control> NPC_1s = new List<NPC_control>();
            foreach(GameObject n in GameObject.FindGameObjectsWithTag("NPC"))
            {
                NPC_control c = n.GetComponent<NPC_control>();
                if(c.type == NPC_control.npc_type.type1)
                {
                    NPC_1s.Add(c);
                }
            }


            // Then destroy half of them
            for(int i = 0; i < NPC_1s.Count / 2; ++i)
            {
                NPC_1s[i].Die();
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // to pick up mask or vaccine
        // change the current_item and UI component as well.
        if (collision.tag.Equals("mask_prop"))
        {
            current_item = Items.mask;
            // change the prop color to grey and disable the collider.
            collision.transform.parent.gameObject.GetComponent<Prop_pickup_respawn>().deactivate("mask");
            
            // mask needs extra scaling
            prop_state_image.GetComponent<RectTransform>().localScale = new Vector3(1.3f, 1, 1);
        }
        else if(collision.tag.Equals("vaccine_prop"))
        {
            current_item = Items.vaccine;
            // change the prop color to grey and disable the collider
            collision.transform.parent.gameObject.GetComponent<Prop_pickup_respawn>().deactivate("vaccine");

            // need to change the scaling back to normal.
            prop_state_image.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        }
    }


    private void updateItemUISprite()
    {
        if(current_item == Items.mask)
        {
            prop_state_image.GetComponent<Image>().sprite = mask_spr;
        }
        else if (current_item == Items.vaccine)
        {
            prop_state_image.GetComponent<Image>().sprite = vaccine_spr;
        }
        else if(current_item == Items.none)
        {
            prop_state_image.GetComponent<Image>().sprite = empty_spr;
        }
        
    }

    public void AddScore(int s)
    {
        score += scoreMulp * s;
        if(s > 0)
            point_sound.PlayOneShot(point_sound.clip);
    }

    private void updateScore()
    {
        score_text.text = score.ToString("D3");
        high_score_text.text = highScore.ToString("D3");
    }

    private void updateHighScore()
    {
        if(score > highScore)
        {
            highScore = score;
        }
    }


    public void ActivateAmbulanceCDTimer()
    {
        AmbulanceCD_timer = AmbulanceCD;
    }

    private void UpdateAmbulanceCDTimer()
    {
        if(AmbulanceCD_timer >= 0)
        {
            AmbulanceCD_timer -= Time.deltaTime;
        }
    }


    private void MovePlayer()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, vertical, 0);
        moveDir = direction.normalized;

        // Update player position
        playerRigidBody.MovePosition(transform.position + 
            moveDir * walkingSpeed * Time.deltaTime);
    }

    private void updateAnimation()
    {
        // walking left
        if (moveDir == Vector3.left)
        {
            isWalkingLeft = true;
        }
        else
        {
            isWalkingLeft = false;
        }
        // walking right
        if (moveDir == Vector3.right)
        {
            isWalkingRight = true;
        }
        else
        {
            isWalkingRight = false;
        }

        // left+up || right+up
        if(moveDir == Vector3.up || 
            moveDir == (Vector3.up + Vector3.left).normalized || 
            moveDir == (Vector3.up + Vector3.right).normalized)
        {
            isWalkingUp = true;
        }
        else
        {
            isWalkingUp = false;
        }

        // left+down || right+down
        if (moveDir == Vector3.down || 
            moveDir == (Vector3.down + Vector3.left).normalized || 
            moveDir == (Vector3.down + Vector3.right).normalized)
        {
            isWalkingDown = true;
        }
        else
        {
            isWalkingDown = false;
        }

        // update animator variable
        playerAnimator.SetBool("walkingUp", isWalkingUp);
        playerAnimator.SetBool("walkingDown", isWalkingDown);
        playerAnimator.SetBool("walkingLeft", isWalkingLeft);
        playerAnimator.SetBool("walkingRight", isWalkingRight);

    }

    public int getHighScore()
    {
        return highScore;
    }

    public int getCurrScore()
    {
        return score;
    }

    public float getWalkingSpeed()
    {
        return walkingSpeed;
    }

    public void setWalkingSpeed(float f)
    {
        walkingSpeed = f;
    }
}
