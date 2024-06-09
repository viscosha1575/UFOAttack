using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCScript : MonoBehaviour
{

    private Rigidbody _rb;
    private Animator _anim;
    private Camera _cam;
    [SerializeField] private float _rotationAngle;
    private bool _onLevTool = false;
    private bool _isGrounded = false;
    private Vector3 _lastPose;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _cam = Camera.main;
        _anim = GetComponent<Animator>();

        _rotationAngle = Random.Range(0, 360);

        InvokeRepeating("ChangeAngle", 20f, Random.Range(15, 20));
        InvokeRepeating("CheckPos", 5, 3);
    }

    private void Update()
    {
        GroundCheck();
        
        _anim.SetBool("IsGrounded", _isGrounded);
        Debug.Log(_lastPose);

        if(_onLevTool)
        {

        }
        else
        {
            if(_isGrounded)
            {

                _rb.velocity = transform.forward;
                transform.rotation = Quaternion.Euler(0, _rotationAngle, 0);
                Vector3 movement = transform.forward * 2f * Time.deltaTime;

                _rb.MovePosition(transform.position + movement);

                transform.rotation = Quaternion.Euler(0, _rotationAngle, 0);
            }
            else
            {
                _rb.velocity = Vector3.down * 5;
            }
        }
    }

    void ChangeAngle()
    {
        _rotationAngle = Random.Range(90, 270);
    }




    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Envi"))
        {
            ChangeAngle();
            if(_rotationAngle  >= 360)
            {
                _rotationAngle = 0;
            }
        }
        _onLevTool = false;
           
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("LevTool"))
        {
            _onLevTool = true;
            _rb.velocity = Vector2.up * 3;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Ufo"))
        {
            Destroy(this.gameObject);
            _cam.GetComponent<NPCSpawner>().count--;
        }
    }

    void GroundCheck()
    {
        RaycastHit hit;
        float distance = 0.1f;
        Vector3 dir = Vector3.down;
        Ray ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(transform.position, dir, out hit, distance)!=null)
        {
            Debug.DrawLine(ray.origin, hit.point, Color.red);
            _isGrounded = true;
            
        }
        else
        {
            _isGrounded = false;
        }
    }

    void CheckPos()
    {
        if(Vector3.Distance(_lastPose, transform.position) < 1)
        {
            ChangeAngle();
        }

        _lastPose = transform.position;
    }


}
