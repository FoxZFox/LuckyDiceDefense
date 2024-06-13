using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacketManager
{
    private delegate void PacketHandle(Packet packet);
    private Dictionary<int, PacketHandle> packetHandles;

    public void Initialize()
    {
        packetHandles = new Dictionary<int, PacketHandle>()
        {
            {(int)Packet.PacketType.S2CLogin,ClientLogin},
            {(int)Packet.PacketType.S2CValidAccount,ValidAccount}
        };

    }
    public void GetPacket(Packet packet)
    {
        int type = packet.ReadInt();
        packetHandles[type](packet);
    }
    private void ClientLogin(Packet packet)
    {
        int id = packet.ReadInt();
        int gold = packet.ReadInt();
        int gem = packet.ReadInt();
        Client.instant.account.ID = id;
        Client.instant.account.Gold = gold;
        Client.instant.account.Gem = gem;
        Debug.Log($"ID = {id} Gold = {gold} Gem = {gem}");
        UnityMainThread.Instant.Enqueue(() => LoginController.Instant.PlayConnectStatus());
    }

    private void ValidAccount(Packet packet)
    {
        Packet.ErrorType errorType = (Packet.ErrorType)packet.ReadInt();
        Debug.Log(errorType.ToString());
        Client.instant.Disconnect();
        UnityMainThread.Instant.Enqueue(() =>
        {
            LoginController.Instant.PlayErrorLoginAnimation(errorType.ToString());
        });
    }
}
