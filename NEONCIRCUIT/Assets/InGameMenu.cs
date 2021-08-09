using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenu : MonoBehaviour
{
    private GameObject _eSCMenu;
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
                Cursor.visible = false;
                _eSCMenu.SetActive(false);
            } else
            {
                Time.timeScale = 0;
                Cursor.visible = true;
                _eSCMenu.SetActive(true);
            }
        }
        
    }

    public void RestartScene()
    {
        try
        {
            //Pascal muss hier vielleicht noch bisschen Magie machen, wenn das funktionieren soll mit dem Zufallsgenerator.
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        } catch
        {

        }
    }

    public void GoToMainMenu()
    {
        try
        {
            SceneManager.LoadScene(0);
        } catch
        {

        }
    }

    public void QuitGame()
    {
        try
        {
            Application.Quit();
        }catch
        {

        }
    }
}
