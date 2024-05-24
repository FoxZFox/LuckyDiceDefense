using System;
using System.Collections.Generic;
using System.Text;

public class Packet
{
    public enum PacketType
    {
        //Client -> Server
        Unknow,
        C2SLogin,
        C2SCreateAccount,
        C2SExit,
        //Server -> Client
        S2CCreateAccount,
        S2CValidAccount,
        S2CExit,
        S2CLogin
    }

    public enum ErrorType
    {
        NoError,
        PlayerAlreadyLoggedIn,
        UsernameExist,
        InvalidAccount,
        UnknowError
    }

    List<byte> buffer;
    int place = 0;

    public Packet()
    {
        buffer = new List<byte>();
    }

    public Packet(int _id)
    {
        buffer = new List<byte>();
        Write(_id);
    }
    public Packet(byte[] _data)
    {
        buffer = new List<byte>();
        SetBytes(_data);
    }
    public void SetBytes(byte[] _data)
    {
        buffer.AddRange(_data);
    }
    public byte[] ToArray()
    {
        return buffer.ToArray();
    }
    public int Length()
    {
        return buffer.Count;
    }
    public void Write(short _data)
    {
        buffer.AddRange(BitConverter.GetBytes(_data));
    }
    public void Write(int _data)
    {
        buffer.AddRange(BitConverter.GetBytes(_data));
    }
    public void Write(float _data)
    {
        buffer.AddRange(BitConverter.GetBytes(_data));
    }
    public void Write(double _data)
    {
        buffer.AddRange(BitConverter.GetBytes(_data));
    }
    public void Write(long _data)
    {
        buffer.AddRange(BitConverter.GetBytes(_data));
    }
    public void Write(string _data)
    {
        Write(_data.Length);
        buffer.AddRange(Encoding.ASCII.GetBytes(_data));
    }

    public short ReadShort(bool _moveReadpos = true)
    {
        if (buffer.Count > place)
        {
            short value = BitConverter.ToInt16(buffer.ToArray(), place);
            if (_moveReadpos)
            {
                place += 2;
            }
            return value;
        }
        else
        {
            throw new Exception("Could not read Short");
        }
    }
    public int ReadInt(bool _moveReadpos = true)
    {
        if (buffer.Count > place)
        {
            int value = BitConverter.ToInt32(buffer.ToArray(), place);
            if (_moveReadpos)
            {
                place += 4;
            }
            return value;
        }
        else
        {
            throw new Exception("Could not read Int");
        }
    }
    public float ReadFloat(bool _moveReadpos = true)
    {
        if (buffer.Count > place)
        {
            float value = BitConverter.ToSingle(buffer.ToArray(), place);
            if (_moveReadpos)
            {
                place += 4;
            }
            return value;
        }
        else
        {
            throw new Exception("Could not read Float");
        }
    }
    public double ReadDouble(bool _moveReadpos = true)
    {
        if (buffer.Count > place)
        {
            double value = BitConverter.ToDouble(buffer.ToArray(), place);
            if (_moveReadpos)
            {
                place += 8;
            }
            return value;
        }
        else
        {
            throw new Exception("Could not read Double");
        }
    }
    public long ReadLong(bool _moveReadpos = true)
    {
        if (buffer.Count > place)
        {
            long value = BitConverter.ToInt64(buffer.ToArray(), place);
            if (_moveReadpos)
            {
                place += 8;
            }
            return value;
        }
        else
        {
            throw new Exception("Could not read Long");
        }
    }
    public string ReadString(bool _moveReadpos = true)
    {
        if (buffer.Count > place)
        {
            int length = ReadInt();
            string value = Encoding.ASCII.GetString(buffer.ToArray(), place, length);
            if (_moveReadpos)
            {
                place += length;
            }
            return value;
        }
        else
        {
            throw new Exception("Could not read String");
        }
    }

    public void Reset()
    {
        buffer.Clear();
        place = 0;
    }
}
