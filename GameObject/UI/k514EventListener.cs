using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class k514EventListener : MonoBehaviour {

    public Animator m_BG;
    public EventSystem m_event;
    public Dictionary<string,GameObject> m_map;
    public List<k514SelectorItem> m_items;
    bool m_itemMoveTrig;

    public enum NOW_ACTIVITY {
           TITLE,ANIMATING,SEARCH,SELECT,END
    }

    NOW_ACTIVITY m_nowActivityEnum;

    float[] m_KeyCool;
    Action m_OnPressedEscape = null;
    Action m_OnPressedEnter = null;
    Action<float> m_OnPressedHorizontal = null;
    Action<float> m_OnPressedVertical = null;
    Action m_OnPressedAnyKey = null;
    public enum ACTION_KEY {
        ESCAPE, ENTER, HORIZONTAL, VERTICAL, ANY
    }

    Action m_nowActivity = null;
    Action m_autoActive = null;

    [NonSerialized]
    public bool m_rightSlide, m_leftSlide;
    float h, v;
    bool m_searchAtOnceTrig = false, m_searchFutureTrig = false;
    string m_searchKeyword;

    Transform m_SelectContainer = null;

    int m_nowItemIndex = 0;
    Text m_desc, m_title;

    void Start() {
        m_KeyCool = new float[5];
        m_map = new Dictionary<string, GameObject>();
        m_items = new List<k514SelectorItem>();
        m_SelectContainer = GameObject.Find("SelectContainer").transform;
        
        AddMap("BlackFade");
        AddMap("Title");
        AddMap("SearchInput");
        AddMap("SearchButton");
        AddMap("SearchMemo");
        AddMap("SelectPanel");
        AddMap("SelectMemo");
        AddMap("SelectMemo2");
        AddMap("LoadingPanel");
        AddMap("LoadingText");
        AddMap("ErrorText");
        AddMap("LeftArrow");
        AddMap("BackKey");
        AddMap("RightArrow");
        AddMap("NameLine");
        AddMap("DescLine");

        var array = m_map.Keys.ToArray();
        for (int i = 0; i < array.Length; i++) m_map[array[i]].SetActive(false);

        k514SelectorItem l_tmp = null;
        for (int i = 0; i < 15; i++) {
            l_tmp = GameObject.Find("SelectorItem" + i).GetComponent<k514SelectorItem>();
            m_items.Add(l_tmp);
        }

        m_nowActivity = TitleActivity;
        CastNextActivity();
    }

    void AddMap(string p_string) {
        m_map.Add(p_string, GameObject.Find(p_string) );
    }

    void ItemsActive(bool p_trig) {
        for (int i = 0; i < 15; i++)
        {
            m_items[i].gameObject.SetActive(p_trig);
        }
    }

    // Update is called once per frame
    bool isAnimating() { return m_nowActivityEnum == NOW_ACTIVITY.ANIMATING; }

	void Update () {

        if (m_nowActivityEnum == NOW_ACTIVITY.SELECT && m_event.currentSelectedGameObject != null) m_event.SetSelectedGameObject(null);

        if (Input.GetKeyDown(KeyCode.Escape) && m_KeyCool[(int)ACTION_KEY.ESCAPE] <= 0f)
        {
            m_KeyCool[(int)ACTION_KEY.ESCAPE] += 0.2f;
           if (!isAnimating() && m_OnPressedEscape != null) m_OnPressedEscape.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.Return) && m_KeyCool[(int)ACTION_KEY.ENTER] <= 0f)
        {
            m_KeyCool[(int)ACTION_KEY.ENTER] += 0.2f;
            if (!isAnimating() && m_OnPressedEnter != null) m_OnPressedEnter.Invoke();
        }
        else if (Input.GetButtonDown("Horizontal") && m_KeyCool[(int)ACTION_KEY.HORIZONTAL] <= 0f)
        {
            h = Input.GetAxis("Horizontal");
            m_KeyCool[(int)ACTION_KEY.HORIZONTAL] += 0.2f;
            if (!isAnimating() && m_OnPressedHorizontal != null) m_OnPressedHorizontal.Invoke(h);
        }
        else if (Input.GetButtonDown("Vertical")  && m_KeyCool[(int)ACTION_KEY.VERTICAL] <= 0f)
        {
            v = Input.GetAxis("Vertical");
            m_KeyCool[(int)ACTION_KEY.VERTICAL] += 0.2f;
            if (!isAnimating() && m_OnPressedVertical != null) m_OnPressedVertical.Invoke(v);
        }
        else if (Input.anyKeyDown && m_KeyCool[(int)ACTION_KEY.ANY] <= 0f)
        {
            m_KeyCool[(int)ACTION_KEY.ANY] += 0.2f;
            if (!isAnimating() && m_OnPressedAnyKey != null) m_OnPressedAnyKey.Invoke();
        }

        if (!isAnimating() && m_autoActive != null) m_autoActive();
        if (m_leftSlide)
        {
            m_leftSlide = false;
            if (!isAnimating()) Slide(-1f);
        }
        if (m_rightSlide)
        {
            m_rightSlide = false;
            if (!isAnimating()) Slide(1f);
        }

        for (int i = 0; i < m_KeyCool.Length; i++) {
            m_KeyCool[i] = m_KeyCool[i] > 0 ? m_KeyCool[i] - Time.deltaTime : 0f;
        }
    }

    void CastNextActivity() {
        m_nowActivity.Invoke();
    }

    void SetUpdateAuto(Action p_event)
    {
        m_autoActive = p_event;
    }

    void ExitActivity() {
        m_nowActivityEnum = NOW_ACTIVITY.ANIMATING;
        m_nowActivity = null;
        m_OnPressedAnyKey = null;
        m_OnPressedEnter = null;
        m_OnPressedEscape = ExitActivity;
        m_OnPressedHorizontal = null;
        m_OnPressedVertical = null;
        m_autoActive = null;
        StartCoroutine(Exit());
    }

    void TitleActivity() {
        m_nowActivityEnum = NOW_ACTIVITY.TITLE;
        m_nowActivity = null;
        m_OnPressedAnyKey = TitleFlickAnimation;
        m_OnPressedEnter = null;
        m_OnPressedEscape = ExitActivity;
        m_OnPressedHorizontal = null;
        m_OnPressedVertical = null;
        m_autoActive = null;

        m_map["Title"].SetActive(true);
        ItemsActive(false);

    }

    #region Title Activity Event
    void TitleFlickAnimation() {
        m_nowActivityEnum = NOW_ACTIVITY.ANIMATING;
        m_OnPressedAnyKey = null;
        m_OnPressedEscape = null;
        StartCoroutine(Flicking(m_map["Title"], 1f, TitleMoveAnimation));
    }

    void TitleMoveAnimation() {
        StartCoroutine(Move(m_map["Title"], Vector2.left * 1000f, 0.2f,0f, TitleActivityEndProcess));
    }

    void TitleActivityEndProcess() {
        m_map["Title"].SetActive(false);
        m_nowActivity = SearchActivity;
        m_map["SearchInput"].SetActive(true);
        m_map["SearchButton"].SetActive(true);
        m_map["SearchMemo"].SetActive(true);

        StartCoroutine(Move(m_map["SearchMemo"], Vector2.down * 200f, 0.2f));
        StartCoroutine(Move(m_map["SearchInput"],Vector2.up*450,0.2f,0f,CastNextActivity));
    }
    #endregion

    void SearchActivity() {
        m_nowActivityEnum = NOW_ACTIVITY.SEARCH;
        m_nowActivity = null;
        m_OnPressedAnyKey = GiveFocusToSearchInput;
        m_OnPressedEnter = InvokeSearchButtonCallBack;
        m_OnPressedEscape = BackToTitle;
        m_OnPressedHorizontal = null;
        m_OnPressedVertical = null;
        m_autoActive = null;

    }

    #region Search Activity Event
    void BackToTitle() {
        m_map["Title"].SetActive(true);
        m_nowActivity = TitleActivity;
        StartCoroutine(Move(m_map["SearchMemo"], Vector2.up * 200f, 0.2f));
        StartCoroutine(Move(m_map["SearchInput"], Vector2.down * 450, 0.2f));
        StartCoroutine(Move(m_map["Title"], Vector2.right * 1000f, 0.2f,0f, CastNextActivity));
    }

    void GiveFocusToSearchInput() {
        m_event.SetSelectedGameObject(m_map["SearchInput"]);
    }

    public void InvokeSearchButtonCallBack()
    {
        if (m_searchAtOnceTrig) return;
        m_searchAtOnceTrig = true;
        m_searchKeyword = m_map["SearchInput"].GetComponent<InputField>().text;
        m_map["SearchInput"].GetComponent<InputField>().text = null;
        k514SystemManager.SYSTEM_YOUTUBE_CONNECTOR.SearchVideo(m_searchKeyword,15,SearchFutureModule);
        PanelAnimation();
    }

    void SearchFutureModule() {
        m_searchFutureTrig = true;
        Debug.Log("Search Completed");
    }

    void PanelAnimation() {
        m_nowActivityEnum = NOW_ACTIVITY.ANIMATING;
        m_nowActivity = null;
        StartCoroutine(Move(m_map["SearchMemo"], Vector2.up * 200f, 0.2f,0f, null));
        StartCoroutine(Move(m_map["SearchInput"], Vector2.down * 450, 0.2f,0f, SearchActivityEndProcess));
    }

    void SearchActivityEndProcess() {
        m_nowActivity = SelectActivity;
        m_map["SearchMemo"].SetActive(false);
        m_map["SearchInput"].SetActive(false);
        m_map["SelectPanel"].SetActive(true);
        m_map["SelectMemo"].SetActive(true);
        m_map["SelectMemo2"].SetActive(true);
        m_map["SelectMemo2"].GetComponentInChildren<Text>().text = m_searchKeyword;

        StartCoroutine(Move(m_map["SelectMemo"], Vector2.down * 140f,0.2f, 0.5f, null));
        StartCoroutine(Move(m_map["SelectMemo2"], Vector2.down * 140f,0.2f, 0.5f, null));
        StartCoroutine(SpreadHoriz(m_map["SelectPanel"], 300f, 0.2f, 0.5f, CastNextActivity));
    }

    #endregion

    void SelectActivity() {
        m_nowActivityEnum = NOW_ACTIVITY.SELECT;
        m_nowActivity = null;
        m_OnPressedAnyKey = null;
        m_OnPressedEnter = null;
        m_OnPressedEscape = null;
        m_OnPressedHorizontal = null;
        m_OnPressedVertical = null;
        m_autoActive = null;

        ShowLoadingAnimation();
    }

    #region Select Activity Event
    void ShowLoadingAnimation() {
        m_map["LoadingPanel"].SetActive(true);
        m_map["LoadingText"].SetActive(true);
        StartCoroutine(Move(m_map["LoadingPanel"], Vector2.right * 1024f, 0.2f, 0.5f, null));
        StartCoroutine(PromiseWait( () => m_searchFutureTrig , 3f , 10f, () => {
            ItemInfoSetter();
            StartCoroutine(Move(m_map["LoadingPanel"], Vector2.left * 1024f, 0.2f, 1f, SelectAnimation));
            SetUpdateAuto(ItemSelector);
        }, ErrorSearch));
    }

    void SelectAnimation()
    {
        m_map["LoadingPanel"].SetActive(false);
        m_map["LoadingText"].SetActive(false);
        m_map["LeftArrow"].SetActive(true);
        m_map["BackKey"].SetActive(true);
        m_map["RightArrow"].SetActive(true);
        m_map["NameLine"].SetActive(true);
        m_map["DescLine"].SetActive(true);

        m_title = m_map["NameLine"].GetComponentsInChildren<Text>()[1];
        m_desc = m_map["DescLine"].GetComponentsInChildren<Text>()[1];

        m_nowActivityEnum = NOW_ACTIVITY.ANIMATING;
        m_nowActivity = null;
        ItemsActive(true);
        StartCoroutine(ItemMove(Vector2.left * 760f,0.1f, 0.2f, 0.5f,null));
        StartCoroutine(ArrayMove(new List<GameObject>() { m_map["LeftArrow"] , m_map["BackKey"] , m_map["RightArrow"] }, Vector2.up * 340f,0.2f, 0.2f, 1f,()=> {
            m_nowActivityEnum = NOW_ACTIVITY.SELECT;
            m_OnPressedEnter = GameStart;
            m_OnPressedEscape = BackToSearch;
            m_OnPressedHorizontal = Slide;
        }));
        StartCoroutine(SpreadHoriz(m_map["NameLine"], 60f, 0.4f, 2f, () => {
            StartCoroutine(Move(m_map["NameLine"],Vector2.left*1024,0.2f,0.5f));
        }));
        StartCoroutine(SpreadVerti(m_map["DescLine"], 300f, 0.4f, 2.2f, ()=> {
            StartCoroutine(Move(m_map["DescLine"], Vector2.up * 768, 0.2f, 0.5f));
        }));

    }

    void ErrorSearch() {
        m_map["LoadingText"].SetActive(false);
        m_map["ErrorText"].SetActive(true);
        m_OnPressedEscape = BackToSearchError;
    }

    void ItemInfoSetter() {
        YoutubeSearcher.YoutubeSnippet[] l_array = k514SystemManager.SYSTEM_YOUTUBE_CONNECTOR.SearchResultSet;
        k514SelectorItem l_tmp = null;
        for (int i = 0; i < 15; i++) {
            m_items[i].Hide();
            if (l_array.Length > i) {
                l_tmp = m_items[i];
                l_tmp.m_thumbImg.gameObject.SetActive(true);
                StartCoroutine(k514SystemManager.SYSTEM_YOUTUBE_MANAGER.DownloadThumb(l_array[i].ThumbnailSet.DefaultThumbnail.ThumbnailURL
                    , l_tmp.m_thumbImg
                ));
            }
        }
    }

    void ItemSelector() {
        try
        {
            if (m_desc != null)
            {
                m_desc.text = k514SystemManager.SYSTEM_YOUTUBE_CONNECTOR.SearchResultSet[m_nowItemIndex].VideoDescription;
            }
            if (m_title != null)
            {
                m_title.text = k514SystemManager.SYSTEM_YOUTUBE_CONNECTOR.SearchResultSet[m_nowItemIndex].Title;
            }
        }
        catch (Exception e) {

        }
        for(int i = 0; i <m_items.Count; i++) {
            if (i == m_nowItemIndex || i == m_nowItemIndex-1) m_items[i].Open();
            else m_items[i].Hide();
        }
    }

    public void Slide(float h)
    {
        if (m_itemMoveTrig) return;
        if (h < 0f && m_nowItemIndex < m_items.Count - 1) {
            m_itemMoveTrig = true;
            StartCoroutine(Move(m_items[m_nowItemIndex].gameObject, Vector2.left * 300, 0.05f, 0.05f,()=> { m_itemMoveTrig = false; }));
            m_nowItemIndex++;
        } else if (h > 0f && m_nowItemIndex > 0) {
            m_nowItemIndex--;
            m_itemMoveTrig = true;
            StartCoroutine(Move(m_items[m_nowItemIndex].gameObject, Vector2.right * 300, 0.05f, 0.05f, () => { m_itemMoveTrig = false; }));
        }
        m_items[m_nowItemIndex].transform.SetParent(null);
        m_items[m_nowItemIndex].transform.SetParent(m_SelectContainer);

    }

    void GameStart() {
        m_itemMoveTrig = true;
        m_nowActivityEnum = NOW_ACTIVITY.ANIMATING;
        m_BG.GetComponent<Animator>().Play("Action");
        m_nowActivity = null;
        m_OnPressedAnyKey = null;
        m_OnPressedEnter = null;
        m_OnPressedEscape = ExitActivity;
        m_OnPressedHorizontal = null;
        m_OnPressedVertical = null;
        m_autoActive = null;
        StartCoroutine(LoadScene());
    }

    void BackToSearchError(){
        m_nowActivityEnum = NOW_ACTIVITY.ANIMATING;
        m_OnPressedEnter = null;
        m_OnPressedEscape = null;
        m_OnPressedHorizontal = null;
        m_nowActivity = SearchActivity;
        StartCoroutine(Move(m_map["SelectMemo"], Vector2.up * 140f, 0.2f, 0.5f,()=>{
            m_map["SelectMemo"].SetActive(false);
        }));
        StartCoroutine(Move(m_map["SelectMemo2"], Vector2.up * 140f, 0.2f, 0.5f,()=>{
            m_map["SelectMemo2"].SetActive(false);
        }));
        StartCoroutine(Move(m_map["LoadingPanel"], Vector2.left * 1024f, 0.2f, 0.8f, ()=>{
            m_map["LoadingText"].SetActive(false);
            m_map["ErrorText"].SetActive(false);
            m_map["LoadingPanel"].SetActive(false);
            StartCoroutine(SpreadHoriz(m_map["SelectPanel"], 1f, 0.2f, 0.5f,()=> {
            
                m_searchKeyword= null;
                m_desc = m_title = null;
                m_nowItemIndex = 0;
                m_autoActive = null;
                m_searchAtOnceTrig  = m_searchFutureTrig= false; 
                
                TitleActivityEndProcess();
            }));
        }));
    }

    public void BackToSearch() {
        if (m_OnPressedEscape == null) return;
        m_nowActivityEnum = NOW_ACTIVITY.ANIMATING;
        m_OnPressedEnter = null;
        m_OnPressedEscape = null;
        m_OnPressedHorizontal = null;
        m_nowActivity = SearchActivity;

        StartCoroutine(Move(m_map["NameLine"], Vector2.right * 1024, 0.2f, 0.5f,()=> {
            StartCoroutine(SpreadHoriz(m_map["NameLine"], 1f, 0.4f, 0.5f));
        }));
        StartCoroutine(Move(m_map["DescLine"], Vector2.down * 768, 0.2f, 0.5f,()=> {
            StartCoroutine(SpreadVerti(m_map["DescLine"], 1f, 0.4f, 0.5f));
        }));
        StartCoroutine(ArrayMove(new List<GameObject>() { m_map["RightArrow"], m_map["BackKey"], m_map["LeftArrow"] }, Vector2.down * 340f, 0.2f, 0.2f, 1f));

        StartCoroutine(SliderReturn(() =>
        {
            StartCoroutine(ItemMove(Vector2.right * 760f, 0.1f, 0.1f, 0.5f, ()=> {
                StartCoroutine(Move(m_map["SelectMemo"], Vector2.up * 140f, 0.2f, 0.5f,()=>{
                    m_map["SelectMemo"].SetActive(false);
                }));
                StartCoroutine(Move(m_map["SelectMemo2"], Vector2.up * 140f, 0.2f, 0.55f,()=>{
                    m_map["SelectMemo2"].SetActive(false);
                }));

                StartCoroutine(SpreadHoriz(m_map["SelectPanel"], 1f, 2f, 0.2f,()=> {
                    TitleActivityEndProcess();
                }));

                for (int i = 0; i < 15; i++) {
                    m_items[i].Hide();
                    m_items[i].SafeDelete();
                }
                ItemsActive(false);
                if(m_desc != null)m_desc.text = null;
                if(m_title != null)m_title.text = null;
                m_searchKeyword= null;
                m_desc = m_title = null;
                m_nowItemIndex = 0;
                m_autoActive = null;
                m_searchAtOnceTrig  = m_searchFutureTrig= false;

                m_map["LeftArrow"].SetActive(false);
                m_map["BackKey"].SetActive(false);
                m_map["RightArrow"].SetActive(false);
                m_map["NameLine"].SetActive(false);
                m_map["DescLine"].SetActive(false);
            }));
        }));


        
    }
    #endregion


    #region UI ANIMATION

    IEnumerator Exit()
    {
        m_map["BlackFade"].SetActive(true);
        Image l_fade = m_map["BlackFade"].GetComponent<Image>();
        l_fade.color = new Color(l_fade.color.r, l_fade.color.g, l_fade.color.b,0f);
        float l_elapsedTime = 0f;
        while (l_elapsedTime < 1f)
        {
            l_elapsedTime += Time.deltaTime*0.5f;
            l_fade.color = new Color(l_fade.color.r, l_fade.color.g, l_fade.color.b,Mathf.Lerp(l_fade.color.a,1f,l_elapsedTime));
            yield return new WaitForEndOfFrame();
        }
    #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
    #else
      Application.Quit();
    #endif
    }


    IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(1.5f);
        m_map["BlackFade"].SetActive(true);
        Image l_fade = m_map["BlackFade"].GetComponent<Image>();
        l_fade.color = new Color(l_fade.color.r, l_fade.color.g, l_fade.color.b, 0f);
        float l_elapsedTime = 0f;
        while (l_elapsedTime < 1f)
        {
            l_elapsedTime += Time.deltaTime;
            Camera.main.orthographicSize = Mathf.Lerp(5f, 3f, l_elapsedTime);
            l_fade.color = new Color(l_fade.color.r, l_fade.color.g, l_fade.color.b, Mathf.Lerp(0f, 1f, l_elapsedTime));

            k514YoutubeManager.SelectedID = k514SystemManager.SYSTEM_YOUTUBE_CONNECTOR.SearchResultSet[m_nowItemIndex].VideoID;
            Debug.Log(k514YoutubeManager.SelectedID);
            yield return new WaitForEndOfFrame();
        }

        SceneManager.LoadScene(1);
    }

    IEnumerator SliderReturn(Action p_callback)
    {

        for (int i = m_nowItemIndex; i > 0; i--)
        {
            m_items[i-1].Open();
            m_items[i-1].transform.SetParent(null);
            m_items[i-1].transform.SetParent(m_SelectContainer);
            yield return StartCoroutine(Move(m_items[i - 1].gameObject, Vector2.right * 300, 0.05f));
        }
        yield return new WaitForSeconds(0.05f);
        p_callback.Invoke();
    }

    IEnumerator PromiseWait(Func<bool> p_checkTrig,float p_minTime,float p_waitTime,Action p_Success,Action p_Fail)
    {
        float l_elapsed = 0f;
        while (!p_checkTrig()) {
            if (l_elapsed > p_waitTime) {
                p_Fail.Invoke();
                yield break;
            }
            l_elapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        while (l_elapsed < p_minTime) {
            l_elapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        p_Success.Invoke();
    }

    IEnumerator ArrayMove(List<GameObject> p_go,Vector2 p_direction, float p_interval, float p_time, float p_firstDelay = 0f, Action m_nextMove = null)
    {
        yield return new WaitForSeconds(p_firstDelay);
        for (int i = 0; i < p_go.Count; i++)
        {
            if(p_go[i].activeSelf && p_go[i] != null) StartCoroutine(Move(p_go[i], p_direction, p_time, 0f, null));
            yield return new WaitForSeconds(p_interval);
        }
        if (m_nextMove != null) m_nextMove.Invoke();
    }

    IEnumerator ItemMove(Vector2 p_direction,float p_interval, float p_time, float p_firstDelay = 0f, Action m_nextMove = null) {
        yield return new WaitForSeconds(p_firstDelay);
        for (int i = 0; i < 15; i++) {
            if(m_items[i].gameObject.activeSelf && m_items[i]!=null) m_items[i].StartCoroutine(Move(m_items[i].gameObject, p_direction, p_time, 0f, null));
            yield return new WaitForSeconds(p_interval);
        }
        if (m_nextMove != null) m_nextMove.Invoke();
    }

    IEnumerator Flicking(GameObject p_go,float p_time,Action m_nextMove = null) {
        Image l_img = p_go.GetComponent<Image>();
        float l_elapsedTime = 0f;
        float l_FlickingSpan = 0.2f;
        while (l_elapsedTime < p_time) {
            if(l_img!=null) l_img.color = new Color(l_img.color.r, l_img.color.g, l_img.color.b, 0.5f);
            yield return new WaitForSeconds(l_FlickingSpan*0.5f);
            if(l_img!=null) l_img.color = new Color(l_img.color.r, l_img.color.g, l_img.color.b, 1f);
            yield return new WaitForSeconds(l_FlickingSpan * 0.5f);
            l_elapsedTime += l_FlickingSpan;
        }
        if (m_nextMove != null) m_nextMove.Invoke();
    }


    IEnumerator Move(GameObject p_go, Vector2 p_direction,float p_time, float p_firstDelay = 0f, Action m_nextMove = null)
    {
        yield return new WaitForSeconds(p_firstDelay);
        RectTransform l_rect = p_go.GetComponent<RectTransform>();
        Image l_img = p_go.GetComponent<Image>();
        Vector3 l_targetPlace = l_rect.anchoredPosition + p_direction;
        float l_elapsedTime = 0f;
        float l_reversedTime = 1f / p_time;
        while (l_elapsedTime < p_time)
        {
            l_elapsedTime += Time.deltaTime;
            if(l_rect!=null) l_rect.anchoredPosition = Vector2.Lerp(l_rect.anchoredPosition, l_targetPlace, l_elapsedTime * l_reversedTime);
            yield return new WaitForEndOfFrame();
        }
        if(m_nextMove != null) m_nextMove.Invoke();
    }


    IEnumerator SpreadHoriz(GameObject p_go,float p_height, float p_time, float p_firstDelay = 0f, Action m_nextMove = null)
    {
        yield return new WaitForSeconds(p_firstDelay);
        RectTransform l_rect = p_go.GetComponent<RectTransform>();
        float l_elapsedTime = 0f;
        float l_reversedTime = 1f / p_time;
        while (l_elapsedTime < p_time)
        {
            l_elapsedTime += Time.deltaTime;
            if(l_rect!=null) l_rect.sizeDelta = new Vector2(4000f, Mathf.Lerp(l_rect.rect.y, p_height, l_elapsedTime * l_reversedTime));
            yield return new WaitForEndOfFrame();
        }
        if (m_nextMove != null) m_nextMove.Invoke();
    }

    IEnumerator SpreadVerti(GameObject p_go, float p_width, float p_time, float p_firstDelay = 0f, Action m_nextMove = null)
    {
        yield return new WaitForSeconds(p_firstDelay);
        RectTransform l_rect = p_go.GetComponent<RectTransform>();
        float l_elapsedTime = 0f;
        float l_reversedTime = 1f / p_time;
        while (l_elapsedTime < p_time)
        {
            l_elapsedTime += Time.deltaTime;
            if(l_rect!=null) l_rect.sizeDelta = new Vector2(Mathf.Lerp(l_rect.rect.y, p_width, l_elapsedTime * l_reversedTime),4000f);
            yield return new WaitForEndOfFrame();
        }
        if (m_nextMove != null) m_nextMove.Invoke();
    }

    #endregion
}
