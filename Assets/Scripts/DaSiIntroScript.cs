using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DaSiIntroScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(Hide(1.33f));
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator Hide(float when)
    {
        yield return new WaitForSeconds(when);
        GetComponent<Text>().text = "";
    }

    public void GameOver(bool win)
    {
        if (win)
        {
            GetComponent<Text>().text = "Complete!!";
        }
        else
        {
            GetComponent<Text>().text = "";
        }
    }
}
