using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ambulance_UI_Control : MonoBehaviour
{
    // need to have a reference to the player's AmbulanceCD_timer to toggle the UI color
    
    [SerializeField] private GameObject player_obj;
    private PlayerControl player;
    // the actual timer variable referencing player's AmbulanceCD_timer
    private float t;

    private Image Ambulance_img;
    private Image Background_img;
    private Progress_bar progress;

    void Start()
    {
        player = player_obj.GetComponent<PlayerControl>();
        Ambulance_img = transform.Find("Ambulance_state").gameObject.GetComponent<Image>();
        Background_img = gameObject.GetComponent<Image>();

        progress = transform.Find("Ambulance_state").GetComponent<Progress_bar>();
        progress.max = player.AmbulanceCD;
        progress.current = player.AmbulanceCD;
    }


    void Update()
    {
        t = player.AmbulanceCD_timer;
        
        if(t >= 0)
        {
            // set color to grey to express a deactive state
            Ambulance_img.color = new Color(0.19f, 0.19f, 0.19f, 0.85f);
            Background_img.color = new Color(0.19f, 0.19f, 0.19f, 0.6f);
            progress.current = progress.max - t;
        }
        else if(t <= 0)
        {
            // set color back to white to express an active state
            Ambulance_img.color = new Color(1f, 1f, 1f, 0.85f);
            Background_img.color = new Color(1f, 1f, 1f, 0.6f);
        }
    }
}
