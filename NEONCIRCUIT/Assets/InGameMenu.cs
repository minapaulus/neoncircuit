using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenu : MonoBehaviour
{
    private GameObject _eSCMenu;
    public Playerstats playerstat;
    public float slowdownlength = .5f;

    public float SlowmoFac = 0.2f;
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
                StopAllCoroutines();
                StartCoroutine(ManipulateTime(1));
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                _eSCMenu.SetActive(false);
            } else
            {
                StopAllCoroutines();
                StartCoroutine(ManipulateTime(SlowmoFac));
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                _eSCMenu.SetActive(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            if (Time.timeScale >= 1)
            {
                StopAllCoroutines();
                StartCoroutine(ManipulateTime(SlowmoFac));
            }
            else
            {
                StopAllCoroutines();
                StartCoroutine(ManipulateTime(1));
            }
        }
        
    }

    IEnumerator ManipulateTime(float target)
    {
        if (Time.timeScale >= target)
        {
            while (Time.timeScale >= target)
            {
                Time.timeScale -= (1f / slowdownlength) * Time.unscaledDeltaTime;
                yield return null;
            }
            Time.timeScale = target;

        }
        else
        {
            while (Time.timeScale < target)
            {
                Time.timeScale += (1f / slowdownlength) * Time.unscaledDeltaTime;
                yield return null;
            }
            Time.timeScale = target;

        }

        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

    public void RestartScenewithCount()
    {
        try
        {
            //Pascal muss hier vielleicht noch bisschen Magie machen, wenn das funktionieren soll mit dem Zufallsgenerator.
            Time.timeScale = 1f;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
            playerstat.SavePlayer();
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
            Time.timeScale = 1f;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
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
            Time.timeScale = 1f;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
            playerstat.SavePlayer();
            Application.Quit();
        }catch
        {

        }
    }
}
