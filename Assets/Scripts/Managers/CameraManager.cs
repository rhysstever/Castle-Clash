using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private float camMoveSpeed, xNegBounds, xPosBounds;

    // Start is called before the first frame update
    void Start()
    {
        if(xPosBounds < xNegBounds)
            xPosBounds = xNegBounds;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void FixedUpdate()
	{
        float movement = cam.transform.position.x;
        if(Input.GetKey(KeyCode.A))
        {
            movement -= (camMoveSpeed * Time.deltaTime);
        }
        else if(Input.GetKey(KeyCode.D))
		{
            movement += (camMoveSpeed * Time.deltaTime);
        }

        movement = Mathf.Clamp(movement, xNegBounds, xPosBounds);
        cam.transform.position = new Vector3(movement, cam.transform.position.y, cam.transform.position.z);
	}
}
