using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LoadJSONFile : MonoBehaviour
{
    public IEnumerator WebRequestJSONFile(string url, Action<bool, string> callBack)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            callBack(false, www.error);
        }
        else
        {
            callBack(true, www.downloadHandler.text);
        }
    }
}
