using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbulanceControl : MonoBehaviour
{
    public Transform TargetPos;
    private float timer = 0.0f;
    private AudioSource amb_sound;
    
    // Start is called before the first frame update
    void Start()
    {
        amb_sound = GetComponent<AudioSource>();
        amb_sound.PlayOneShot(amb_sound.clip);
    }

    // Update is called once per frame
    void Update()
    {
        if(TargetPos == null)
        {
            Destroy(gameObject);
            return;
        }
        // move to NPC
        if((transform.position.x - TargetPos.position.x) > 0.1)
        {
            transform.position -= new Vector3(15 * Time.deltaTime, 0, 0);
            return;
        }

        // wait for 1 second
        if(timer < 1.0f)
        {
            timer += Time.deltaTime;
            return;
        }

        // deactivate NPC
        TargetPos.gameObject.SetActive(false);

        // move out the scene
        if(transform.position.x > -15)
        {
            transform.position -= new Vector3(20 * Time.deltaTime, 0, 0);
            return;
        }

        // finally destroy self
        Destroy(gameObject);
        Destroy(TargetPos.gameObject);




    }
}
