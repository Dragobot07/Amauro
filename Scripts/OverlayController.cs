using RDG;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OverlayController : MonoBehaviour
{

    private int hintCounter = 1;
    public GameObject Entities;
    public GameObject Hints;
    public GameObject Died;
    public GameObject Win;
    public GameObject Text;
 
    public string levelSelectScene;

    // Start is called before the first frame update
    void Start()
    {
        if (Hints != null)
        {
            StartHints();
        }
        else
        {
            Entities.SetActive(true);
        }
        //RDG.Vibration.setOverlay(this);

    }   
    public void DebugText(string text)
    {
        Text.GetComponent<UnityEngine.UI.Text>().text = text;
    }

    private void Update()
    {

        if (Application.platform == RuntimePlatform.Android)
        {

            // Check if Back was pressed this frame
            if (Input.GetKeyDown(KeyCode.Escape))
            {

                // go back to the level select
                SceneManager.LoadScene(levelSelectScene);
                Vibration.Cancel();
            }
        } //back button condition in level
    }

    public void ShowDeathPanel(bool display)
    {
        Died.SetActive(display);
        Entities.SetActive(!display);
    }
    public void ShowVictoryPanel(bool display)
    {

        Win.SetActive(display);
        Entities.SetActive(!display);


        Scene CurrentScene = SceneManager.GetActiveScene();
        int j=-1;
        System.Int32.TryParse(CurrentScene.name.Remove(0, CurrentScene.name.IndexOf("_") + 1), out j);
        if (j != -1)
        {
            PlayerPrefs.SetInt("LevelPassed", j);
        }
        else
        {
            Debug.LogError("Unable to get current level number");
        }
        
    }
    public void ReturnToMenu()
    {
        SceneManager.LoadScene("Menu");
        Vibration.Cancel();
    }
    public void GoToNextLevel()
    {
        Scene CurrentScene = SceneManager.GetActiveScene();
        string nextLevelName;
        int j;

        if (System.Int32.TryParse(CurrentScene.name.Remove(0, CurrentScene.name.IndexOf("_") + 1), out j))
        {
            if (SceneManager.GetSceneByName("Level_" + (j + 1)).IsValid())
            {
                nextLevelName = "Level_" + (j + 1);
            }
            else
            {
                nextLevelName = "LevelSelect";
            } 
        }
        else
        {
            Debug.Log(CurrentScene.name.Remove(0, CurrentScene.name.IndexOf("_") + 1));
            nextLevelName = "Level_1";
        }
        Debug.Log("went to " + nextLevelName);
        SceneManager.LoadScene(nextLevelName);
    }

    private void StartHints()
    {
        showHint(0);            //enables the grey overlay and next button
        showHint(1);            //enables first hint
    }

    public void NextHint()
    {
        hideHint(hintCounter);
        hintCounter += 1;
        if (hintCounter < Hints.transform.childCount)
        {
            showHint(hintCounter);
        }
        else
        {
            hideHint(0);
            Entities.SetActive(true);
        }
        
    }

    private void showHint(int hintCounter)
    {
        Hints.transform.GetChild(hintCounter).gameObject.SetActive(true);
    }

    private void hideHint(int hintCounter)
    {
        Hints.transform.GetChild(hintCounter).gameObject.SetActive(false);
    }
}
