using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skull : MonoBehaviour
{
    [SerializeField]
    private float maxTimeAlive;
    private float timeAlive;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void FixedUpdate()
	{
        if(GameManager.instance.CurrentMenuState == MenuState.Game)
        {
            timeAlive += Time.deltaTime;
            if(timeAlive >= maxTimeAlive)
                Destroy(gameObject);
        }
	}
}
