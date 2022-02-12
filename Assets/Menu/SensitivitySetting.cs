using UnityEngine;
using UnityEngine.UI;

public class SensitivitySetting : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private InputField inputField;
    [Tooltip("The value is shown this many times bigger in the menu compared to Settings.mouseSensitivity.")]
    [SerializeField] private float scalar = 10000f;

    private bool ignoreNextCallback = false;
    private bool initialized = false;

    private void Start()
    {
        //load values for slider and inputField from Settings
        float sensitivity = Settings.mouseSensitivity * scalar;
        slider.value = sensitivity;
        inputField.text = sensitivity.ToString();

        initialized = true;
    }

    public void SetValue(float value)
    {
        if (ignoreNextCallback || !initialized)
        {
            ignoreNextCallback = false;
            return;
        }

        ignoreNextCallback = true;//the below line will make inputField send a callback
        inputField.text = Mathf.RoundToInt(value).ToString();

        Settings.mouseSensitivity = value / scalar;
    }

    public void SetValue(string value)
    {
        if (ignoreNextCallback || !initialized)
        {
            ignoreNextCallback = false;
            return;
        }

        int intValue;
        bool correctFormat = int.TryParse(value, out intValue);
        if (!correctFormat)
            intValue = 0;

        ignoreNextCallback = true;
        slider.value = intValue;

        Settings.mouseSensitivity = intValue / scalar;
    }

    public void FinishEditing()//ment to be called by inputField (On End Edit event)
    {
        //if inputField.text is incorrect in any way, make it correct
        int intValue;
        bool correctFormat = int.TryParse(inputField.text, out intValue);
        if (!correctFormat || intValue < slider.minValue)
        {
            ignoreNextCallback = false;
            inputField.text = Mathf.RoundToInt(slider.minValue).ToString();
        }
        else if (intValue > slider.maxValue)
        {
            ignoreNextCallback = false;
            inputField.text = Mathf.RoundToInt(slider.maxValue).ToString();
        }
    }
}