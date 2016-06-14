using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Tut1NetworkCallback : Bolt.GlobalEventListener
{
    public override void SceneLoadLocalDone(string map)
    {
        // randomize a position
        var pos = new Vector3(Random.Range(-16, 16), 4, Random.Range(-16, 16));

        // instantiate cube
        BoltNetwork.Instantiate(BoltPrefabs.Cube, pos, Quaternion.identity);

        if(BoltNetwork.isServer)
        {
            this.gameObject.AddComponent<Tut1ServerCallbacks>();
        }
    }


    List<string> logMessages = new List<string>();
    public override void OnEvent(LogEvent evnt)
    {
        logMessages.Insert(0, evnt.Msg);
    }

    void OnGUI()
    {
        // only display max the 5 latest log messages
        int maxMessages = Mathf.Min(5, logMessages.Count);

        GUILayout.BeginArea(new Rect(Screen.width / 2 - 200, Screen.height - 100, 400, 100), GUI.skin.box);

        for (int i = 0; i < maxMessages; ++i)
        {
            GUILayout.Label(logMessages[i]);
        }

        GUILayout.EndArea();
    }
}