using Parse;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

[ParseClassName("Transaction")]
public class Transaction : ParseObject 
{

	[ParseFieldName("username")]
	public string Username   
	{
		get { return GetProperty<string>("Username"); }
		set { SetProperty<string>(value, "Username"); }
	}
	
	[ParseFieldName("name")]
	public string Name   
	{
		get { return GetProperty<string>("Name"); }
		set { SetProperty<string>(value, "Name"); }
	}
	
	[ParseFieldName("dd_spent")]
	public int DDSpent  
	{
		get { return GetProperty<int>("DDSpent"); }
		set { SetProperty<int>(value, "DDSpent"); }
	}

	[ParseFieldName("dd_bought")]
	public int DDBought
	{
		get { return GetProperty<int>("DDBought"); }
		set { SetProperty<int>(value, "DDBought"); }
	}

	[ParseFieldName("dd_win")]
	public string DDWin  
	{
		get { return GetProperty<string>("DDWin"); }
		set { SetProperty<string>(value, "DDWin"); }
  }

	[ParseFieldName("dd_total")]
	public int DDTotal
	{
		get { return GetProperty<int>("DDTotal"); }
		set { SetProperty<int>(value, "DDTotal"); }
	}

	[ParseFieldName("type")]
	public string Type   
	{
		get { return GetProperty<string>("Type"); }
		set { SetProperty<string>(value, "Type"); }
	}

	[ParseFieldName("comment")]
	public string Comment  
	{
		get { return GetProperty<string>("Comment"); }
		set { SetProperty<string>(value, "Comment"); }
  }

	public Task UpdateCloudAsync()
	{
		return SaveAsync ();
	}

	public void PrintProperties()
	{
		Debug.Log ("Transaction Properties username=" + Username + " type=" + Type + " DD Spent=" + DDSpent + " DD Bought=" + DDBought + " DD Total=" + DDTotal + " comment=" + Comment + " DDWin=" + DDWin);
	}
	
}
