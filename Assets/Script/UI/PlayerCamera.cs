using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField]
    private Transform cam;
    [SerializeField]
    private Transform player;

    private void LateUpdate()
    {
        cam.transform.position = new Vector3(player.position.x, player.position.y, cam.position.z);
    }
}
