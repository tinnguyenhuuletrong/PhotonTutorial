using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


static class PlayerManager
{
    static List<PlayerObject> players = new List<PlayerObject>();

    // create a player for a connection
    // note: connection can be null
    static PlayerObject CreatePlayer(BoltConnection connection)
    {
        PlayerObject p;

        // create a new player object, assign the connection property
        // of the object to the connection was passed in
        p = new PlayerObject();
        p.Connection = connection;

        // if we have a connection, assign this player
        // as the user data for the connection so that we
        // always have an easy way to get the player object
        // for a connection
        if (p.Connection != null)
        {
            p.Connection.UserData = p;
        }

        // add to list of all players
        players.Add(p);

        return p;
    }

    // this simply returns the 'players' list cast to
    // an IEnumerable<T> so that we hide the ability
    // to modify the player list from the outside.
    public static IEnumerable<PlayerObject> AllPlayers
    {
        get { return players; }
    }

    // finds the server player by checking the
    // .isServer property for every player object.
    public static PlayerObject ServerPlayer
    {
        get { return players.First(x => x.IsServer); }
    }

    // utility function which creates a server player
    public static PlayerObject CreateServerPlayer()
    {
        return CreatePlayer(null);
    }

    // utility that creates a client player object.
    public static PlayerObject CreateClientPlayer(BoltConnection connection)
    {
        return CreatePlayer(connection);
    }

    // utility function which lets us pass in a
    // BoltConnection object (even a null) and have
    // it return the proper player object for it.
    public static PlayerObject FindPlayerFromConnection(BoltConnection connection)
    {
        if (connection == null)
        {
            return ServerPlayer;
        }

        return (PlayerObject)connection.UserData;
    }
}

