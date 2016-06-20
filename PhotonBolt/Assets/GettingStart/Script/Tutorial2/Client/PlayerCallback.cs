using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//Running on Both Client and server
[BoltGlobalBehaviour("Tutorial2")]
class PlayerCallback : Bolt.GlobalEventListener
{
    public override void SceneLoadLocalDone(string map)
    {
       
    }

    public override void ControlOfEntityGained(BoltEntity arg)
    {
        // this tells the player camera to look at the entity we are controlling
        CameraManager.SetCameraTarget(arg);
    }
}
