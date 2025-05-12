using UnityEngine;
using Fusion;

/// <summary>
/// D�clenche l�ajout de points et un son lorsqu�un ballon entre dans la zone
/// </summary>
public class ZoneBut : NetworkBehaviour
{
    // D�tecte l'entr�e d'un objet dans la zone de trigger
    private void OnTriggerEnter(Collider other)
    {
        // V�rifie si l�objet entrant est un ballon et qu�il n�a pas d�j� �t� trait�
        TirVersCible ballon = other.GetComponent<TirVersCible>();
        if (ballon != null && !ballon.HasBeenHandled())
        {
            // Marque le but c�t� ballon (d�sactive le collider, etc.)
            ballon.MarquerBut();
            // Ajoute un point au score
            GestionPartie.instance.AjouterPoint();
            // Joue le son de but
            GestionAudio.instance.JouerSonBut();
        }
    }
}
