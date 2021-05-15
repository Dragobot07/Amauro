using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{
    public string menuScene;

    private void Update()
    {

        if (Application.platform == RuntimePlatform.Android)
        {

            // Check if Back was pressed this frame
            if (Input.GetKeyDown(KeyCode.Escape))
            {

                // go back to the menu
                SceneManager.LoadScene(menuScene);
            }
        } //back button condition in level
    }
    public void ChangeScene(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }
    public void resetPlayerPrefs()
	{
		PlayerPrefs.DeleteAll();
	}

}
