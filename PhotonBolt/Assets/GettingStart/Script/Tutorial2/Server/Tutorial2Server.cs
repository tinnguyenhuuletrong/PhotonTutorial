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
        PlayerManager.CreateServerPlayer();
    }

    public override void Connected(BoltConnection arg)
    {
        PlayerManager.CreateClientPlayer(arg);
    }

    public override void SceneLoadLocalDone(string map)
    {
        PlayerManager.ServerPlayer.Spawn();
    }

    public override void SceneLoadRemoteDone(BoltConnection connection)
    {
        PlayerManager.FindPlayerFromConnection(connection).Spawn();
    }
}

