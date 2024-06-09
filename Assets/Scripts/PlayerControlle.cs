using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlle : MonoBehaviour
{
    [SerializeField] private float _deadZone = 10f;
    [SerializeField] private float _speed = 0.10f;

    private Rigidbody rb;
    private Vector3 direction;
    private bool _canMove = true;
    public GameObject levTool;
    private float _speedLimit = 150f;

    private float damp = 0.15f;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        var CursorPos = Input.mousePosition;

        var x = Screen.width / 2;
        var y = Screen.height / 2;

        var dirToCursor = new Vector2(CursorPos.x - x, CursorPos.y - y);

        if(dirToCursor.x > _speedLimit)
        {
            dirToCursor.x = _speedLimit;
        }

        if (dirToCursor.x < -_speedLimit)
        {
            dirToCursor.x = -_speedLimit;
        }

        if (dirToCursor.y > _speedLimit)
        {
            dirToCursor.y = _speedLimit;
        }

        if (dirToCursor.y < -_speedLimit)
        {
            dirToCursor.y = -_speedLimit;
        }

        direction = Vector2.zero;

        if(Vector2.Distance(dirToCursor, Vector2.zero) > _deadZone && _canMove )
        {
            direction = new Vector3(dirToCursor.x, 0, dirToCursor.y) * -1;
        }  

        else
        {
            direction = Vector2.zero;
        }

        if(Input.GetMouseButton(0))
        {
            levTool.GetComponent<CapsuleCollider> ().enabled = true;

            if(levTool.transform.localScale.x < 1.5)
            {
                levTool.transform.localScale = new Vector3(levTool.transform.localScale.x + 0.1f, levTool.transform.localScale.y, levTool.transform.localScale.z + 0.1f);
            }
        }
        else
          {
             levTool.transform.localScale = new Vector3(0, levTool.transform.localScale.y, 0);
           }

        rb.velocity = Vector3.Lerp(rb.velocity, direction * _speed, damp);

        transform.rotation = Quaternion.Euler(rb.velocity.normalized * 0.5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Wall"))
        {
            direction = new Vector3(other.transform.position.x, 0, other.transform.position.z) * -1;
            rb.velocity = Vector3.Lerp(rb.velocity, direction * _speed * 400, damp);
            _canMove = false;
            StartCoroutine(WaitForMove());
        }

        else
        {
            rb.velocity = Vector3.Lerp(rb.velocity, direction * _speed, damp);
        }
    }

    private IEnumerator WaitForMove()
    {
        yield return new WaitForSeconds(0.5f);
        _canMove = true;
    }


}
