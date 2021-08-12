using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenu : MonoBehaviour
{
    private GameObject _eSCMenu;
    public Playerstats playerstat;

    public float SlowmoFac;
    // Start is called before the first frame update
    void Start()
    {
        _eSCMenu = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_eSCMenu.activeSelf)
            {
                Time.timeScale = 1;
                Time.fixedDeltaTime = Time.timeScale * 0.02f;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                _eSCMenu.SetActive(false);
            } else
            {
                Time.timeScale = SlowmoFac;
                Time.fixedDeltaTime = Time.timeScale * 0.02f;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                _eSCMenu.SetActive(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            playerstat.LoadPlayer();
        }
        
    }

    public void RestartScenewithCount()
    {
        try
        {
            //Pascal muss hier vielleicht noch bisschen Magie machen, wenn das funktionieren soll mit dem Zufallsgenerator.
            GameObject.FindGameObjectWithTag("Manager").GetComponent<LoadAndSafeScene>().LoadsScene = true;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        } catch
        {

        }
    }

    public void RestartScenenNewSafeFile()
    {
        try
        {
            //Pascal muss hier vielleicht noch bisschen Magie machen, wenn das funktionieren soll mit dem Zufallsgenerator.
            GameObject.FindGameObjectWithTag("Manager").GetComponent<LoadAndSafeScene>().LoadsScene = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        catch
        {

        }
    }

    public void GoToMainMenu()
    {
        try
        {
            playerstat.SavePlayer();
            SceneManager.LoadScene(0);
        } catch
        {

        }
    }

    public void SaveScene()
    {
        playerstat.SavePlayer();
    }

    public void LoadScene()
    {
        playerstat.LoadPlayer();
    }

    public void QuitGame()
    {
        try
        {
            playerstat.SavePlayer();
            Application.Quit();
        }catch
        {

        }
    }
}
