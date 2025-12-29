using UnityEngine;

public class BulletManager : MonoBehaviour
{
    private Rigidbody bulletRigidbody;

    [SerializeField]
    private float moveSpeed = 10f; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bulletRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        BulletMove();
    }

    private void BulletMove()
    {
        bulletRigidbody.linearVelocity = transform.forward * moveSpeed;
    }



}
