using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitTrigger : MonoBehaviour
{
    public int nextScene;

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Transition out of scene
        CrossFade.Instance.FadeOut(nextScene);
    }
}
