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
    private float magnify = 10f;
    [SerializeField]
    private float zoomTime = 2;

    private float currentTime = 0;

    private void LateUpdate()
    {
        LookPlayer();
    }

    void LookPlayer()
    {
        camTr.transform.position = new Vector3(player.position.x, player.position.y, camTr.position.z);
    }

    public IEnumerator ZoomCoroutine()
    {
        currentTime = 0;

        while (currentTime < zoomTime)
        {
            currentTime += Time.unscaledDeltaTime;

            float t = currentTime / zoomTime;

            cam.orthographicSize = Mathf.Lerp(basicZoom, magnify, t);

            yield return null;
        }

        currentTime = 0;

        while (currentTime < zoomTime)
        {
            currentTime += Time.unscaledDeltaTime;

            float t = currentTime / zoomTime;

            cam.orthographicSize = Mathf.Lerp(magnify, basicZoom, t);

            yield return null;
        }

        cam.orthographicSize = basicZoom;

        yield break;
    }
}
