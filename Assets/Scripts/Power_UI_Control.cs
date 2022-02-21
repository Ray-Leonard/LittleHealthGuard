using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Power_UI_Control : MonoBehaviour
{
    [SerializeField] private GameObject player_obj;
    private Power pow;
    private float t;

    private Image Star_img;
    private Image Background_img;
    private Progress_bar progress;
    
    // Start is called before the first frame update
    void Start()
    {
        pow = player_obj.GetComponent<Power>();
        Star_img = transform.Find("Power_state").gameObject.GetComponent<Image>();
        Background_img = gameObject.GetComponent<Image>();

        progress = transform.Find("Power_state").GetComponent<Progress_bar>();
        progress.max = pow.PowerCD;
        progress.current = pow.PowerCD;
    }

    // Update is called once per frame
    void Update()
    {
        t = pow.PowerCD_timer;

        if(t >= 0)
        {
            // set color to grey to express a deactive state
            Star_img.color = new Color(0.19f, 0.19f, 0.19f, 0.85f);
            Background_img.color = new Color(0.19f, 0.19f, 0.19f, 0.6f);
            progress.current = progress.max - t;
        }
        else if (t <= 0)
        {
            // set color back to white to express an active state
            Star_img.color = new Color(1f, 1f, 1f, 0.85f);
            Background_img.color = new Color(1f, 1f, 1f, 0.6f);
        }
    }
}
