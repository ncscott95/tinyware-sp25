using UnityEngine;
using UnityEngine.SceneManagement;

public class CrossFade : MonoBehaviour
{
    public static CrossFade Instance;
    [Header("Fade Settings")]
    [SerializeField] float fadeTime;

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        FadeIn();
    }

    // When fading into the scene
    public void FadeIn()
    {
        canvasGroup.alpha = 1f;
        canvasGroup.LeanAlpha(0f, fadeTime);
    }

    // When fading out of the scene
    public void FadeOut(int nextScene)
    {
        canvasGroup.alpha = 0f;
        if (PlayerControls.Instance != null) PlayerControls.Instance.Inputs.Player.Disable();
        canvasGroup.LeanAlpha(1f, fadeTime).setOnComplete(FadeOutComplete, nextScene);
    }

    private void FadeOutComplete(object nextScene)
    {
        SceneManager.LoadScene((int)nextScene);
    }
}
