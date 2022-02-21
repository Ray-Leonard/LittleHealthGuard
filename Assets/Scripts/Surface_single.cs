using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surface_single : MonoBehaviour
{
    private ParticleSystem effect;
    public bool isInfected = false;
    public float time_hasBeenInfected = 0;

    public float disinfect_timer = 0.0f;
    public float disinfect_time;
    [SerializeField] GameObject progress_bar_obj;
    [SerializeField] public float unintended_threshold;
    private Transform Canvas_scene;
    private Progress_bar progress_bar;
    private bool do_once = false;

    private PlayerControl player;

    // Start is called before the first frame update
    void Start()
    {
        effect = GetComponent<ParticleSystem>();
        effect.Stop();
        player = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerControl>();
        Canvas_scene = GameObject.FindGameObjectsWithTag("SceneUI")[0].transform;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateInfectionTime();
        DeductPoint_UnintendedSurface();
    }

    private void FixedUpdate()
    {
        UpdateEffect();
    }

    private void DeductPoint_UnintendedSurface()
    {
        if(time_hasBeenInfected >= unintended_threshold)
        {
            AddPoint(-1, player);
            time_hasBeenInfected = 0;
        }
    }


    private void UpdateInfectionTime()
    {
        if (isInfected)
        {
            time_hasBeenInfected += Time.deltaTime;
        }
        if (!isInfected)
        {
            time_hasBeenInfected = 0;
        }
    }

    private void UpdateEffect()
    {
        if (isInfected)
        {
            effect.Play();
        }
        else if (!isInfected)
        {
            effect.Stop();
            effect.Clear();
        }
    }

    // NPC collides with surface -- infect npc or surface get infected!
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if(collision.gameObject.tag == "NPC")
        {
            NPC_control npc = collision.gameObject.GetComponent<NPC_control>();
            // when contacted with type 5 NPC, it gets infected.
            if(npc.type == NPC_control.npc_type.type5)
            {
                isInfected = true;
            }
            
            if(npc.type != NPC_control.npc_type.type5 && isInfected)
            {
                npc.life--;
            }
        }
    }



    // +++++++++++++++++disinfect surface++++++++++++++++++++
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Spray") && isInfected)
        {
            disinfect_timer = 0;
            do_once = false;
            // instantiate a progress bar at top of the surface
            progress_bar = Instantiate(progress_bar_obj, transform.position + new Vector3(0, 0.7f, 0), Quaternion.identity, Canvas_scene).GetComponent<Progress_bar>();
            progress_bar.max = disinfect_time;
        }

    }

    private void AddPoint(int s, PlayerControl p)
    {
        p.AddScore(s);
    }

    // disinfect surface
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag.Equals("Spray") && isInfected)
        {
            if (disinfect_timer < disinfect_time)
            {
                // update timer
                disinfect_timer += Time.fixedDeltaTime;
                // update progress bar
                progress_bar.current = disinfect_timer;
            }
            else if (disinfect_timer >= disinfect_time)
            {
                if (!do_once)
                {
                    disinfect_timer = 0;
                    // successfully infected, set the surface status to false
                    isInfected = false;
                    // add point to player
                    AddPoint(2, player);
                    // destroy the progress bar
                    if(progress_bar != null)
                        Destroy(progress_bar.gameObject);
                    do_once = true;
                }

            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.Equals("Spray") && isInfected)
        {
            if(progress_bar != null)
                Destroy(progress_bar.gameObject);
        }
    }


}
