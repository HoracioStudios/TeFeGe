using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;

public class ClientCommunication : MonoBehaviour
{
    [Serializable]
    class User
    {
        public string username;
        public string email;
        public string password;
    }

    public static int LogIn(string username, string password)
    {
        string url = "http://localhost:25565/login";
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        User user = new User();
        user.username = username;
        user.password = password;
        string json = JsonUtility.ToJson(user);

        return int.Parse(Send(json, url));
    }

    public static int SignIn(string username, string email, string password)
    {
        string url = "http://localhost:25565/signin";
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        User user = new User();
        user.username = username;
        user.email = email;
        user.password = password;
        string json = JsonUtility.ToJson(user);

        return int.Parse(Send(json, url));
    }


    public static string StartQueue(int id)
    {
        string url = "http://localhost:25565/startQueue";
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        string json = $"{{\"id\":\"{id}\"}}";
        return Send(json, url);
    }

    public static string CancelQueue(int id)
    {
        string url = "http://localhost:25565/cancelQueue";
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        string json = $"{{\"id\":\"{id}\"}}";
        return Send(json, url);
    }

    public static string GetELO(int id)
    {
        string url = "http://localhost:25565/getelo";
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        string json = $"{{\"id\":\"{id}\"}}";
        return Send(json, url);
    }

    public static string Send(string json, string url)
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
        catch (System.Exception)
        {

            throw;
        }
    }
}
