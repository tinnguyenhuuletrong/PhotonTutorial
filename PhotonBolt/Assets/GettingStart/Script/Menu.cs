using UnityEngine;
using System.Collections;

public class Menu : Bolt.GlobalEventListener
{
    string Endpoint;
    string Scene;
    void OnGUI()
    {
        //Update EndPoint
        Endpoint = GUI.TextField(new Rect(0, 0, Screen.width, 50), "127.0.0.1:27000");

        Scene = GUI.TextField(new Rect(0, 60, Screen.width, 50), "Tutorial1");

        GUILayout.BeginArea(new Rect(10, 130, Screen.width - 20, Screen.height - 20));

        if (GUILayout.Button("Start Server", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)))
        {
            // START SERVER
            BoltLauncher.StartServer(UdpKit.UdpEndPoint.Parse(Endpoint));
        }

        if (GUILayout.Button("Start Client", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)))
        {
            // START CLIENT
            BoltLauncher.StartClient();
        }

        GUILayout.EndArea();
    }

    public override void BoltStartDone()
    {
        if (BoltNetwork.isServer)
            BoltNetwork.LoadScene(Scene);
        else
            BoltNetwork.Connect(UdpKit.UdpEndPoint.Parse(Endpoint));

    }
}