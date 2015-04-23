using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenerateWave : MonoBehaviour {

    public GameObject[] enemyKind;
    public GameObject spawnPoint;

    //private List<GameObject> wave1;
    private int count;
    private float additionalHP;

	void Start () {
        count = 0;
        additionalHP = 100;
        /*
        wave1 = new List<GameObject>();
        for (int i = 0; i < 20; i++)
        {
            wave1.Add(enemyKind[0]);
        }
        Wave1();*/
        InvokeRepeating("SimpleGenerate", .1f, 2f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void SimpleGenerate()
    {
        count++;
        GameObject go = Instantiate(enemyKind[0], spawnPoint.transform.position, Quaternion.identity) as GameObject;
        var script = go.GetComponent<TemporaryEnemyScript>();
        if (count % 5 == 0)
        {
            additionalHP += 70.0f;
        }
        script.currentHP = additionalHP;
        script.maxHP = additionalHP;
    }
}
