using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class AniSearch : MonoBehaviour
{
    public string SearchURL = "https://consumet-api-production-7895.up.railway.app/meta/anilist/";

    public AniSearchData data;
    [Header("UI")] 
    public InputField SearchInput;
    public Button SearchButton;
    public Transform Container;
    
    [Header("References")]
    public SearchItem SearchItem;
    
    private void Start()
    {
        SearchButton.onClick.AddListener(() =>
        {
            LoadingManager.Instance.Loading();
            SearchAnime();
        });
        SearchInput.onSubmit.AddListener((e) =>
        {
            LoadingManager.Instance.Loading();
            SearchAnime();
        });
    }

    private void SearchAnime()
    {
        if (SearchInput.text.Length < 1)
        {
            return;
        }

        string URL = SearchURL + SearchInput.text + "?perPage=100";
        StartCoroutine(SearchData(URL));
    }


    IEnumerator SearchData(string url)
    {

        for (int i = 0; i < Container.childCount; i++)
        {
            Destroy(Container.GetChild(i).gameObject);
        }
        
        UnityWebRequest req = UnityWebRequest.Get(url);
        yield return req.SendWebRequest();
        if (req.isNetworkError || req.isHttpError)
        {
            ErrorManager.Instance.ShowErrorMsg(req.error);
        }

        if (req.isDone)
        {
            string Json = req.downloadHandler.text;
            data = JsonUtility.FromJson<AniSearchData>(Json);
            
            ShowSearchItem();
        }
    }

    void ShowSearchItem()
    {
        for (int i = 0; i < data.results.Count; i++)
        {
            AniSearchResult result = data.results[i];
            SearchItem item = Instantiate(SearchItem, Container);
            
            
            StartCoroutine(LoadImagesfromURL(result.image, (Texture img) =>
            {
                if (!string.IsNullOrEmpty(result.title.english))
                {
                    item.SetItemData(img,result.title.english,result.type);
                }else if (!string.IsNullOrEmpty(result.title.native))
                {
                    item.SetItemData(img,result.title.native,result.type);
                    
                }else if (!string.IsNullOrEmpty(result.title.romaji))
                {
                    item.SetItemData(img,result.title.romaji,result.type);
                }
                else if(!string.IsNullOrEmpty(result.title.userPreferred))
                {
                    item.SetItemData(img,result.title.userPreferred,result.type);
                }
                else
                {
                    item.SetItemData(img,"Null",result.type);
                }
            }));
            item.SearchMe.onClick.AddListener(() =>
            {
                AnimeInfoViewer.instance.SearchAnime(result.id);
            });
        }
        LoadingManager.Instance.HideLoadingScreen();
    }
    
    
    IEnumerator LoadImagesfromURL(string url, System.Action<Texture> callback)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();
        if (www.isDone)
        {
            Texture LoadedTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            callback(LoadedTexture);
        }
    }
}
[System.Serializable]
public class AniSearchResult
{
    public string id;
    public int? malId;
    public AniSearchTitle title;
    public string status;
    public string image;
    public string imageHash;
    public string cover;
    public string coverHash;
    public int popularity;
    public string description;
    public int? rating;
    public List<string> genres;
    public string color;
    public int totalEpisodes;
    public int currentEpisodeCount;
    public string type;
    public int? releaseDate;
}
[System.Serializable]
public class AniSearchData
{
    public int currentPage;
    public bool hasNextPage;
    public List<AniSearchResult> results;
}
[System.Serializable]
public class AniSearchTitle
{
    public string romaji;
    public string english;
    public string native;
    public string userPreferred;
}
