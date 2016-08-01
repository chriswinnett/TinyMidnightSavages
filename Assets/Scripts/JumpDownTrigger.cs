using UnityEngine;
using System.Collections;

public class JumpDownTrigger : MonoBehaviour {
    public Transform[] jumpPoints;
	// Use this for initialization
	void OnTriggerEnter2D(Collider2D c)
    {
        if(c.tag == "Enemy")
        {
            int closestJumppoint = 0;
            for (int i = 0; i < jumpPoints.Length; i++)
            {
                if(Vector3.Distance(jumpPoints[i].position, c.transform.position) <= Vector3.Distance(jumpPoints[closestJumppoint].position, c.transform.position))
                {
                    closestJumppoint = i;
                }
            }
            c.SendMessage("JumpDown", jumpPoints[closestJumppoint].transform.position,SendMessageOptions.DontRequireReceiver);
        }
    }
}
