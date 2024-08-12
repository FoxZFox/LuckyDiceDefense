using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Networkmanager : MonoBehaviour
{
    public static Networkmanager Instant;

    private Client client;
    [Header("Network")]
    [SerializeField] private bool stanalone = false;
    [SerializeField] private string ipAddress;
    [SerializeField] private int port;
    [Header("UI-Link")]
    private PacketManager packetManager;
    private string id, pass;
    public bool StanAlone => stanalone;
    void Start()
    {
        if (Instant == null)
        {
            Instant = this;
            if (!stanalone)
            {
                DontDestroyOnLoad(gameObject);
                client = new Client();
                packetManager = new PacketManager();
                packetManager.Initialize();
            }
            else
            {
                SceneManager.Instant.LoadScene(SceneManager.sceneName.mainmenu);
            }
        }
    }

    void OnApplicationQuit()
    {
        if (!stanalone)
        {
            client.Disconnect();
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
