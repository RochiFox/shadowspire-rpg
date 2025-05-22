using UnityEngine;

public class FadeScreenUI : MonoBehaviour
{
    private static readonly int Out = Animator.StringToHash("fadeOut");
    private static readonly int In = Animator.StringToHash("fadeIn");

    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void FadeOut() => anim.SetTrigger(Out);
    public void FadeIn() => anim.SetTrigger(In);
}