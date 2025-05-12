using UnityEngine;

public class MouvementCible : MonoBehaviour
{
    public float largeurMax = 2.0f;
    public float hauteurMax = 1.0f; 
    public float vitesseHorizontale = 1.5f;
    public float vitesseVerticale = 1.0f;
    private Vector3 positionInitiale;

    void Start()
    {
        positionInitiale = transform.position;
    }

    void Update()
    {
        // D�placement horizontal
        float deplacementX = Mathf.Sin(Time.time * vitesseHorizontale) * largeurMax;
        // D�placement vertical 
        float deplacementY = Mathf.Sin(Time.time * vitesseVerticale) * hauteurMax * 0.5f;
        // Nouvelle position bas�e sur les deplacements
        transform.position = positionInitiale + new Vector3(deplacementX, deplacementY, 0);
    }
}
