using UnityEngine;
using System.Collections;

public class SimpleCameraFollow : MonoBehaviour {

    public GameObject objectToFollow;
    public float speed;
    public float minX = 0;
    public float maxX = 0;
	
	// Update is called once per frame
	void Update ()
    {
	    if(objectToFollow != null)
        {
            //Gets jumpy when smoothing due to distances changing. Needs to be a bit better.
            transform.position = new Vector3(Mathf.Clamp(objectToFollow.transform.position.x, minX, maxX), transform.position.y, -10);
            Player.instance = objectToFollow;
        }
	}

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 1, 0, .2f);
        Gizmos.DrawCube(new Vector3((minX + maxX) / 2,0,0), new Vector3((maxX - minX)+18, 10, 10));
    }
    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 1, 0, .2f);
        Gizmos.DrawWireCube(new Vector3((minX + maxX) / 2, 0, 0), new Vector3((maxX - minX) + 18, 10, 10));
    }
}
