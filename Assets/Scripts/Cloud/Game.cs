using Parse;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

[ParseClassName("Game")]
public class Game : ParseObject 
{

	[ParseFieldName("challengerFBId")]
	public string ChallengerFBId   
	{
		get { return GetProperty<string>("ChallengerFBId"); }
		set { SetProperty<string>(value, "ChallengerFBId"); }
	}
	
	[ParseFieldName("challengeeFBId")]
	public string ChallengeeFBId   
	{
		get { return GetProperty<string>("ChallengeeFBId"); }
		set { SetProperty<string>(value, "ChallengeeFBId"); }
	}


	[ParseFieldName("challengerName")]
	public string ChallengerName   
	{
		get { return GetProperty<string>("ChallengerName"); }
		set { SetProperty<string>(value, "ChallengerName"); }
	}

	[ParseFieldName("challengeeName")]
	public string ChallengeeName   
	{
		get { return GetProperty<string>("ChallengeeName"); }
		set { SetProperty<string>(value, "ChallengeeName"); }
	}

	[ParseFieldName("challengerUsername")]
	public string ChallengerUsername   
	{
		get { return GetProperty<string>("ChallengerUsername"); }
		set { SetProperty<string>(value, "ChallengerUsername"); }
	}

	[ParseFieldName("challengeeUsername")]
	public string ChallengeeUsername   
	{
		get { return GetProperty<string>("ChallengeeUsername"); }
		set { SetProperty<string>(value, "ChallengeeUsername"); }
	}

	[ParseFieldName("winnerName")]
	public string WinnerName   
	{
		get { return GetProperty<string>("WinnerName"); }
		set { SetProperty<string>(value, "WinnerName"); }
	}


	[ParseFieldName("winnerUsername")]
	public string WinnerUsername   
	{
		get { return GetProperty<string>("WinnerUsername"); }
		set { SetProperty<string>(value, "WinnerUsername"); }
	}

  [ParseFieldName("winnerFBId")]
  public string WinnerFBId
  {
    get { return GetProperty<string>("WinnerFBId"); }
    set { SetProperty<string>(value, "WinnerFBId"); }
  }
	
	[ParseFieldName("status")]
	public string Status   
	{
		get { return GetProperty<string>("Status"); }
		set { SetProperty<string>(value, "Status"); }
	}

	[ParseFieldName("challengerScore")]
	public int ChallengerScore   
	{
		get { return GetProperty<int>("ChallengerScore"); }
		set { SetProperty<int>(value, "ChallengerScore"); }
	}

	[ParseFieldName("challengeeScore")]
	public int ChallengeeScore   
	{
		get { return GetProperty<int>("ChallengeeScore"); }
		set { SetProperty<int>(value, "ChallengeeScore"); }
	}

	[ParseFieldName("challengerScoreList")]
	public IList<object> ChallengerScoreList   
	{
		get { return GetProperty<IList<object>>("ChallengerScoreList"); }
		set { SetProperty<IList<object>>(value, "ChallengerScoreList"); }
	}
	
	[ParseFieldName("challengeeScoreList")]
	public IList<object> ChallengeeScoreList   
	{
		get { return GetProperty<IList<object>>("ChallengeeScoreList"); }
		set { SetProperty<IList<object>>(value, "ChallengeeScoreList"); }
	}

	[ParseFieldName("challengerLevel")]
	public int ChallengerLevel   
	{
		get { return GetProperty<int>("ChallengerLevel"); }
		set { SetProperty<int>(value, "ChallengerLevel"); }
	}


	[ParseFieldName("challengerComment")]
	public string ChallengerComment   
	{
		get { return GetProperty<string>("ChallengerComment"); }
		set { SetProperty<string>(value, "ChallengerComment"); }
	}

	[ParseFieldName("challengeeComment")]
	public string ChallengeeComment   
	{
		get { return GetProperty<string>("ChallengeeComment"); }
		set { SetProperty<string>(value, "ChallengeeComment"); }
	}
	

	[ParseFieldName("danceCombo")]
	public IList<object> DanceCombo   
	{
		get { return GetProperty<IList<object>>("DanceCombo"); }
		set { SetProperty<IList<object>>(value, "DanceCombo"); }
	}
	
	[ParseFieldName("songName")]
	public string SongName   
	{
		get { return GetProperty<string>("SongName"); }
		set { SetProperty<string>(value, "SongName"); }
	}

	[ParseFieldName("songURL")]
	public string SongURL   
	{
		get { return GetProperty<string>("SongURL"); }
		set { SetProperty<string>(value, "SongURL"); }
	}

	[ParseFieldName("videoURL")]
	public string VideoURL   
	{
		get { return GetProperty<string>("VideoURL"); }
		set { SetProperty<string>(value, "VideoURL"); }
	}
	
  [ParseFieldName("matchSemaphore")]
  public int MatchSemaphore 
  {    
    get { return GetProperty<int>("MatchSemaphore"); }
    set { SetProperty<int>(value, "MatchSemaphore"); }  
  }

  [ParseFieldName("challengeeLevel")]
  public int ChallengeeLevel   
  {
    get { return GetProperty<int>("ChallengeeLevel"); }
    set { SetProperty<int>(value, "ChallengeeLevel"); }
  }

  [ParseFieldName("winningScore")]
  public int WinningScore   
  {
    get { return GetProperty<int>("WinningScore"); }
    set { SetProperty<int>(value, "WinningScore"); }
  }
 

  private IList<object> initListScores()
  {
    var myList = new List<object>();
    for (var i = 0 ; i < 4 ; i++) {
      myList.Add(0);
    }
    return myList;
  }
 
	public void SetStartingValues(string challengerFBId, string challengeeFBId,string challengerUsername, string challengerName, string status, 
	                              int challengerScore, IList<object> scoresList, int challengerLevel, string message, IList<object> danceCombo,
	                              string songName, string urlSong,string urlVideo)
	{
		ChallengerFBId = challengerFBId;
		ChallengeeFBId = challengeeFBId;
		ChallengerUsername = challengerUsername;
		ChallengerName = challengerName;
		ChallengeeUsername = "";
		ChallengeeName = "";
		WinnerUsername = "";
		WinnerName = "";
		Status = status;
		ChallengerScore = challengerScore;
		ChallengeeScore = 0;
		ChallengerScoreList = scoresList;
		ChallengeeScoreList = initListScores(); 
		ChallengerLevel = challengerLevel;
		ChallengerComment = message;
		ChallengeeComment = "";
		DanceCombo = danceCombo;
		SongName = songName;
		SongURL = urlSong; // "http://GangnamStyle.com";
		VideoURL = urlVideo; // "http://www.youtube.com/watch?v=9bZkp7q19f0";
    MatchSemaphore = 0;
    WinningScore = 0;
	}

	public void SetStartingValues(string challengerFBId, string challengeeFBId, string challengerUsername, string challengerName, int challengerLevel, string status)
	{
		ChallengerFBId = challengerFBId;
		ChallengeeFBId = challengeeFBId;
		ChallengerUsername = challengerUsername;
		ChallengerName = challengerName;
		ChallengeeUsername = "";
		WinnerUsername = "";
		WinnerName = "";
		Status = status;
		ChallengerScoreList = initListScores();
  	ChallengeeScoreList = initListScores(); 
		ChallengerLevel = challengerLevel;
		ChallengeeLevel = 0;
		DanceCombo = new List<object>();
		ChallengerComment = "";
		ChallengeeComment = "";
		ChallengeeScore = 0;
		MatchSemaphore = 0;
		WinningScore = 0;

	}

	
	public Task UpdateCloudAsync()
	{
		return SaveAsync();
	}

	public void PrintProperties()
	{
		Debug.Log ("Game Properties challengerUsername=" + ChallengerUsername + " challengeeUsername=" + ChallengeeUsername + " winnerUsername=" + WinnerUsername 
						+ " status=" + Status + " challengerScore=" + ChallengerScore.ToString () + " challengeeScore=" + ChallengeeScore.ToString () + " challengerComment=" + ChallengerComment 
						+ " challengeeComment=" + ChallengeeComment + " songName=" + SongName + " songURL=" + SongURL + " videoURL=" + VideoURL + " winningScore=" + WinningScore + " winnerName=" + WinnerName);


	}
	
}
