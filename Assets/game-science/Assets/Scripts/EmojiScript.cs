using UnityEngine;

public class EmojiScript : MonoBehaviour {

    public enum EmojiTypes
    {
        Hurt_Emoji,
        Anger_Emoji,
        Laugh_Emoji,
        Heart_Emoji,
    }

    private float time = 0;
    private float addHeight = 0;
    private float riseSpeed = 0.03f;
	
	// Update is called once per frame
	void Update () {
        gameObject.transform.position =
            new Vector3(gameObject.transform.position.x,
            gameObject.transform.position.y + riseSpeed,
            gameObject.transform.position.z);

        time += Time.deltaTime;
	    if(time > 2)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
	}
}
