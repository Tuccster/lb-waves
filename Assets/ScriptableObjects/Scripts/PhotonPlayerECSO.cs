using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace Lemon.EventChannels
{
    [CreateAssetMenu(fileName = "PhotonPlayerECSO", menuName = "EventChannelSO/PhotonPlayerECSO", order = 2)]
    public class PhotonPlayerECSO : ScriptableObject
    {
        [Header("Settings")]
        public bool m_Networked;
        public ReceiverGroup m_ReceiverGroup;
        public const byte PHOTON_PLAYER_ESCO = 1; // Probably should be centrallized (automatically assigned?)

        public delegate void PhotonPlayerEventHandler(object sender, PlayerEventArgs args);
        public event PhotonPlayerEventHandler PhotonPlayerEvent;

        public void RaiseEvent(object sender, PlayerEventArgs args)
        {
            // We cannot send PlayerEventArgs over the network without creating methods for serializing and deserialzing it as well as registering it.
            // https://doc.photonengine.com/en-US/realtime/current/reference/serialization-in-photon

            if (!m_Networked)
                PhotonPlayerEvent?.Invoke(sender, args);
            else
            {
                object[] content = new object[] { sender, args };
                RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = m_ReceiverGroup };
                PhotonNetwork.RaiseEvent(PHOTON_PLAYER_ESCO, content, raiseEventOptions, ExitGames.Client.Photon.SendOptions.SendReliable);
            }
        }
    }
}
