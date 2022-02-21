using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Power : MonoBehaviour
{
    // need a reference to the player controller to access the fields
    private PlayerControl p;
    private SpriteRenderer spr;
    [SerializeField] private float power_last_time;
    private float power_in_time = 0.0f;
    public float PowerCD;
    public float PowerCD_timer = 0.0f;

    private GameObject[] surfaces;

    // backup of player attributes that need to change
    private float normalWalkingSpeed;
    private float normalAmbulanceCD;
    private float normalDisinfectTime;

    // hard coded margins
    private float min_x = -11.39f;
    private float min_y = -7.27f;
    private float max_x = 11.39f;
    private float max_y = 7.27f;

    public bool isPowerOn = false;


    // a list of colors to change
    private Color[] color_list = {
        new Color(0, 0, 1),
        new Color(0, 1, 0),
        new Color(1, 0, 0),
        new Color(0, 1, 1),
        new Color(1, 1, 0),
        new Color(1, 0, 1)

    };

    [SerializeField] AudioSource bgm;
    
    // Start is called before the first frame update
    void Start()
    {
        p = GetComponent<PlayerControl>();
        spr = GetComponent<SpriteRenderer>();
        normalWalkingSpeed = p.getWalkingSpeed();
        normalAmbulanceCD = p.AmbulanceCD;
        surfaces = GameObject.FindGameObjectsWithTag("Surface");
        normalDisinfectTime = surfaces[0].GetComponent<Surface_single>().disinfect_time;
    }

    // Update is called once per frame
    void Update()
    {
        EnterPowerMode();
        CountPowerTime();
        ExitPowerMode();
        UpdatePowerCDTimer();
    }

    // when Q is pressed, enter power mode for power_last_time seconds
    private void EnterPowerMode()
    {
        // only enter the power mode if the power is ready
        if (Input.GetButton("Power") && PowerCD_timer <= 0.0f)
        {
            if (!isPowerOn)
            {
                // reset the CD timer
                PowerCD_timer = PowerCD;
                // boost walking speed
                p.setWalkingSpeed(normalWalkingSpeed * 2);
                // make ambulance cd faster (1/10 of normal)
                p.AmbulanceCD = normalAmbulanceCD / 10;
                p.AmbulanceCD_timer = 0.0f;
                // set the player collider to trigger so it can walk through walls and NPCs
                p.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
                // set disinfect time to 1/10 of normal
                foreach(GameObject s in surfaces)
                {
                    s.GetComponent<Surface_single>().disinfect_time = normalDisinfectTime / 10;
                }

                p.scoreMulp = 2;

                bgm.pitch = 1.5f;
                
                isPowerOn = true;
            }




        }
    }

    private void ExitPowerMode()
    {
        if(power_in_time >= power_last_time && isPowerOn)
        {
            // reset the player attributes

            // reset color to normal
            spr.color = new Color(1,1,1);
            // reset walking speed
            p.setWalkingSpeed(normalWalkingSpeed);
            // reset ambulance CD
            p.AmbulanceCD = normalAmbulanceCD;
            // reset collision
            p.gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
            // reset disinfect time
            foreach (GameObject s in surfaces)
            {
                s.GetComponent<Surface_single>().disinfect_time = normalDisinfectTime;
            }
            bgm.pitch = 1;

            p.scoreMulp = 1;

            // turn power off
            isPowerOn = false;
            // reset power_in_time
            power_in_time = 0;
        }
    }

    private void CountPowerTime()
    {
        if (isPowerOn)
        {
            power_in_time += Time.deltaTime;
            // sprite to change
            spr.color = color_list[Random.Range(0, color_list.Length)];
            // fix player with in map (since player collision is set to trigger)
            MapConstrain();
        }
    }

    private void MapConstrain()
    {
        float new_x = transform.position.x;
        float new_y = transform.position.y;
        if (new_x < min_x)
        {
            new_x = min_x;
        }
        else if (new_x > max_x)
        {
            new_x = max_x;
        }

        if (new_y < min_y)
        {
            new_y = min_y;
        }
        else if (new_y > max_y)
        {
            new_y = max_y;
        }

        transform.position = new Vector3(new_x, new_y, transform.position.z);
    }

    private void UpdatePowerCDTimer()
    {
        if(PowerCD_timer >= 0)
        {
            PowerCD_timer -= Time.deltaTime;
        }
    }
}
