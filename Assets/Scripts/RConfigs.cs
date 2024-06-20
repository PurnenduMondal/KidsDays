using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Firebase.Extensions;
using Firebase.RemoteConfig;
using Firebase;
public class RConfigs : MonoBehaviour
{ 

    #region Config Variables
    public float t0r1 = 0, t0r2, c0r1, c0r2, r0r1, r0r2, r0r3, r0r4, s0r1, s0r2, s0r3, s0r4; 
    public float t1r1, t1r2, c1r1, c1r2, r1r1, r1r2, r1r3, r1r4, s1r1, s1r2, s1r3, s1r4;
    public float t2r1, t2r2, c2r1, c2r2, r2r1, r2r2, r2r3, r2r4, s2r1, s2r2, s2r3, s2r4;
    public float t3r1, t3r2, c3r1, c3r2, r3r1, r3r2, r3r3, r3r4, s3r1, s3r2, s3r3, s3r4;
    public float t4r1, t4r2, c4r1, c4r2, r4r1, r4r2, r4r3, r4r4, s4r1, s4r2, s4r3, s4r4;
    public float t5r1, t5r2, c5r1, c5r2, r5r1, r5r2, r5r3, r5r4, s5r1, s5r2, s5r3, s5r4;
    public float t6r1, t6r2, c6r1, c6r2, r6r1, r6r2, r6r3, r6r4, s6r1, s6r2, s6r3, s6r4;
    public float t7r1, t7r2, c7r1, c7r2, r7r1, r7r2, r7r3, r7r4, s7r1, s7r2, s7r3, s7r4;
    public float t8r1, t8r2, c8r1, c8r2, r8r1, r8r2, r8r3, r8r4, s8r1, s8r2, s8r3, s8r4;
    public float t9r1, t9r2, c9r1, c9r2, r9r1, r9r2, r9r3, r9r4, s9r1, s9r2, s9r3, s9r4;
    public float t10r1, t10r2, c10r1, c10r2, r10r1, r10r2, r10r3, r10r4, s10r1, s10r2, s10r3, s10r4;
    public float t11r1, t11r2, c11r1, c11r2, r11r1, r11r2, r11r3, r11r4, s11r1, s11r2, s11r3, s11r4;
    public float t12r1, t12r2, c12r1, c12r2, r12r1, r12r2, r12r3, r12r4, s12r1, s12r2, s12r3, s12r4;
    public float t13r1, t13r2, c13r1, c13r2, r13r1, r13r2, r13r3, r13r4, s13r1, s13r2, s13r3, s13r4;
    #endregion
 
    public static float tr1 = 1.0f,tr2 = 2f,cr1 = 0.5f,cr2 = 1.5f,rr1 = 1.0f,rr2 = 2f,rr3 = 0.5f,rr4 = 1.0f,sr1 = 2f,sr2 = 4f,sr3 = 1f,sr4 = 1.5f;
    public static string jsonConfig = null;
    DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;
    public static bool M = false;
    public static bool A = false;
    void Start()
    {
      if(jsonConfig == null)
        FatchRemoteConfigs();
      else
        DisplayData();
    }
  public void DisplayData() 
	{
    JsonUtility.FromJsonOverwrite(jsonConfig,this);
    SetValue();    
  }
  void Update()
  {
  }
    void SetValue()
    {
        if(SceneChanger.currentCategory == "Aquatic_Animals" && t0r1 != 0) {
        tr1 = t0r1;tr2 = t0r2;cr1 = c0r1;cr2 = c0r2;rr1 = r0r1;rr2 = r0r2;rr3 = r0r3;rr4 = r0r4;sr1 = s0r1;sr2 = s0r2;sr3 = s0r3;sr4 = s0r4;
        }
        if(SceneChanger.currentCategory == "Berries_and_Nuts" && t0r1 != 0) {
        tr1 = t1r1;tr2 = t1r2;cr1 = c1r1;cr2 = c1r2;rr1 = r1r1;rr2 = r1r2;rr3 = r1r3;rr4 = r1r4;sr1 = s1r1;sr2 = s1r2;sr3 = s1r3;sr4 = s1r4;
        }
        if(SceneChanger.currentCategory == "Birds" && t0r1 != 0) {
        tr1 = t2r1;tr2 = t2r2;cr1 = c2r1;cr2 = c2r2;rr1 = r2r1;rr2 = r2r2;rr3 = r2r3;rr4 = r2r4;sr1 = s2r1;sr2 = s2r2;sr3 = s2r3;sr4 = s2r4;
        }
        if(SceneChanger.currentCategory == "Clothes" && t0r1 != 0) {
        tr1 = t3r1;tr2 = t3r2;cr1 = c3r1;cr2 = c3r2;rr1 = r3r1;rr2 = r3r2;rr3 = r3r3;rr4 = r3r4;sr1 = s3r1;sr2 = s3r2;sr3 = s3r3;sr4 = s3r4;
        }
        if(SceneChanger.currentCategory == "Domestic_Animals" && t0r1 != 0) {
        tr1 = t4r1;tr2 = t4r2;cr1 = c4r1;cr2 = c4r2;rr1 = r4r1;rr2 = r4r2;rr3 = r4r3;rr4 = r4r4;sr1 = s4r1;sr2 = s4r2;sr3 = s4r3;sr4 = s4r4;        
        }
        if(SceneChanger.currentCategory == "Flowers" && t0r1 != 0) {
        tr1 = t5r1;tr2 = t5r2;cr1 = c5r1;cr2 = c5r2;rr1 = r5r1;rr2 = r5r2;rr3 = r5r3;rr4 = r5r4;sr1 = s5r1;sr2 = s5r2;sr3 = s5r3;sr4 = s5r4;        
        }
        if(SceneChanger.currentCategory == "Fruits" && t0r1 != 0) {
        tr1 = t6r1;tr2 = t6r2;cr1 = c6r1;cr2 = c6r2;rr1 = r6r1;rr2 = r6r2;rr3 = r6r3;rr4 = r6r4;sr1 = s6r1;sr2 = s6r2;sr3 = s6r3;sr4 = s6r4;        
        }
        if(SceneChanger.currentCategory == "Home" && t0r1 != 0) {
        tr1 = t7r1;tr2 = t7r2;cr1 = c7r1;cr2 = c7r2;rr1 = r7r1;rr2 = r7r2;rr3 = r7r3;rr4 = r7r4;sr1 = s7r1;sr2 = s7r2;sr3 = s7r3;sr4 = s7r4;        
        }
        if(SceneChanger.currentCategory == "Insects" && t0r1 != 0) {
        tr1 = t8r1;tr2 = t8r2;cr1 = c8r1;cr2 = c8r2;rr1 = r8r1;rr2 = r8r2;rr3 = r8r3;rr4 = r8r4;sr1 = s8r1;sr2 = s8r2;sr3 = s8r3;sr4 = s8r4;        
        }
        if(SceneChanger.currentCategory == "Numbers" && t0r1 != 0) {
        tr1 = t9r1;tr2 = t9r2;cr1 = c9r1;cr2 = c9r2;rr1 = r9r1;rr2 = r9r2;rr3 = r9r3;rr4 = r9r4;sr1 = s9r1;sr2 = s9r2;sr3 = s9r3;sr4 = s9r4;        
        }
        if(SceneChanger.currentCategory == "Snacks_and_Meals" && t0r1 != 0) {
        tr1 = t10r1;tr2 = t10r2;cr1 = c10r1;cr2 = c10r2;rr1 = r10r1;rr2 = r10r2;rr3 = r10r3;rr4 = r10r4;sr1 = s10r1;sr2 = s10r2;sr3 = s10r3;sr4 = s10r4;
        }
        if(SceneChanger.currentCategory == "Vegetables" && t0r1 != 0) {
        tr1 = t11r1;tr2 = t11r2;cr1 = c11r1;cr2 = c11r2;rr1 = r11r1;rr2 = r11r2;rr3 = r11r3;rr4 = r11r4;sr1 = s11r1;sr2 = s11r2;sr3 = s11r3;sr4 = s11r4;
        }
        if(SceneChanger.currentCategory == "Vehicles" && t0r1 != 0) {
        tr1 = t12r1;tr2 = t12r2;cr1 = c12r1;cr2 = c12r2;rr1 = r12r1;rr2 = r12r2;rr3 = r12r3;rr4 = r12r4;sr1 = s12r1;sr2 = s12r2;sr3 = s12r3;sr4 = s12r4;
        }
        if(SceneChanger.currentCategory == "Wild_Animals" && t0r1 != 0) {
        tr1 = t13r1;tr2 = t13r2;cr1 = c13r1;cr2 = c13r2;rr1 = r13r1;rr2 = r13r2;rr3 = r13r3;rr4 = r13r4;sr1 = s13r1;sr2 = s13r2;sr3 = s13r3;sr4 = s13r4;
        }
    }
#region DataFatching
    void FatchRemoteConfigs()
    {
      FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
      dependencyStatus = task.Result;
      if (dependencyStatus == DependencyStatus.Available) {
			Dictionary<string, object> defaults = new Dictionary<string, object>();
			defaults.Add("config_test_string", "default local string");
			defaults.Add("config_test_int", 1);
			defaults.Add("config_test_float", 1.0);
			defaults.Add("config_test_bool", false); 
			FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(defaults).ContinueWith(atask => {
				Debug.Log("RemoteConfig configured and ready!");
				FetchDataAsync();
			  });
      } else {
          Debug.LogError(
            "Could not resolve all Firebase dependencies: " + dependencyStatus);
       }
      });
    }
    public Task FetchDataAsync() {
      Debug.Log("Fetching data...");
    //FirebaseRemoteConfig.DefaultInstance.FetchAsync(System.TimeSpan.Zero) use it to get the latest data instantly
    //By Default the cacheExpiration time is 12 hours
      Task fetchTask = FirebaseRemoteConfig.DefaultInstance.FetchAsync();
      return fetchTask.ContinueWith(FetchComplete);
    }
    void FetchComplete(Task fetchTask) {
      if (fetchTask.IsCanceled) {
        Debug.Log("Fetch canceled.");
      } else if (fetchTask.IsFaulted) {
        Debug.Log("Fetch encountered an error.");
      } else if (fetchTask.IsCompleted) {
        Debug.Log("Fetch completed successfully!");
      }

      var info = FirebaseRemoteConfig.DefaultInstance.Info;
      switch (info.LastFetchStatus) {
        case LastFetchStatus.Success:
          FirebaseRemoteConfig.DefaultInstance.ActivateAsync().ContinueWith(task => 
          {
            Debug.Log(String.Format("Remote data loaded and ready (last fetch time {0}).",info.FetchTime));
            jsonConfig = FirebaseRemoteConfig.DefaultInstance.GetValue("Configs").StringValue;
            M = FirebaseRemoteConfig.DefaultInstance.GetValue("isMultiplayerDisabled").BooleanValue;
            A = FirebaseRemoteConfig.DefaultInstance.GetValue("isAdsDisabled").BooleanValue;
			      DisplayData();
          });

          break;
        case LastFetchStatus.Failure:
          switch (info.LastFetchFailureReason) {
            case FetchFailureReason.Error:
              Debug.Log("Fetch failed for unknown reason");
              break;
            case FetchFailureReason.Throttled:
              Debug.Log("Fetch throttled until " + info.ThrottledEndTime);
              break;
          }
          break;
        case LastFetchStatus.Pending:
          Debug.Log("Latest Fetch call still pending.");
          break;
      }
    }
#endregion
}
