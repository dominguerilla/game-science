using UnityEngine;

public class EmojiScript : MonoBehaviour {

    private float time = 0;
    private float addHeight = 0;
    private float riseSpeed = 0.1f;
	
	// Update is called once per frame
	void Update () {
        gameObject.transform.position =
            new Vector3(gameObject.transform.position.x,
            gameObject.transform.position.y + riseSpeed,
            gameObject.transform.position.z);

        time += Time.deltaTime;
	    if(time > 3)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
	}
}
