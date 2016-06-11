using UnityEngine;
using System.Collections;

public class CubeBehaviour : Bolt.EntityBehaviour<ICubeState>
{
    //Equivalent of the Start method which exists in Unity, but it's called after the game object has been setup inside Bolt and exists on the network.
    public override void Attached()
    {
        //Bind Properties
        state.SetTransforms(state.Transform, transform);

        //Color Random Owner only
        if (entity.isOwner)
            state.Color = new Color(Random.value, Random.value, Random.value);

        //Register State Change Callback
        state.AddCallback("Color", ColorChanged);
    }

    void ColorChanged()
    {
        Debug.Log("Color Change" + state.Color);
        GetComponent<Renderer>().material.color = state.Color;
    }

    // The "Owner" and this is where SimulateOwner will be called, and only called on that computer
    public override void SimulateOwner()
    {
        var speed = 4f;
        var movement = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) { movement.z += 1; }
        if (Input.GetKey(KeyCode.S)) { movement.z -= 1; }
        if (Input.GetKey(KeyCode.A)) { movement.x -= 1; }
        if (Input.GetKey(KeyCode.D)) { movement.x += 1; }

        if (movement != Vector3.zero)
        {
            transform.position = transform.position + (movement.normalized * speed * BoltNetwork.frameDeltaTime);
        }
    }
}