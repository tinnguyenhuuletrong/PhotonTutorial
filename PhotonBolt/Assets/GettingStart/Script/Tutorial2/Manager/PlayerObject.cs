using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class PlayerObject
{
    public BoltConnection Connection { get; set; }
    public BoltEntity Entity { get; set; }

    public bool IsServer
    {
        get
        {
            return Connection == null;
        }
    }

    public bool IsClient
    {
        get
        {
            return Connection != null;
        }
    }

    public void Spawn()
    {
        if (!Entity)
        {
            Entity = BoltNetwork.Instantiate(BoltPrefabs.Character);

            if (IsServer)
            {
                Entity.TakeControl();
            }
            else
            {
                Entity.AssignControl(Connection);
            }
        }

        // teleport entity to a random spawn position
        Entity.transform.position = RandomPosition();
    }

    Vector3 RandomPosition()
    {
        return new Vector3(Random.Range(-16, 16), 4, Random.Range(-16, 16));
    }
}

