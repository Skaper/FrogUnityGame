using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MosquitoController : MonoBehaviour
{
    private float speed = 5;
    private float scaleStep = 0.09f;
    public bool isDead = false;
    private bool isScaling = false;
    Vector2 targetPosition;
    // Start is called before the first frame update
    void Start()
    {
        targetPosition = transform.position;     
    }

    // Update is called once per frame
    void Update()
    {
        if (!targetPosition.Equals(transform.position))
        {
            Vector2 vectorToTarget = targetPosition - (Vector2)transform.position;
            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - 90;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = q;
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
            if(isScaling && transform.localScale.x > 0 + scaleStep) transform.localScale -= new Vector3(scaleStep, scaleStep, scaleStep);
           
        }
    }

    public void DoStep()
    {
        float x = Random.Range((int)transform.position.x - 1, (int)transform.position.x + 2);
        float y = Random.Range((int)transform.position.y - 1, (int)transform.position.y + 2);
        targetPosition = new Vector2(x, y);
    }
    public void DoEat(Vector2 postition)
    {
        targetPosition = postition;
        isScaling = true;
        speed = 10;
    }
}
