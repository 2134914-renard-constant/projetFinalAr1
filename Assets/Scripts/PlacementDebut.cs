using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Fusion;
using UnityEngine.UI;

/// <summary>
/// Permet de d�tecter un plan AR et d�y placer dynamiquement un terrain et un point de spawn pour les ballons
/// </summary>
public class PlacementDebut : MonoBehaviour
{
    // Prefab r�seau du terrain � instancier
    public NetworkPrefabRef prefabTerrain;  
    //public NetworkPrefabRef prefabBallon;
    private TirVersCible scriptTir;         
    public Button boutonTirer;
    // Permet de d�tecter les plans dans l�espace AR
    private ARRaycastManager raycastManager;
    // Liste des r�sultats du raycast AR
    private List<ARRaycastHit> hits = new();
    // Pour s�assurer que le terrain est plac� une seule fois
    private bool contenuPlace = false;
    // Entr�e utilisateur
    private Touch touch; 

    void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
        touch = new Touch();
    }

    // Active la d�tection tactile � l�activation du script
    void OnEnable()
    {
        touch.ARControls.Enable();
        touch.ARControls.ToucherEcran.performed += OnTouchDetecte;
    }

    // D�sactive la d�tection tactile � la d�sactivation du script
    void OnDisable()
    {
        touch.ARControls.ToucherEcran.performed -= OnTouchDetecte;
        touch.ARControls.Disable();
    }

    /// <summary>
    /// D�clench� lorsqu�on touche l��cran
    /// </summary>
    private void OnTouchDetecte(InputAction.CallbackContext context)
    {
        if (contenuPlace) return; 

        Vector2 positionTouche = context.ReadValue<Vector2>();

        // Lance un raycast AR pour d�tecter un plan
        if (raycastManager.Raycast(positionTouche, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose pose = hits[0].pose;

            Vector3 positionTerrain = pose.position;
            // Oriente le terrain pour qu�il regarde dans la m�me direction que la cam�ra
            Vector3 directionCam = Camera.main.transform.forward;
            // ignore l'inclinaison verticale
            directionCam.y = 0;
            Quaternion rotationTerrain = Quaternion.LookRotation(directionCam) * Quaternion.Euler(0, 90, 0);

            NetworkRunner runner = GestionReseau.instance.Runner;

            // Instancie le terrain � la position d�tect�e
            runner.Spawn(prefabTerrain, positionTerrain, rotationTerrain, runner.LocalPlayer);

            // Recherche dans la sc�ne le point de spawn nomm� "positionSpawn"
            Transform pointDeSpawn = GameObject.Find("positionSpawn")?.transform;

            // Si tout est pr�t, on lance la partie avec les bonnes r�f�rences
            if (GestionPartie.instance != null && pointDeSpawn != null)
            {
                GestionPartie.instance.positionSpawnBallon = pointDeSpawn;
                Debug.Log(" Point de respawn d�fini depuis le terrain instanci�.");
                GestionPartie.instance.LancerPartie();
            }

            contenuPlace = true; 
        }
        else
        {
            Debug.Log(" Aucun plan d�tect�.");
        }
    }
}
