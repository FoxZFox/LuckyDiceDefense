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
    private string id, pass;
    void Start()
    {
        if (Instant == null)
        {
            DontDestroyOnLoad(gameObject);
            Instant = this;
            client = new Client();
            packetManager = new PacketManager();
            packetManager.Initialize();
        }
    }

    public void Login(string user, string pass)
    {
        id = user;
        this.pass = pass;
        ConnectServer();
    }
    [ContextMenu("ConnectServer")]
    private void ConnectServer()
    {
        if (!client.IsConnect)
            client.StartConnect(ipAddress, port);

    }
    public void SentLoginPacket()
    {
        Packet packet = new Packet((int)Packet.PacketType.C2SLogin);
        packet.Write(id);
        packet.Write(pass);
        client.TrySentPacket(packet);
    }

    public void HandlePacket(Packet packet)
    {
        packetManager.GetPacket(packet);
    }
}
