// UserSubClass.cs
using Parse;
using UnityEngine; // To support Debug.Log only
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
 
// [ParseClassName("UserSubClass")]
public class UserSubClass : ParseUser
{
  [ParseFieldName("fbId")]
  public string FbId 
  {
    get { return GetProperty<string>("FbId"); }
    set { SetProperty<string>(value, "FbId"); }
  }

  [ParseFieldName("email")]
  public string Email
  {
    get { return GetProperty<string>("Email"); }
    set { SetProperty<string>(value, "Email"); }
  }

  [ParseFieldName("deviceId")]
  public string DeviceId 
  {
    get { return GetProperty<string>("DeviceId"); }
    set { SetProperty<string>(value, "DeviceId"); }
  }

  [ParseFieldName("name")]
  public string Name 
  {
    get { return GetProperty<string>("Name"); }
    set { SetProperty<string>(value, "Name"); }
  }

  [ParseFieldName("profileName")]
  public string ProfileName 
  {
    get { return GetProperty<string>("ProfileName"); }
    set { SetProperty<string>(value, "ProfileName"); }
  }

	[ParseFieldName("AvatarName")]
	public string AvatarName 
	{
		get { return GetProperty<string>("AvatarName"); }
		set { SetProperty<string>(value, "AvatarName"); }
	}	

  [ParseFieldName("picture")]
  public string Picture 
  {
    get { return GetProperty<string>("Picture"); }
    set { SetProperty<string>(value, "Picture"); }
  }

  [ParseFieldName("dollars")]
  public int Dollars 
  {
    get { return GetProperty<int>("Dollars"); }
    set { SetProperty<int>(value, "Dollars"); }
  }

  [ParseFieldName("slots")]
  public int Slots 
  {
    get { return GetProperty<int>("Slots"); }
    set { SetProperty<int>(value, "Slots"); }
  }


	[ParseFieldName("games")]
	public int Games 
	{
		get { return GetProperty<int>("Games"); }
		set { SetProperty<int>(value, "Games"); }
	}

  [ParseFieldName("experience")]
  public int Experience 
  {
    get { return GetProperty<int>("Experience"); }
    set { SetProperty<int>(value, "Experience"); }
  }

  [ParseFieldName("movesOwned")]
  public IList<object> MovesOwned
  {
    get { return GetProperty<IList<object>>("MovesOwned"); }
    set { SetProperty<IList<object>>(value, "MovesOwned"); }
  }


	[ParseFieldName("songsOwned")]
	public IList<object> SongsOwned
	{
		get { return GetProperty<IList<object>>("SongsOwned"); }
		set { SetProperty<IList<object>>(value, "SongsOwned"); }
	}

	[ParseFieldName("acceptGames")]
	public int AcceptGames 
	{
		get { return GetProperty<int>("AcceptGames"); }
		set { SetProperty<int>(value, "AcceptGames"); }
	}

	[ParseFieldName("completetGames")]
	public int CompleteGames 
	{
		get { return GetProperty<int>("CompleteGames"); }
		set { SetProperty<int>(value, "CompleteGames"); }
	}

  [ParseFieldName("enabled")]
  public bool Enabled 
  {
    get { return GetProperty<bool>("Enabled"); }
    set { SetProperty<bool>(value, "Enabled"); }
  }

  [ParseFieldName("highScore")]
  public int HighScore 
  {
    get { return GetProperty<int>("HighScore"); }
    set { SetProperty<int>(value, "HighScore"); }
  }

  [ParseFieldName("highScoreMoves")]
  public IList<object> HighScoreMoves
  {
    get { return GetProperty<IList<object>>("HighScoreMoves"); }
    set { SetProperty<IList<object>>(value, "HighScoreMoves"); }
  }

  public void SetStartingValues(string objectId, string username, int slots, int currency, int experience, string fbId, string deviceId, JsonDict fbUserInfo)
  {
    ObjectId = objectId;
    Username = username; 
    FbId = fbId;
	  Slots = slots;
	  Dollars = currency;
	  Experience = experience;
	  AvatarName = "";
	  MovesOwned = new List<object>(); 
	  SongsOwned = new List<object>();
    DeviceId = deviceId;
	  Games = 0;
	  AcceptGames = 0;
	  CompleteGames = 0;
    Enabled = true;
	  HighScore = 0;
	  HighScoreMoves = new List<object>(); 

    if (fbUserInfo != null) 
	  {
      Email = fbId + "@danceoff.com"; // fbUserInfo.GetString ("email");
      Name = fbUserInfo.GetString ("name");
      ProfileName = fbUserInfo.GetString ("name");
      Picture = fbUserInfo.GetDict("picture").GetDict("data").GetString("url");
    }
  }

  // Note, if you want to store locally and Parse still hasn't implemented caching
  // for Unity then you can use Unity's PlayerPrefs
  // http://docs.unity3d.com/Documentation/ScriptReference/PlayerPrefs.html
  public Task UpdateCloudAsync()
  {
	return SaveAsync ();
  }

  public void PrintProperties()
  {
    Debug.Log("UserSubClass Properties Username=" + Username + " FbId=" + FbId + " Name=" + Name + " Email=" + Email + " ProfileName=" + ProfileName + " Picture=" + Picture + " Dollars=" + Dollars + " Slots=" + Slots + " Experience=" + Experience + " MovesOwned=" + MovesOwned + " Enabled=" + Enabled + " High Score=" + HighScore + " HighScoreMoves=" + HighScoreMoves);
  }


}
 
