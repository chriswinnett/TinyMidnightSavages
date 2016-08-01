using UnityEngine;
using System.Collections;

public class Parallax : MonoBehaviour {
    public float speed = 5f;
    public Sprite mainSprite;

    Vector3 lastPos;
    public float width = 0;
	// Use this for initialization
	void Start () {
        width = mainSprite.textureRect.width*transform.localScale.x;
    }
	
	// Update is called once per frame
	void Update ()
    {
        transform.localPosition = new Vector3(((transform.root.position.x*speed % width)), transform.localPosition.y, transform.localPosition.z);
        if(transform.localPosition.x < -width)
        {
            transform.localPosition += new Vector3(width,0,0);
        }else if(transform.localPosition.x > 0)
        {
            transform.localPosition -= new Vector3(width, 0, 0);
        }
	}
}
