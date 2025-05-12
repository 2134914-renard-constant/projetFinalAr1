using UnityEngine;

/// <summary>
/// Script pour faire osciller le gardien de gauche � droite
/// </summary>
public class Gardien : MonoBehaviour
{
    public float vitesse = 1f;      
    public float amplitude = 0.5f;  
    private Vector3 positionInitiale; 

    void Start()
    {
        // Enregistre la position initiale au d�part
        positionInitiale = transform.position;
    }

    void Update()
    {
        // Calcul du d�placement horizontal en fonction du temps
        float deplacement = Mathf.Sin(Time.time * vitesse) * amplitude;
        // Mise � jour de la position du gardien
        transform.position = positionInitiale + new Vector3(deplacement, 0, 0);
    }
}
