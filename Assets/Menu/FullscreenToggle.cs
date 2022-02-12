using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class FullscreenToggle : MonoBehaviour
{
    private Toggle toggle;

    private void Start()
    {
        toggle = GetComponent<Toggle>();
        toggle.isOn = Screen.fullScreen;
    }

    public void SetFullscreen(bool enabled)
    {
        if (enabled)
        {
            Resolution fullRes = Screen.currentResolution;
            Screen.SetResolution(fullRes.width, fullRes.height, enabled);
        }
        Screen.fullScreen = enabled;
    }
}