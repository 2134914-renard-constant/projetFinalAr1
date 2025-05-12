using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Fusion;
using UnityEngine.UI;

/// <summary>
/// Permet de détecter un plan AR et d’y placer dynamiquement un terrain et un point de spawn pour les ballons
/// </summary>
public class PlacementDebut : MonoBehaviour
{
    // Prefab réseau du terrain à instancier
    public NetworkPrefabRef prefabTerrain;  
    //public NetworkPrefabRef prefabBallon;
    private TirVersCible scriptTir;         
    public Button boutonTirer;
    // Permet de détecter les plans dans l’espace AR
    private ARRaycastManager raycastManager;
    // Liste des résultats du raycast AR
    private List<ARRaycastHit> hits = new();
    // Pour s’assurer que le terrain est placé une seule fois
    private bool contenuPlace = false;
    // Entrée utilisateur
    private Touch touch; 

    void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
        touch = new Touch();
    }

    // Active la détection tactile à l’activation du script
    void OnEnable()
    {
        touch.ARControls.Enable();
        touch.ARControls.ToucherEcran.performed += OnTouchDetecte;
    }

    // Désactive la détection tactile à la désactivation du script
    void OnDisable()
    {
        touch.ARControls.ToucherEcran.performed -= OnTouchDetecte;
        touch.ARControls.Disable();
    }

    /// <summary>
    /// Déclenché lorsqu’on touche l’écran
    /// </summary>
    private void OnTouchDetecte(InputAction.CallbackContext context)
    {
        if (contenuPlace) return; 

        Vector2 positionTouche = context.ReadValue<Vector2>();

        // Lance un raycast AR pour détecter un plan
        if (raycastManager.Raycast(positionTouche, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose pose = hits[0].pose;

            Vector3 positionTerrain = pose.position;
            // Oriente le terrain pour qu’il regarde dans la même direction que la caméra
            Vector3 directionCam = Camera.main.transform.forward;
            // ignore l'inclinaison verticale
            directionCam.y = 0;
            Quaternion rotationTerrain = Quaternion.LookRotation(directionCam) * Quaternion.Euler(0, 90, 0);

            NetworkRunner runner = GestionReseau.instance.Runner;

            // Instancie le terrain à la position détectée
            runner.Spawn(prefabTerrain, positionTerrain, rotationTerrain, runner.LocalPlayer);

            // Recherche dans la scène le point de spawn nommé "positionSpawn"
            Transform pointDeSpawn = GameObject.Find("positionSpawn")?.transform;

            // Si tout est prêt, on lance la partie avec les bonnes références
            if (GestionPartie.instance != null && pointDeSpawn != null)
            {
                GestionPartie.instance.positionSpawnBallon = pointDeSpawn;
                Debug.Log(" Point de respawn défini depuis le terrain instancié.");
                GestionPartie.instance.LancerPartie();
            }

            contenuPlace = true; 
        }
        else
        {
            Debug.Log(" Aucun plan détecté.");
        }
    }
}
