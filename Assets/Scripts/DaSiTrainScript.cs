using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaSiTrainScript : MonoBehaviour {

    private int hitTargets = 0;
    private GameObject smoke;
    private AudioSource sound;

	// Use this for initialization
	void Start () {
        smoke = GameObject.Find("Smoker");
        if (smoke == null)
            smoke = GameObject.Find("DaSiSmoker");
        sound = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Run()
    {
        smoke.GetComponent<ParticleSystem>().Play();
        sound.Play();
    }

    public int NumHit()
    {
        return hitTargets;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Target"))
        {
            hitTargets++;
        }
    }
}
