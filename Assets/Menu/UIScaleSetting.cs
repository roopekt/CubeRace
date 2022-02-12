using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIScaleSetting : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private GameObject OKbutton;
    [SerializeField] private float confirmationTime = 1.5f;
    [SerializeField] private CanvasScaler menu;

    private void Start()
    {
        menu.scaleFactor = Settings.UIScale;
        slider.value = Settings.UIScale;
    }

    public void UpdateValue()
    {
        StopAllCoroutines();
        StartCoroutine(UpdateValueCoroutine());
    }

    public void ConfirmValueChange()
    {
        StopAllCoroutines();
        OKbutton.SetActive(false);
    }

    private IEnumerator UpdateValueCoroutine()
    {
        float oldValue = Settings.UIScale;
        float value = slider.value;

        Settings.UIScale = value;
        menu.scaleFactor = value;
        OKbutton.SetActive(true);

        Slider timer = OKbutton.GetComponentInChildren<Slider>();
        for (float time = confirmationTime; time > 0f; time -= Time.deltaTime)
        {
            timer.value = time / confirmationTime;
            yield return null;
        }

        slider.value = oldValue;
        Settings.UIScale = oldValue;
        menu.scaleFactor = oldValue;
        OKbutton.SetActive(false);
    }
}