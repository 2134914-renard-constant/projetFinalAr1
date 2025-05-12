using UnityEngine;

/// <summary>
/// G�re la lecture des sons dans le jeu (but et collision avec le gardien)
/// </summary>
public class GestionAudio : MonoBehaviour
{
    // Instance unique de la gestion audio
    public static GestionAudio instance;
    // Source audio jou�e lorsqu�un but est marqu�
    public AudioSource sourceBut;
    // Source audio jou�e lorsqu�il y a une collision avec le gardien
    public AudioSource sourceGardien;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void JouerSonBut()
    {
        if (sourceBut != null)
            sourceBut.Play();
    }

    public void JouerSonGardien()
    {
        if (sourceGardien != null)
            sourceGardien.Play();
    }
}
