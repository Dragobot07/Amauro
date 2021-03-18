using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectManager : MonoBehaviour
{
    public void Select (string levelName)
    {
        SceneManager.LoadScene(levelName);
    } 

}
