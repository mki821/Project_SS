using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public Vector3 dir;

    [SerializeField]
    private float speed;

    private void Awake()
    {
        GameManager.instance.player = gameObject;
    }

    private void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        dir = new Vector2(x, y);

        transform.position += dir.normalized * Time.deltaTime * speed;
    }
}
