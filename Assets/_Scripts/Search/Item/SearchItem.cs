using System;
using UnityEngine;
using UnityEngine.UI;

public class SearchItem : MonoBehaviour
{
    public Button SearchMe;
    public RawImage AnimeImage;
    public Text Title;
    public Text Type;

    public void SetItemData(Texture img,string title,string type)
    {
        AnimeImage.texture = img;
        Title.text = title;
        Type.text = type;
    }
}
