using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using static Packet;
using UnityEngine;
using System.Threading.Tasks;

public class Client
{
    public static Client instant;
    private TcpClient client;
    private NetworkStream stream;
    private bool isConnect => client.Client.Connected;
    private byte[] buffer;
    protected int MAX_BUFFER_SIZE = 4096;
    private Queue<Packet> packetque = new Queue<Packet>();
    public Client()
    {
        client = new TcpClient();
        buffer = new byte[MAX_BUFFER_SIZE];
        instant = this;
    }

    public void StartConnect(string ip, int port)
    {
        client.BeginConnect(ip, port, new AsyncCallback(ConnectCallback), null);
    }

    private void ConnectCallback(IAsyncResult result)
    {
        try
        {
            client.EndConnect(result);
            if (!isConnect)
            {
                Debug.LogError("Can't connect to server");
                return;
            }
            stream = client.GetStream();
            Debug.Log("Connect to server");
            TryReadPacket();
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    private void TryReadPacket()
    {
        while (true)
        {
            if (stream.DataAvailable)
                stream.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(OnReciveCallback), null);
        }
    }
    private void OnReciveCallback(IAsyncResult result)
    {
        try
        {
            if (isConnect)
            {
                int size = stream.EndRead(result);
                if (size > 0)
                {
                    Packet packet = new Packet(buffer);
                    ValidPacket(packet);
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    private void ValidPacket(Packet packet)
    {
        if (Enum.IsDefined(typeof(PacketType), packet.ReadInt(false)))
        {
            packetque.Enqueue(packet);
        }
        else
        {
            Debug.LogError("Packet is invalid");
        }
    }

    public async Task<Packet> GetPacket()
    {
        while (true)
        {
            if (packetque.Count > 0)
            {
                break;
            }
            await Task.Yield();
        }
        return packetque.Dequeue();
    }

    public void TrySentPacket(Packet packet)
    {
        try
        {
            stream.BeginWrite(packet.ToArray(), 0, packet.Length(), new AsyncCallback(CompletePacketSend), packet);
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
        }

    }
    private void CompletePacketSend(IAsyncResult result)
    {
        Packet packet = (Packet)result.AsyncState;
        stream.EndWrite(result);
        Debug.Log($"Sent packet {packet.ReadPacketName()}");
    }

    public async Task<bool> WaitForConnect()
    {
        while (!isConnect)
        {
            await Task.Yield();
        }
        return isConnect;
    }
}

public class Account
{
    public int ID { get { if (ID <= 0) { return -1; } return ID; } set { if (value <= 0) { ID = -1; } if (ID != 0) { return; } ID = value; } }

}
