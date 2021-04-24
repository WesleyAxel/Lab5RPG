using UnityEngine;
using UnityEngine.SceneManagement;

public class ManageButtonRestart : MonoBehaviour
{

    public void RestartGame()
    {
        SceneManager.LoadScene("Lab4RPG_mainscene");
    }
}
