using UnityEngine;
using Fusion;

/// <summary>
/// Contrôle le tir du ballon vers la cible et la détection des collisions
/// </summary>
public class TirVersCible : NetworkBehaviour
{
    private Transform cible;                  
    public float vitesse = 3f;                
    public Rigidbody rb;
    // Référence au NetworkObject de Fusion
    private NetworkObject netObj;
    // Empêche de traiter plusieurs fois la même collision
    private bool dejaTraite = false;          

    // Appelé lorsque l'objet est instancié par Fusion
    public override void Spawned()
    {
        netObj = GetComponent<NetworkObject>();
        // Recherche automatique d’un objet avec le tag "Cible"
        GameObject cibleGO = GameObject.FindWithTag("Cible");
        if (cibleGO != null)
        {
            cible = cibleGO.transform;
            Debug.Log("Cible trouvée automatiquement !");
        }
        else
        {
            Debug.LogWarning("Aucune cible trouvée !");
        }
    }

    /// <summary>
    /// Applique une impulsion pour tirer le ballon vers la cible
    /// </summary>
    public void TirerVersLaCible()
    {
        // Calcul de la direction vers la cible (en visant légèrement au-dessus)
        Vector3 positionCibleCorrigee = cible.position + new Vector3(0, 0.3f, 0);
        Debug.Log("Position de la cible : " + cible.position);
        Debug.Log("Position du ballon : " + transform.position);
        Vector3 direction = (positionCibleCorrigee - transform.position).normalized;

        // Petite variation aléatoire pour rendre le tir moins robotique
        Vector3 variation = new Vector3(
            Random.Range(-0.02f, 0.02f),
            Random.Range(0.0f, 0.05f),
            0f
        );

        // Annule les forces précédentes
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Applique la force vers la cible avec la variation
        rb.linearVelocity = (direction + variation) * vitesse;

        // Notifie le gestionnaire de partie que le ballon a été tiré
        if (GestionPartie.instance != null)
        {
            GestionPartie.instance.GererTirBallon();
        }
    }

    /// <summary>
    /// Détection de collision avec le gardien
    /// </summary>
    void OnCollisionEnter(Collision collision)
    {
        if (dejaTraite) return;

        if (collision.gameObject.CompareTag("Gardien"))
        {
            dejaTraite = true;
            Debug.Log("Collision avec le gardien !");

            // Joue un son si l'instance audio existe
            if (GestionAudio.instance != null)
            {
                GestionAudio.instance.JouerSonGardien();
            }

            // Désactive le collider du ballon pour éviter les collisions répétées
            Collider monCollider = GetComponent<Collider>();
            if (monCollider != null)
            {
                monCollider.enabled = false;
            }

            // Notifie la perte d'une vie
            if (GestionPartie.instance != null)
                GestionPartie.instance.PerdreVie();
        }
    }

    /// <summary>
    /// Indique si le ballon a déjà été géré (but ou gardien)
    /// </summary>
    public bool HasBeenHandled()
    {
        return dejaTraite;
    }

    /// <summary>
    /// Appelé lorsque le ballon entre dans la zone de but
    /// </summary>
    public void MarquerBut()
    {
        if (dejaTraite) return;

        dejaTraite = true;

        // Empêche toute nouvelle interaction
        GetComponent<Collider>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
    }
}
