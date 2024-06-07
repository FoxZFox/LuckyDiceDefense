using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Runtime.Remoting.Contexts;
using UnityEngine.UI;

public class Networkmanager : MonoBehaviour
{
    public static Networkmanager Instant;
    private Client client;
    [Header("Network")]
    [SerializeField] private string ipAddress;
    [SerializeField] private int port;
    [Header("UI-Link")]
    private PacketManager packetManager;
    void Start()
    {
        if (Instant == null)
        {
            DontDestroyOnLoad(gameObject);
            Instant = this;
            client = new Client();
            packetManager = new PacketManager();
        }
    }

    public void Login(string user, string pass)
    {
        ConnectServer(user, pass);
    }
    [ContextMenu("ConnectServer")]
    private async void ConnectServer(string id, string pass)
    {
        if (!client.IsConnect)
            client.StartConnect(ipAddress, port);
        if (await client.WaitForConnect())
        {
            Packet packet = new Packet((int)Packet.PacketType.C2SLogin);
            packet.Write(id);
            packet.Write(pass);
            Packet packet1 = await PacketHandle.SentLogin(packet);
            packetManager.GetPacket(packet1);
        }
        else
        {
            Debug.LogError("Not Connect");
        }

    }
}
