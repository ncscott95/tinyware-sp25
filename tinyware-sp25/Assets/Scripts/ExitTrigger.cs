using UnityEngine;

public class ExitTrigger : MonoBehaviour
{
    public const string LEVEL_ENTERED_KEY = "LastLevelEntered";
    public const string LEVEL_COMPLETE_KEY = "LastLevelCompleted";
    public int nextScene;

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);
        Debug.Log(collision.GetComponent<PlayerControls>());
        // Transition out of scene
        if (collision.GetComponent<PlayerControls>() != null)
        {
            if (nextScene > 1 && nextScene < 6)
            {
                // leaving box, just starting a level
                PlayerPrefs.SetInt(LEVEL_ENTERED_KEY, PlayerPrefs.GetInt(LEVEL_COMPLETE_KEY) + 1);
                nextScene = PlayerPrefs.GetInt(LEVEL_ENTERED_KEY);
            }
            else if (nextScene == 1)
            {
                // returning to box, just completed a level
                PlayerPrefs.SetInt(LEVEL_COMPLETE_KEY, PlayerPrefs.GetInt(LEVEL_ENTERED_KEY));
            }

            CrossFade.Instance.FadeOut(nextScene);
        }
    }
}
