using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class AnimeInfoViewer : MonoBehaviour
{
    public static AnimeInfoViewer instance;
    private void Awake()
    {
        instance = this;
    }

    public string InfoURL = "https://consumet-api-production-7895.up.railway.app/meta/anilist/info/";

    public AnimeInfoData data;
    [Header("UI")] 
    public GameObject Page;
    public RawImage AnimeImage;
    public Text Title;
    public Text Type;
    public Text Rating;
    public Text TotalEpisodes;
    public Text Status;
    public Text Popularity;
    public Text Duration;
    public Text subordub;
    public Text CountryOfOrigin;
    public Text ReleaseData;
    public Text Season;
    public Text Description;
    
    [Header("References")] 
    public Transform GenresHolder;
    public Transform CharecterHolder;
    public Transform RecommendationHolder;
    public GameObject GenresItem;
    public CharecterItem CharecterItem;
    public RecommendationItem RecommendationItem;

    public void SearchAnime(string ID)
    {
        LoadingManager.Instance.Loading();
        string URL = InfoURL + ID;
        StartCoroutine(SearchInfo(URL));
    }

    IEnumerator SearchInfo(string url)
    {
        UnityWebRequest req = UnityWebRequest.Get(url);
        yield return req.SendWebRequest();
        if (req.isNetworkError || req.isHttpError)
        {
            Debug.LogWarning(req.error);
        }

        if (req.isDone)
        {
            string json = req.downloadHandler.text;
            data = JsonUtility.FromJson<AnimeInfoData>(json);
            SetItemData();
        }
    }

    void SetItemData()
    {
        ClearAllDataInInfo();
        StartCoroutine(LoadImagesfromURL(data.image, (Texture img) =>
        {
            AnimeImage.texture = img;
        }));
        SetTitleName();
        Type.text = data.type;
        Rating.text = "Rating: " +data.rating.ToString();
        TotalEpisodes.text = "Total Episodes: " +data.totalEpisodes.ToString();
        Status.text = "Status: " + data.status;
        
        Popularity.text = "Popularity: " + data.popularity.ToString();
        Duration.text = "Duration: " + data.duration.ToString() + "m";
        subordub.text = "Sub Or Dub: "+data.subOrDub;
        CountryOfOrigin.text = "Country of Origin: "+data.countryOfOrigin;
        ReleaseData.text = "ReleaseData: " + data.releaseDate.ToString();
        Season.text = "Season: " + data.season.ToString();
        Description.text = data.description;
        
        foreach (var VARIABLE in data.genres)
        {
            GameObject gb = Instantiate(GenresItem, GenresHolder);
            Text tex = gb.transform.GetChild(0).GetComponent<Text>();
            tex.text = VARIABLE;
        }

        foreach (var VARIABLE in data.characters)
        {
            CharecterItem item = Instantiate(CharecterItem, CharecterHolder);
            item.Name.text = VARIABLE.name.full;
            StartCoroutine(LoadImagesfromURL(VARIABLE.image, (Texture img) =>
            {
                item.Img.texture = img;
            }));
        }

        foreach (var VARIABLE in data.recommendations)
        {
            RecommendationItem item = Instantiate(RecommendationItem, RecommendationHolder);
            
            
            if (!string.IsNullOrEmpty(VARIABLE.title.english))
            {
                item.Name.text = VARIABLE.title.english;
            
            }else if (!string.IsNullOrEmpty(VARIABLE.title.native))
            {
                item.Name.text = VARIABLE.title.native;
                    
            }else if (!string.IsNullOrEmpty(VARIABLE.title.romaji))
            {
                item.Name.text = VARIABLE.title.romaji;
            }
            else if(!string.IsNullOrEmpty(VARIABLE.title.userPreferred))
            {
                item.Name.text = VARIABLE.title.userPreferred;
            }
            else
            {
                item.Name.text = "Null";
            }
            
            StartCoroutine(LoadImagesfromURL(VARIABLE.image, (Texture img) =>
            {
                item.Img.texture = img;
            }));
            
            item.RecommendBtn.onClick.AddListener(() =>
            {
                LoadingManager.Instance.Loading();
                SearchAnime(VARIABLE.id.ToString());
            });
        }
        
        
        Page.SetActive(true);
        LoadingManager.Instance.HideLoadingScreen();
    }

    void SetTitleName()
    {
        if (!string.IsNullOrEmpty(data.title.english))
        {
            Title.text =  data.title.english;
            
        }else if (!string.IsNullOrEmpty(data.title.native))
        {
            Title.text =  data.title.native;
                    
        }else if (!string.IsNullOrEmpty(data.title.romaji))
        {
            Title.text =  data.title.romaji;
        }
        else if(!string.IsNullOrEmpty(data.title.userPreferred))
        {
            Title.text =  data.title.userPreferred;
        }
        else
        {
            Title.text = "Null";
        }
    }

    void ClearAllDataInInfo()
    {
        for (int i = 0; i < GenresHolder.childCount; i++)
        {
            var item =  GenresHolder.GetChild(i);
            Destroy(item.gameObject);
        }

        for (int i = 0; i < CharecterHolder.childCount; i++)
        {
            var item = CharecterHolder.GetChild(i);
            Destroy(item.gameObject);
        }

        for (int i = 0; i < RecommendationHolder.childCount; i++)
        {
            var item =  RecommendationHolder.GetChild(i);
            Destroy(item.gameObject);
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
    public class Character
    {
        public int id;
        public string role;
        public Name name;
        public string image;
        public string imageHash;
        public List<VoiceActor> voiceActors;
    }
[Serializable]
    public class EndDate
    {
        public int year;
        public int month;
        public int day;
    }
[Serializable]
    public class Episode
    {
        public string id;
        public string title;
        public string image;
        public string imageHash;
        public int number;
        public DateTime createdAt;
        public object description;
        public string url;
    }
[Serializable]
    public class Name
    {
        public string first;
        public string last;
        public string full;
        public string native;
        public string userPreferred;
    }
[Serializable]
    public class Recommendation
    {
        public int id;
        public int malId;
        public Title title;
        public string status;
        public int? episodes;
        public string image;
        public string imageHash;
        public string cover;
        public string coverHash;
        public int rating;
        public string type;
    }
[Serializable]
    public class Relation
    {
        public int id;
        public string relationType;
        public int malId;
        public Title title;
        public string status;
        public int? episodes;
        public string image;
        public string imageHash;
        public string color;
        public string type;
        public string cover;
        public string coverHash;
        public int? rating;
    }
[Serializable]
    public class AnimeInfoData
    {
        public string id;
        public Title title;
        public int malId;
        public List<string> synonyms;
        public bool isLicensed;
        public bool isAdult;
        public string countryOfOrigin;
        public Trailer trailer;
        public string image;
        public string imageHash;
        public int popularity;
        public string color;
        public string cover;
        public string coverHash;
        public string description;
        public string status;
        public int releaseDate;
        public StartDate startDate;
        public EndDate endDate;
        public int totalEpisodes;
        public int currentEpisode;
        public int rating;
        public int duration;
        public List<string> genres;
        public string season;
        public List<string> studios;
        public string subOrDub;
        public string type;
        public List<Recommendation> recommendations;
        public List<Character> characters;
        public List<Relation> relations;
        public List<Episode> episodes;
    }
[Serializable]
    public class StartDate
    {
        public int year;
        public int month;
        public int day;
    }
[Serializable]
    public class Title
    {
        public string romaji;
        public string english;
        public string native;
        public string userPreferred;
    }
[Serializable]
    public class Trailer
    {
        public string id;
        public string site;
        public string thumbnail;
        public string thumbnailHash;
    }
[Serializable]
    public class VoiceActor
    {
        public int id;
        public string language;
        public Name name;
        public string image;
        public string imageHash;
    }

