using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Remoting.Contexts;

public class Networkmanager : MonoBehaviour
{
    public static Networkmanager Instant;
    private Client client;
    [SerializeField] private string ipAddress;
    [SerializeField] private int port;
    void Start()
    {
        if (Instant == null)
        {
            DontDestroyOnLoad(gameObject);
            Instant = this;
            client = new Client();
        }
    }

    [ContextMenu("ConnectServer")]
    private async void ConnectServer()
    {
        client.StartConnect(ipAddress, port);
        Packet packet = new Packet((int)Packet.PacketType.C2SLogin);
        packet.Write("test1");
        packet.Write("12345");
        await client.WaitForConnect();
        await PacketHandle.SentLogin(packet);
    }
}
