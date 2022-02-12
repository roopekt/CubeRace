using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SettingsToggle : MonoBehaviour
{
    private Animator animator;

    private bool on = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Toggle()
    {
        if (!animator)
            return;

        on = !on;//toggle

        if (on)
            animator.Play("ShowSettings");
        else
            animator.Play("HideSettings");
    }
}