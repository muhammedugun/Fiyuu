using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class CubeTest : MonoBehaviour
{
    public MMF_Player player;
    private void Start()
    {
        player = GetComponent<MMF_Player>();

    }

    private void OnCollisionEnter(Collision collision)
    {
        player.PlayFeedbacks();
    }
}
