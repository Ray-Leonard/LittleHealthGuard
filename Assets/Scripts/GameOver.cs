using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField] PlayerControl p;
    [SerializeField] AudioSource bgm;
    [SerializeField] Text score_text;
    [SerializeField] AudioSource gameover_sound;

    public bool isGameOver = false;

    private int score_high;
    private int score_curr;



    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        score_high = p.getHighScore();
        score_curr = p.getCurrScore();
        if (score_curr <= -10)
        {
            if(!isGameOver)
            {
                transform.GetChild(0).gameObject.SetActive(true);
                Time.timeScale = 0;
                bgm.Stop();
                score_text.text = score_high.ToString("D3");
                gameover_sound.Play();
                isGameOver = true;
            }

        }
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
    }
}
