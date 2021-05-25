using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class ClientCommunication
{
    const string IP = "localhost";
    const string PORT = "25565";
    const string PORT_GAME_SERVER = "25564";
    const string URL = "http://" + IP + ":" + PORT;
    const string URL_GAME = "http://" + IP + ":" + PORT_GAME_SERVER;

    static string authToken = "";
    static string refreshToken = "";

    //Devuelve el id del usuario
    public static Message LogIn(string password, string username)
    {
        string url = URL + "/accounts/sessions";

        LoginInfo user = new LoginInfo();
        user.nick = username;
        user.password = password;
        string json = JsonUtility.ToJson(user);

        int code;

        try
        {
            var reply = Post(json, url, out code);
            //var reply = Post(json, url, out code, out message);


            if (code != 200)
            {
                REST_Error message = new REST_Error();

                if (code < 0)
                    message.message = "Error de socket, no se puede abrir una conexión";
                else
                {
                    try
                    {
                        message = JsonUtility.FromJson<REST_Error>(reply);
                    }
                    catch (Exception e)
                    {
                        message.message = reply;
                    }
                }

                message.code = code;

                return message;
            }
            else
            {
                Login message = new Login();

                message = JsonUtility.FromJson<Login>(reply);

                authToken = message.accessToken;
                refreshToken = message.refreshToken;

                message.code = code;

                return message;
            }
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    //Devuelve el id del usuario
    public static Message LogOut()
    {
        string url = URL + "/accounts/sessions";

        RefreshData user = new RefreshData();
        user.refreshToken = refreshToken;
        string json = JsonUtility.ToJson(user);

        int code;

        try
        {
            var reply = Delete(json, url, out code, true);
            //var reply = Post(json, url, out code, out message);
            REST_Error message = new REST_Error();

            if (code < 0)
                message.message = "Error de socket, no se puede abrir una conexión";
            else
            {
                try
                {
                    message = JsonUtility.FromJson<REST_Error>(reply);
                }
                catch (Exception e)
                {
                    message.message = reply;
                }
            }

            message.code = code;

            return message;
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public static Message SignIn(string password, string username, string email)
    {
        string url = URL + "/accounts";

        LoginInfo user = new LoginInfo();
        user.nick = username;
        user.password = password;
        user.email = email;
        string json = JsonUtility.ToJson(user);

        int code;

        try
        {
            var reply = Post(json, url, out code);
            //var reply = Post(json, url, out code, out message);

            REST_Error message = new REST_Error();

            if (code < 0)
                message.message = "Error de socket, no se puede abrir una conexión";
            else if(code != 200)
            {
                try
                {
                    message = JsonUtility.FromJson<REST_Error>(reply);
                }
                catch (Exception e)
                {
                    message.message = reply;
                }
            }

            message.code = code;

            return message;
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    // 1 si esta disponible 0 si no lo esta
    public static Message GetAvailable(string nick = "", string email = "")
    {
        string url = URL + "/accounts/check-availability/?";

        if (nick != "")
        {
            url += "nick=" + nick;

            if (email != "")
                url += "&";
        }

        if (email != "")
            url += "email=" + email;

        int code;

        try
        {
            var reply = Get(url, out code);
            //var reply = Post(json, url, out code, out message);

            if (code != 200)
            {
                REST_Error message = new REST_Error();

                if (code < 0)
                    message.message = "Error de socket, no se puede abrir una conexión";
                else
                {
                    try
                    {
                        message = JsonUtility.FromJson<REST_Error>(reply);
                    }
                    catch (Exception e)
                    {
                        message.message = reply;
                    }
                }

                message.code = code;

                return message;
            }
            else
            {
                Available message = new Available();

                message = JsonUtility.FromJson<Available>(reply);

                message.code = code;

                return message;
            }
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public static Message SendRoundInfo(GameData gameData)
    {
        string url = URL + "/accounts/rounds";

        GameEndMessage msg = new GameEndMessage();
        msg.code = 0;
        msg.results = gameData;
        string json = JsonUtility.ToJson(msg);

        int code;

        try
        {
            var reply = Post(json, url, out code, true);
            //var reply = Post(json, url, out code, out message);

            REST_Error message = new REST_Error();

            if (code < 0)
                message.message = "Error de socket, no se puede abrir una conexión";
            else if (code != 200)
            {
                try
                {
                    message = JsonUtility.FromJson<REST_Error>(reply);
                }
                catch (Exception e)
                {
                    message.message = reply;
                }
            }

            message.code = code;

            return message;
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public static Message AddToQueue()
    {
        string url = URL + "/matchmaking";

        Message msg = new Message();
        msg.code = 0;
        string json = JsonUtility.ToJson(msg);

        int code;

        try
        {
            var reply = Post(json, url, out code, true);
            //var reply = Post(json, url, out code, out message);

            REST_Error message = new REST_Error();

            if (code < 0)
                message.message = "Error de socket, no se puede abrir una conexión";
            else if (code != 200)
            {
                try
                {
                    message = JsonUtility.FromJson<REST_Error>(reply);
                }
                catch (Exception e)
                {
                    message.message = reply;
                }
            }

            message.code = code;

            return message;
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public static Message SearchPair(float waitTime)
    {
        string url = URL + "/matchmaking/?waitTime=" + waitTime;

        int code;

        try
        {
            var reply = Get(url, out code, true);
            //var reply = Post(json, url, out code, out message);

            if (code != 200)
            {
                REST_Error message = new REST_Error();

                if (code < 0)
                    message.message = "Error de socket, no se puede abrir una conexión";
                else
                {
                    try
                    {
                        message = JsonUtility.FromJson<REST_Error>(reply);
                    }
                    catch (Exception e)
                    {
                        message.message = reply;
                    }
                }

                message.code = code;

                return message;
            }
            else
            {
                PairSearch message = new PairSearch();

                message = JsonUtility.FromJson<PairSearch>(reply);

                message.code = code;

                return message;
            }
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public static Message LeaveQueue()
    {
        string url = URL + "/matchmaking";

        Message msg = new Message();
        msg.code = 0;
        string json = JsonUtility.ToJson(msg);

        int code;

        try
        {
            var reply = Delete(json, url, out code, true);
            //var reply = Post(json, url, out code, out message);

            REST_Error message = new REST_Error();

            if (code < 0)
                message.message = "Error de socket, no se puede abrir una conexión";
            else if (code != 200)
            {
                try
                {
                    message = JsonUtility.FromJson<REST_Error>(reply);
                }
                catch (Exception e)
                {
                    message.message = reply;
                }
            }

            message.code = code;

            return message;
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public static Message Refresh()
    {
        string url = URL + "/accounts/sessions/refresh";

        RefreshData msg = new RefreshData();
        msg.refreshToken = refreshToken;
        string json = JsonUtility.ToJson(msg);

        int code;

        try
        {
            var reply = Post(json, url, out code);
            //var reply = Post(json, url, out code, out message);

            if (code != 200)
            {
                REST_Error message = new REST_Error();

                if (code < 0)
                    message.message = "Error de socket, no se puede abrir una conexión";
                else
                {
                    try
                    {
                        message = JsonUtility.FromJson<REST_Error>(reply);
                    }
                    catch (Exception e)
                    {
                        message.message = reply;
                    }
                }

                message.code = code;

                return message;
            }
            else
            {
                RefreshMessage message = new RefreshMessage();

                message = JsonUtility.FromJson<RefreshMessage>(reply);

                authToken = message.accessToken;

                message.code = code;

                return message;
            }
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    //id1: id de este jugador id2: id del rival
    public static Message FindServerInfo(int id1, int id2)
    {
        string url = URL_GAME + "/game-instances";

        Identifiers IDs = new Identifiers();
        IDs.ID1 = id1;
        IDs.ID2 = id2;
        string json = JsonUtility.ToJson(IDs);

        int code;

        try
        {
            var reply = Post(json, url, out code);
            //var reply = Post(json, url, out code, out message);

            if (code != 200)
            {
                REST_Error message = new REST_Error();

                if (code < 0)
                    message.message = "Error de socket, no se puede abrir una conexión";
                else
                {
                    try
                    {
                        message = JsonUtility.FromJson<REST_Error>(reply);
                    }
                    catch (Exception e)
                    {
                        message.message = reply;
                    }
                }

                message.code = code;

                return message;
            }
            else
            {
                ServerMatchInfo message = new ServerMatchInfo();

                message = JsonUtility.FromJson<ServerMatchInfo>(reply);

                message.code = code;

                return message;
            }
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public static Message FinishMatch(int id1, int id2)
    {
        string url = URL_GAME + "/game-instances";

        Identifiers IDs = new Identifiers();
        IDs.ID1 = id1;
        IDs.ID2 = id2;
        string json = JsonUtility.ToJson(IDs);

        int code;

        try
        {
            var reply = Delete(json, url, out code);
            //var reply = Post(json, url, out code, out message);
            REST_Error message = new REST_Error();

            if (code < 0)
                message.message = "Error de socket, no se puede abrir una conexión";
            else if (code != 200)
            {
                try
                {
                    message = JsonUtility.FromJson<REST_Error>(reply);
                }
                catch (Exception e)
                {
                    message.message = reply;
                }
            }

            message.code = code;

            return message;
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    /*
     * 
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

    public static string GetVersion()
    {
        string url = "http://" + ip + ":" + puerto + messages[(int)MESSAGE.VERSION];
        string version = Get(url, out code_, out message_);

        if(code_ == 200)
            return version;

        throw new RestResponseException(message_, code_);
    }
    */

    private static string HandleRequest(HttpWebRequest request, out int code)
    {
        try
        {
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                code = (int)response.StatusCode;
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
            using (HttpWebResponse response = (HttpWebResponse)ex.Response)
            {
                if (response == null)
                {
                    code = -1;
                    return "";
                }

                code = (int)response.StatusCode;

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
        catch (SocketException e)
        {
            code = -1;
            Debug.Log("xi");
            return e.Message;
        }
    }

    private static string Post(string json, string url, out int code, bool useAuth = false)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "POST";
        request.ContentType = "application/json";
        request.Accept = "application/json";

        if(useAuth) request.Headers.Add("Authorization", "Bearer " + authToken);

        code = -1;

        try
        {
            using (StreamWriter streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
            return "";
        }

        return HandleRequest(request, out code);
    }

    private static string Get(string url, out int code, bool useAuth = false)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "GET";
        request.ContentType = "application/json";
        request.Accept = "application/json";

        if (useAuth) request.Headers.Add("Authorization", "Bearer " + authToken);

        code = -1;

        return HandleRequest(request, out code);
    }

    private static string Delete(string json, string url, out int code, bool useAuth = false)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "DELETE";
        request.ContentType = "application/json";
        request.Accept = "application/json";

        if (useAuth) request.Headers.Add("Authorization", "Bearer " + authToken);

        code = -1;

        try
        {
            using (StreamWriter streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
            return "";
        }

        return HandleRequest(request, out code);
    }
}
