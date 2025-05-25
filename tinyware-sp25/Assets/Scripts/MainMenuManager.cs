using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public GameObject Credits;

    void Awake()
    {
        Credits.SetActive(false);
    }

    public void OnButtonStart()
    {
        PlayerPrefs.DeleteAll();
        if (!PlayerPrefs.HasKey(ExitTrigger.LEVEL_ENTERED_KEY)) PlayerPrefs.SetInt(ExitTrigger.LEVEL_ENTERED_KEY, 0);
        if (!PlayerPrefs.HasKey(ExitTrigger.LEVEL_COMPLETE_KEY)) PlayerPrefs.SetInt(ExitTrigger.LEVEL_COMPLETE_KEY, 0);

        CrossFade.Instance.FadeOut(0);
    }

    public void OnButtonCredits()
    {
        Credits.SetActive(true);
    }

    public void OnButtonBack()
    {
        Credits.SetActive(false);
    }
}
