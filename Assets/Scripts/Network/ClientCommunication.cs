using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

enum MESSAGE { LOGIN, SIGNIN, GETELO, AVAILABLENICK, AVAILABLEEMAIL
        , DELETEACC, ONLINEUSERS, GETINFO, SENDROUND, SEARCHPAIR, LEAVEQ, VERSION }

public class ClientCommunication
{
    static int code_;
    static string message_;

    static string[] messages = { "/login", "/signin", "/getelo", "/available/nick/?nick="
            , "/available/email/?email=", "/deleteAccount"
            , "/petition/onlineUsers", "/petition/getInfo/?playerID=", "/sendRoundInfo"
            , "/searchPair", "/leaveQueue", "/version" };

    static string ip = "localhost";
    static string puerto = "25565";

    //Devuelve el id del usuario
    public static int LogIn(string password, string username = null, string email = null)
    {
        string url = "http://" + ip + ":" + puerto + messages[(int)MESSAGE.LOGIN];
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        User user = new User();
        user.nick = username;
        user.password = password;
        user.email = email;
        string json = JsonUtility.ToJson(user);
        int id = JsonUtility.FromJson<Response>(Post(json, url, out code_, out message_)).ID;

        if(code_ == 200)
            return id;

        throw new RestResponseException(message_, code_);
    }


    public static void SignIn(string password, string username = null, string email= null)
    {
        string url = "http://" + ip + ":" + puerto + messages[(int)MESSAGE.SIGNIN];
        User user = new User();
        user.nick = username;
        user.email = email;
        user.password = password;
        string json = JsonUtility.ToJson(user);
        Post(json, url, out code_, out message_);

        if (code_ != 200)
            throw new RestResponseException(message_, code_);
    }

    // 1 si esta disponible 0 si no lo esta
    public static int GetNickAvailable(string nick)
    {
        string url = "http://" + ip + ":" + puerto + messages[(int)MESSAGE.AVAILABLENICK] + nick;
        Availability response = JsonUtility.FromJson<Availability>(Get(url, out code_, out message_));

        if (code_ == 200)
            return Convert.ToInt32(response.reply);

        throw new RestResponseException(message_, code_);
    }

    // 1 Email available     0 Email not available 
    public static int GetEmailAvailable(string email)
    {
        string url = "http://" + ip + ":" + puerto + messages[(int)MESSAGE.AVAILABLEEMAIL] + email;
        Availability response = JsonUtility.FromJson<Availability>(Get(url, out code_, out message_));

        if (code_ == 200)
            return Convert.ToInt32(response.reply);

        throw new RestResponseException(message_, code_);
    }

    // Throw and exception if can't delete the acc
    public static void DeleteAccount(string password, string username = null, string email = null)
    {
        string url = "http://" + ip + ":" + puerto + messages[(int)MESSAGE.DELETEACC];
        User user = new User();
        user.nick = username;
        user.email = email;
        user.password = password;
        string json = JsonUtility.ToJson(user);
        Post(json, url, out code_, out message_);

        if (code_ != 200)
            throw new RestResponseException(message_, code_);
    }

    public static int[] OnlineUsers()
    {
        string url = "http://" + ip + ":" + puerto + messages[(int)MESSAGE.ONLINEUSERS];

        StatusInfo statusInfo = JsonUtility.FromJson<StatusInfo>(Get(url, out code_, out message_));
        if (code_ == 200)
            return statusInfo.onlineUsers;
        
        Debug.Log("Error code " + code_.ToString() + ": " + message_);
        throw new RestResponseException(message_, code_);
    }

    public static Data GetInfo(int id, string nick)
    {
        string url = "http://" + ip + ":" + puerto + messages[(int)MESSAGE.GETINFO] + id.ToString() + "&playerNick=" + nick;
        Response response = JsonUtility.FromJson<Response>(Get(url, out code_, out message_));

        if(code_ == 200)
            return response.data;

        throw new RestResponseException(message_, code_);
    }


    public static void SendRoundInfo(string password, string[] results, string nick = null, string email = null)
    {
        string url = "http://" + ip + ":" + puerto + messages[(int)MESSAGE.SENDROUND];
        User user = new User();
        user.nick = nick;
        user.email = email;
        user.password = password;
        user.results = results;
        string json = JsonUtility.ToJson(user);

        Post(json, url, out code_, out message_);

        if (code_ != 200)
            throw new RestResponseException(message_, code_);
    }

    public static Rival SearchPair(int id, float waitTime)
    {
        string url = "http://" + ip + ":" + puerto + messages[(int)MESSAGE.SEARCHPAIR];
        User user = new User();
        user.playerID = id;
        user.waitTime = waitTime;
        string json = JsonUtility.ToJson(user);

        Rival response = JsonUtility.FromJson<Rival>(Post(json, url, out code_, out message_));

        if (code_ == 200)
            return response;

        throw new RestResponseException(message_, code_);
    }

    public static void LeaveQueue(int id)
    {
        string url = "http://" + ip + ":" + puerto + messages[(int)MESSAGE.LEAVEQ];
        User user = new User();
        user.playerID = id;
        string json = JsonUtility.ToJson(user);

        Post(json, url, out code_, out message_);
        if (code_ != 200)
            throw new RestResponseException(message_, code_);
    }

    public static string GetVersion()
    {
        string url = "http://" + ip + ":" + puerto + messages[(int)MESSAGE.VERSION];
        string version = Get(url, out code_, out message_);

        if(code_ == 200)
            return version;

        throw new RestResponseException(message_, code_);
    }

    private static string Post(string json, string url, out int code, out string message)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "POST";
        request.ContentType = "application/json";
        request.Accept = "application/json";

        code = -1;
        message = "Message lost";
        ServerCode serverResponse;

        try
        {
            using (StreamWriter streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }
        }
        catch (SocketException ex)
        {
            Debug.Log(ex.Message);
            return "";
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
                        serverResponse = JsonUtility.FromJson<ServerCode>(responseBody);
                        code = serverResponse.code;
                        message = serverResponse.message;
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
                        serverResponse = JsonUtility.FromJson<ServerCode>(responseBody);
                        code = serverResponse.code;
                        message = serverResponse.message;
                        return responseBody;
                    }
                }
            }
        }
        catch (SocketException e)
        {
            Debug.Log("xi");
            message = e.Message;
            return "";
        }
    }

   
    private static string Get(string url, out int code, out string message)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "GET";
        request.ContentType = "application/json";
        request.Accept = "application/json";

        code = -1;
        message = "Message lost";
        ServerCode serverResponse;

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
                        serverResponse = JsonUtility.FromJson<ServerCode>(responseBody);
                        code = serverResponse.code;
                        message = serverResponse.message;
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
                        serverResponse = JsonUtility.FromJson<ServerCode>(responseBody);
                        code = serverResponse.code;
                        message = serverResponse.message;
                        return responseBody;
                    }
                }
            }
        }
        catch (SocketException e)
        {
            Debug.Log("xi");
            message = e.Message;
            return "";
        }
    }
}
