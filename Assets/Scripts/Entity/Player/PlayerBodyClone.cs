using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBodyClone : MonoBehaviour
{
    private Animator animator = null;
    private List<SkinnedMeshRenderer> skinnedMeshRenderers = new();

    private float fadeOutTime = 0f;
    private float fadeOutTimer = 0f;

    private float fadeOutTimerNormalized => fadeOutTimer / fadeOutTime;

    private bool fadeOutTimerStart = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>().ToList();
    }

    void Update()
    {
        CheckFadeOutTimer();
    }

    private void CheckFadeOutTimer()
    {
        if(fadeOutTimerStart && fadeOutTimer > 0f)
        {
            fadeOutTimer -= Time.deltaTime;

            // FadeOutCode 가능하면 넣어보기
            // foreach(var s in skinnedMeshRenderers)
            // {
            //     foreach(var m in s.materials)
            //     {
            //         m.color.SetColorAlpha(fadeOutTimerNormalized);
            //     }
            // }

            if(fadeOutTimer <= 0f)
            {
                fadeOutTimerStart = false;

                PoolManager.Instance.SetObject("PlayerBodyClone", gameObject);
            }
        }
    }

    public void OnSpawn(float _fadeOutTime)
    {
        fadeOutTime = _fadeOutTime;
        fadeOutTimer = fadeOutTime;

        fadeOutTimerStart = true;
    }

    public void SetMotion(string animName, int layer, float normalizedTime, float animSpeed)
    {
        animator.Play(animName, layer, normalizedTime);

        animator.speed = animSpeed;
    }
}
