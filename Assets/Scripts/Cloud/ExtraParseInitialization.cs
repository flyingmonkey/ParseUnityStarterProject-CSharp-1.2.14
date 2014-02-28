// ExtraParseInitialization.cs (attach to your Parse

// Drag this script into a GameObject in the main scense of the Unity Editor
using UnityEngine;
using Parse;
 
public class ExtraParseInitialization : MonoBehaviour 
{
  void Awake()
  {
    ParseObject.RegisterSubclass<UserSubClass>();
    ParseObject.RegisterSubclass<DanceCombo>();
	ParseObject.RegisterSubclass<Game>();
  }
}
