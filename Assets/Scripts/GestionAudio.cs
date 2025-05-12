using UnityEngine;

/// <summary>
/// Gère la lecture des sons dans le jeu (but et collision avec le gardien)
/// </summary>
public class GestionAudio : MonoBehaviour
{
    // Instance unique de la gestion audio
    public static GestionAudio instance;
    // Source audio jouée lorsqu’un but est marqué
    public AudioSource sourceBut;
    // Source audio jouée lorsqu’il y a une collision avec le gardien
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
