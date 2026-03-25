using UnityEngine;
using System.Collections.Generic;

public class XRColorChanger : MonoBehaviour
{
    public List<Color> colors = new List<Color>();

    private bool enableColorChange = true;

    private Renderer rend;
    private int colorIndex;

    private void Awake()
    {
        rend = GetComponent<Renderer>();
    }

    public void EnableColorChange(bool enable)
    {
        enableColorChange = enable;
    }

    public void ChangeColor()
    {
        if (!enableColorChange || colors.Count == 0)
            return;

        colorIndex = (colorIndex + 1) % colors.Count;
        rend.material.color = colors[colorIndex];
    }
}
