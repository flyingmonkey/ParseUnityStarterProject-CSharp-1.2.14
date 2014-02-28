// DanceCombo.cs
using Parse;
using UnityEngine; // To support Debug.Log only
using System.Collections;
using System.Collections.Generic;
 
[ParseClassName("DanceCombo")]
public class DanceCombo : ParseObject
{
  
  [ParseFieldName("username")]
  public string Username   
  {
    get { return GetProperty<string>("Username"); }
    set { SetProperty<string>(value, "Username"); }
  }

  [ParseFieldName("moves")]
  public IList<object> Moves
  {
    get { return GetProperty<IList<object>>("Moves"); }
    set { SetProperty<IList<object>>(value, "Moves"); }
  }

  public void UpdateCloudAsync()
  {
    SaveAsync().ContinueWith(t => {
      if (t.IsFaulted || t.IsCanceled)
      {
          // The login failed. Check t.Exception to see why.
          Debug.Log("Failed to update DanceCombo with t.Exception=" + t.Exception);
      } else
        Debug.Log("Successfully updated DanceCombo");
    });
  }

  public void PrintProperties()
  {
    Debug.Log("DanceCombo Properties Username=" + Username + " Moves={" 
      + Moves[0] + "," 
      + Moves[1] + "," 
      + Moves[2] + "," 
      + Moves[3] + "}"); 
  }
}
 
