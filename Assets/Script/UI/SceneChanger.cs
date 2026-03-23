using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Scenes
{ 
    Lobby = 0,
    Stage_01,
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




    private Dictionary<Scenes, string> sceneDic = new Dictionary<Scenes, string>();

    Coroutine coroutine = null;

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
            coroutine = StartCoroutine(LoadSceneCoroutine(_name));
        }
    }

    IEnumerator LoadSceneCoroutine(string _name)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(_name);


        op.allowSceneActivation = false;

        // 씬 이동 이벤트



        while (op.progress < 0.9f)
        {
            yield return null;
        }



        // 씬 이동 이벤트

        op.allowSceneActivation = true;

        ClearCoroutine();
        yield break;
    }

    void ClearCoroutine()
    {
        coroutine = null;
    }
}
