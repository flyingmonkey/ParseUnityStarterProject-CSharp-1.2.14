using UnityEngine;
using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Parse;
using Facebook;

public class CloudStorageController : MonoBehaviour {

	#region Enums
	#endregion
	
	#region Delegates
	#endregion
	
	#region Constants
	private const string CHALLENGE_CREATED = "created";
	private const string CHALLENGE_CHALLENGED	= "challenged";
	private const string CHALLENGE_CHALLENGEE_STARTED	= "challengee_started";
	private const string CHALLENGE_COMPLETED	= "completed";
  private const double BASE_EXP_PER_LEVEL = 300.0;
	private const string GRAPH_URL = "http://graph.facebook.com/{0}/picture?type=small";
  private const string FBFriendsURL = "/{0}?fields=friends.limit(5000).fields(id,name)&access_token={1}";
  // https://api.facebook.com/method/friends.getAppUsers?fields=friends.fields(id,name)&access_token=CAAGCZCIXDRqsBAESb6ZAcKO09qSh9gT1HeNLq9xjGNDrZC8QNrvQT2Of2syiH86VPbEBUGw4ZAZAZAcLzOjQhvuGqrHX9ypx5ZCjPghkZAXvWCZBRKrU2Tlyij2d3qZCkORP28xNvtgpvEepJDjPApMiQzteNl0ZCMMioUs6ALI0Mz4cFcZBzbh9ueVvgst2ZBQ412IsZD&format=json
	#endregion
	
	#region Variables
	private string ACCESSTOKEN = "CAAUQpWTWwRQBAHKTZCo8B6V95Gl9oB84JvMAXVKPNgX8GdVM9ofbjbIw3ab7RhVkZCkPqlEzDJt7FFMr0kea2qsmgKjKANuM0fQLlFx6SHVkVZBALCxWjE2I94uGkV7S2MFSmucW8pp9e27ZAlOdFTMARWp7ufxDLHxs0sexZBWvfMPR7oLPW&expires";
	private string FBID = "100007351565888";
    public static string fb_payment_id;
	
	// private string ACCESSTOKEN = "CAAUQpWTWwRQBAA5lKV98pBZBZCtPcrj3Sh256i7TjFOVd0TZBzRtyN8aOzvANXYsZBWRRMb0S1vwer6wDHG8M0BiAzZA4Tn9FJUMPdLfGbeJhSnbJlf47Dli4guPdz8fidZA4ZCpFaauBxc6dZCZBZCHoHWuTZBZACAQnkndO2fDWhW5ltENpFyClrOD";
	// private string FBID = "100007388732153";

	private bool _hasAvatar = false;
	private bool _isLoggedIn = false;
	private bool _isAcceptChallengeReady = false;
	private bool _isCompleteChallengeReady = false;
	private bool _isFindChallengeReady = false;
	private bool _isWaitingRandomChallenge = false;
	private bool _isWaitingAcceptChallenge = false;
	private bool _isWaitingCompleteChallenge = false;
	private bool _isWaitingGeneralGames = false;
	private bool _needShowPopUp = false;
	private bool _needShowRechallengeOption = false;
  private JsonDict _friendsList = null;	

	private int _firstAcceptChallenge = 0;
	private int _firstCompleteChallenge = 0;

	private JsonDict _fbUserInfo = null;
	private string _fbUserID = "";
	private string _accessToken = "";
	private string _deviceId = "";
	private string _userObjId = "";
	private string _userName = "";
	private string _fbChallengeeId = "";
  private static List<object> _globalFriendsList = new List<object>();

	private int _challengeeExperience = 0;
	private Texture2D _challengeePicture = null;
	private Texture2D _myPicture = null;

	private int _experience = 0;
	private int _slots = 4;
	private int _currency = 0;
	private static int _boughtDanceDollars = 0;
	private int _tempAcceptChallengeCount = 0;
	private static Action _callBackDanceDollar = null;


	//Only for test
	private int userAcceptChallenge = 0;
	private int userCompleteChallenge = 0;

	private int _completeChallengesCount = 0;
	private int _acceptChallengesCount = 0;
	private IEnumerable<Game> _completeChallenges = null;
	private IEnumerable<Game> _acceptChallenges = null;
	private Game _challenge = null;
	public static UserSubClass User = null;
		
	private static CloudStorageController _instance;
	#endregion
	
	#region Properties

	public static CloudStorageController Instance
	{
		get{return _instance; } 
	}

	public bool IsWaitingRandomChallenge
	{
		set{_isWaitingRandomChallenge = value;}
		get{return _isWaitingRandomChallenge;}
	}

	public bool IsWaitingGeneralGames
	{
		set{_isWaitingGeneralGames = value;}
		get{return _isWaitingGeneralGames;}
	}


	public bool IsWaitingAcceptChallenge
	{
		set{_isWaitingAcceptChallenge = value;}
		get{return _isWaitingAcceptChallenge;}
	}

	public bool HasAvatar
	{
		get{return _hasAvatar;}
	}

	public bool NewAcceptChallenge
	{
		get{return _acceptChallengesCount > User.AcceptGames;}
	}

	public bool NewCompleteChallenge
	{
		get{return _completeChallengesCount > User.CompleteGames;}
	}

	public bool NeedShowPopUp
	{
		get{return _needShowPopUp;}
		set{_needShowPopUp = value;}
	}

	public bool NeedShowRechallengeOption
	{
		get{return _needShowRechallengeOption;}
		set{_needShowRechallengeOption = value;}
	}

	public bool IsLoggedIn
	{
		get{return _isLoggedIn;}
	}

	public bool IsAcceptChallengeReady
	{
		get{return _isAcceptChallengeReady;}
	}

	public bool IsCompleteChallengeReady
	{
		get{return _isCompleteChallengeReady;}
	}

	public bool IsFindChallengeReady
	{
		get{return _isFindChallengeReady;}
	}

	public string FBChallengeeID
	{
		set{_fbChallengeeId = value;}
		get{return _fbChallengeeId;}
	}

	public Texture2D ChallengeePicture
	{
		set{_challengeePicture = value;}
		get{return _challengeePicture;}
	}

  public Game Challenge
  {
    set{_challenge = value;}
    get{return _challenge;}
  }

	public Texture2D MyPicture
	{
		get{return _myPicture;}
	}

	public IEnumerable<Game> AcceptChallenges
	{
		get{return _acceptChallenges;}
		set{_acceptChallenges = value;}
	}

	public IEnumerable<Game> CompleteChallenges
	{
		get{return _completeChallenges;}
		set{_completeChallenges = value;}
	}

	public string AccesToken
	{
		get{return _accessToken;}
	}

  public static List<object> GlobalFriendsList 
  {
    get{return _globalFriendsList;}
    set{_globalFriendsList = value;}
  }

	public int AmountNewAcceptChallenges
	{
		get 
		{
			return _acceptChallengesCount - User.AcceptGames;
		}
	}

	public int AmountNewCompleteChallenges
	{
		get
		{
			return _completeChallengesCount - User.CompleteGames;
		}
	}

	#endregion
	
	#region Constructors

	private void Awake()
	{
		_instance = this;
	}
	#endregion
	
	#region Methods	

	#region InitializeParse

  /// <summary>
  /// Raises the hide unity event.
  /// </summary>
  /// <param name="isGameShown">If set to <c>true</c> is game shown.</param>
  private void OnHideUnity(bool isGameShown)
  {
    Debug.Log("Is game showing? " + isGameShown);
  }

  /// <summary>
  /// Raises the init complete event.
  /// </summary>
  private void OnInitComplete()
  {
    Debug.Log("FB.Init completed: Is user logged in? " + FB.IsLoggedIn);
    enabled = true;
    //#if UNITY_EDITOR
    InitParse();
    //#else
    //CallFBLogin();
    //#endif
  }

  private void Start()
  {
    FB.Init(OnInitComplete, OnHideUnity);
    Debug.Log("CloudStorageController::Start entered");
  }

	/// <summary>
	/// Inits the parse.
	/// </summary>
	public void InitParse()
	{
		_accessToken = "";
		// #if UNITY_EDITOR
		switch ( System.Environment.UserName )
		{
			case "marcovinicio": // FB Test User Name for marcovinicio: 
			_fbUserID = "100001521857393";
			_accessToken = "CAACEdEose0cBABDnZA0Pz6hziUQc5FjJLVZBIjjXtxzA6uyWVYQdVbNfhlzBFPoZBhrYzHeNZAYpZAqmZBrr3xhocZBO6vFxONwD02sYBoyjmlmk8ncpEkrwZBxnHogWa61RemH8BWunRhlprdKNGfmCwQaGLEzEinp9yZBtdiBWHXudaRysZAf9BBsEY5oDo8OrIZD";
      break;
//#if !UNITY_EDITOR
      case "cozz": // Actually Hamilton
      Debug.Log("recognized user cozz so setting access token");
      // _fbUserID = "618595200";
      _fbUserID = "750005908";
      // _accessToken = "CAAGCZCIXDRqsBALZALKVoY8M8Y3PIHb3vtAoiZBOMdzruynCdtidaUezck4xW1T5MjjcnxUZCxrpaZBDzYDpLy3z1Vnb5ZACSK1qFWkWTmsXhfcthHASvO5LQomafQxIwJ8aAhcGZAelRHlH7fn31ijicX4BTLodMh2vEtAwnoClk9RwaYbvCwhJE3dx55Lyw24ny4QWfzZCXQZDZD"; // Production

      _accessToken = "CAAUQpWTWwRQBAHbPt9WvLZA4gQERIRtc5gGE2m4GqrdaX8SbjjlL4ijHDy9haFQgSAclO4WrgnVFkIGL1EjHq8BItnZBojXTYGIbCe5a4HK58oIALapDQSF23cuWtQ2wNryvCnZARZCyQJNK4BCsIlerUJoDYrcqrgKwYTAet1a4yLspc5lVOsuVkRUiSOIZD";
			break;
//#endif
			default: // Test User Name: Dick, FBID: 100007351565888
			_fbUserID = FBID;
			_accessToken = ACCESSTOKEN;
			break;
		}
/*
		#else
		_fbUserID = FB.UserId;
		_accessToken = FB.AccessToken;
		#endif
*/
    _fbUserID = "750005908";
    _accessToken = "CAAUQpWTWwRQBAHbPt9WvLZA4gQERIRtc5gGE2m4GqrdaX8SbjjlL4ijHDy9haFQgSAclO4WrgnVFkIGL1EjHq8BItnZBojXTYGIbCe5a4HK58oIALapDQSF23cuWtQ2wNryvCnZARZCyQJNK4BCsIlerUJoDYrcqrgKwYTAet1a4yLspc5lVOsuVkRUiSOIZD";
		Debug.Log("CloudStorage Start. userId=" + _fbUserID);
		string url = string.Format("/me?fields=id,username,name,picture&access_token={0}", _accessToken);
		FB.API(url, HttpMethod.GET, FBUserCallback);

    Debug.Log("Calling ParseFacebookUtils.LogInAsync with _fbUserID=" + _fbUserID + " _accessToken=" + _accessToken);		
		// Task<ParseUser> task = ParseFacebookUtils.LogInAsync (_fbUserID, _accessToken, DateTime.Now);
		Task<ParseUser> task = ParseFacebookUtils.LogInAsync (_fbUserID, _accessToken, DateTime.Now);
    // task.ContinueWith(t => {
    //   OnLoginCompleted(t);
    // });

		StartCoroutine(DGUtils.WaitTaskAndDoAction(task, OnLoginCompleted));
	}

  /// <summary>
  /// Calls Facebook API for an opengraph story given the Facebook endpoint, e.g. me/danceoffexp:won 
  /// </summary>
  public void FBCreateOpenGraphStory(string action, string obj, string who = "me") 
  {
    // https://graph.facebook.com/me/strangelings:breed?access_token=ACCESS_TOKEN&method=POST&strangeling=http%3A%2F%2Fsamples.ogp.me%2F382255755240654
    var parame = new Dictionary<string, string>();
    string endpoint = who + "/dance_off:" + action;
    var url = "http://danceoff.fmigames.com/opengraph/" + action + "_" + obj + ".html";
    parame[obj] = url;
    Debug.Log("FBCreateOpenGraphStory has who=" + who + " action=" + action + " endpoint=" + endpoint + " url=" + url);
#if !UNITY_EDITOR
    FB.API (endpoint, Facebook.HttpMethod.POST, 
      delegate(FBResult r) { Debug.Log("Facebook Opengraph Story Result: " + r.Text); }, parame);
#endif
  }

	/// <summary>
	/// FBs the user callback.
	/// </summary>
	/// <param name="response">Response.</param>
	private void FBUserCallback(FBResult response) 
	{
		_fbUserInfo = new JsonDict ();
		_fbUserInfo.Deserialize (response.Text);
	}

  // Example code
  private void OnGetRandomGameCompleted(Task task)
  {
    Debug.Log("ENTERED OnGetRandomGameCompleted");

    Task<Game> taskGame = (Task<Game>)task;
   
    if (task.IsFaulted || task.IsCanceled) {
      Debug.Log ("OnGetRandomGameCompleted: Failed to get game with t.IsFaulted=" + task.IsFaulted);
      Debug.Log ("OnGetRandomGameCompleted: Failed to get game with t.IsCanceled=" + task.IsCanceled);
      // The login failed. Check t.Exception to see why.
      Debug.Log ("OnGetRandomGameCompleted: Failed to update User with t.Exception=" + task.Exception + task.Exception.Message);

      Debug.Log ("OnGetRandomGameCompleted, which probably means no game is looking for a challengee so creating Game Object...");
      FBChallengeeID = "";
      CreateChallenge();
      SaveChallengeToParse();
      Debug.Log("OnGetRandomGameCompleted, saving new Game to cloud");
    } else {
			Challenge = taskGame.Result;
      Debug.Log("OnGetRandomGameCompleted found game with gameObjectId=" + _challenge.ObjectId);
    }
  }
	

	/// <summary>
	/// Raises the login completed event.
	/// </summary>
	/// <param name="task">Task.</param>
	private void OnLoginCompleted (Task task)
	{
		Task<ParseUser> taskUser = (Task<ParseUser>)task;
		
		if (task.IsFaulted || task.IsCanceled) {
			// The login failed. Check t.Exception to see why.
			Debug.Log ("Parse Login failed");
			_isLoggedIn = false;
		}
		else {
			var userFB = taskUser.Result;
			_userObjId = userFB.ObjectId;
			_userName = userFB.Username;
			Debug.Log ("Parse Login Succeeded with userFB.ObjectId = " + userFB.ObjectId + " userFB.Username=" + userFB.Username);
			_isLoggedIn = true;
			User = (UserSubClass) ParseUser.CurrentUser;
			_userObjId = User.ObjectId;
			_userName = User.Username;
			InitUser();
		}
	}

  /// <summary>
  /// Sorts the friend list.
  /// </summary>
  /// <returns>The friend list.</returns>
  /// <param name="friendList">Friend list.</param>
  public static List<object> SortFriendList(List<object> friendList)
  {
    friendList.Sort(
      delegate(object firstFriend, object secondFriend)
      {
        Dictionary<string, object> tempFirstFriend = firstFriend as Dictionary<string, object>;
        string  firstFriendName= Convert.ToString(tempFirstFriend["name"]);

        Dictionary<string, object> tempSecondFriend = secondFriend as Dictionary<string, object>;
        string  SecondFriendName= Convert.ToString(tempSecondFriend["name"]);

        return firstFriendName.CompareTo(SecondFriendName);
      }
    );
    return friendList;
  }

  /// <summary>
  /// Generates the facebook list.
  /// </summary>
  /// <param name="result">Result.</param>
  private void CacheFacebookFriendsList(FBResult result)
  {
    _friendsList = new JsonDict ();
    _friendsList.Deserialize(result.Text);
    if (_friendsList.ContainsKey("friends")) {
      JsonDict temp = _friendsList.GetDict("friends");
      GlobalFriendsList = SortFriendList(temp.GetList("data"));
      Debug.Log("CacheFacebookFriendsList: Cached " + GlobalFriendsList.Count + " Facebook Friends");
    }
  }

  /// <summary>
  /// Gets the facebook friends and cache them 
  /// </summary>
  /// <returns>The facebook profile picture.</returns>
  /// <param name="FBid">F bid.</param>
  private void LoadFacebookFriendsData()
  {
    if(GlobalFriendsList.Count == 0)
    {
      string url = string.Format (FBFriendsURL, _fbUserID, _accessToken);
      FB.API(url, HttpMethod.GET, CacheFacebookFriendsList);
    } 
  }

	/// <summary>
	/// Gets the facebook profile picture.
	/// </summary>
	/// <returns>The facebook profile picture.</returns>
	/// <param name="FBid">F bid.</param>
	IEnumerator GetFacebookProfilePicture(string FBid) {
		var request = string.Format(GRAPH_URL, FBid);
		WWW loader = new WWW(request);
		yield return loader;

		_myPicture = loader.texture;
    LoadFacebookFriendsData(); 
	}

	#endregion

	#region User

  /// <summary>
  ///  
  /// </summary>
  /// <param name="task">Task.</param>
  private void OnSaveUser(Task task)
  {
    if (task.IsFaulted || task.IsCanceled)
    {
			Debug.Log ("OnSaveUser::Failed to update User with t.IsFaulted=" + task.IsFaulted);
			// Debug.Log ("OnSaveUser::Failed to update User with t.IsCanceled=" + task.IsCanceled);
			
			// The login failed. Check t.Exception to see why.
			// Debug.Log ("OnSaveUser::Failed to update User with t.Exception=" + task.Exception + task.Exception.Message);
      GetInnerException(task.Exception);
			User.PrintProperties ();
      User.SaveAsync(); // Try Again
    }
    else
    {
      Debug.Log("OnSaveUser user saved properly");
    }
  }

	/// <summary>
	/// Inits the user.
	/// </summary>
	private void InitUser()
	{
		if (User.CreatedAt == User.UpdatedAt || User.Slots == 0)
		{
			Debug.Log ("New user created so initializing...");			
			_currency = 50;
			User.SetStartingValues (User.ObjectId, User.Username,_slots, _currency, _experience, _fbUserID, SystemInfo.deviceUniqueIdentifier, _fbUserInfo);
			_hasAvatar = false;
			// User.PrintProperties ();
			// User.UpdateCloudAsync ();

      User.SaveAsync().ContinueWith(t => {
        Debug.Log("InitUser User.SaveAsync completed");
        OnSaveUser(t);
        GetAcceptChallengeCount();
        GetCompleteChallengeCount();
        //FacebookConnection.Instance.GetRequestIDs(OnRequestIdsComplete);
        CleanupGameStatus();
        Debug.Log("CleanupGameStatus completed");
      });
 
      // StartCoroutine (DGUtils.WaitTaskAndDoAction(User.SaveAsync(), OnSaveUser));
		}
		else 
		{
			Debug.Log("user values already initialized so not re-initializing");
			string avatarName = GetStringUserAttribute("AvatarName");
			Debug.Log("Avatar name= "+ avatarName);
			_hasAvatar = avatarName == ""? false : true;

      GetAcceptChallengeCount();
      GetCompleteChallengeCount();
      CleanupGameStatus();
      Debug.Log("CleanupGameStatus completed 2");
		}
	  StartCoroutine(GetFacebookProfilePicture(_fbUserID));

	}
	/// <summary>
	/// Raises the save user complete event.
	/// </summary>
	/// <param name="task">Task.</param>
	private void OnSaveUserComplete(Task task)
	{
		if (task.IsFaulted || task.IsCanceled)
		{
			Debug.Log ("Failed to update User with t.IsFaulted=" + task.IsFaulted);
			Debug.Log ("Failed to update User with t.IsCanceled=" + task.IsCanceled);
			
			// The login failed. Check t.Exception to see why.
			Debug.Log ("Failed to update User with t.Exception=" + task.Exception + task.Exception.Message);
			
		} else
			Debug.Log ("Successfully updated user");
	}

	/// <summary>
	/// Saves the int user attribute.
	/// </summary>
	/// <param name="attribute">Attribute.</param>
	/// <param name="value">Value.</param>
	public void SaveIntUserAttribute(string attribute, int value)
	{
		User[attribute] = value;
		User.UpdateCloudAsync();
	}

	/// <summary>
	/// Gets the int user attribute.
	/// </summary>
	/// <returns>The int user attribute.</returns>
	/// <param name="attribute">Attribute.</param>
	public int GetIntUserAttribute(string attribute)
	{
		return User.Get<int>(attribute);
	}

	/// <summary>
	/// Saves the string user attribute.
	/// </summary>
	/// <param name="attribute">Attribute.</param>
	/// <param name="value">Value.</param>
	public void SaveStringUserAttribute(string attribute, string value)
	{
		User[attribute] = value;
		User.UpdateCloudAsync();
	}

	/// <summary>
	/// Gets the string user attribute.
	/// </summary>
	/// <returns>The string user attribute.</returns>
	/// <param name="attribute">Attribute.</param>
	public string GetStringUserAttribute(string attribute)
	{
		return User.Get<string>(attribute);
	}


	/// <summary>
	/// Gets the experience.
	/// </summary>
	/// <returns>The experience.</returns>
	public int GetExperience()
	{
		return GetIntUserAttribute("experience");
	}

	/// <summary>
	/// Sets the experience.
	/// </summary>
	/// <returns></returns>
	/// <param name="experience">Experience.</param>
	public void SetExperience(int experience)
	{
    Debug.Log("CloudStorageController::SetExperience to " + experience);
		SaveIntUserAttribute("experience", experience);
	}

	/// <summary>
	/// Sets the experience to opponent.
	/// </summary>
	/// <returns></returns>
	/// <param name="experience">Experience.</param>
	/// <param name="fbId">facebook id.</param>
	public void SetExperience(int experience, string fbId)
	{
    Debug.Log("SetExperience, about to try and get fbId=" + fbId);

		_challengeeExperience = experience;
		ParseQuery<UserSubClass>  query = new ParseQuery<UserSubClass>()
			.WhereEqualTo("fbId", fbId)
			.Limit(1);
		
    query.FindAsync().ContinueWith(t => {
      OnGetUserFromParse(t);
    });	
		// StartCoroutine (DGUtils.WaitTaskAndDoAction(query.FindAsync(), OnGetUserFromParse));
	}

	/// <summary>
	/// Raises the get user from parse event.
	/// </summary>
	/// <param name="task">Task.</param>
	private void OnGetUserFromParse(Task task)
	{
		Task<IEnumerable<UserSubClass>> taskUser = (Task<IEnumerable<UserSubClass>>)task;
		
		if (task.IsFaulted || task.IsCanceled)
		{
			Debug.Log ("Failed getting parse's user");
		}
		else 
		{
			UserSubClass tempUser = new UserSubClass();
			var users = taskUser.Result;
			// foreach(UserSubClass user in users) // TODO: Can restore this code once Unity issues patch
      AotSafe.ForEach<UserSubClass>(users, (user) =>
			{
				tempUser = user;
			});
			int experience = tempUser.Experience;
			tempUser.Experience = (experience + _challengeeExperience);
			tempUser.UpdateCloudAsync();
		}
	}


	/// <summary>
	/// Gets the games.
	/// </summary>
	/// <returns>The games.</returns>
	public int GetGames()
	{
		return GetIntUserAttribute("games");
	}
	
	/// <summary>
	/// Sets the games.
	/// </summary>
	public void SetGames()
	{
		int gameTemps = User.Games + 1;
		SaveIntUserAttribute("games", gameTemps);
	}

	/// <summary>
	/// Gets the dance dollars.
	/// </summary>
	/// <returns>The dance dollars.</returns>
	public int GetDanceDollars()
	{
		return GetIntUserAttribute("dollars");
	}

  /// <summary>
  /// Saves the int user attribute.
  /// </summary>
  /// <param name="attribute">Attribute.</param>
  /// <param name="value">Value.</param>
  public void WriteTransaction(string transType, int price, int total, string comment)
  {
    Debug.Log("WriteTransaction called for transType=" + transType + " price=" + price);

    Transaction transaction = new Transaction(); 
    transaction["username"] = User.Username;
    transaction["name"] = User.Name;
    transaction["type"] = transType;
    if (transType == "purchase") {
      transaction["dd_bought"] = price;
      transaction["dd_spent"] = 0; 
    } else if (transType == "win") {
      transaction["dd_bought"] = 0; 
      transaction["dd_spent"] = 0;
      transaction["dd_win"] = price;
    } else {
      transaction["dd_bought"] = 0; 
      transaction["dd_spent"] = price;
    }
    transaction["dd_total"] = total;
    transaction["comment"] = comment;
    transaction.UpdateCloudAsync();
  }

	/// <summary>
	/// Sets the dance dollars.
	/// </summary>
	/// <param name="currency">Currency.</param>
	public void SetDanceDollars(int currency, int price = 0, string transType = "purchase", string comment = "")
	{
		SaveIntUserAttribute("dollars", currency);
    WriteTransaction(transType, price, currency, comment); 
	}
	
	#endregion


	#region Challenges

  /// <summary>
  /// Get high score for a move. Returns 0 if not found
  /// </summary>
  /// <param name="score">Score.</param>
  public int GetHighScoreForMove(int move)
  {
    int index = GetHighScoreIndexForMove(move); 
    if (index >= 0)
      return Convert.ToInt32(User.HighScoreMoves[index+1]);
    else
      return 0;
  }

  /// <summary>
  /// Get high score index for a move. Returns -1 if not found
  /// </summary>
  /// <param name="score">Score.</param>
  public int GetHighScoreIndexForMove(int move)
  {
    var newHighScoreMoves = User.HighScoreMoves;
    var i = 0;
    bool updateCloud = false;

    int index = -1;
    int movesCount = User.HighScoreMoves.Count;
    int j = 0;
    while (j < movesCount && index < 0) {
      int idMove = Convert.ToInt32(User.HighScoreMoves[j]);
      if (move == idMove) {
        index = j;
      }
      j = j + 2;
    }
    return index;
  }

  /// <summary>
  /// Updates high score per move 
  /// </summary>
  /// <param name="scores">Scores.</param>
  public void UpdateHighScoresForMoves(IList<object> scores)
  {
    int[] moveList = new int[4] {100, 101, 200, 201};
    IList<object> moves = new List<object> ();

    var newHighScoreMoves = User.HighScoreMoves;
    var i = 0;
    bool updateCloud = false;

    foreach(int idMove in moveList)
    {
      int highScore = (int) scores[i];

      int index = GetHighScoreIndexForMove(idMove);

      if (index == -1) {
        newHighScoreMoves.Add(idMove);
        newHighScoreMoves.Add(highScore);
        updateCloud = true;
        Debug.Log("ADDED HIGH SCORE of " + highScore + " for new move " + idMove);
      } else {
        int oldScore = Convert.ToInt32(User.HighScoreMoves[index+1]);
        if (highScore > oldScore) {
          newHighScoreMoves[index+1] = highScore;
          updateCloud = true;
          Debug.Log("SET NEW HIGH SCORE of " + highScore + " over oldScore " + oldScore + " for move " + idMove);
        } 
      }
    
      i++;
      if (updateCloud)
        User.UpdateCloudAsync();
    }
  }

	/// <summary>
	/// Creates the challenge.
	/// </summary>
	/// <param name="score">Score.</param>
	/// <param name="message">Message.</param>
	public void CreateChallenge(int score, IList<object> scores, string message)
	{
		Debug.Log ("CreateChallenge, score= " + score);
		
		_challenge = new Game();
		int level = 5;

		_challenge.SetStartingValues(_fbUserID, FBChallengeeID, User.Username, User.Name, CHALLENGE_CREATED,score, scores, level, message, CreateDanceCombo(), "Rock me baby","Rock me baby","");
		_challenge.UpdateCloudAsync();

    FBCreateOpenGraphStory("choose", "song", User.FbId);
	}

	/// <summary>
	/// Creates the challenge.
	/// </summary>
	/// <param name="message">Message.</param>
	public void CreateChallenge()
	{
		Debug.Log ("CreateChallenge");

		_challenge = new Game();
		int level = 5;
		_challenge.SetStartingValues(_fbUserID, FBChallengeeID, User.Username, User.Name, level, CHALLENGE_CREATED);
		_challenge.UpdateCloudAsync();

		Debug.Log ("Challenge objectId = " + _challenge.ObjectId);
	}

  /// <summary>
  /// Update challenge.
  /// </summary>
  public void UpdateChallengeAtStartDancing()
  {
    Debug.Log ("UpdateChallengeAtStartDancing");
   
    _challenge.ChallengerLevel = 3;
    _challenge.SongName = "Rock me baby";
    _challenge.SongURL = "http://www.google.com";
    _challenge.DanceCombo = CreateDanceCombo();
    _challenge.UpdateCloudAsync();
  }

	/// <summary>
	/// Sets the combo challenge.
	/// </summary>
	public void SetComboChallenge()
	{
		_challenge.DanceCombo = CreateDanceCombo ();
    _challenge.UpdateCloudAsync();
	}

	/// <summary>
	/// Sets the status challenge.
	/// </summary>
	/// <param name="status">Status.</param>
	public void SetStatusChallenge(string status)
	{
		_challenge.Status = status;
	}

	/// <summary>
	/// Sets the score list.
	/// </summary>
	/// <param name="scores">Scores.</param>
	public void SetScoreList(IList<object> scores)
	{
    Debug.Log("SetScoreList");

		if(false)
			_challenge.ChallengeeScoreList = scores;
		else
			_challenge.ChallengerScoreList = scores;

    _challenge.UpdateCloudAsync();

    UpdateHighScoresForMoves(scores);
	}

  /// <summary>
  /// Set Facebook Score
  /// </summary>
  /// <param name="score">Score.</param>
  public void SetFacebookScore(int score)
  {
    Debug.Log("CloudStorageController::SetFacebookScore to " + score);
    string accessToken = "";
#if UNITY_EDITOR
    _fbUserID = FBID;
    accessToken = ACCESSTOKEN;
#else
    _fbUserID = FB.UserId;
    accessToken = FB.AccessToken;
#endif
    string url = string.Format("https://graph.facebook.com/" + _fbUserID + "/scores?score=" + Convert.ToInt32(score) + "&access_token={0}", accessToken);
		FB.API(url, HttpMethod.GET, FBScoreCallback);
  }

	/// <summary>
	/// FBs the score callback.
	/// </summary>
	/// <param name="response">Response.</param>
	private void FBScoreCallback(FBResult response) 
	{
		Debug.Log(response.Text != null ? "got Facebook score callback with data" : "got Facebook score callback with no data");
		FbDebug.Log("FB text=" + response.Text);
	}

	/// <summary>
	/// Sets the total score.
	/// </summary>
	/// <param name="score">Score.</param>
	public void SetTotalScore(int score)
	{
    Debug.Log("SetTotalScore score=" + score);

		if(false)
			_challenge.ChallengeeScore = score;
		else
			_challenge.ChallengerScore = score;

    _challenge.UpdateCloudAsync();

    if (score > User.HighScore) 
      User.HighScore = score;
    
    User.UpdateCloudAsync();
    SetFacebookScore(score);
	}

	/// <summary>
	/// Sets the message.
	/// </summary>
	/// <param name="message">Message.</param>
	public void SetMessage(string message)
	{
		if(false)
			_challenge.ChallengeeComment = message;
		else
			_challenge.ChallengerComment = message;

    _challenge.UpdateCloudAsync();
	}

	/// <summary>
	/// Gets the level.
	/// </summary>
	/// <returns>The level.</returns>
	/// <param name="message">Message.</param>
	public int GetLevel()
	{
		if(false)
			return _challenge.ChallengeeLevel;
		else
			return _challenge.ChallengerLevel;
	}

  /// <summary>
  /// Gets the level from experience.
  /// </summary>
  /// <returns>The level.</returns>
  public int GetLevelFromExp()
  {
    int experience = GetExperience();
    if (experience == 0)
      return 0;

    double expDiv = Convert.ToDouble(experience) / BASE_EXP_PER_LEVEL;
    double levelDouble = Math.Pow(expDiv, 0.6);
    // Debug.Log("GetLevelFromExp, experience=" + experience + " level=" + levelDouble);

    return Convert.ToInt32(Math.Floor(levelDouble));
  }

  /// <summary>
  /// Gets the level progress from experience.
  /// </summary>
  /// <returns>The level.</returns>
  public float GetLevelProgressFromExp(int levelA = -1)
  {
    int experience = GetExperience();

    if (experience == 0)
      return (float) 0.0;

    double expDiv = Convert.ToDouble(experience) / BASE_EXP_PER_LEVEL;
    double levelDouble = Math.Pow(expDiv, 0.6);
    int level = Convert.ToInt32(Math.Floor(levelDouble));

    /*if (level == -1) {
      double expDiv = Convert.ToDouble(experience) / BASE_EXP_PER_LEVEL;
      double levelDouble = Math.Pow(expDiv, 0.6);
      level = (int) Convert.ToInt32(Math.Floor(levelDouble));
    }  */

    int levelPlusOne = level + 1;
    double expFloat = 1.0 / 0.6;
    double levelExp = BASE_EXP_PER_LEVEL * Math.Pow(level, (expFloat));
    double levelPlusOneExp = BASE_EXP_PER_LEVEL * Math.Pow(levelPlusOne, (expFloat));
    float progress = (float) (((double) experience - levelExp) / (levelPlusOneExp - levelExp));
    // float progress = (float)levelDouble - (float)level;
    Debug.Log("GetLevelProgressFromExp, experience=" + experience + " level=" + level + " progress=" + progress + " levelExp=" + levelExp + " levelPlusOneExp=" + levelPlusOneExp + " levelA = " + levelA);
    return progress;
  }

	/// <summary>
	/// Sets the track.
	/// </summary>
	public void SetTrack()
	{
		Debug.LogWarning(_challenge != null);

		_challenge.SongName = "Rock me baby";
		_challenge.SongURL = "http://www.google.com";
		_challenge.VideoURL = "http://www.youtube.com/watch?v=9bZkp7q19f0";
	}

	/// <summary>
	/// Finishs the challenge.
	/// </summary>
	public void FinishChallenge()
	{
    Debug.Log("FinishChallenge");
		int level = 5;
		_challenge.ChallengeeLevel = level;
		_challenge.WinnerUsername = _challenge.ChallengeeUsername;
		_challenge.WinnerName = _challenge.ChallengeeName;
		_challenge.WinnerFBId = _challenge.ChallengeeFBId;

    int scoreDiff = Math.Abs(_challenge.ChallengerScore - _challenge.ChallengeeScore);

    string action = "win";
    // Neither barely win nor school work and leave game in bad state so don't uncomment and leave in broken state
    // if (scoreDiff < 200)
    //  action = "barely_win";
    // TODO: Can't get this to work
    // else if (scoreDiff > 300)
    //  action = "school";

		_challenge.Status = CHALLENGE_COMPLETED;
		if(_challenge.ChallengerScore >= _challenge.ChallengeeScore)
		{
			_challenge.WinnerUsername = _challenge.ChallengerUsername;
			_challenge.WinnerName = _challenge.ChallengerName;
			_challenge.WinnerFBId = _challenge.ChallengerFBId;
/* Not giving XP to challenger winner since skips awarding Dance $, Writing Transaction and User doesn't see themselves level up
			//gains xp for opponent when opponent player wins challenge
      if (_challenge.ChallengerFBId != _fbUserID && DGConfiguration.isAcceptingChallenge) 
			  SetExperience(DGSettings.GetChallengeWinsXP(), _challenge.ChallengerFBId);
*/
      _challenge.WinningScore = _challenge.ChallengerScore;
      FBCreateOpenGraphStory(action, "dance battle", _challenge.ChallengerFBId);
		}
		else
		{
      _challenge.WinningScore = _challenge.ChallengeeScore;
      FBCreateOpenGraphStory(action, "dance_battle");
		}
	}

	/// <summary>
	/// Saves the challenge to parse.
	/// </summary>
	public Task SaveChallengeToParse()
	{
    Debug.Log("SaveChallengeToParse entered");
		return _challenge.UpdateCloudAsync();
	}

	/// <summary>
	/// Updates the challenge.
	/// </summary>
	/// <param name="challenge">Challenge.</param>
	public void UpdateChallenge(Game challenge)
	{
    Debug.Log("UpdateChallenge called");
		_challenge = new Game();
		_challenge = challenge;
		_challenge.ChallengeeName = User.Name;
		_challenge.ChallengeeUsername = User.Username;
		_challenge.ChallengeeFBId = User.FbId;
    _challenge.UpdateCloudAsync();
	}

	/// <summary>
	/// Gets the challenge scores list.
	/// </summary>
	/// <returns>The challenge scores list.</returns>
	public IList<object> GetChallengeScoresList()
	{
		return _challenge.ChallengerScoreList;
	}

	/// <summary>
	/// Gets the remaining challenges.
	/// </summary>
	/// <param name="action">Action.</param>
	public void GetRemainingChallenges(Action<IEnumerable<Game>> action)
	{
    Debug.Log("GetRemainingChallenges");
		_isWaitingAcceptChallenge = true;
		var query = new ParseQuery<Game>()
			.WhereEqualTo("challengeeFBId", _fbUserID)
			.WhereEqualTo("status", CHALLENGE_CHALLENGED)
			// .WhereNotEqualTo("danceCombo", "[0,0,0,0]")
			.WhereGreaterThan("challengerScore", -1)
			.OrderByDescending("createdAt");
		StartCoroutine (WaitingAcceptChallenge(query.FindAsync(), action));

		/*if(action!=null)
		{
			User.AcceptGames = _acceptChallengesCount;
			User.UpdateCloudAsync();
		}*/
	}
	
	/// <summary>
	/// Gets the accept challenge count.
	/// </summary>
	public void GetAcceptChallengeCount()
	{
    Debug.Log("GetAcceptChallengeCount entering and about to call query");
		var query = new ParseQuery<Game>()
			.WhereEqualTo("challengeeFBId", _fbUserID)
			.WhereEqualTo("status", CHALLENGE_CHALLENGED)
			.WhereGreaterThan("challengerScore", -1);

    Debug.Log("GetAcceptChallengeCount called query");

    query.CountAsync().ContinueWith(t => {
      Debug.Log("GetAcceptChallengeCount did CountAsync");

      OnAcceptChallengeCount(t);
    });

    Debug.Log("GetAcceptChallengeCount exiting");
		// StartCoroutine (DGUtils.WaitTaskAndDoAction (query.CountAsync(), OnAcceptChallengeCount));	 
	}

	/// <summary>
	/// Raises the accept challenge count event.
	/// </summary>
	/// <param name="task">Task.</param>
	private void OnAcceptChallengeCount(Task task)
	{
		int count = 0;
		if(!(task.IsCanceled || task.IsFaulted))
		{
			Task<int> result = (Task<int>)task;
			count = result.Result;
		}
		_acceptChallengesCount = count;
		_isAcceptChallengeReady = true;
	}
	
	/// <summary>
	/// Gets the completed challenges.
	/// </summary>
	/// <param name="action">Action.</param>
	public void GetCompletedChallenges(Action<IEnumerable<Game>> action)
	{
    Debug.Log("CloudStorageController::GetCompletedChallenges called");
		_isWaitingCompleteChallenge = true;
		var challengee = new ParseQuery<Game>()
			.WhereEqualTo("challengeeFBId", _fbUserID)
			.WhereEqualTo("status", CHALLENGE_COMPLETED);
		
		var challenger = new ParseQuery<Game>()
			.WhereEqualTo("challengerFBId", _fbUserID)
			.WhereEqualTo("status", CHALLENGE_COMPLETED);
		
		var query = challengee.Or(challenger)
			.OrderByDescending("updatedAt");

		StartCoroutine (WaitingCompleteChallenge(query.FindAsync(), action));

		/*if(action!=null)
		{
			User.CompleteGames = _completeChallengesCount;
			User.UpdateCloudAsync();
		}*/
	}

  /// <summary>
  /// Callback passed list of highest winning score called 
  /// </summary>
  /// <param name="task">Task.</param>
  private void OnGetHighestWinningScores(IEnumerable<Game> games)
  {
   
	  // foreach (Game game in games) // TODO: Can restore after Unity patch
    AotSafe.ForEach<Game>(games, (game) =>
    {
      Debug.Log("Winning score: " + game.WinningScore + " for " + game.WinnerName + " with FB ID " + game.WinnerFBId);
    });
  }

  /// <summary>
  /// Gets the list of highest winning scores 
  /// </summary>
  /// <param name="action">Action.</param>
  // public void GetHighestWinningScores(int days = 1, Action<IEnumerable<Game>> action = null)
  public void GetHighestWinningScores(int days = 1, Action<IEnumerable<Game>> action = null)
  {
    Debug.Log("Inside GetHighestWinningScores...");

    DateTime pastTime = DateTime.UtcNow - TimeSpan.FromHours(days * 24);

    ParseQuery<Game> query = new ParseQuery<Game>()
      .WhereEqualTo("status", CHALLENGE_COMPLETED)
      .WhereGreaterThan("updatedAt", pastTime)
      .Limit(10)
      .OrderByDescending("winningScore");

    if (action == null)
      action = OnGetHighestWinningScores;

    StartCoroutine (WaitingGeneralGames(query.FindAsync(), action));
  }
	
	/// <summary>
	/// Gets the complete challenge count.
	/// </summary>
	public void GetCompleteChallengeCount()
	{
		// var challenger = ParseObject.GetQuery("Game")
		var challenger = new ParseQuery<Game>()
			.WhereEqualTo("challengerFBId", _fbUserID)
			.WhereEqualTo("status", CHALLENGE_COMPLETED);

		// var challengee = ParseObject.GetQuery("Game")
		var challengee = new ParseQuery<Game>()
			.WhereEqualTo("challengeeFBId", _fbUserID)
			.WhereEqualTo("status", CHALLENGE_COMPLETED);

		var query = challengee.Or(challenger);

    query.CountAsync().ContinueWith(t => {
      OnCompleteChallengeCount(t);
    });
		// StartCoroutine (DGUtils.WaitTaskAndDoAction (query.CountAsync(), OnCompleteChallengeCount));	 
	}
	
	/// <summary>
	/// Raises the complete challenge count event.
	/// </summary>
	/// <param name="task">Task.</param>
	private void OnCompleteChallengeCount(Task task)
	{
		int count = 0;
		if(!(task.IsCanceled || task.IsFaulted))
		{
			Task<int> result = (Task<int>)task;
			count = result.Result;
		}
		_completeChallengesCount = count;
		_isCompleteChallengeReady = true;
	}

  /// <summary>
  /// Gets the complete challenge count.
  /// </summary>
  public void CleanupGameStatus()
  {
    Debug.Log("CleanupGameStatus, _fbUserID=" + _fbUserID);

    var challenger = new ParseQuery<Game>()
      .WhereEqualTo("challengerFBId", _fbUserID)
      .WhereEqualTo("status", CHALLENGE_CREATED);

    var challengee = new ParseQuery<Game>()
      .WhereEqualTo("challengeeFBId", _fbUserID)
      .WhereEqualTo("status", CHALLENGE_CHALLENGEE_STARTED);

    var query = challengee.Or(challenger);
    // StartCoroutine (DGUtils.WaitTaskAndDoAction (query.FindAsync(), OnCleanupGameStatus));
    query.FindAsync().ContinueWith(t => {
      OnCleanupGameStatus(t);
    });
  }

  /// <summary>
  /// Raises the complete challenge count event.
  /// </summary>
  /// <param name="task">Task.</param>
  private void OnCleanupGameStatus(Task task)
  {
    Debug.Log("OnCleanupGameStatus entered");

    if(!(task.IsCanceled || task.IsFaulted))
    {
      Debug.Log("OnCleanupGameStatus no error");
      Task<IEnumerable<Game>> result = task as Task<IEnumerable<Game>>;
      var games = result.Result;

      // foreach(Game game in games) {
      AotSafe.ForEach<Game>(games, (game) => {
        string status = (string) game["status"];
        Debug.Log("OnCleanupGameStatus status=" + status + " CHALLENGE_CREATED=" + CHALLENGE_CREATED + " CHALLENGE_COMPLETED=" + CHALLENGE_COMPLETED);
        if (status == CHALLENGE_CREATED) {
          game["status"] = CHALLENGE_CHALLENGED;
          Debug.Log("Found game in created state for FB User=" + _fbUserID);
          game.UpdateCloudAsync();
        } else {
          game["status"] = CHALLENGE_COMPLETED;
          if(game.ChallengerScore >= game.ChallengeeScore)
          {
            game.WinnerUsername = game.ChallengerUsername;
            game.WinnerName = game.ChallengerName;
            game.WinnerFBId = game.ChallengerFBId;
            game.WinningScore = game.ChallengerScore;
          } else {
            game.WinnerUsername = game.ChallengeeUsername;
            game.WinnerName = game.ChallengeeName;
            game.WinnerFBId = game.ChallengeeFBId;
            game.WinningScore = game.ChallengeeScore;
          }

          Debug.Log("Found game in challengee started state for FB User=" + _fbUserID);
          game.UpdateCloudAsync(); 
        } 
      });
    }
  }
	
	/// <summary>
	/// Gets the completed challenges info.
	/// </summary>
	/// <param name="action">Action.</param>
	public void GetCompletedChallengesInfo(Action<IEnumerable<Game>> action)
	{
		var challengee = new ParseQuery<Game>()
			.WhereEqualTo("challengeeFBId", _fbUserID)
			.WhereEqualTo("challengerFBId", _fbChallengeeId)
			.WhereEqualTo("status", CHALLENGE_COMPLETED);
		
		var challenger = new ParseQuery<Game>()
			.WhereEqualTo("challengerFBId", _fbUserID)
			.WhereEqualTo("challengeeFBId", _fbChallengeeId)
			.WhereEqualTo("status", CHALLENGE_COMPLETED);
		
		var query = challengee.Or(challenger)
			.OrderByDescending("updatedAt");
		
		StartCoroutine (WaitingGeneralGames(query.FindAsync(), action));	    
	}

	/// <summary>
	/// Gets the challenges wins.
	/// </summary>
	/// <param name="challengerFBid">Challenger F bid.</param>
	/// <param name="challengeeFBid">Challengee F bid.</param>
	/// <param name="action">Action.</param>
	public void GetChallengesWins(string challengerFBid, string challengeeFBid, Action<Task> action)
	{
		var challengerWins = new ParseQuery<Game>()
			.WhereEqualTo("challengerFBId", challengerFBid)
			.WhereEqualTo("challengeeFBId", challengeeFBid)
			.WhereEqualTo("winnerFBId", challengerFBid)
			.WhereEqualTo("status", CHALLENGE_COMPLETED);
		
		var challengeeWins = new ParseQuery<Game>()
			.WhereEqualTo("challengerFBId", challengeeFBid)
			.WhereEqualTo("challengeeFBId", challengerFBid)
			.WhereEqualTo("winnerFBId", challengerFBid)
			.WhereEqualTo("status", CHALLENGE_COMPLETED);
		
		var query = challengerWins.Or(challengeeWins);

    query.CountAsync().ContinueWith(t => {
      action(t);
    });

		// StartCoroutine (DGUtils.WaitTaskAndDoAction(query.CountAsync(), action));	    
	}

	/// <summary>
	/// Gets the challenges global wins.
	/// </summary>
	/// <param name="action">Action.</param>
	public void GetChallengesGlobalWins(Action<Task> action)
	{
    Debug.Log("CloudStorageController::GetChallengesGlobalWins entered");
		var challengerGlobalWins = new ParseQuery<Game>()
			.WhereEqualTo("challengerFBId", _fbUserID)
			.WhereEqualTo("winnerFBId", _fbUserID)
			.WhereEqualTo("status", CHALLENGE_COMPLETED);
		
		var challengeeGlobalWins = new ParseQuery<Game>()
			.WhereEqualTo("challengeeFBId", _fbUserID)
			.WhereEqualTo("winnerFBId", _fbUserID)
			.WhereEqualTo("status", CHALLENGE_COMPLETED);

		var query = challengeeGlobalWins.Or(challengerGlobalWins);
	
    query.CountAsync().ContinueWith(t => {
      action(t);
    });
	
		// StartCoroutine (DGUtils.WaitTaskAndDoAction(query.CountAsync(), action));	    
	}

	/// <summary>
	/// Gets the challenges global lose.
	/// </summary>
	/// <param name="action">Action.</param>
	public void GetChallengesLose(string challengerFBid, string challengeeFBid, Action<Task> action)
	{
    Debug.Log("GetChallengesLose challengerFBid=" + challengerFBid + " challengeeFBid=" + challengeeFBid);

		var challengerLose = new ParseQuery<Game>()
			.WhereEqualTo("challengerFBId", challengerFBid)
			.WhereEqualTo("challengeeFBId", challengeeFBid)
			.WhereNotEqualTo("winnerFBId", challengerFBid)
			.WhereEqualTo("status", CHALLENGE_COMPLETED);
		
		var challengeeLose = new ParseQuery<Game>()
			.WhereEqualTo("challengerFBId", challengeeFBid)
			.WhereEqualTo("challengeeFBId", challengerFBid)
			.WhereNotEqualTo("winnerFBId", challengerFBid)
			.WhereEqualTo("status", CHALLENGE_COMPLETED);
		
		var query = challengerLose.Or(challengeeLose);
	  query.CountAsync().ContinueWith(t => {
      action(t);
    });
	
		// StartCoroutine (DGUtils.WaitTaskAndDoAction(query.CountAsync(), action));	    
	}

	/// <summary>
	/// Gets the challenges global lose.
	/// </summary>
	/// <param name="action">Action.</param>
	public void GetChallengesGlobalLose(Action<Task> action)
	{
    Debug.Log("CloudStorageController::GetChallengesGlobalLose entered");
		var challengerGlobalLose = new ParseQuery<Game>()
			.WhereEqualTo("challengerFBId", _fbUserID)
			.WhereNotEqualTo("winnerFBId", _fbUserID)
			.WhereEqualTo("status", CHALLENGE_COMPLETED);
		
		var challengeeGlobalLose = new ParseQuery<Game>()
			.WhereEqualTo("challengeeFBId", _fbUserID)
			.WhereNotEqualTo("winnerFBId", _fbUserID)
			.WhereEqualTo("status", CHALLENGE_COMPLETED);
		
		var query = challengeeGlobalLose.Or(challengerGlobalLose);
	  query.CountAsync().ContinueWith(t => {
      action(t);
    });
	
		// StartCoroutine (DGUtils.WaitTaskAndDoAction(query.CountAsync(), action));	    
	}

	/// <summary>
	/// Gets the last challenge.
	/// </summary>
	/// <param name="action">Action.</param>
	public void GetLastChallenge(Action<Task> action)
	{
		var challenger = new ParseQuery<Game>()
			.WhereEqualTo("challengerFBId", _fbUserID)
			.WhereEqualTo("status", CHALLENGE_COMPLETED);
		
		var challengee = new ParseQuery<Game>()
			.WhereEqualTo("challengeeFBId", _fbUserID)
			.WhereEqualTo("status", CHALLENGE_COMPLETED);
		
		var query = challengee.Or(challenger)
			.OrderByDescending("updatedAt")
			.Limit(1);

    query.FindAsync().ContinueWith(t => {
      action(t);
    });
		// StartCoroutine (DGUtils.WaitTaskAndDoAction(query.FindAsync(), action));	    
	}

	/// <summary>
	/// Gets the last challenge.
	/// </summary>
	/// <param name="action">Action.</param>
	public void GetLastChallenge(string challengerFBid, string challengeeFBid, Action<Task> action)
	{
		var challenger = new ParseQuery<Game>()
			.WhereEqualTo("challengerFBId", challengerFBid)
			.WhereEqualTo("challengeeFBId", challengeeFBid)
			.WhereEqualTo("status", CHALLENGE_COMPLETED);
		
		var challengee = new ParseQuery<Game>()
			.WhereEqualTo("challengerFBId", challengeeFBid)
			.WhereEqualTo("challengeeFBId", challengerFBid)
			.WhereEqualTo("status", CHALLENGE_COMPLETED);
		
		var query = challengee.Or(challenger)
			.OrderByDescending("updatedAt")
				.Limit(1);
	
    query.FindAsync().ContinueWith(t => {
      action(t);
    });

		// StartCoroutine (DGUtils.WaitTaskAndDoAction(query.FindAsync(), action));	    
	}


	/// <summary>
	/// Gets the current challenge.
	/// </summary>
	/// <returns>The current challenge.</returns>
	public Game GetCurrentChallenge()
	{
		return _challenge;
	}

	/// <summary>
	/// Gets the challenge at challengeId and action.
	/// </summary>
	/// <param name="challengeId">Challenge identifier.</param>
	/// <param name="action">Action.</param>
	public void GetChallengeAt(string challengeId, Action<Task> action)
	{
		var query = new ParseQuery<Game>()
			.WhereEqualTo("objectId", challengeId)
			.WhereEqualTo("status", CHALLENGE_CHALLENGED);
	
    query.FindAsync().ContinueWith(t => {
      action(t);
    });	
		// StartCoroutine (DGUtils.WaitTaskAndDoAction(query.FindAsync(), action));	    
	}

  /// <summary>
  /// Sets the status to challenged 
  /// </summary>
  /// <param name="score">Score.</param>
  public void SetStatusChallenged()
  {
    Debug.Log("SetStatusChallenged called");
    if (_challenge.Status == CHALLENGE_CHALLENGED)
		  _challenge.Status = CHALLENGE_CHALLENGEE_STARTED;
    else
		  _challenge.Status = CHALLENGE_CHALLENGED;
		_challenge.UpdateCloudAsync();
  }

	/// <summary>
	/// Updates the cloud challenge.
	/// </summary>
	/// <param name="score">Score.</param>
	public void UpdateCloudChallenge(int score)
	{
    Debug.Log("SCORE: UpdateCloudChallenge setting _challenge.ChallengeeScore = score");

		_challenge.ChallengeeScore = score;
		_challenge.WinnerUsername = _challenge.ChallengeeUsername;
		_challenge.WinnerName = _challenge.ChallengeeName;
		_challenge.WinnerFBId = _challenge.ChallengeeFBId;
		if(_challenge.ChallengerScore >= score)
		{
			_challenge.WinnerUsername = _challenge.ChallengerUsername;	
			_challenge.WinnerName = _challenge.ChallengerName;	
			_challenge.WinnerFBId = _challenge.ChallengerFBId;	
		}
		_challenge.Status = CHALLENGE_COMPLETED;
		_challenge.UpdateCloudAsync();
	}

  // Find Game ObjectId that needs a challengee given the User's username and their experience
  public void GetRandomGame(Action<Task> action)
  {
	
	_isWaitingRandomChallenge = true;
    Debug.Log("ENTERING GetRandomGame");
    var args = new Dictionary<string, object>();
    args["fb_id"] = User.FbId;
    args["level"] = GetLevelFromExp();
	CloudStorageController.Instance.StartCoroutine(WaitingRandomChallenge(ParseCloud.CallFunctionAsync<Game>("getRandomGame", args), action));
    Debug.Log("EXITING GetRandomGame 1");
  }
	
	#endregion

	#region Items

	/// <summary>
	/// Gets the user items info.
	/// </summary>
	/// <param name="action">Action.</param>
	public void GetUserItemsInfo()
	{
		OnUserItemsComplete();
	}

	/// <summary>
	/// Raises the user items complete event.
	/// </summary>
	private void OnUserItemsComplete()
	{
		bool hasItems = false;

		if(User.SongsOwned.Count == 0)
		{
			hasItems = false;
			return;
		}
			
	}
		
	/// <summary>
	/// Updates the items song.
	/// </summary>
	/// <param name="conten">Conten.</param>
	public void UpdateItemsSong()
	{
		Debug.Log ("Updating song's items");
		IList<object> songs = new List<object> ();
		User.SongsOwned = songs;
	}

	/// <summary>
	/// Updates the items moves.
	/// </summary>
	/// <param name="content">Content.</param>
	public void UpdateItemsMoves()
	{
		IList<object>  moves = new List<object> ();
		User.MovesOwned = moves;
	}

	/// <sumary>
	/// Updates the cloud user items.
	/// </summary>
	public void UpdateCloudUserItems()
	{
		User.UpdateCloudAsync();
	}

	/// <summary>
	/// Executes the call back.
	/// </summary>
	/// <param name="conten">Conten.</param>
	private void ExecuteCallBack()
	{
	}

  public static void GetInnerException(Exception exception)
  {
      if (exception.GetType().Equals(typeof(AggregateException)))
      {
          AggregateException aggregateException = (AggregateException)exception;
          foreach (Exception innerException in aggregateException.InnerExceptions)
          {
              GetInnerException(innerException);
          }
      }
      else
      {
          Debug.Log("InnerException: " + exception.ToString());
      }
  }

  public static void UpdateDanceDollars(Task task)
  {
    if (task.IsFaulted || task.IsCanceled)
    {
      // The login failed. Check t.Exception to see why.
      Debug.Log ("UpdateDanceDollars: Failed to update User with t.IsFaulted=" + task.IsFaulted);
      Debug.Log ("UpdateDanceDollars: Failed to update User with t.IsCanceled=" + task.IsCanceled);
 
      // The login failed. Check t.Exception to see why.
      Debug.Log ("UpdateDanceDollars: Failed to update User with t.Exception=" + task.Exception + task.Exception.Message);
      GetInnerException(task.Exception);
    }
    else
    {
      Task<string> taskRes = (Task<string>) task;
      int totalDollars = Convert.ToInt32(taskRes.Result);

      Debug.Log("UpdateDanceDollars: successfully bought " + _boughtDanceDollars + " and now have a total of " + totalDollars);

      CloudStorageController.Instance.SetDanceDollars(totalDollars, _boughtDanceDollars, "purchase", "unity");
		
      if(_callBackDanceDollar!=null)
      {
        _callBackDanceDollar();
        _callBackDanceDollar = null;
      }
      Debug.Log("UpdateDanceDollars: updated danceDollars User.dollars=" + User.Dollars);
      _boughtDanceDollars = 0;
    }
  }

  public static void BuyDanceDollarsFromFacebook(int quantity, Action action = null)
  {
    Debug.Log("ENTERING BuyDanceDollarsFromFacebook with quantity=" + quantity);

	_callBackDanceDollar = action;

    //setup product id's from Facebook Object browser: e.g.  https://developers.facebook.com/tools/debug/og/object?q=http%3A%2F%2Fdanceoff.fmigames.com%2Fopengraph%2F5000dds.html
    // https://graph.facebook.com/688688701183032 http://danceoff.fmigames.com/opengraph/200dds.html
    string canvas200 = "218299325021399"; 

    // https://graph.facebook.com/1399594546960391 http://danceoff.fmigames.com/opengraph/500dds.html
    string canvas500 = "1428809377355846"; 

    // https://graph.facebook.com/1428809377355846 http://danceoff.fmigames.com/opengraph/1000dds.html
    string canvas1000 = "1439993812885273";

    // https://graph.facebook.com/1439993812885273 http://danceoff.fmigames.com/opengraph/2500dds.html 
    string canvas2500 = "688688701183032"; 

    // https://graph.facebook.com/218299325021399 http://danceoff.fmigames.com/opengraph/5000dds.html
    string canvas5000 = "1399594546960391"; 
    string canvasprodid;

    switch (quantity) {
      case 200: 
        canvasprodid = canvas200;
        break;
      case 500:
      case 525:
        canvasprodid = canvas500;
        break;
      case 1000:
      case 1100:
        canvasprodid = canvas1000;
        break;
      case 2500:
      case 2800:
        canvasprodid = canvas2500;
        break;
      case 5000:
      case 6500:
        canvasprodid = canvas5000;
        break;
      default:
        canvasprodid = "";
        Debug.Log("ERROR: BuyDanceDollarsFromFacebook called with an illegal quantity " + quantity + " of dance dollars");
        return;
    }
#if !UNITY_EDITOR
    Debug.Log("About to call FB.Canvas.Pay");
    FB.Canvas.Pay(canvasprodid,callback: delegate(FBResult response) {
      string result = (string) response.Text;
      bool containsError = result.Contains("error_code");
      Debug.Log("FB.Canvas.Pay result = " + result + " result.Contains=" + containsError);
      if (!containsError) {
        JsonDict _fb_payment_id = new JsonDict(response.Text);
        var args = new Dictionary<string, object>();
        fb_payment_id = _fb_payment_id.GetString("payment_id");
        args["quantity"] = quantity;
#else
        var args = new Dictionary<string, object>();
        fb_payment_id = "567890";
        args["quantity"] = "500";
#endif
        args["username"] = User.Username;
        args["fb_payment_id"] = fb_payment_id;
        _boughtDanceDollars = quantity;
        Debug.Log("about to call ParseCloud buyFacebookDD with quantity=" + args["quantity"] + " username=" + args["username"] + " fb_payment_id=" + args["fb_payment_id"]);
        CloudStorageController.Instance.StartCoroutine(DGUtils.WaitTaskAndDoAction(ParseCloud.CallFunctionAsync<string>("buyFacebookDD", args), UpdateDanceDollars));
        Debug.Log("UpdateDanceDollars completed");
#if !UNITY_EDITOR
      } else {
        Debug.Log("Facebook payment purchase failed with error =" + response.Text);
      }
    });
#endif
  }
	
	#endregion
	/// <summary>
	/// Creates the dance combo.
	/// </summary>
	/// <returns>The dance combo.</returns>
	private IList<object> CreateDanceCombo()
	{
		int[] moveList = new int[4]{100, 101, 200, 201};
		IList<object> moves = new List<object> ();

		foreach(int idMove in moveList)
		{
			moves.Add (idMove);
		}
		return moves;
	}

	/// <summary>
	/// Waitings the random challenge.
	/// </summary>
	/// <returns>The random challenge.</returns>
	/// <param name="task">Task.</param>
	/// <param name="action">Action.</param>
	private IEnumerator WaitingRandomChallenge(Task task, Action<Task> action)
	{
		while (!task.IsCompleted)
			yield return null;

		if(_isWaitingRandomChallenge)
		{
			_isWaitingRandomChallenge = false;
			action(task);
		}
	}

	/// <summary>
	/// Waitings the accept challenge.
	/// </summary>
	/// <returns>The accept challenge.</returns>
	/// <param name="task">Task.</param>
	/// <param name="action">Action.</param>
	private IEnumerator WaitingAcceptChallenge(Task task, Action<IEnumerable<Game>> action)
	{
		while (!task.IsCompleted)
			yield return null;

		Task<IEnumerable<Game>> result = task as Task<IEnumerable<Game>>;
		_acceptChallenges = result.Result;

		if(_isWaitingAcceptChallenge)
		{
			_isWaitingAcceptChallenge = false;
			if(action!=null)
			{
				action(_acceptChallenges);
				User.AcceptGames = _acceptChallengesCount;
				User.UpdateCloudAsync();
			}
		}
	}
	
	/// <summary>
	/// Waitings the complete challenge.
	/// </summary>
	/// <returns>The complete challenge.</returns>
	/// <param name="task">Task.</param>
	/// <param name="action">Action.</param>
	private IEnumerator WaitingCompleteChallenge(Task task, Action<IEnumerable<Game>> action)
	{
		while (!task.IsCompleted)
			yield return null;
		
		Task<IEnumerable<Game>> result = task as Task<IEnumerable<Game>>;
		_completeChallenges = result.Result;

		if(_isWaitingCompleteChallenge)
		{
			_isWaitingCompleteChallenge = false;
			if(action!=null)
			{
				action(_completeChallenges);
				User.CompleteGames = _completeChallengesCount;
				User.UpdateCloudAsync();
			}
		}
	}

	/// <summary>
	/// Waitings the learder board.
	/// </summary>
	/// <returns>The learder board.</returns>
	/// <param name="task">Task.</param>
	/// <param name="action">Action.</param>
	private IEnumerator WaitingGeneralGames(Task task, Action<IEnumerable<Game>> action)
	{
		while (!task.IsCompleted)
			yield return null;
		
		Task<IEnumerable<Game>> result = task as Task<IEnumerable<Game>>;
		var games = result.Result;

		if(_isWaitingGeneralGames)
		{
			if(action!=null)
				action(games);
		}
	}
	
	/// <summary>
	/// FBs the request callback.
	/// </summary>
	/// <param name="response">Response.</param>
	private void OnRequestIdsComplete(FBResult response) 
	{
		if(response == null)
		{
			//GetChallengeAt("aFHf096kyD", OnFindChallenge);
			_isFindChallengeReady = true;
			return;
		}
		JsonDict result =  new JsonDict ();
		result.Deserialize(response.Text);
		string challengeId = result.GetString("data");
		GetChallengeAt(challengeId, OnFindChallenge);
	}

	/// <summary>
	/// Raises the get challenge event.
	/// </summary>
	/// <param name="task">Task.</param>
	private void OnFindChallenge(Task task)
	{
		Task<IEnumerable<Game>> result = task as Task<IEnumerable<Game>>;
		var games = result.Result;
		foreach(Game challenge in games)
		{
			_challenge = challenge;
			_needShowPopUp = true;
		}
		_isFindChallengeReady = true;
	}
	#endregion
	
	#region Test
	#endregion

}
