using System;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

namespace Lemon.EventChannels
{
    [Serializable]
    public class PlayerEventArgs : EventArgs
    {
        public Player m_Player;
        public PlayerEventArgs(Player player)
        {
            m_Player = player;
        }
    }

    public class PlayerPropertiesUpdateEventArgs : EventArgs
    {
        public Player m_Player;
        public Hashtable m_Hashtable;
        public PlayerPropertiesUpdateEventArgs(Player player, Hashtable hashtable)
        {
            m_Player = player;
            m_Hashtable = hashtable;
        }
    }

    public class RoomPropertiesUpdateEventArgs : EventArgs
    {
        public Hashtable m_Hashtable;
        public RoomPropertiesUpdateEventArgs(Hashtable hashtable)
        {
            m_Hashtable = hashtable;
        }
    }


    /// <summary>
    /// This Scriptable Object is used to get various networking information to scripts that are not networked
    /// </summary>
    [CreateAssetMenu(fileName = "PhotonInRoomCallbacksECSO", menuName = "EventChannelSO/PhotonInRoomCallbacksECSO", order = 1)]
    public class PhotonInRoomCallBacksECSO : ScriptableObject, IInRoomCallbacks
    {
        public delegate void MasterClientSwitchedEventHandler(object sender, PlayerEventArgs e);
        public event MasterClientSwitchedEventHandler MasterClientSwitchedEvent;
        public delegate void PlayerEnteredRoomEventHandler(object sender, PlayerEventArgs e);
        public event PlayerEnteredRoomEventHandler PlayerEnteredRoomEvent;
        public delegate void PlayerLeftRoomEventHandler(object sender, PlayerEventArgs e);
        public event PlayerLeftRoomEventHandler PlayerLeftRoomEvent;
        public delegate void PlayerPropertiesUpdateEventHandler(object sender, PlayerPropertiesUpdateEventArgs e);
        public event PlayerPropertiesUpdateEventHandler PlayerPropertiesUpdateEvent;
        public delegate void RoomPropertiesUpdateEventHandler(object sender, RoomPropertiesUpdateEventArgs e);
        public event RoomPropertiesUpdateEventHandler RoomPropertiesUpdateEvent;

        public void OnMasterClientSwitched(Player newMasterClient)
        {
            MasterClientSwitchedEvent?.Invoke(this, new PlayerEventArgs(newMasterClient));
        }

        public void OnPlayerEnteredRoom(Player newPlayer)
        {
            PlayerEnteredRoomEvent?.Invoke(this, new PlayerEventArgs(newPlayer));
        }

        public void OnPlayerLeftRoom(Player otherPlayer)
        {
            PlayerLeftRoomEvent?.Invoke(this, new PlayerEventArgs(otherPlayer));
        }

        public void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            PlayerPropertiesUpdateEvent?.Invoke(this, new PlayerPropertiesUpdateEventArgs(targetPlayer, changedProps));
        }

        public void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
            RoomPropertiesUpdateEvent?.Invoke(this, new RoomPropertiesUpdateEventArgs(propertiesThatChanged));
        }
    }
}
