using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using YoutubeLight;

[RequireComponent(typeof(RequestResolver))]
public class k514YoutubeManager : MonoBehaviour {

	public static k514YoutubeManager singleton = null;
    public static string SelectedID = "fj06fHS3x0s";
    public static string SelectedUrl = null;

    private RequestResolver m_resolver = null;

    void Awake(){
		if(singleton == null) singleton = this;
		else if(singleton != this) Destroy(gameObject);
    }

    void Start(){
        Init();
		Debug.Log("Youtube Main Loaded");
    }

    void Init() {
        m_resolver = GetComponent<RequestResolver>();
    }

   public IEnumerator DownloadThumb(string p_thumbUrl,Image p_target)
    {
        WWW www = new WWW(p_thumbUrl);
        yield return www;
        Texture2D thumb = new Texture2D(100, 100);
        www.LoadImageIntoTexture(thumb);
        p_target.sprite = Sprite.Create(thumb, new Rect(0, 0, thumb.width, thumb.height), new Vector2(0.5f, 0.5f), 100);
    }

    public void GetUrl() {
        StartCoroutine(m_resolver.GetDownloadUrls(DescryptUrl, SelectedID, false));
    }

    public void DescryptUrl() {
        List<VideoInfo> l_videoInfos = m_resolver.videoInfos;
        VideoInfo l_tmp = null;
        for (int i = 0; i < l_videoInfos.Count; i++) {
            l_tmp = l_videoInfos[i];
            //Read MP4/360 url
            if (l_tmp.VideoType == VideoType.Mp4 && l_tmp.Resolution == 360) {
                if (l_tmp.RequiresDecryption)
                {
                    StartCoroutine(m_resolver.DecryptDownloadUrl(DescryptCallback, l_tmp));
                }
                else {
                    SelectedUrl = l_tmp.DownloadUrl;
                }
                break;
            }
        }
    }

    public void DescryptCallback(string p_url) {
        SelectedUrl = p_url;
    }


}
