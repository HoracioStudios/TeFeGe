﻿using System;
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
    public string[] results;
    public float waitTime;
}

[Serializable]
public class Response
{
    public int ID;
    public Data data;
}

[Serializable]
public class Availability
{
    public bool reply;
}


[Serializable]
public class Data
{
    public int _id;
    public string password;
    public string email;
    //history: 0;
    public int pending;
    public string creation;
    public string lastLogin;
}

[Serializable]
public class StatusInfo
{
    public int[] onlineUsers;
}


[Serializable]
public class Rival
{
    public bool found;
    public bool finished;
    public int rivalID;
    public string rivalNick;
    public string message;
}

[Serializable]
public class ServerCode
{
    public int code;
    public string message;
}