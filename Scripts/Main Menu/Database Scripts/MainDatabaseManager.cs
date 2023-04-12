using MongoDB.Bson;
using MongoDB.Driver;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.ProBuilder;

public class MainDatabaseManager : MonoBehaviour
{


    [Header("Main Database Attributes")]
    [SerializeField] public string key;
    [SerializeField] private string getDatabasename;
    [SerializeField] private string getCollectionname;
    MongoClient client;
    IMongoDatabase database;
    IMongoCollection<BsonDocument> collection;
    [HideInInspector] public bool calculating;
    [Header("Misc")]
    public bool closeDATABASE;
    public bool loggedIn;
    public DatabasePiercer databasePiercer;
    [HideInInspector] public static MainDatabaseManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            MongoClient clientTemp = new MongoClient(key);
            client = clientTemp;
            database = client.GetDatabase("myFirstDatabase");
            collection = database.GetCollection<BsonDocument>("msgschems");
        }
        else
        {
            Debug.Log("I Destroyed Myself");
            Destroy(this.gameObject);
        }
    }
    public async Task<bool> GetDataFromIdAndToken(string DiscordID, string TokenID)
    {
        calculating = true;
        var filter = Builders<BsonDocument>.Filter.Eq("discordId", DiscordID);
        var projection = Builders<BsonDocument>.Projection.Include("username");
        var UniversalUsername = "NoneAtm";
        var ppstringString = "";
        try
        {
            var usernameDoc = collection.Find(filter).Project(projection).First()["username"].AsString;
            UniversalUsername = usernameDoc;
            Debug.Log("UsernameFound");
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
            Debug.Log("Error Accured Problably No Username Found");
            calculating = false;
            return false; 
        }
        var projectionv2 = Builders<BsonDocument>.Projection.Include("gametoken");
        try
        {
            var gameTokenDoc = collection.Find(filter).Project(projectionv2).First()["gametoken"].AsString;
            Debug.Log("TokenFound");
            if (TokenID == gameTokenDoc)
            {
                Debug.Log("TokenApproved");
            }
            else
            {
                Debug.Log("TokenDenied");
                calculating = false;
                return false;
            }
        }
        catch (System.Exception er)
        {
            Debug.Log("Error Accured Problably No Token Found");
            calculating = false;
            return false;
        }
        var projectionv3 = Builders<BsonDocument>.Projection.Include("tokenactive");
        try
        {
            var gameTokenBoolDoc = collection.Find(filter).Project(projectionv3).First()["tokenactive"].AsBoolean;
            Debug.Log("Token Bool Found");
            if (gameTokenBoolDoc)
            {
                Debug.Log("Token is active!");
            }
            else
            {
                Debug.Log("Token is already used");
                calculating = false;
                return false;
            }
        }
        catch (System.Exception er)
        {
            Debug.Log("Error Accured Problably No Token Found");
            calculating = false;
            return false;
        }
        bool whatShouldIReturn = await CloseTokenFirst(UniversalUsername, DiscordID, ppstringString);
        return whatShouldIReturn;
    }
    public async Task<bool> CloseTokenFirst(string username, string DiscordId,string getSpriteURL)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("discordId", DiscordId);
        var projectionv3 = Builders<BsonDocument>.Projection.Include("tokenactive");
        try
        {
            var gameTokenBoolDoc = collection.Find(filter).Project(projectionv3).First()["tokenactive"].AsBoolean;
            Debug.Log("Token Bool Found");
            if (gameTokenBoolDoc)
            {
                Debug.Log("Token is active!");
            }
            else
            {
                Debug.Log("Token is already used");
                calculating = false;
                return false;
            }
        }
        catch (System.Exception er)
        {
            Debug.Log("Error Accured Problably No Token Found");
            calculating = false;
            return false;
        } //reading one more time
        var update = Builders<BsonDocument>.Update.Set("tokenactive", false);
        try
        {
            await collection.UpdateOneAsync(filter, update);
            Debug.Log("Updated");
        }
        catch (System.Exception er)
        {
            calculating = false;
            return false;
        }
        databasePiercer.playerName = username;
        databasePiercer.playerID = DiscordId;
        databasePiercer.playerSpriteURL = getSpriteURL;
        loggedIn = true;
        return true;
    }
    public async void banAndCloseGame(string discordId)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("discordId", discordId);
        var update = Builders<BsonDocument>.Update.Set("isItBanned", true);
        try
        {
            await collection.UpdateOneAsync(filter, update);
            Debug.Log("No Error Accured");
            Invoke("QuitG", 5f);
        }
        catch 
        (System.Exception e)
        {
            Debug.LogError("ERROR OCCURED");
        }
    }
    public async void WinGame(string discordId)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("discordId", discordId);
        var update = Builders<BsonDocument>.Update.Set("didPlayerWin", true);
        try
        {
            await collection.UpdateOneAsync(filter, update);
            Debug.Log("No Error Accured");
        }
        catch
        (System.Exception e)
        {
            Debug.LogError("ERROR OCCURED");
        }
    }
    public void QuitG()
    {
        Application.Quit();
    }
}
    