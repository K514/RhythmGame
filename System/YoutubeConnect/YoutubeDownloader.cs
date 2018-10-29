using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YoutubeLight;

namespace K514
{
    public class YoutubeDownloader : Singleton<YoutubeDownloader>
    {

        #region <Fields>

        public string VideoID = "fj06fHS3x0s";
        public string VideoURL;

        // use cots for decoding url and download video info from url
        private RequestResolver _reqResolver;

        #endregion

        #region <UnityCallbacks>

        private void Awake()
        {
            _reqResolver = GetComponent<RequestResolver>();
        }

        #endregion

        #region <Methods>

        public void GetUrl()
        {
            StartCoroutine(_reqResolver.GetDownloadUrls(ArrangeTargetVideoInfo, VideoID));
        }

        public void ArrangeTargetVideoInfo()
        {
            var l_VideoInfos = _reqResolver.videoInfos;
            foreach (var videoInfo in l_VideoInfos)
            {
                //Read MP4/360 url
                if (videoInfo.VideoType == VideoType.Mp4 && videoInfo.Resolution == 360)
                {
                    if (videoInfo.RequiresDecryption)
                    {
                        StartCoroutine(_reqResolver.DecryptDownloadUrl(DecodeEnd=>GetInstance.VideoURL = DecodeEnd, videoInfo));
                    }
                    else
                    {
                        VideoURL = videoInfo.DownloadUrl;
                    }
                    break;
                }
            }
        }

        #endregion

        #region <Coroutines>

        public IEnumerator DownloadThumbnail(string p_ThumbUrl, Image p_Target)
        {
            WWW l_WaitForResponse = new WWW(p_ThumbUrl);
            yield return l_WaitForResponse;
            
            Texture2D thumb = new Texture2D(100, 100);
            l_WaitForResponse.LoadImageIntoTexture(thumb);
            p_Target.sprite = Sprite.Create(
                    thumb, 
                    new Rect(0, 0, thumb.width, thumb.height), 
                    new Vector2(0.5f, 0.5f),
                    100
                );
        }

        #endregion

    }
}