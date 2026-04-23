using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class VRUrlOpener : MonoBehaviour
{
    [Header("Enter the destination URL in the Inspector")]
    [SerializeField] private string url = "https://example.com";

    [Header("Optional safety")]
    [SerializeField] private bool requireHttps = true;
    [SerializeField] private bool logResult = true;

    public void OpenUrl()
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            if (logResult) Debug.LogWarning("VRUrlOpener: URL is empty.");
            return;
        }

        string trimmedUrl = url.Trim();

        if (requireHttps && !trimmedUrl.StartsWith("https://"))
        {
            if (logResult) Debug.LogWarning("VRUrlOpener: URL must start with https://");
            return;
        }

        Application.OpenURL(trimmedUrl);

        if (logResult) Debug.Log("VRUrlOpener: Opening URL -> " + trimmedUrl);
    }

    public void SetUrl(string newUrl)
    {
        url = newUrl;
    }

    public string GetUrl()
    {
        return url;
    }
}