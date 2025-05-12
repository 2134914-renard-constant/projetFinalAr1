using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System.Collections.Generic;

/// <summary>
/// Gère la session réseau multijoueur avec Fusion
/// Démarre la session partagée, conserve une référence global
/// Implémente les callbacks nécessaires pour la communication réseau
/// </summary>
public class GestionReseau : MonoBehaviour, INetworkRunnerCallbacks
{
    // Instance statique accessible globalement
    public static GestionReseau instance;

    // Référence au NetworkRunner pour gérer la partie réseau
    public NetworkRunner Runner { get; private set; }

    // Crée une instance unique
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    // Démarre automatiquement la session réseau au lancement
    void Start()
    {
        LancerSessionMultijoueur();
    }

    /// <summary>
    /// Initialise une session réseau partagée avec Fusion
    /// </summary>
    public async void LancerSessionMultijoueur()
    {
        Runner = gameObject.AddComponent<NetworkRunner>();
        // Permet de gérer les entrées utilisateur
        Runner.ProvideInput = true; 

        // Démarre la partie en mode partagé pour tout le monde
        await Runner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Shared,
            SessionName = "SessionPenaltyAR", 
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>() 
        });
    }

    // Callback appelé lorsqu’un joueur rejoint la session
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log("Joueur connecté : " + player);
    }

    // Callback appelé lorsqu’un joueur quitte la session
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log("Joueur déconnecté : " + player);
    }

    // Callbacks obligatoires à implémenter (même s’ils sont vides)
    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnInput(NetworkRunner runner, NetworkInput input) { }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, System.ArraySegment<byte> data) { }
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
}
