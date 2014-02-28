using UnityEngine;
using System.Collections.Generic;
using MiniJSON;

public class JsonDict
{
	public Dictionary<string, object> data;

	public JsonDict ()
	{
		data = new Dictionary<string, object> ();
	}
	
	public JsonDict (Dictionary<string, object> data)
	{
		this.data = data;
	}
	
	public JsonDict (string jsonString)
	{
		Deserialize (jsonString);
	}
	
	public bool ContainsKey (string key)
	{
		return data != null && data.ContainsKey(key);
	}
	
	public void Deserialize (string jsonString)
	{
        if (string.IsNullOrEmpty(jsonString))
        {
			return;
        } 

		object obj = MiniJSON.Json.Deserialize (jsonString);

		if (obj is List<object> &&
			((List<object>)obj).Count > 0)
		{
			obj = ((List<object>)obj)[0];
			this.data = (Dictionary<string, object>)obj;
		}
		else if (obj is Dictionary<string, object>)
		{
			this.data = (Dictionary<string, object>) obj;
		}
		else
		{
			this.data = new Dictionary<string, object>();
		}
	}
	
	public bool GetBool (string key)
	{
		return GetInt(key) != 0;
	}
	
	public JsonDict GetDict (string key)
	{
		if (!data.ContainsKey(key)) return null;

		var newData = data[key];

		if (newData is Dictionary<string, object>)
			return new JsonDict(newData as Dictionary<string, object>);

		if (newData is List<object>)
			return new JsonDict((newData as List<object>)[0] as Dictionary<string, object>);

		return null;
	}
	

	public int GetInt (string key)
	{
		if (data != null && data.ContainsKey(key) && data[key] != null)
		{
			
			//return _getInt(data[key]);
			
			if (data[key] is int)
				return (int)data[key];
			
			if (data[key] is long)
				return (int)((long)data[key]);
			
			if (data[key] is string)
			{
				int result = 0;
				if (int.TryParse((string)data[key], out result))
					return result;
			}
			
		}
		return 0;
	}
	
	private int _getInt(object rawData)
	{
		if (rawData is int)
			return (int)rawData;
		
		if (rawData is long)
			return (int)((long)rawData);
		
		if (rawData is string)
		{
			int result = 0;
			if (int.TryParse((string)rawData, out result))
				return result;
		}
		
		return 0;
	}
	

	public List<object> GetList (string key)
	{
		if (data != null && data.ContainsKey(key) && data[key] is List<object>)
			return (List<object>)data[key];
		return null;
	}
	

	public long GetLong (string key)
	{
		if (data != null && data.ContainsKey(key) && data[key] != null)
		{
			if (data[key] is int)
				return (long)data[key];
			
			if (data[key] is long)
				return (long)data[key];
			
			if (data[key] is string)
			{
				int result = 0;
				if (int.TryParse((string)data[key], out result))
					return result;
			}
		}
		return 0;
	}
	
	public string GetString (string key)
	{
		if (data != null && data.ContainsKey(key) && data[key] != null)
		{
			if (data[key] is string)
				return (string)data[key];
			return data[key].ToString ();
		}
		return "";		
	}
	
	public List<string> GetStringList (string key)
	{
		string source = GetString (key);
		
		if (!string.IsNullOrEmpty (source))
		{
			List<string> list = new List<string> (source.Split(new char[] {','}, System.StringSplitOptions.RemoveEmptyEntries));
			list = list.ConvertAll<string> (item => item.Trim ());
			return list;
		}
		return new List<string> ();
	}	
	
	
	public List<int> GetIntList (string key)
	{
		List<object> _rawList = GetList(key);
	
		if (_rawList!=null)
		{
			return _rawList.ConvertAll<int>(item => _getInt(item));
		}
		
		return new List<int> ();
	}	
	
	
	public string Serialize ()
	{
		return MiniJSON.Json.Serialize (data);
	}
}
