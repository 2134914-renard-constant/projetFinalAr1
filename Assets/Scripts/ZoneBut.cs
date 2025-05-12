using UnityEngine;
using Fusion;

/// <summary>
/// Déclenche l’ajout de points et un son lorsqu’un ballon entre dans la zone
/// </summary>
public class ZoneBut : NetworkBehaviour
{
    // Détecte l'entrée d'un objet dans la zone de trigger
    private void OnTriggerEnter(Collider other)
    {
        // Vérifie si l’objet entrant est un ballon et qu’il n’a pas déjà été traité
        TirVersCible ballon = other.GetComponent<TirVersCible>();
        if (ballon != null && !ballon.HasBeenHandled())
        {
            // Marque le but côté ballon (désactive le collider, etc.)
            ballon.MarquerBut();
            // Ajoute un point au score
            GestionPartie.instance.AjouterPoint();
            // Joue le son de but
            GestionAudio.instance.JouerSonBut();
        }
    }
}
