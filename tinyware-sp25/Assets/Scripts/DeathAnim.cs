using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathAnim : MonoBehaviour
{
    public void ReloadSceneOnDeath()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
