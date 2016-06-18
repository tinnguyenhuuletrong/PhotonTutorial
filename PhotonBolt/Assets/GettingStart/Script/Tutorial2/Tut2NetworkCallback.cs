using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Tut2NetworkCallback : Bolt.GlobalEventListener
{
    public override void SceneLoadLocalDone(string map)
    {
        // randomize a position
        var pos = new Vector3(Random.Range(-16, 16), 4, Random.Range(-16, 16));

        // instantiate cube
        BoltNetwork.Instantiate(BoltPrefabs.Character, pos, Quaternion.identity);
    }
}