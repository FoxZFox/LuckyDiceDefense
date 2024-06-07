using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using static Packet;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.UIElements;

public class Client
{
    public static Client instant;
    public Account account;
    private TcpClient client;
    private NetworkStream stream;
    public bool IsConnect;
    private byte[] buffer;
    protected int MAX_BUFFER_SIZE = 4096;
    private Queue<Packet> packetque = new Queue<Packet>();
    private bool startCon = false;
    public bool StartCon => startCon;
    public Client()
    {
        buffer = new byte[MAX_BUFFER_SIZE];
        instant = this;
        account = new Account();
    }

    public void StartConnect(string ip, int port)
    {
        client = new TcpClient();
        startCon = true;
        client.BeginConnect(ip, port, new AsyncCallback(ConnectCallback), null);
    }

    private void ConnectCallback(IAsyncResult result)
    {
        try
        {
            client.EndConnect(result);
            IsConnect = client.Client.Connected;
            if (!IsConnect)
            {
                Debug.LogError("Can't connect to server");
                return;
            }
            stream = client.GetStream();
            Debug.Log("Connect to server");
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
            Disconnect();
        }
        finally
        {
            startCon = false;
        }
        TryReadPacket();
    }

    private void TryReadPacket()
    {
        while (IsConnect)
        {
            if (stream.DataAvailable)
                stream.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(OnReciveCallback), null);
        }
    }
    private void OnReciveCallback(IAsyncResult result)
    {
        try
        {
            if (IsConnect)
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
        while (IsConnect)
        {
            if (packetque.Count > 0)
            {
                break;
            }
            await Task.Delay(1000);
        }
        return packetque.Dequeue();
    }

    public bool CheckPacket()
    {
        if (packetque.Count > 0)
        {
            return true;
        }
        return false;
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
        while (!IsConnect)
        {
            if (!startCon)
            {
                return false;
            }
            await Task.Delay(100);
        }
        return IsConnect;
    }

    public void Disconnect()
    {
        stream.Close();
        client.Close();
        IsConnect = client.Client.Connected;
        Debug.Log("Close connect");
    }
}

public class Account
{
    public int ID { get; set; }
    public int Gold { get; set; }
    public int Gem { get; set; }
}
