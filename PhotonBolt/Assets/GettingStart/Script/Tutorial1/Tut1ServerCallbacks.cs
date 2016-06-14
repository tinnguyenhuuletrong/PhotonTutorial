using UnityEngine;
using System.Collections;
using UdpKit;

public class Tut1ServerCallbacks : Bolt.GlobalEventListener
{
    public override void Connected(BoltConnection connection)
    {
        var log = LogEvent.Create();
        log.Msg = string.Format("{0} connected", connection.RemoteEndPoint);
        log.Send();
    }

    public override void Disconnected(BoltConnection connection)
    {
        var log = LogEvent.Create();
        log.Msg = string.Format("{0} disconnected", connection.RemoteEndPoint);
        log.Send();
    }
}