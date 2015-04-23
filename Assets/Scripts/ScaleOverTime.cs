using UnityEngine;
using System.Collections;

public class ScaleOverTime : MonoBehaviour {

    public float time = 1f;
    public float initialScale = .1f;
    public float endScale = 5f;
    public float damage = 0;
	// Use this for initialization
	void Start () {
        StartCoroutine("ScaleOverTimeMethod");
	}

    IEnumerator ScaleOverTimeMethod()
    {
        float t = 0;
        while(t <= time)
        {
            t+= Time.deltaTime;
            float scale = Mathf.Lerp(initialScale, endScale, t/time);
            gameObject.transform.localScale = new Vector3(scale, scale, scale);
            yield return null;
        }

        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Collider>().tag == "Enemy")
        {
            var script = other.GetComponent<Collider>().gameObject.GetComponent<TemporaryEnemyScript>();
            script.currentHP -= (5 * damage);
        }
    }
}
