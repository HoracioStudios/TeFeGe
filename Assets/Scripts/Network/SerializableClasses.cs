using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;





//public class SerializableClasses
//{

//}

//[Serializable]
//public class User
//{
//    public string nick;
//    public string email;
//    public string password;
//    public int playerID;
//    public string[] results;
//    public float waitTime;
//}

//[Serializable]
//public class Response
//{
//    public int ID;
//    public Data data;
//}

//[Serializable]
//public class Availability
//{
//    public bool reply;
//}


//[Serializable]
//public class Data
//{
//    public int _id;
//    public string password;
//    public string email;
//    //history: 0;
//    public int pending;
//    public string creation;
//    public string lastLogin;
//}

//[Serializable]
//public class StatusInfo
//{
//    public int[] onlineUsers;
//}


//[Serializable]
//public class Rival
//{
//    public bool found;
//    public bool finished;
//    public int rivalID;
//    public string rivalNick;
//    public string message;
//}

[Serializable]
public class LoginInfo
{
    public string nick;
    public string email;
    public string password;
}

[Serializable]
public class RoundResult
{
    public RoundResult(float res, float t) { result = res; time = t; }

    public float result;
    public float time;
}

[Serializable]
public class GameData
{
    public RoundResult[] rounds;

    public string matchID;

    public int rivalID = 0;

    public string playerChar;

    public string rivalChar;

    public int shotsFired = 0;

    public float dmgDealt = 0;

    public float accuracy = 0;

}

[Serializable]
public class RefreshData
{
    public string refreshToken;
}

[Serializable]
public class RefreshMessage : Message
{
    public string accessToken;
}

[Serializable]
public class REST_Error : Message
{
    public string message;
}

[Serializable]
public class Login : Message
{
    public int id;
    public string accessToken;
    public string refreshToken;
}

[Serializable]
public class Available : Message
{
    public bool emailAvailable = false;
    public bool nickAvailable = false;
}

[Serializable]
public class PairSearch : Message
{
    public bool found = false;
    public bool finished = false;
    public int rivalID = -1;
    public string rivalNick = "";
}

[Serializable]
public class GameEndMessage : Message
{
    public GameData results;

}

[Serializable]
public class Message
{
    public int code = 0;
}
