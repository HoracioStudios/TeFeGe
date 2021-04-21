using UnityEngine;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;


public class Model_Account
{
    public struct Matches
    {
        int result;
        float time;
        int opponentId;
    }

    [BsonId]
    public ObjectId _id;

    //Si está conectado o no
    public int activeConnection { get; set; }
    //El nombre
    public string nick { get; set; }
    //El identificador de cada jugador
    public string discriminator { get; set; }
    //El email del jugador
    public string email { get; set; }
    //La contraseña encriptada
    public string shaPassword { get; set; }
    //Salt, no sé si hará falta
    public string salt { get; set; }
    //Aún no sé lo que es esto, lo meto por si acaso
    public string token { get; set; }
    //Puntuación del jugador
    public float rating { get; set; }
    //Desviación del jugador
    public float RD { get; set; }


    public int id { get; set; }


    //Última vez que el jugador ha echado una partida
    public DateTime lastPlayed { get; set; }
    //Lista de partidas jugadas en el periodo actual
    public Matches[] matches {get; set;}
}
