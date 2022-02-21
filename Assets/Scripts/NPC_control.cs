using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_control : MonoBehaviour
{
    public enum npc_type { type1, type2, type3, type4, type5 };
    public npc_type type;
    public bool hasMask;
    public bool hasVaccine;
    public int life;

    // object references
    [SerializeField] GameObject ambulance;
    [SerializeField] GameObject spawner;
    private Animator anim;
    private PlayerControl player;

    private bool do_once = false;
    private bool do_once_mote = false;
    
    // Start is called before the first frame update
    void Start()
    {
        // assign initial life to NPC1-4
        // NPC 5 wont run out of life!
        switch (type)
        {
            case npc_type.type1:
                life = 10;
                break;
            case npc_type.type2:
                life = 4;
                break;
            case npc_type.type3:
                life = 4;
                break;
            case npc_type.type4:
                life = 2;
                break;
            case npc_type.type5:
                life = 999;
                break;
        }
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerControl>();
    }

    public void setPlayerReference(PlayerControl p)
    {
        player = p;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Promote();
        Demote();
    }

    private void Promote()
    {
        // when hasMask and hasVaccine both equal to true and npc type = 2 or 3 or 4, promote to type 1
        if(hasMask && hasVaccine && (type == npc_type.type2 || type == npc_type.type3 || type == npc_type.type4))
        {
            if (!do_once_mote)
            {
                // instantiate a new NPC_1 object and delete the current NPC
                spawner.GetComponent<NPC_Spawn>().SpawnNPC_1_AtLocation(transform);
                Destroy(gameObject);
                do_once_mote = true;
            }
        }
    }

    private void Demote()
    {
        // when run out of life, npc 1-3 will turn to npc 5, but npc4 will die!
        if (life <= 0)
        {
            if(type == npc_type.type1 || type == npc_type.type2 || type == npc_type.type3)
            {
                if (!do_once_mote)
                {
                    do_once_mote = true;
                    // instantiate a new NPC_5 object and delete the current NPC
                    if(Random.Range(0,10) >= 5)
                    {
                        spawner.GetComponent<NPC_Spawn>().SpawnNPC_4_AtLocation(transform);
                    }else
                    {
                        spawner.GetComponent<NPC_Spawn>().SpawnNPC_5_AtLocation(transform);
                    }

                    // substract point from player
                    AddPoint(-3, player);
                    Destroy(gameObject);
                }
            }
            else if(type == npc_type.type4)
            {
                if (!do_once_mote)
                {
                    do_once_mote = true;
                    // substract point
                    AddPoint(-3, player);
                    // die!
                    Die();

                }

            }
        }
    }

    public void Die()
    {
        // freeze position
        gameObject.GetComponent<NPC_move>().FreezePosition_DisableCollision();
        // play die animation
        anim.Play("NPC_die");
    }

    // add point to player
    private void AddPoint(int s, PlayerControl p)
    {
        p.AddScore(s);
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        
        if (collision.tag.Equals("Player") && Input.GetButton("Useprop"))
        {
            if (!do_once)
            {
                // get a reference of player
                PlayerControl p = collision.gameObject.GetComponent<PlayerControl>();
                // give mask to NPC(only restricted to NPC_type == 3, 4)
                if (!hasMask && p.current_item == PlayerControl.Items.mask && (type == npc_type.type3 || type == npc_type.type4))
                {
                    // indicate that NPC has mask now
                    hasMask = true;
                    // turn on the mask icon for NPC
                    transform.Find("Mask").gameObject.SetActive(true);

                    // set the player's item to none (mask is used up, need to pick up new one)
                    p.current_item = PlayerControl.Items.none;

                    AddPoint(1, p);
                }

                if (!hasVaccine && p.current_item == PlayerControl.Items.vaccine && (type == npc_type.type2 || type == npc_type.type3 || type == npc_type.type4))
                {
                    // indicate that NPC has mask now
                    hasVaccine = true;
                    // turn on the mask icon for NPC
                    transform.Find("Vaccine").gameObject.SetActive(true);

                    // set the player's item to none (vaccine is used up, need to pick up new one)
                    p.current_item = PlayerControl.Items.none;

                    AddPoint(1, p);
                }

                do_once = true;
            }

        }



        // if npc_type == 5, player can call ambulance to and send it to isolation when near it
        if(collision.tag.Equals("Player") && type == npc_type.type5 && Input.GetButton("Phonecall") )
        {
            if (!do_once)
            {
                // set the CD of calling ambulance
                PlayerControl p = collision.gameObject.GetComponent<PlayerControl>();
                if(p.AmbulanceCD_timer <= 0.0f)
                {
                    p.ActivateAmbulanceCDTimer();
                    AddPoint(1, p);
                    CallAmbulance();
                    do_once = true;
                }
            }

        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        do_once = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //do_once = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // deduct life if in contact with npc 5 (self should not be npc 5)
        if(collision.gameObject.tag.Equals("NPC") && 
            collision.gameObject.GetComponent<NPC_control>().type == npc_type.type5 && 
            type != npc_type.type5)
        {
            life--;
        }
    }


    private void CallAmbulance()
    {
        // first freeze the position and disable collision of the npc
        gameObject.GetComponent<NPC_move>().FreezePosition_DisableCollision();
        // then instantiate an ambulance from the right of the screen
        GameObject ambulanceInstance = Instantiate(ambulance, new Vector3(15, transform.position.y, 0), Quaternion.identity);
        ambulanceInstance.GetComponent<AmbulanceControl>().TargetPos = transform;
    }

}
