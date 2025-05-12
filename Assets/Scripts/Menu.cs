using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// G�re les actions du menu principal
/// </summary>
public class Menu : MonoBehaviour
{
    // Charge la sc�ne principale du jeu
    public void LancerJeu()
    {
        SceneManager.LoadScene("JeuPenalty");
    }

    // Ferme l'application
    public void QuitterJeu()
    {
        Application.Quit();
    }
}
