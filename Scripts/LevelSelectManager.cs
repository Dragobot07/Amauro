using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectManager : MonoBehaviour
{


	public Button[] levelButtons;
    public string menuScene;

    void Start()
	{
		int levelPassed = PlayerPrefs.GetInt("LevelPassed")+1;
		Debug.Log(levelPassed);  // TODO called twice on entering level Select???
		for (int i = 0; i< levelButtons.Length; i++)
        {

			if (i+1 > levelPassed)
            {
				levelButtons[i].interactable = false;
            }
        }
	}

    private void Update()
    {

        if (Application.platform == RuntimePlatform.Android)
        {

            // Check if Back was pressed this frame
            if (Input.GetKeyDown(KeyCode.Escape))
            {

                // go back to the menu
                ChangeScene(menuScene);
            }
        } //back button condition in level
    }
    public void Select (string levelName)

    {
        Debug.Log("Loading " + levelName);
        SceneManager.LoadScene(levelName);
    }

    public void ChangeScene(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }
}
