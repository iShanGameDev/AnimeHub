using System;
using System.Collections.Generic;
using System.Collections;
using System.Data.Common;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Networking;

public class WatchEpisodeViewer : MonoBehaviour
{
    
    public static WatchEpisodeViewer Instance;

    private void Awake()
    {
        Instance = this;
    }

    public string EpisodeURL = "https://consumet-api-production-7895.up.railway.app/meta/anilist/watch/";
    public EpisodeData data;
    
    public UniWebView webview;
    public GameObject Pannel;
    public void GetEpisodeData(string id)
    {
        string url = EpisodeURL + id;
        StartCoroutine(GetData(url));
    }

    IEnumerator GetData(string url)
    {
        UnityWebRequest req =  UnityWebRequest.Get(url);
        yield return req.SendWebRequest();

        if (req.isNetworkError || req.isHttpError)
        {
            ErrorManager.Instance.ShowErrorMsg(req.error);
        }

        if (req.isDone)
        {
            string json = req.downloadHandler.text;
            data = JsonUtility.FromJson<EpisodeData>(json);
            WebView();
        }
    }

    void WebView()
    {
        webview.Load(data.sources[0].url);
        //Pannel.SetActive(true);
    }
}
[System.Serializable]
public class Headers
{
    public string Referer;
}
[System.Serializable]
public class Intro
{
    public int start;
    public int end;
}
[System.Serializable]
public class Outro
{
    public int start;
    public int end;
}
[System.Serializable]
public class EpisodeData
{
    public Headers headers;
    public Intro intro;
    public Outro outro;
    public List<Source> sources;
    public List<Subtitle> subtitles;
}
[System.Serializable]
public class Source
{
    public string url;
    public bool isM3U8;
    public string type;
}
[System.Serializable]
public class Subtitle
{
    public string url;
    public string lang;
}