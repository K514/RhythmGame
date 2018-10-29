using System;
using System.Collections;
using System.Collections.Generic;
using K514;
using UnityEngine;
using UnityEngine.UI;

using SimpleJSON;

public class YoutubeSearcher : Singleton<YoutubeSearcher>
{

    #region <Fields>

    [NonSerialized] public YoutubeSnippet[] SearchResultSet;
    [NonSerialized] public int ResultCount;
    private const string APIKey = "AIzaSyDUfxZEbccAg9euTxiFjs6JDElf7NSRhHM";

    #endregion

    #region <Methods>

    public void SearchVideo(string p_SearchKeyWord, int p_MaxResultCount, Action p_OnSearchSuccessed) 
        => StartCoroutine(C_SearchVideo(p_SearchKeyWord, p_MaxResultCount, p_OnSearchSuccessed));
    
    #endregion

    #region <Coroutines>
    
    IEnumerator C_SearchVideo(string p_SearchKeyWord, int p_MaxResultCount, Action p_OnSearchSuccessed)
    {
        // translate url
        p_SearchKeyWord = p_SearchKeyWord.Replace(" ", "%20");
        string l_EncodedURL = WWW.EscapeURL("https://www.googleapis.com/youtube/v3/search/?q=" + p_SearchKeyWord + "&type=video&maxResults=" + p_MaxResultCount + "&part=snippet,id&key=" + APIKey);
        WWW l_waitForResponse = new WWW(WWW.UnEscapeURL(l_EncodedURL));
        Debug.Log("Url : " + l_waitForResponse.url);
        
        // try connect
        yield return l_waitForResponse;

        // parsing the json reserved
        JSONNode l_jsonResult = JSON.Parse(l_waitForResponse.text);
        SearchResultSet = new YoutubeSnippet[l_jsonResult["items"].Count];
        ResultCount = SearchResultSet.Length;
        Debug.Log("count : " + SearchResultSet.Length);

        for (var i = 0; i < ResultCount; i++)
        {
            SearchResultSet[i].VideoID = l_jsonResult["items"][i]["id"]["videoId"];
            SearchResultSet[i].Title = l_jsonResult["items"][i]["snippet"]["title"];
            SearchResultSet[i].VideoDescription = l_jsonResult["items"][i]["snippet"]["description"];
            var DefaultThumb = new YoutubeThumbnail
            {
                ThumbnailURL = l_jsonResult["items"][i]["snippet"]["thumbnails"]["default"]["url"],
                Width = l_jsonResult["items"][i]["snippet"]["thumbnails"]["default"]["width"],
                Height = l_jsonResult["items"][i]["snippet"]["thumbnails"]["default"]["height"]
            };
            var MidThumb = new YoutubeThumbnail
            {
                ThumbnailURL = l_jsonResult["items"][i]["snippet"]["thumbnails"]["medium"]["url"],
                Width = l_jsonResult["items"][i]["snippet"]["thumbnails"]["medium"]["width"],
                Height = l_jsonResult["items"][i]["snippet"]["thumbnails"]["medium"]["height"]
            };
            var StdThumb = new YoutubeThumbnail
            {
                ThumbnailURL = l_jsonResult["items"][i]["snippet"]["thumbnails"]["standard"]["url"],
                Width = l_jsonResult["items"][i]["snippet"]["thumbnails"]["standard"]["width"],
                Height = l_jsonResult["items"][i]["snippet"]["thumbnails"]["standard"]["height"]
            };
            var HQThumb = new YoutubeThumbnail
            {
                ThumbnailURL = l_jsonResult["items"][i]["snippet"]["thumbnails"]["high"]["url"],
                Width = l_jsonResult["items"][i]["snippet"]["thumbnails"]["high"]["width"],
                Height = l_jsonResult["items"][i]["snippet"]["thumbnails"]["high"]["height"]
            };
            var ThumbSet = new YoutubeThumbnailSet
            {
                DefaultThumbnail = DefaultThumb,
                MidQualThumbnail = MidThumb,
                StandardQualThumbnail = StdThumb,
                HQualThumbnail = HQThumb
            };
            SearchResultSet[i].ThumbnailSet = ThumbSet;
        }
        p_OnSearchSuccessed.Invoke();
    }

    #endregion
    
    #region <Structs>

    public struct YoutubeSnippet {
        public string VideoID;
        public string Title;
        public string VideoDescription;
        public YoutubeThumbnailSet ThumbnailSet;
    }

    public struct YoutubeThumbnailSet {
        public YoutubeThumbnail DefaultThumbnail;
        public YoutubeThumbnail StandardQualThumbnail;
        public YoutubeThumbnail MidQualThumbnail;
        public YoutubeThumbnail HQualThumbnail;
    }

    public struct YoutubeThumbnail{
        public string ThumbnailURL;
        public int Width, Height;
    }

    #endregion
}
