using UnityEngine;
using System.Collections;

/// <summary>
/// Controls the camera and keyboard controls. Meant to be used in conjunction with ToyGUI.
/// </summary>
public class ToyRTS : MonoBehaviour {

	GameObject player;



	public float speed = 5.0f;
	public float minZoom = 3.0f;
	public float maxZoom = 300.0f;
	public float rotateSpeed = 20.0f;
	public GameObject rotationPivot;

	Camera m_Camera;

	// Use this for initialization
	void Start () {
		player = this.gameObject;
		if (player) {
			m_Camera = player.GetComponentInChildren<Camera> ();
			RaycastHit hit;
			if (Physics.Raycast (m_Camera.transform.position, m_Camera.transform.forward, out hit)) {
				rotationPivot.transform.position = hit.point;
			} else {
				Debug.Log ("No suitable area to set rotation pivot.");
			}
		} else {
			Debug.Log ("No 'Player' gameobject set for ToyRTS!");
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.W))
		{
			player.transform.position = player.transform.position + (new Vector3(0, 0, 1) * speed * Time.deltaTime);
		}

		if (Input.GetKey(KeyCode.A))
		{

			player.transform.position = player.transform.position + (new Vector3(-1, 0, 0) * speed * Time.deltaTime);
		}

		if (Input.GetKey(KeyCode.S))
		{

			player.transform.position = player.transform.position + (new Vector3(0, 0, -1) * speed * Time.deltaTime);
		}

		if (Input.GetKey(KeyCode.D))
		{

			player.transform.position = player.transform.position + (new Vector3(1, 0, 0) * speed * Time.deltaTime);
		}

		if (Input.GetKey (KeyCode.Q)) {
			transform.RotateAround (rotationPivot.transform.position, new Vector3(0.0f,1.0f,0.0f),20 * Time.deltaTime * rotateSpeed);
		}else if(Input.GetKey(KeyCode.E)){
			transform.RotateAround (rotationPivot.transform.position, new Vector3(0.0f,1.0f,0.0f),20 * Time.deltaTime * rotateSpeed * -1);
		}

		if (Input.GetAxis("Mouse ScrollWheel") > 0f)
		{
			if (player.transform.position.y <= minZoom)
			{

			}
			else
			{
				player.transform.position = player.transform.position + (m_Camera.transform.forward * 40f * Time.deltaTime);
			}
		}
		else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
		{
			if (player.transform.position.y >= maxZoom)
			{

			}
			else
			{
				player.transform.position = player.transform.position + (m_Camera.transform.forward * -1 * 40f * Time.deltaTime);
			}
		}
	}
}
