using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    private bool isPaused;

    [SerializeField] GameOver gameOver_obj;
    [SerializeField] AudioSource bgm;

    private bool isGameOver;

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
    }

    // Update is called once per frame
    private void Update()
    {
        isGameOver = gameOver_obj.isGameOver;
        if (!isPaused && Input.GetKeyDown(KeyCode.Escape) && !isGameOver)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            isPaused = true;
            Time.timeScale = 0;
            bgm.pitch = 0;
        }
        else if (isPaused && Input.GetKeyDown(KeyCode.Escape) && !isGameOver)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            isPaused = false;
            Time.timeScale = 1;
            bgm.pitch = 1;
        }
    }

    public void BackToGame()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        isPaused = false;
        Time.timeScale = 1;
        bgm.pitch = 1;
    }
}
