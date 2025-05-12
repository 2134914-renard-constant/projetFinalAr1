using UnityEngine;

/// <summary>
/// Script pour faire osciller le gardien de gauche à droite
/// </summary>
public class Gardien : MonoBehaviour
{
    public float vitesse = 1f;      
    public float amplitude = 0.5f;  
    private Vector3 positionInitiale; 

    void Start()
    {
        // Enregistre la position initiale au départ
        positionInitiale = transform.position;
    }

    void Update()
    {
        // Calcul du déplacement horizontal en fonction du temps
        float deplacement = Mathf.Sin(Time.time * vitesse) * amplitude;
        // Mise à jour de la position du gardien
        transform.position = positionInitiale + new Vector3(deplacement, 0, 0);
    }
}
