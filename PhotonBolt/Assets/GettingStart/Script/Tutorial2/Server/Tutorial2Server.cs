using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//Running on Server Only
[BoltGlobalBehaviour(BoltNetworkModes.Host, "Tutorial2")]
class Tutorial2Server : Bolt.GlobalEventListener
{
    void Awake()
    {
#if UNITY_EDITOR
        PlayerManager.CreateServerPlayer();
#endif
    }

    public override void Connected(BoltConnection arg)
    {
        PlayerManager.CreateClientPlayer(arg);
    }

    public override void SceneLoadLocalDone(string map)
    {
#if UNITY_EDITOR
        PlayerManager.ServerPlayer.Spawn();
#endif
    }

    public override void SceneLoadRemoteDone(BoltConnection connection)
    {
        PlayerManager.FindPlayerFromConnection(connection).Spawn();
    }
}

