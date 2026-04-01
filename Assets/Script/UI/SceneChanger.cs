using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;

public enum Scenes
{ 
    Lobby = 0,
    Stage_01,
    STG_001,
    STG_002,
    STG_003,
    SceneCount
}

public class SceneChanger : MonoBehaviour
{
    public static SceneChanger instance;

    [Serializable]
    private class SceneLib
    {
        public Scenes scene;
        public string name;
    }

    [SerializeField]
    private List<SceneLib> sceneLib = new List<SceneLib>();
    [SerializeField]
    private CanvasGroup faded;
    [SerializeField]
    private RectTransform fadeText;
    [SerializeField]
    private Vector3 bigSize;
    [SerializeField]
    private float fadeTime;
    [SerializeField]
    private GameObject fadeEndText;


    private Dictionary<Scenes, string> sceneDic = new Dictionary<Scenes, string>();

    Coroutine coroutine = null;

    Scenes currentScene = Scenes.Lobby;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);

        if (sceneLib.Count < (int)Scenes.SceneCount)
        {
            CPrint.Error("씬 설정 오류 - 인스펙터 확인");
        }

        for (int i = 0; i < sceneLib.Count; i++)
        {
            sceneDic.Add(sceneLib[i].scene, sceneLib[i].name);
        }
    }

    public void MoveScene(Scenes target)
    {
        if (coroutine != null)
        {
            CPrint.Log("이미 로딩중");
            return;
        }

        if (sceneDic.TryGetValue(target, out string _name))
        {
            currentScene = target;
            coroutine = StartCoroutine(LoadSceneCoroutine(_name));
        }
    }

    public void ReLoadScene()
    {
        if (coroutine != null)
        {
            CPrint.Log("이미 로딩중");
            return;
        }

        string _name = NowScene();

        coroutine = StartCoroutine(LoadSceneCoroutine(_name));
    }

    IEnumerator LoadSceneCoroutine(string _name)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(_name);


        op.allowSceneActivation = false;



        fadeEndText.SetActive(false);

        faded.alpha = 1;
        faded.blocksRaycasts = true;
        faded.interactable = true;

        float currentTime = 0;
        float t;

        while (currentTime < fadeTime)
        {
            currentTime += Time.deltaTime;

            t = currentTime / fadeTime;

            t = MathF.Sin(t * MathF.PI * 0.5f);

            fadeText.localScale = Vector3.Lerp(bigSize, Vector3.zero, t);
            yield return null;
        }

        fadeText.localScale = Vector3.zero;

        while (op.progress < 0.9f)
        {
            yield return null;
        }

        op.allowSceneActivation = true;

        fadeEndText.SetActive(true);

        yield return new WaitForSeconds(1f);

        faded.alpha = 0;
        faded.blocksRaycasts = false;
        faded.interactable = false;

        ClearCoroutine();
        yield break;
    }

    void ClearCoroutine()
    {
        coroutine = null;
    }

    public string NowScene()
    {
        if (sceneDic.TryGetValue(currentScene, out string _name))
        {
            return _name;
        }

        CPrint.Error("현재 씬 불명");

        return null;
    }
}
