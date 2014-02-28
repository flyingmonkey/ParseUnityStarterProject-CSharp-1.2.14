#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

public class DGUtils 
{
	#region Enums
	#endregion

	#region Delegates
	#endregion

	#region Constants
	#endregion

	#region Variables
	#endregion

	#region Properties
	#endregion

	#region Constructors
	#endregion

	#region Methods
	//Creates and instance of the prefab on the given path
	public static GameObject GetPrefabInstanceFromPath (string path)
	{
		UnityEngine.Object binary = Resources.Load(path);
		
		if (binary == null)
		{
			Debug.Log("The following prefab couldn't be instantiated: " + path);
			return null;
		}
		
		GameObject instance = GameObject.Instantiate(binary, Vector3.zero, Quaternion.identity) as GameObject;
		return instance;
	}

	public static GameObject GetPrefabInstanceFromPrefab (GameObject prefab)
	{		
		if (prefab == null)
		{
			Debug.Log("A prefab couldn't be instantiated");
			return null;
		}
		
		GameObject instance = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
		return instance;
	}
	
	//Convert seconds to beats with the given beats per minute
	public static float SecondsToBeats( float seconds, float bpm )
	{
		return seconds * bpm / 60;
	}

	//Convert beats to seconds in the given beats per minute
	public static float BeatsToSeconds( float beats, float bpm )
	{
		return ( beats * 60 ) / bpm;
	}
	
	public static IEnumerator WaitAndDoAction(float secconds, Action action)
	{
		yield return new WaitForSeconds(secconds);
		action();
	}

	public static IEnumerator WaitTaskAndDoAction(Task task, Action<Task> action)
	{
		while (!task.IsCompleted)
			yield return null;
		action(task);
	}


	/// <summary>
	/// Generates the list.
	/// </summary>
	/// <returns>The list.</returns>
	/// <param name="list">List.</param>
	public static int[] GenerateList(IList<object> list)
	{
		int[] comboInts = new int[4];
		int index = 0;
		foreach (object indexCombo in list) 
		{
			comboInts[index] = Convert.ToInt32(indexCombo);
			index++;
		}
		return comboInts;
	}

	public static IEnumerator DownLoadAsset<T>(string url, string assetName, int version, Action<T> OnDownloadComplete) where T : UnityEngine.Object
	{
		#if UNITY_EDITOR
		Caching.CleanCache();
		#endif
		WWW www = WWW.LoadFromCacheOrDownload (url, version);
		
		// Wait for download to complete
		yield return www;

		if(!string.IsNullOrEmpty(www.error))
		{			
			OnDownloadComplete(null);
		}
		else
		{		
			// Load and retrieve the AssetBundle
			AssetBundle bundle = www.assetBundle;
			
			// Load the object asynchronously		
			// UnityEngine.Object[] objects = bundle.LoadAll();
						
			// foreach(var item in objects)
			//	F2ULogManager.F2ULog("Downloaded " + item.name + " of type " + item.GetType().FullName, F2ULogManager.Type.Notice);

			AssetBundleRequest request = bundle.LoadAsync(assetName, typeof(T));
			
			// Wait for completion
			yield return request;
			
			// Get the reference to the loaded object
			T obj = request.asset as T;
			
			OnDownloadComplete(obj);
			
			// Unload the AssetBundles compressed contents to conserve memory
			bundle.Unload(false);
			
			// Frees the memory from the web stream
			www.Dispose();	
		}
	}

	public static string GetFileNameFromPath (string path)
	{
		int lastIndex = path.LastIndexOf ("/");
		return path.Substring (lastIndex + 1);
	}

	#endregion
}
