using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacketManager
{
    public void GetPacket(Packet packet)
    {
        Packet.PacketType packetType = (Packet.PacketType)packet.ReadInt();
        switch (packetType)
        {
            case Packet.PacketType.S2CLogin:
                ClientLogin(packet);
                break;
            case Packet.PacketType.S2CValidAccount:
                ValidAccount(packet);
                break;

        }
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
    }

    private void ValidAccount(Packet packet)
    {
        Packet.ErrorType errorType = (Packet.ErrorType)packet.ReadInt();
        Debug.Log(errorType.ToString());
        Client.instant.Disconnect();
        LoginController.Instant.PlayErrorLoginAnimation(errorType.ToString());
    }
}
