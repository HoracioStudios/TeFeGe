using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerializableClasses
{ 

}

[Serializable]
public class User
{
    public string nick;
    public string email;
    public string password;
    public int playerID;
    public Result result;
    public float waitTime;
}

[Serializable]
public class Response
{
    public int code;
    public bool reply;
    public string message;
    public int ID;
    public Data data;
}

[Serializable]
public class Data
{
    public int _id;
    public string password;
    //salt: 0
    public string email;
    //history: 0;
    public int pending;
    public string creation;
    public string lastLogin;
}

[Serializable]
public class StatusInfo
{
    public int code;
    public int[] onlineUsers;
}

[Serializable]
public class Result
{
    public float[] round;
}

[Serializable]
public class Rival
{
    public int code;
    public bool found;
    public bool finished;
    public int rivalID;
    public string rivalNick;
    public string message;
}
