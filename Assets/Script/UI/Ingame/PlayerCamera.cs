using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private Transform camTr;
    [SerializeField]
    private Transform player;
    [SerializeField]
    private float basicZoom = 4.5f;
    [SerializeField]
    private float magnify = 8f;
    [SerializeField]
    private float zoomTime = 2f;

    private float currentTime = 0;

    private Coroutine coroutine;

    private void LateUpdate()
    {
        LookPlayer();
    }

    void LookPlayer()
    {
        camTr.transform.position = new Vector3(player.position.x, player.position.y, camTr.position.z);
    }

    public void ZoomIn()
    { 
        if(coroutine != null)
        {
            StopCoroutine(coroutine);    
        }

        coroutine = StartCoroutine(ZoomCoroutine(basicZoom));
    }

    public void ZoomOut()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        coroutine = StartCoroutine(ZoomCoroutine(magnify));
    }

    IEnumerator ZoomCoroutine(float end)
    {
        currentTime = 0;

        float start = cam.orthographicSize;

        while (currentTime < zoomTime)
        {
            currentTime += Time.unscaledDeltaTime;

            float t = currentTime / zoomTime;

            cam.orthographicSize = Mathf.Lerp(start, end, t);

            yield return null;
        }

        cam.orthographicSize = end;

        yield break;
    }
}
