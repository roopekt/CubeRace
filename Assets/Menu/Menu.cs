using System;
using System.Collections;
using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private Text completionPercent;
    [SerializeField] private int completionPercentDecimals = 1;
    [SerializeField] private Text finishTime;
    [SerializeField] private int finishTimeDecimals = 2;
    [SerializeField] private bool fadeIn = true;

    private Animator animator;
    private float initTime;

    private void Start()
    {
        animator = GetComponent<Animator>();
        initTime = Time.unscaledTime;

        GetComponent<CanvasScaler>().scaleFactor = Settings.UIScale;

        if (fadeIn)
            animator.Play("FadeIn");
    }

    public void DeathScreen(float _completionPercent)
    {
        completionPercent.text = FloatToString(_completionPercent * 100f, completionPercentDecimals) + "%";
        animator.Play("ShowMenuDead");
    }

    public void FinishScreen()
    {
        float time = Time.unscaledTime - initTime;
        finishTime.text = FloatToString(time, finishTimeDecimals) + "s";
        animator.Play("ShowMenuFinished");
    }

    public void FadeOut() =>
        animator.Play("FadeOut");

    public void LoadLevel(int buildIndex) =>
        StartCoroutine(LoadLevelWithFadeOut(buildIndex));

    private IEnumerator LoadLevelWithFadeOut(int buildIndex)
    {
        //play FadeOut and wait for it to finish
        animator.Play("FadeOut");
        AnimationClip[] animations = animator.runtimeAnimatorController.animationClips;
        AnimationClip fadeOut = Array.Find(animations, anim => anim.name == "FadeOut");
        yield return new WaitForSeconds(fadeOut.length);

        SceneManager.LoadScene(buildIndex);
    }

    private string FloatToString(float value, int decimalCount)
    {
        return value.ToString("F" + decimalCount.ToString(), CultureInfo.InvariantCulture);
    }
}
