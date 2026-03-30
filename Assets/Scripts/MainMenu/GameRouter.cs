using UnityEngine;
using UnityEngine.SceneManagement;

public class GameRouter : MonoBehaviour
{
    public void GoScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
