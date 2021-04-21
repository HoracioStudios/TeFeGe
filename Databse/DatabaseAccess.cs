using UnityEngine;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

public class DatabaseAccess : MonoBehaviour
{
    private const string MONGO_URI = "mongodb://dbUser:grossigrossi@cositas-shard-00-02.tkmov.mongodb.net:27017/test";
    private const string DATABASE_NAME = "grossi";
    private const string COLLECTION_NAME = "test";

    private MongoClient client_;
    private MongoServer server_;
    private MongoDatabase db_;

    private MongoCollection<Model_Account> accounts;

    public Model_Account LoginAccount(string nickOrEmail, string password, int cnnId, string token)
    {
        Model_Account myAccount = null;
        IMongoQuery query = null;
        string pass = Utility.sha256FromString(password);
        //Find my account
        if(Utility.isEmail(nickOrEmail))
        {
            //If i logged in using an email
            query = Query.And(
            Query<Model_Account>.EQ(u => u.nick, nickOrEmail),
            Query<Model_Account>.EQ(u => u.shaPassword, pass));

            myAccount = accounts.FindOne(query);
        }
        else
        {
            //If i logged in using username#discriminator
            string[] data = nickOrEmail.Split('#');
            if(data[1] != null)
            {
                query = Query.And(
                            Query<Model_Account>.EQ(u => u.nick, data[0]),
                            Query<Model_Account>.EQ(u => u.discriminator, data[1]),
                            Query<Model_Account>.EQ(u => u.shaPassword, pass));

                myAccount = accounts.FindOne(query);
            }
            else
            {
                Debug.Log("Please, insert your identification code such as Pepito#3567");
            }
        }

        if(myAccount != null)
        {
            //We found the account, let's go in
            myAccount.activeConnection = cnnId;
            myAccount.token = token;

            //update de la database de mongo
            accounts.Update(query, Update<Model_Account>.Replace(myAccount));
        }
        else
        {
            //Didn't find anything with those credentials
            Debug.Log("Esa cuenta no existe.");
        }

        return myAccount;

    }

    public void Init()
    {
        client_ = new MongoClient(MONGO_URI);
        server_ = client_.GetServer();
        db_ = client_.GetServer().GetDatabase(DATABASE_NAME);

        //Initialize collections
        accounts = db_.GetCollection<Model_Account>(COLLECTION_NAME);

        Debug.Log("Database initialized");
    }

    public void Shutdown()
    {
        client_ = null;
        server_.Shutdown();
        db_ = null;
    }

    //Por ahora es de prueba
    #region Insert
    public bool InsertAccount(string nick, string password, string email)
    {
        if (!Utility.isEmail(email))
        {
            Debug.Log(email + " is not a valid email.");
            return false;
        }

        if (FindAccountByEmail(email) != null)
        {
            Debug.Log(email + " is already in use, please, select another email.");
            return false;
        }

        Model_Account newAccount = new Model_Account();
        newAccount.nick = nick;
        string pass = Utility.sha256FromString(password);
        newAccount.shaPassword = pass;
        newAccount.email = email;

        newAccount.discriminator = "0000";

        //Find unique discriminator
        int rollcount = 0;
        while (FindAccountByUsernameAndDiscriminator(newAccount.nick, newAccount.discriminator) != null)
        {
            newAccount.discriminator = Random.Range(0, 9999).ToString("0000");

            rollcount++;
            if(rollcount > 1000)
            {
                Debug.Log("This username is not available, please, select another.");
                return false;
            }
        }

        newAccount.RD = 350;
        newAccount.rating = 1500;
        accounts.Insert(newAccount);
        return true;
    }
    #endregion

    #region Fetch
    public Model_Account FindAccountByEmail(string email)
    {
        var query = Query<Model_Account>.EQ(u => u.email, email);
        return accounts.FindOne(query);
    }

    public Model_Account FindAccountByName(string nick)
    {
        var query = Query<Model_Account>.EQ(u => u.nick, nick);
        return accounts.FindOne(query);
    }

    public Model_Account FindAccountByUsernameAndDiscriminator(string username, string discriminator)
    {
        var query = Query.And(
            Query<Model_Account>.EQ(u => u.nick, username),
            Query<Model_Account>.EQ(u => u.discriminator, discriminator));

        return accounts.FindOne(query);
    }
    #endregion
}
