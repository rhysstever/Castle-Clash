using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAnimate : MonoBehaviour
{
    [SerializeField]
    private float minTime, maxTime;
    [SerializeField]
    private string triggerName;
    private float timeToAnimate, currentTime;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        SetNewAnimateTime();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        currentTime += Time.deltaTime;
        if(currentTime >= timeToAnimate)
        {
            animator.SetTrigger(triggerName);
            currentTime = 0;
            SetNewAnimateTime();
        }
    }

    private void SetNewAnimateTime()
    {
        timeToAnimate = Random.Range(minTime, maxTime);
    }
}
