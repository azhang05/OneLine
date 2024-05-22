using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{
    private GameObject player;
    private string sceneName;
    public AudioSource StartGameSFX;
    public string nextLevelName;

    // Start is called before the first frame update
    void Start()
    {
        sceneName = SceneManager.GetActiveScene().name;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        StartGameSFX.Play();
        SceneManager.LoadScene("Level_1");
    }

    public void NextLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(nextLevelName);
    }

    public void Credits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void Controls()
    {
        SceneManager.LoadScene("Controls");
    }

    public void ResetLevel()
    {
        Time.timeScale = 1f;
        GameHandler_PauseMenu.GameisPaused = false;
        SceneManager.LoadScene(sceneName);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        GameHandler_PauseMenu.GameisPaused = false;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
                  UnityEditor.EditorApplication.isPlaying = false;
            #else
                  Application.Quit();
            #endif
    }

    public void Back()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
