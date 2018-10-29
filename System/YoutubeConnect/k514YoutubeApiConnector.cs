using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using SimpleJSON;

public class k514YoutubeApiConnector : MonoBehaviour
{

    public static k514YoutubeApiConnector singleton = null;
    public k514514YoutubeData[] m_results;
    [System.NonSerialized]public int m_SearchCount = 0;
    private const string APIKey = "AIzaSyDUfxZEbccAg9euTxiFjs6JDElf7NSRhHM";

    void Awake()
    {
        if (singleton == null) singleton = this;
        else if (singleton != this) Destroy(gameObject);
    }

    void Start()
    {
        Init();
        Debug.Log("Youtube Connector Loaded");
    }

    void Init()
    {
    }

    void Update()
    {

    }

    public void SearchVideo(string p_keyWord, int p_maxResult, Action p_callback) {
        StartCoroutine(C_SearchVideo(p_keyWord, p_maxResult, p_callback));
    }

    IEnumerator C_SearchVideo(string p_keyWord, int p_maxResult, Action p_callback)
    {
        // make url
        p_keyWord = p_keyWord.Replace(" ", "%20");
        string l_encodedURL = WWW.EscapeURL("https://www.googleapis.com/youtube/v3/search/?q=" + p_keyWord + "&type=video&maxResults=" + p_maxResult + "&part=snippet,id&key=" + APIKey);
        WWW l_calling = new WWW(WWW.UnEscapeURL(l_encodedURL));
        Debug.Log("Url : " + l_calling.url);
        
        // try connect
        yield return l_calling;

        // parsing the json reserved
        JSONNode l_jsonResult = JSON.Parse(l_calling.text);
        m_results = new k514514YoutubeData[l_jsonResult["items"].Count];
        m_SearchCount = m_results.Length;
        Debug.Log("count : " + m_results.Length);

        // make objects about title, description . etc... of video
        k514k514YoutubeThumbs l_thumbs = null;
        k514k514YoutubeThumb l_thumb = null;

        for (int i = 0; i<m_results.Length; i++)
        {
            m_results[i] = new k514514YoutubeData();
            m_results[i].m_videoId = l_jsonResult["items"][i]["id"]["videoId"];
            m_results[i].m_title = l_jsonResult["items"][i]["snippet"]["title"];
            m_results[i].m_desc = l_jsonResult["items"][i]["snippet"]["description"];
            l_thumbs = m_results[i].m_thumbs = new k514k514YoutubeThumbs();
            l_thumb = l_thumbs.m_default = new k514k514YoutubeThumb();
            l_thumb.m_url = l_jsonResult["items"][i]["snippet"]["thumbnails"]["default"]["url"];
            l_thumb.m_width = l_jsonResult["items"][i]["snippet"]["thumbnails"]["default"]["width"];
            l_thumb.m_height = l_jsonResult["items"][i]["snippet"]["thumbnails"]["default"]["height"];
            l_thumb = l_thumbs.m_stdQual = new k514k514YoutubeThumb();
            l_thumb.m_url = l_jsonResult["items"][i]["snippet"]["thumbnails"]["standard"]["url"];
            l_thumb.m_width = l_jsonResult["items"][i]["snippet"]["thumbnails"]["standard"]["width"];
            l_thumb.m_height = l_jsonResult["items"][i]["snippet"]["thumbnails"]["standard"]["height"];
            l_thumb = l_thumbs.m_midQual = new k514k514YoutubeThumb();
            l_thumb.m_url = l_jsonResult["items"][i]["snippet"]["thumbnails"]["medium"]["url"];
            l_thumb.m_width = l_jsonResult["items"][i]["snippet"]["thumbnails"]["medium"]["width"];
            l_thumb.m_height = l_jsonResult["items"][i]["snippet"]["thumbnails"]["medium"]["height"];
            l_thumb = l_thumbs.m_highQual = new k514k514YoutubeThumb();
            l_thumb.m_url = l_jsonResult["items"][i]["snippet"]["thumbnails"]["high"]["url"];
            l_thumb.m_width = l_jsonResult["items"][i]["snippet"]["thumbnails"]["high"]["width"];
            l_thumb.m_height = l_jsonResult["items"][i]["snippet"]["thumbnails"]["high"]["height"];
        }

        p_callback.Invoke();
    }
    public void LoadVideo(string p_videoId, Action<k514YoutubeData> p_callback) {
        StartCoroutine(C_LoadVideo(p_videoId, p_callback));
    }

    IEnumerator C_LoadVideo(string p_videoId, Action<k514YoutubeData> p_callback) {
        yield return null;
    }



    // inner class

    public class k514514YoutubeData {
        public string m_videoId;
        public string m_title;
        public string m_desc;
        public k514k514YoutubeThumbs m_thumbs;
    }

    public class k514k514YoutubeThumbs {
        public k514k514YoutubeThumb m_default;
        public k514k514YoutubeThumb m_stdQual;
        public k514k514YoutubeThumb m_midQual;
        public k514k514YoutubeThumb m_highQual;
    }

    public class k514k514YoutubeThumb{
        public string m_url;
        public int m_width, m_height;
    }

}
