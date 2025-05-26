using UnityEngine;

public class EndScreenManager : MonoBehaviour
{
    public CanvasGroup EndScreen;
    public CanvasGroup Credits;
    public float FadeTime;

    void Awake()
    {
        Credits.alpha = 0f;
        EndScreen.alpha = 1f;
    }

    public void OnScreenClick()
    {
        EndScreen.LeanAlpha(0f, FadeTime).setOnComplete(ShowCredits);
    }

    private void ShowCredits()
    {
        EndScreen.gameObject.SetActive(false);
        Credits.LeanAlpha(1f, FadeTime);
    }

    public void OnButtonAdvance()
    {
        CrossFade.Instance.FadeOut(5);
    }
}
