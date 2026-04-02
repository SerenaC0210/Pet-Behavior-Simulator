using UnityEngine;

public class QuitApp : MonoBehaviour
{
    public void Quit()
    {
        Debug.Log("Quit pressed");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}