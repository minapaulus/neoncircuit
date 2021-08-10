using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main_Menu : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject credits;
    public float SlowmoFactor;

    public Material hexagonmat;
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = SlowmoFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

    // Update is called once per frame
    void Update()
    {
        hexagonmat.SetVector("MousePos", Input.mousePosition - (this.transform.position));
    }

    public void QuitGame()
    {
        try
        {
            Application.Quit();
        }
        catch
        {

        }
    }

    public void Credits()
    {
        if (!credits.activeSelf)
        {
            credits.SetActive(true);
        }else
        {
            credits.SetActive(false);
        }
    }

    public void NewGame()
    {
        try
        {
            //Newgame soll die erste Scene sein nach dem Hauptmenü um Build menu.
            Time.timeScale = 1;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
            SceneManager.LoadScene(1);
        }
        catch
        {

        }
    }
}
