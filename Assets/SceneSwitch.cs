using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{
    public string sceneName;

    public void SwitchScene()
    {
        Debug.Log("Button clicked, loading: " + sceneName);
        SceneManager.LoadScene(sceneName);
    }
}