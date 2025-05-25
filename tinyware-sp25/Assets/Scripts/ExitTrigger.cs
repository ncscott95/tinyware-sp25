using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitTrigger : MonoBehaviour
{
    public const string LEVEL_ENTERED_KEY = "LastLevelEntered";
    public const string LEVEL_COMPLETE_KEY = "LastLevelCompleted";
    public int nextScene;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (nextScene > 0 && nextScene < 5)
        {
            // leaving box, just starting a level
            PlayerPrefs.SetInt(LEVEL_ENTERED_KEY, PlayerPrefs.GetInt(LEVEL_COMPLETE_KEY) + 1);
            nextScene = PlayerPrefs.GetInt(LEVEL_ENTERED_KEY);
        }
        else if (nextScene == 0)
        {
            // returning to box, just completed a level
            PlayerPrefs.SetInt(LEVEL_COMPLETE_KEY, PlayerPrefs.GetInt(LEVEL_ENTERED_KEY));
        }
        Debug.Log($"Entered: {PlayerPrefs.GetInt(LEVEL_ENTERED_KEY)}  Complete: {PlayerPrefs.GetInt(LEVEL_COMPLETE_KEY)}");

        // Transition out of scene
        if (collision.GetComponent<PlayerControls>() != null) CrossFade.Instance.FadeOut(nextScene);
    }
}
