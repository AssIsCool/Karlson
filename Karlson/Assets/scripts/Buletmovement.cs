using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buletmovement : MonoBehaviour
{
    Rigidbody rb;
    public float timer;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.AddForce(new Vector3(0, 0, 2), ForceMode.Impulse);
        timer += Time.deltaTime;

        if (timer > 2)
            Destroy(gameObject);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 0)
            Destroy(gameObject);
    }
}
