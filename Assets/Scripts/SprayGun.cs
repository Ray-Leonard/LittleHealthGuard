using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprayGun : MonoBehaviour
{
    private ParticleSystem sprayGun;
    private Collider2D m_collider;
    [SerializeField] Transform playerPos;
    private AudioSource spray_sound;
    private bool do_once = false;

    // Start is called before the first frame update
    void Start()
    {
        sprayGun = GetComponent<ParticleSystem>();
        m_collider = GetComponent<BoxCollider2D>();
        spray_sound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        UseSprayGun();
    }

    private void UseSprayGun()
    {
        if (Input.GetButton("FireRight"))
        {
            transform.position = playerPos.position + new Vector3(0.27f, -0.27f, 0);
            transform.rotation = Quaternion.identity;
            sprayGun.Play();
            m_collider.enabled = true;
            if (!do_once)
            {
                spray_sound.PlayOneShot(spray_sound.clip);
                do_once = true;
            }

        }
        else if (Input.GetButton("FireLeft"))
        {
            sprayGun.transform.position = playerPos.position + new Vector3(-0.27f, -0.15f, 0);
            sprayGun.transform.eulerAngles = new Vector3(0.0f, 0.0f, 180.0f);
            sprayGun.Play();
            m_collider.enabled = true;
            if (!do_once)
            {
                spray_sound.PlayOneShot(spray_sound.clip);
                do_once = true;
            }
        }
        else if (Input.GetButton("FireUp"))
        {
            sprayGun.transform.position = playerPos.position + new Vector3(0.05f, 0.1f, 0);
            sprayGun.transform.eulerAngles = new Vector3(0.0f, 0.0f, 90.0f);
            sprayGun.Play();
            m_collider.enabled = true;
            if (!do_once)
            {
                spray_sound.PlayOneShot(spray_sound.clip);
                do_once = true;
            }
        }
        else if (Input.GetButton("FireDown"))
        {
            sprayGun.transform.position = playerPos.position + new Vector3(-0.066f, -0.252f, 0);
            sprayGun.transform.eulerAngles = new Vector3(0.0f, 0.0f, 270.0f);
            sprayGun.Play();
            m_collider.enabled = true;
            if (!do_once)
            {
                spray_sound.PlayOneShot(spray_sound.clip);
                do_once = true;
            }
        }
        else
        {
            sprayGun.Stop();
            sprayGun.Clear();
            m_collider.enabled = false;
            do_once = false;
        }
    }

}
