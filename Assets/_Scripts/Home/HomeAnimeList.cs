using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class HomeAnimeList : MonoBehaviour
{
    public string TrendingAnimeURL = "https://consumet-api-production-7895.up.railway.app/meta/anilist/trending";
    public string PopularAnimeURL = "https://consumet-api-production-7895.up.railway.app/meta/anilist/popular";
    public string AiringAnimeURL = "https://consumet-api-production-7895.up.railway.app/meta/anilist/airing-schedule";
    
    public TrendingData trending;
    public PopularData popular;
    public Top_AiringData airing;

    [Header("References")] 
    public Transform Container;
    
    public AnimeItemHolder PopularAnimeHolder;
    public AnimeItemHolder TrendingAnimeHolder;
    public AnimeItemHolder AiringAnimeHolder;
    
    private void Start()
    {
        StartCoroutine(LoadData());
    }


    IEnumerator LoadData()
    {
        LoadingManager.Instance.Loading();
        yield return TrendingAnime();
        yield return new WaitForSeconds(1);
        yield return PopularAnime();
        yield return new WaitForSeconds(1);
        yield return AiringAnime();
        yield return new WaitForSeconds(1);
        LoadingManager.Instance.HideLoadingScreen();
    }
    
    IEnumerator TrendingAnime()
    {
        UnityWebRequest req =  UnityWebRequest.Get(TrendingAnimeURL);
        yield return req.SendWebRequest();

        if (req.isNetworkError || req.isHttpError)
        {
            ErrorManager.Instance.ShowErrorMsg(req.error);
        }

        if (req.isDone)
        {
            string json = req.downloadHandler.text;
            trending = JsonUtility.FromJson<TrendingData>(json);
            SetTrendingData();
        }
    }

    void SetTrendingData()
    {
        AnimeItemHolder aih = Instantiate(TrendingAnimeHolder, Container);

        foreach (var VARIABLE in trending.results)
        {
            PopularAnimeItem item = Instantiate(aih.AnimeItem, aih.AnimeItemHolders);
            item.Title.text = VARIABLE.title.english;
            
            if (!string.IsNullOrEmpty(VARIABLE.title.english))
            {
                item.Title.text = VARIABLE.title.english;
            
            }else if (!string.IsNullOrEmpty(VARIABLE.title.native))
            {
                item.Title.text = VARIABLE.title.native;
                    
            }else if (!string.IsNullOrEmpty(VARIABLE.title.romaji))
            {
                item.Title.text = VARIABLE.title.romaji;
            }
            else if(!string.IsNullOrEmpty(VARIABLE.title.userPreferred))
            {
                item.Title.text = VARIABLE.title.userPreferred;
            }
            else
            {
                item.Title.text = "Null";
            }
            
            item.SearchBTN.onClick.AddListener(() =>
            {
                AnimeInfoViewer.instance.SearchAnime(VARIABLE.id);
            });
            
            StartCoroutine(LoadImagesfromURL(VARIABLE.image, (Texture img) =>
            {
                item.Img.texture = img;
            }));
        }
        
    }
    IEnumerator PopularAnime()
    {
        UnityWebRequest req =  UnityWebRequest.Get(PopularAnimeURL);
        yield return req.SendWebRequest();

        if (req.isNetworkError || req.isHttpError)
        {
            ErrorManager.Instance.ShowErrorMsg(req.error);
        }

        if (req.isDone)
        {
            string json = req.downloadHandler.text;
            popular = JsonUtility.FromJson<PopularData>(json);
            SetPopularData();
        }
    }    
    
    void SetPopularData()
    {
        AnimeItemHolder aih = Instantiate(PopularAnimeHolder, Container);

        foreach (var VARIABLE in popular.results)
        {
            PopularAnimeItem item = Instantiate(aih.AnimeItem, aih.AnimeItemHolders);
            item.Title.text = VARIABLE.title.english;
            
            if (!string.IsNullOrEmpty(VARIABLE.title.english))
            {
                item.Title.text = VARIABLE.title.english;
            
            }else if (!string.IsNullOrEmpty(VARIABLE.title.native))
            {
                item.Title.text = VARIABLE.title.native;
                    
            }else if (!string.IsNullOrEmpty(VARIABLE.title.romaji))
            {
                item.Title.text = VARIABLE.title.romaji;
            }
            else if(!string.IsNullOrEmpty(VARIABLE.title.userPreferred))
            {
                item.Title.text = VARIABLE.title.userPreferred;
            }
            else
            {
                item.Title.text = "Null";
            }
            
            item.SearchBTN.onClick.AddListener(() =>
            {
                AnimeInfoViewer.instance.SearchAnime(VARIABLE.id);
            });
            
            StartCoroutine(LoadImagesfromURL(VARIABLE.image, (Texture img) =>
            {
                item.Img.texture = img;
            }));
        }
        
    }
    
    IEnumerator AiringAnime()
    {
        UnityWebRequest req =  UnityWebRequest.Get(AiringAnimeURL);
        yield return req.SendWebRequest();

        if (req.isNetworkError || req.isHttpError)
        {
            ErrorManager.Instance.ShowErrorMsg(req.error);
        }

        if (req.isDone)
        {
            string json = req.downloadHandler.text;
            airing = JsonUtility.FromJson<Top_AiringData>(json);
            SetAiringData();
        }
    }
    void SetAiringData()
    {
        AnimeItemHolder aih = Instantiate(AiringAnimeHolder, Container);

        foreach (var VARIABLE in airing.results)
        {
            PopularAnimeItem item = Instantiate(aih.AnimeItem, aih.AnimeItemHolders);
            item.Title.text = VARIABLE.title.english;
            
            if (!string.IsNullOrEmpty(VARIABLE.title.english))
            {
                item.Title.text = VARIABLE.title.english;
            
            }else if (!string.IsNullOrEmpty(VARIABLE.title.native))
            {
                item.Title.text = VARIABLE.title.native;
                    
            }else if (!string.IsNullOrEmpty(VARIABLE.title.romaji))
            {
                item.Title.text = VARIABLE.title.romaji;
            }
            else if(!string.IsNullOrEmpty(VARIABLE.title.userPreferred))
            {
                item.Title.text = VARIABLE.title.userPreferred;
            }
            else
            {
                item.Title.text = "Null";
            }
            
            item.SearchBTN.onClick.AddListener(() =>
            {
                AnimeInfoViewer.instance.SearchAnime(VARIABLE.id);
            });
            
            StartCoroutine(LoadImagesfromURL(VARIABLE.image, (Texture img) =>
            {
                item.Img.texture = img;
            }));
        }
        
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
[Serializable]
public class Result
{
    public string id;
    public int malId;
    public Title title;
    public string image;
    public string imageHash;
    public Trailer trailer;
    public string description;
    public string status;
    public string cover;
    public string coverHash;
    public int rating;
    public int releaseDate;
    public string color;
    public List<string> genres;
    public int? totalEpisodes;
    public int duration;
    public string type;
}
[Serializable]
public class TrendingData
{
    public int currentPage;
    public bool hasNextPage;
    public List<Result> results;
}

[Serializable]
public class PopularData
{
    public int currentPage;
    public bool hasNextPage;
    public List<Result> results;
}



[Serializable]
public class AirResult
{
    public string id;
    public int? malId;
    public int episode;
    public int airingAt;
    public Title title;
    public string country;
    public string image;
    public string imageHash;
    public string description;
    public string cover;
    public string coverHash;
    public List<string> genres;
    public string color;
    public int? rating;
    public int? releaseDate;
    public string type;
}
[Serializable]
public class Top_AiringData
{
    public int currentPage;
    public bool hasNextPage;
    public List<AirResult> results;
}
