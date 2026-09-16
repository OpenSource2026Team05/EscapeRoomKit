using UnityEngine;
using static PlasticPipe.PlasticProtocol.Client.ConnectionCreator.PlasticProtoSocketConnection;

namespace EscapeRoomKit
{
    public class Object_Door : Door, IRayInteractable
    {
        public void OnRayClick()
        {
            if (isOpen) CloseDoor();
            else OpenDoor();
        }
    }
}
