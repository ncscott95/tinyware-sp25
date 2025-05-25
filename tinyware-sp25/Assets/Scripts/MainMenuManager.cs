using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public void OnButtonStart()
    {
        PlayerPrefs.DeleteAll();
        if (!PlayerPrefs.HasKey(ExitTrigger.LEVEL_ENTERED_KEY)) PlayerPrefs.SetInt(ExitTrigger.LEVEL_ENTERED_KEY, 0);
        if (!PlayerPrefs.HasKey(ExitTrigger.LEVEL_COMPLETE_KEY)) PlayerPrefs.SetInt(ExitTrigger.LEVEL_COMPLETE_KEY, 0);

        CrossFade.Instance.FadeOut(0);
    }

    public void OnButtonCredits()
    {
        // TODO: show credits
    }
}
