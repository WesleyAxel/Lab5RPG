using UnityEngine;
using UnityEngine.SceneManagement;

public class ManageButtonEnd : MonoBehaviour
{
    public void GoToEnding()                                    // metodo do botão para carregar a cena de creditos
    {
        SceneManager.LoadScene("Lab4RPG_Credits");
    }
}
