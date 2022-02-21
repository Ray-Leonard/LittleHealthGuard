using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop_pickup_respawn : MonoBehaviour
{
    [SerializeField] private float respawnTime;
    private float timer_mask = 0.0f;
    private float timer_vaccine = 0.0f;

    [SerializeField] AudioSource pickup_sound;

    private GameObject mask;
    private GameObject vaccine;

    private void Start()
    {
        mask = transform.Find("Mask").gameObject;
        vaccine = transform.Find("Vaccine").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (mask.GetComponent<BoxCollider2D>().enabled == false)
        {
            timer_mask += Time.deltaTime;
        }

        if(vaccine.GetComponent<BoxCollider2D>().enabled == false)
        {
            timer_vaccine += Time.deltaTime;
        }

        if(timer_mask > respawnTime)
        {
            timer_mask = 0;
            activate("mask");
        }

        if (timer_vaccine > respawnTime)
        {
            timer_vaccine = 0;
            activate("vaccine");
        }
    }

    public void deactivate(string target)
    {
        pickup_sound.Play();
        if (target == "mask")
        {
            mask.GetComponent<SpriteRenderer>().color = new Color(0.35f, 0.35f, 0.35f, 1);
            mask.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = new Color(0.35f, 0.35f, 0.35f, 1);
            mask.GetComponent<BoxCollider2D>().enabled = false;
        }

        else if(target == "vaccine")
        {
            vaccine.GetComponent<SpriteRenderer>().color = new Color(0.35f, 0.35f, 0.35f, 1);
            vaccine.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = new Color(0.35f, 0.35f, 0.35f, 1);
            vaccine.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    private void activate(string target)
    {
        if (target == "mask")
        {
            mask.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            mask.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = new Color(1,1,1,1);
            mask.GetComponent<BoxCollider2D>().enabled = true;
        }

        else if (target == "vaccine")
        {
            vaccine.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            vaccine.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            vaccine.GetComponent<BoxCollider2D>().enabled = true;
        }
    }
}
