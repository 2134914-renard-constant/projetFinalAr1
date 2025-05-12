using UnityEngine;
using Fusion;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// G�re l�ensemble de la partie
/// </summary>
public class GestionPartie : NetworkBehaviour
{
    // Instance unique pour acc�s global
    public static GestionPartie instance; 

    public float dureePartie = 30f;
    // D�lai entre chaque respawn de ballon
    public float delaiRespawn = 3f;
    // Prefab r�seau du ballon
    public NetworkPrefabRef prefabBallon;
    // Point de spawn du ballon
    [HideInInspector] 
    public Transform positionSpawnBallon;
    public TextMeshProUGUI texteTimer;   
    public Button boutonTirer;           
    public GameObject ecranGameOver;
    // R�f�rence au ballon actif
    [HideInInspector]
    public TirVersCible ballonActuel; 
    private int score = 0;
    private int vies = 3;
    public TextMeshProUGUI texteScore;   
    public TextMeshProUGUI texteVies;  
    private float tempsRestant;          

    void Awake()
    {
        // Initialisation du singleton
        if (instance == null)
            instance = this;
    }

    public override void Spawned()
    {
        // Lancement automatique de la partie � l'apparition r�seau
        LancerPartie();
    }

    /// <summary>
    /// D�marre la partie
    /// </summary>
    public void LancerPartie()
    {
        tempsRestant = dureePartie;
        StartCoroutine(TimerPartie());
        SpawnNouveauBallon();
    }

    /// <summary>
    /// G�re le compte � rebours de la partie
    /// </summary>
    private IEnumerator TimerPartie()
    {
        while (tempsRestant > 0f)
        {
            if (texteTimer != null)
                texteTimer.text = "Temps : " + Mathf.CeilToInt(tempsRestant) + "s";

            yield return new WaitForSeconds(1f);
            tempsRestant--;
        }

        // Appelle FinPartie quand le temps est �coul�
        FinPartie();
    }

    /// <summary>
    /// D�sactive le ballon actuel et programme le prochain spawn
    /// </summary>
    public void GererTirBallon()
    {
        if (ballonActuel != null)
        {
            ballonActuel = null;

            if (boutonTirer != null)
                boutonTirer.interactable = false;

            StartCoroutine(RespawnBallonApresDelai());
        }
    }

    /// <summary>
    /// D�lai avant de g�n�rer un nouveau ballon
    /// </summary>
    private IEnumerator RespawnBallonApresDelai()
    {
        yield return new WaitForSeconds(delaiRespawn);
        SpawnNouveauBallon();
    }

    /// <summary>
    /// Incr�mente le score et met � jour l�interface
    /// </summary>
    public void AjouterPoint()
    {
        score++;
        texteScore.text = "Score : " + score;
    }

    /// <summary>
    /// Cr�e un nouveau ballon et le relie au bouton tirer
    /// </summary>
    private void SpawnNouveauBallon()
    {
        NetworkRunner runner = GestionReseau.instance.Runner;

        NetworkObject obj = runner.Spawn(
            prefabBallon,
            positionSpawnBallon.position,
            Quaternion.identity,
            runner.LocalPlayer
        );

        TirVersCible script = obj.GetComponent<TirVersCible>();
        if (script != null)
        {
            ballonActuel = script;

            if (boutonTirer != null)
            {
                boutonTirer.onClick.RemoveAllListeners();
                boutonTirer.onClick.AddListener(() => {
                    if (ballonActuel != null)
                    {
                        ballonActuel.TirerVersLaCible();
                        boutonTirer.interactable = false;
                    }
                });
                boutonTirer.interactable = true;
                Debug.Log("Bouton li� au nouveau ballon.");
            }
        }
    }

    /// <summary>
    /// R�duit les vies et d�clenche la fin si elles tombent � 0
    /// </summary>
    public void PerdreVie()
    {
        vies--;

        if (texteVies != null)
            texteVies.text = "Vies : " + vies;

        Debug.Log("Vie perdue. Vies restantes : " + vies);

        if (vies <= 0)
        {
            FinPartie();
        }
    }

    /// <summary>
    /// Termine la partie
    /// </summary>
    private void FinPartie()
    {
        if (texteTimer != null)
            texteTimer.text = "Fin de la partie";

        if (boutonTirer != null)
            boutonTirer.interactable = false;

        if (ecranGameOver != null)
            ecranGameOver.SetActive(true);

        Debug.Log("Partie termin�e - Game Over !");

        StartCoroutine(RetourAuMenuApresDelai(3f));
    }

    /// <summary>
    /// Retourne automatiquement au menu apr�s un d�lai
    /// </summary>
    private IEnumerator RetourAuMenuApresDelai(float delai)
    {
        yield return new WaitForSeconds(delai);
        SceneManager.LoadScene("MenuScene");
    }
}
