using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;

enum MESSAGE { LOGIN, SIGNIN, GETELO, AVAILABLENICK, AVAILABLEEMAIL
        , DELETEACC, ONLINEUSERS, GETINFO, SENDROUND, SEARCHPAIR, LEAVEQ, VERSION }

public class ClientCommunication
{
    static string[] messages = { "/login", "/signin", "/getelo", "/available/nick/?nick="
            , "/available/email/?email=", "/deleteAccount"
            , "/petition/onlineUsers", "/petition/getInfo/?playerID=", "/sendRoundInfo"
            , "/searchPair", "/leaveQueue", "/version" };
    static string ip = "localhost";
    static string puerto = "25565";

    // 1 Everything okay     -1 Server Down || WrongPassword || No Player with that nick or email
    public static int LogIn(string username, string password)
    {
        string url = "http://" + ip + ":" + puerto + messages[(int)MESSAGE.LOGIN];
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        User user = new User();
        user.nick = username;
        user.password = password;
        string json = JsonUtility.ToJson(user);
        Response response = JsonUtility.FromJson<Response>(Post(json, url));

        if (response.code == 200)
            return response.ID;
        if (response.code == 500)
        {
            Debug.Log("Error code " + response.code.ToString() + ": " + response.message);
            return -1;
        }
        Debug.Log(response.message);
        return -1;
    }

    // 1 Everything okay     0 Something wrong     -1 Server Down
    public static int SignIn(string username, string email, string password)
    {
        string url = "http://" + ip + ":" + puerto + messages[(int)MESSAGE.SIGNIN];
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        User user = new User();
        user.nick = username;
        user.email = email;
        user.password = password;
        string json = JsonUtility.ToJson(user);
        Response response = JsonUtility.FromJson<Response>(Post(json, url));

        switch (response.code)
        {
            case 200:
                return 1;
            case 500:
                Debug.Log("Error code " + response.code.ToString() + ": " + response.message);
                return -1;
            default:
                break;
        }
        Debug.Log(response.message);
        return 0;
    }


    /*public static string StartQueue(int id)
    {
        string url = "http://" + ip + ":" + puerto + messages[(int)MESSAGE.STARTQ];
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        User user = new User();
        user.playerID = id;
        string json = JsonUtility.ToJson(user);
        return Post(json, url);
    }

    public static string CancelQueue(int id)
    {
        string url = "http://" + ip + ":" + puerto + messages[(int)MESSAGE.CANCELQ];
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        User user = new User();
        user.playerID = id;
        string json = JsonUtility.ToJson(user);
        return Post(json, url);
    }

    public static string GetELO(int id)
    {
        string url = "http://" + ip + ":" + puerto + messages[(int)MESSAGE.GETELO];
        User user = new User();
        user.playerID = id;
        string json = JsonUtility.ToJson(user);
        return Post(json, url);
    }*/

    // 1 Nick available     0 Nick not available     -1 Server Down
    public static int GetNickAvailable(string nick)
    {
        string url = "http://" + ip + ":" + puerto + messages[(int)MESSAGE.AVAILABLENICK] + nick;
        Response response = JsonUtility.FromJson<Response>(Get(url));

        if (response.code == 200)
            return Convert.ToInt32(response.reply);


        Debug.Log("Error code " + response.code.ToString() + ": " + response.message);
        return -1;       
    }

    // 1 Email available     0 Email not available     -1 Server Down
    public static int GetEmailAvailable(string email)
    {
        string url = "http://" + ip + ":" + puerto + messages[(int)MESSAGE.AVAILABLEEMAIL] + email;
        Response response = JsonUtility.FromJson<Response>(Get(url));

        if (response.code == 200)
            return Convert.ToInt32(response.reply);


        Debug.Log("Error code " + response.code.ToString() + ": " + response.message);
        return -1;
    }

    // 1 Everything okay     0 Somehitng wrong      -1 Server Down
    public static int DeleteAccount(string username, string email, string password)
    {
        string url = "http://" + ip + ":" + puerto + messages[(int)MESSAGE.DELETEACC];
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        User user = new User();
        user.nick = username;
        user.email = email;
        user.password = password;
        string json = JsonUtility.ToJson(user);
        Response response = JsonUtility.FromJson<Response>(Post(json, url));

        if (response.code == 200)
            return 1;
        if (response.code == 500)
        {
            Debug.Log("Error code " + response.code.ToString() + ": " + response.message);
            return -1;
        }
        Debug.Log(response.message);
        return 0;
    }

    public static int[] OnlineUsers()
    {
        string url = "http://" + ip + ":" + puerto + messages[(int)MESSAGE.ONLINEUSERS];
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

        StatusInfo statusInfo = JsonUtility.FromJson<StatusInfo>(Get(url));
        if (statusInfo.code == 200)
            return statusInfo.onlineUsers;

        Debug.Log("Codigo de error: " + statusInfo.code.ToString());
        return null;
    }

    public static Data GetInfo(int id, string nick)
    {
        string url = "http://" + ip + ":" + puerto + messages[(int)MESSAGE.GETINFO] + id.ToString() + "&playerNick=" + nick;
        Response response = JsonUtility.FromJson<Response>(Get(url));
        switch (response.code)
        {
            case 200:
                return response.data;
            case 400:
            case 500:
                Debug.Log("Error code " + response.code.ToString() + ": " + response.message);
                break;
        }
        return null;
    }

    // 1 Send everything okay      0 Something went wrong
    public static int SendRoundInfo(string nick, string email, string password, float[] round)
    {
        string url = "http://" + ip + ":" + puerto + messages[(int)MESSAGE.SENDROUND];
        User user = new User();
        user.nick = nick;
        user.email = email;
        user.password = password;
        user.result = new Result();
        user.result.round = round;
        string json = JsonUtility.ToJson(user);

        Response response = JsonUtility.FromJson<Response>(Post(json, url));
        switch (response.code)
        {
            case 200:
                return 1;
            case 400:
            case 500:
                Debug.Log("Error code " + response.code.ToString() + ": " + response.message);
                break;
        }
        return 0;
    }

    public static Rival SearchPair(int id, float waitTime)
    {
        string url = "http://" + ip + ":" + puerto + messages[(int)MESSAGE.SEARCHPAIR];
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        User user = new User();
        user.playerID = id;
        string json = JsonUtility.ToJson(user);

        Rival response = JsonUtility.FromJson<Rival>(Post(json, url));
        switch (response.code)
        {
            case 200:
                return response;
            case 400:
            case 500:
                Debug.Log("Error code " + response.code.ToString() + ": " + response.message);
                break;
        }
        return null;
    }

    public static int LeaveQueue(int id)
    {
        string url = "http://" + ip + ":" + puerto + messages[(int)MESSAGE.LEAVEQ];
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        User user = new User();
        user.playerID = id;
        string json = JsonUtility.ToJson(user);

        Response response = JsonUtility.FromJson<Response>(Post(json, url));
        switch (response.code)
        {
            case 200:
                return 1;
            case 400:
            case 500:
                Debug.Log("Error code " + response.code.ToString() + ": " + response.message);
                break;
        }
        return 0;
    }

    public static string GetVersion()
    {
        string url = "http://" + ip + ":" + puerto + messages[(int)MESSAGE.VERSION];
        return Get(url);
    }

    private static string Post(string json, string url)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "POST";
        request.ContentType = "application/json";
        request.Accept = "application/json";

        using (StreamWriter streamWriter = new StreamWriter(request.GetRequestStream()))
        {
            streamWriter.Write(json);
            streamWriter.Flush();
            streamWriter.Close();
        }

        try
        {
            using (WebResponse response = request.GetResponse())
            {
                using (Stream strReader = response.GetResponseStream())
                {
                    if (strReader == null) return "";
                    using (StreamReader objReader = new StreamReader(strReader))
                    {
                        string responseBody = objReader.ReadToEnd();

                        return responseBody;
                    }
                }
            }
        }
        catch (WebException ex)
        {
            using (WebResponse response = ex.Response)
            {
                using (Stream strReader = response.GetResponseStream())
                {
                    if (strReader == null) return "";
                    using (StreamReader objReader = new StreamReader(strReader))
                    {
                        string responseBody = objReader.ReadToEnd();

                        return responseBody;
                    }
                }
            }
        }
    }

   
    private static string Get(string url)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "GET";
        request.ContentType = "application/json";
        request.Accept = "application/json";

        try
        {
            using (WebResponse response = request.GetResponse())
            {
                using (Stream strReader = response.GetResponseStream())
                {
                    if (strReader == null) return "";
                    using (StreamReader objReader = new StreamReader(strReader))
                    {
                        string responseBody = objReader.ReadToEnd();
                        return responseBody;
                    }
                }
            }
        }
        catch (WebException ex)
        {
            using (WebResponse response = ex.Response)
            {
                using (Stream strReader = response.GetResponseStream())
                {
                    if (strReader == null) return "";
                    using (StreamReader objReader = new StreamReader(strReader))
                    {
                        string responseBody = objReader.ReadToEnd();

                        return responseBody;
                    }
                }
            }
        }
    }


}
