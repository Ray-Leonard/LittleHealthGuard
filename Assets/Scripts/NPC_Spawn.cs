using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// also serves as level difficulty controller
public class NPC_Spawn : MonoBehaviour
{
    // create a list of dictionaries to store four spawn ranges (top, bottom, left, right)
    private Dictionary<string, float>[] range = new Dictionary<string, float>[]
    {
        new Dictionary<string, float>(),
        new Dictionary<string, float>(),
        new Dictionary<string, float>(),
        new Dictionary<string, float>()
    };

    // an array to keep reference to five NPC prefabs
    [SerializeField] GameObject[] NPC_prefab;

    // spawn effect
    [SerializeField] GameObject spawnEffect;

    // UI text to record difficulty level
    [SerializeField] Text difficulty_text;

    // need a reference to player to get the current score.
    private PlayerControl player;

    [SerializeField] private AudioSource spawn_sound;

    // other meta data about the level difficulty
    public float spawnTime;
    public int maxNPC;
    private int currNPC_count;
    private float burst_interval = 100;  // give it a initial interval, subsequent burst shall be in range 60-120
    private float burst_timer = 0;
    private int level = 1;
    private int prev_level = 1;


    // Start is called before the first frame update
    void Awake()
    {
        // add min_x, max_x, min_y, max_y in each dictionary
        // extract these variables from the range points in the scene using get child
        // top range box
        range[0].Add("min_x", transform.Find("Range_top").Find("Point_top_1").transform.position.x);
        range[0].Add("max_y", transform.Find("Range_top").Find("Point_top_1").transform.position.y);
        range[0].Add("max_x", transform.Find("Range_top").Find("Point_top_2").transform.position.x);
        range[0].Add("min_y", transform.Find("Range_top").Find("Point_top_2").transform.position.y);
        // bottom range box
        range[1].Add("min_x", transform.Find("Range_bottom").Find("Point_bottom_1").transform.position.x);
        range[1].Add("max_y", transform.Find("Range_bottom").Find("Point_bottom_1").transform.position.y);
        range[1].Add("max_x", transform.Find("Range_bottom").Find("Point_bottom_2").transform.position.x);
        range[1].Add("min_y", transform.Find("Range_bottom").Find("Point_bottom_2").transform.position.y);
        // left range box
        range[2].Add("min_x", transform.Find("Range_left").Find("Point_left_1").transform.position.x);
        range[2].Add("max_y", transform.Find("Range_left").Find("Point_left_1").transform.position.y);
        range[2].Add("max_x", transform.Find("Range_left").Find("Point_left_2").transform.position.x);
        range[2].Add("min_y", transform.Find("Range_left").Find("Point_left_2").transform.position.y);
        // right range box
        range[3].Add("min_x", transform.Find("Range_right").Find("Point_right_1").transform.position.x);
        range[3].Add("max_y", transform.Find("Range_right").Find("Point_right_1").transform.position.y);
        range[3].Add("max_x", transform.Find("Range_right").Find("Point_right_2").transform.position.x);
        range[3].Add("min_y", transform.Find("Range_right").Find("Point_right_2").transform.position.y);
    }

    void Start()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerControl>();
        StartCoroutine(Spawn());
        
    }

    IEnumerator Spawn()
    {
        while (true)
        {
            // update the current npc count
            currNPC_count = GameObject.FindGameObjectsWithTag("NPC").Length;
            

            // when curr npc count is smaller than the max npc allowed, spawn a new npc
            if (currNPC_count <= maxNPC)
            {
                // generate one of the five random NPC
                int npc_index = Random.Range(0, 5);
                // generate from one of the four ranges, random x and y
                int range_index = Random.Range(0, 4);
                float x = Random.Range(range[range_index]["min_x"], range[range_index]["max_x"]);
                float y = Random.Range(range[range_index]["min_y"], range[range_index]["max_y"]);

                Instantiate(NPC_prefab[npc_index], new Vector3(x, y, 0), Quaternion.identity);
                Instantiate(spawnEffect, new Vector3(x, y + 1, 0), Quaternion.identity);
                spawn_sound.PlayOneShot(spawn_sound.clip);
            }
            yield return new WaitForSeconds(spawnTime);
        }
    }

    public void SpawnNPC_1_AtLocation(Transform loc)
    {

        Instantiate(NPC_prefab[0], loc.position, Quaternion.identity);
        Instantiate(spawnEffect, loc.position + new Vector3(0, 1, 0), Quaternion.identity);
        spawn_sound.PlayOneShot(spawn_sound.clip);

    }

    public void SpawnNPC_4_AtLocation(Transform loc)
    {

        Instantiate(NPC_prefab[3], loc.position, Quaternion.identity);
        Instantiate(spawnEffect, loc.position + new Vector3(0, 1, 0), Quaternion.identity);
        spawn_sound.PlayOneShot(spawn_sound.clip);

    }

    public void SpawnNPC_5_AtLocation(Transform loc)
    {
        Instantiate(NPC_prefab[4], loc.position, Quaternion.identity);
        Instantiate(spawnEffect, loc.position + new Vector3(0, 1, 0), Quaternion.identity);
        spawn_sound.PlayOneShot(spawn_sound.clip);
    }


    private void UpdateLevel()
    {
        int score = player.getHighScore();
        float walkingSpeed = 3.5f;
        // High Score: 0-20 = level 1
        // 21-40 = level 2
        // 41+ = level 3
        if(score >= 0 && score <= 20)
        {
            level = 1;
            walkingSpeed = 3.5f;

        }
        else if(score >= 21 && score <= 40)
        {
            level = 2;
            walkingSpeed = 4.5f;
        }
        else if(score >= 41)
        {
            level = 3;
            walkingSpeed = 5.5f;
        }

        // increase all npc walking speed
        foreach (GameObject n in GameObject.FindGameObjectsWithTag("NPC"))
        {
            n.GetComponent<NPC_move>().walkingSpeed = walkingSpeed;
        }
    }

    private void IncreaseDifficulty()
    {
        if(prev_level != level)
        {
            prev_level = level;

            // increase difficulty
            spawnTime--;
            maxNPC += 5;

        }
    }

    private void BurstControl()
    {
        burst_timer += Time.deltaTime;
        if(burst_timer >= burst_interval || BurstOnStuck())
        {
            // reach a time to burst, reset the timer and call NPC_burst()
            burst_timer = 0;
            NPC_burst();
            // give burst_interval a new value
            burst_interval = Random.Range(60,100);
        }
    }

    private bool BurstOnStuck()
    {
        // when all npc present are npc1, make a burst
        GameObject[] current_npcs = GameObject.FindGameObjectsWithTag("NPC");
        foreach(GameObject n in current_npcs)
        {
            if(n.GetComponent<NPC_control>().type != NPC_control.npc_type.type1)
            {
                return false;
            } 
        }

        //return true;
        return true;
    }

    private void NPC_burst()
    {
        // spawn 5 NPCs at the bottom spawn box but random position
        // 2 of them needs to be type5, the other 3 are random.

        // three random npcs
        // 1
        int npc_index = Random.Range(0, 5);
        // generate from bottom, random x and y
        int range_index = Random.Range(0,4);
        float x = Random.Range(range[range_index]["min_x"], range[range_index]["max_x"]);
        float y = Random.Range(range[range_index]["min_y"], range[range_index]["max_y"]);
        Instantiate(NPC_prefab[npc_index], new Vector3(x, y, 0), Quaternion.identity);
        Instantiate(spawnEffect, new Vector3(x, y + 1, 0), Quaternion.identity);
        spawn_sound.PlayOneShot(spawn_sound.clip);

        // 2
        npc_index = Random.Range(0, 5);
        x = Random.Range(range[range_index]["min_x"], range[range_index]["max_x"]);
        y = Random.Range(range[range_index]["min_y"], range[range_index]["max_y"]);
        Instantiate(NPC_prefab[npc_index], new Vector3(x, y, 0), Quaternion.identity);
        Instantiate(spawnEffect, new Vector3(x, y + 1, 0), Quaternion.identity);
        spawn_sound.PlayOneShot(spawn_sound.clip);

        // 3
        npc_index = Random.Range(0, 5);
        x = Random.Range(range[range_index]["min_x"], range[range_index]["max_x"]);
        y = Random.Range(range[range_index]["min_y"], range[range_index]["max_y"]);
        Instantiate(NPC_prefab[npc_index], new Vector3(x, y, 0), Quaternion.identity);
        Instantiate(spawnEffect, new Vector3(x, y + 1, 0), Quaternion.identity);
        spawn_sound.PlayOneShot(spawn_sound.clip);

        // two NPC-5
        npc_index = 4;
        x = Random.Range(range[range_index]["min_x"], range[range_index]["max_x"]);
        y = Random.Range(range[range_index]["min_y"], range[range_index]["max_y"]);
        Instantiate(NPC_prefab[npc_index], new Vector3(x, y, 0), Quaternion.identity);
        Instantiate(spawnEffect, new Vector3(x, y + 1, 0), Quaternion.identity);
        spawn_sound.PlayOneShot(spawn_sound.clip);

        x = Random.Range(range[range_index]["min_x"], range[range_index]["max_x"]);
        y = Random.Range(range[range_index]["min_y"], range[range_index]["max_y"]);
        Instantiate(NPC_prefab[npc_index], new Vector3(x, y, 0), Quaternion.identity);
        Instantiate(spawnEffect, new Vector3(x, y + 1, 0), Quaternion.identity);
        spawn_sound.PlayOneShot(spawn_sound.clip);

    }

    // Update is called once per frame
    void Update()
    {
        UpdateLevel();
        IncreaseDifficulty();
        BurstControl();
        UpdateDifficultyUI();
        BurstOnStuck();
    }

    private void UpdateDifficultyUI()
    {
        if(level == 1)
        {
            difficulty_text.text = "!";
        }
        else if(level == 2)
        {
            difficulty_text.text = "!!";
        }
        else if(level == 3)
        {
            difficulty_text.text = "!!!";
        }

    }
}
