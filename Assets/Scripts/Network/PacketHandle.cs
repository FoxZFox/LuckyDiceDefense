using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PacketHandle
{
    public static async Task<Packet> SentLogin(Packet packet)
    {
        Client.instant.TrySentPacket(packet);
        Packet result = await Client.instant.GetPacket();
        Debug.Log("Recive Data");
        return result;
    }
}
