using UnityEngine;
using System.Collections;

public class JsonTest : MonoBehaviour {
    public TextAsset textSource;


	// Use this for initialization
	void Start () {
        if (textSource != null)
        {
            LitJson.JsonData jd = LitJson.JsonMapper.ToObject(textSource.text);
            Debug.Log((string)jd["Name"]);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
