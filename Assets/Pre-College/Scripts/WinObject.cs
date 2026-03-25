using UnityEngine;

public class WinObject : MonoBehaviour
{
    [Tooltip("Score amount that the WinObject counts towards.")]
    public int scoreAmount = 1;
    [Tooltip("Win Object is destroyed upon entering a Win Container.")]
    public bool destroyOnEnter = false;
    [Tooltip("Parent GameObject to be destroyed. If Destroy On Enter is false, then you do not need to set this variable.")]
    public GameObject parentGameObject;

    private void Start()
    {
        if (destroyOnEnter)
        {
            if (!parentGameObject)
            {
                Debug.Log("Parent GameObject not assigned in the inspector. Object will not be destroyed upon entry to a win container.");
                parentGameObject = gameObject;
            }
        }
    }
    
    public void OnEnterContainer()
    {
        //Debug.Log("OnEnterContainer");
        if (!destroyOnEnter || !parentGameObject)
            return;

        //Debug.Log("Destroying parent GameObject...");
        Destroy(parentGameObject);
    }
}
