using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main_Menu : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject credits;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
            SceneManager.LoadScene(1);
        }
        catch
        {

        }
    }
}
