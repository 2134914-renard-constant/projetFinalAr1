using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System.Collections.Generic;

/// <summary>
/// G�re la session r�seau multijoueur avec Fusion
/// D�marre la session partag�e, conserve une r�f�rence global
/// Impl�mente les callbacks n�cessaires pour la communication r�seau
/// </summary>
public class GestionReseau : MonoBehaviour, INetworkRunnerCallbacks
{
    // Instance statique accessible globalement
    public static GestionReseau instance;

    // R�f�rence au NetworkRunner pour g�rer la partie r�seau
    public NetworkRunner Runner { get; private set; }

    // Cr�e une instance unique
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    // D�marre automatiquement la session r�seau au lancement
    void Start()
    {
        LancerSessionMultijoueur();
    }

    /// <summary>
    /// Initialise une session r�seau partag�e avec Fusion
    /// </summary>
    public async void LancerSessionMultijoueur()
    {
        Runner = gameObject.AddComponent<NetworkRunner>();
        // Permet de g�rer les entr�es utilisateur
        Runner.ProvideInput = true; 

        // D�marre la partie en mode partag� pour tout le monde
        await Runner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Shared,
            SessionName = "SessionPenaltyAR", 
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>() 
        });
    }

    // Callback appel� lorsqu�un joueur rejoint la session
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log("Joueur connect� : " + player);
    }

    // Callback appel� lorsqu�un joueur quitte la session
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log("Joueur d�connect� : " + player);
    }

    // Callbacks obligatoires � impl�menter (m�me s�ils sont vides)
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
