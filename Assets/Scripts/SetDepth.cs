using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class SetDepth : MonoBehaviour {

    Transform shadow;

	// Use this for initialization
	void Start () {
        shadow = transform.FindChild("shadow");
	}
	
	// Update is called once per frame
	void LateUpdate () {
        if (shadow == null)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
        }
        else
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, shadow.transform.position.y);
        }
	}
}
