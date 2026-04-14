using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    private StageRegistry stageRegistry;
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
    }

    public void MoveScene(Scenes target)
    {
        if (coroutine != null)
        {
            CPrint.Log("이미 로딩중");
            return;
        }

        currentScene = target;
        string _name = stageRegistry.GetStageDataByEnum(target);

        if (_name == null)
        {
            CPrint.Error("정체불명의 씬");
            return;
        }
        coroutine = StartCoroutine(LoadSceneCoroutine(_name));
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
            currentTime += Time.unscaledDeltaTime;

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

        yield return new WaitForSecondsRealtime(0.5f);

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
        string nowScene = stageRegistry.GetStageDataByEnum(currentScene);

        if (nowScene == null)
        {
            CPrint.Error("정체불명의 씬");
            return stageRegistry.GetStageDataByEnum(Scenes.Lobby);
        }

        return nowScene;
    }
}
