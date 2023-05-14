using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    private IEnumerator _movementCoroutine;
    private Quaternion startingRotation;
    public float speed = 1;
    private bool _isMoving = false;
    public float hitboxSize = 1.5f;
    public Transform destination;
    private Vector3 _destination;

    void Start()
    {
        _destination = destination.position;
    }

    void Update()
    {
        //if (!_moving)
        //{
        //    if(Input.GetAxis("Horizontal") > 0)
        //    {
        //        _moving = true;
        //        destination = transform.position + (Vector3.right * 2);
        //        _rotationCoroutine = Rotate(new Vector3(0, 0, -90));
        //        StartCoroutine(_rotationCoroutine);
        //    }
        //    else if(Input.GetAxis("Horizontal") < 0)
        //    {
        //        _moving = true;
        //        destination = transform.position + (Vector3.left * 2);
        //        _rotationCoroutine = Rotate(new Vector3(0, 0, 90));
        //        StartCoroutine(_rotationCoroutine);
        //    }
        //    else if(Input.GetAxis("Vertical") > 0)
        //    {
        //        _moving = true;
        //        destination = transform.position + (Vector3.forward * 2);
        //        _rotationCoroutine = Rotate(new Vector3(90, 0, 0));
        //        StartCoroutine(_rotationCoroutine);
        //    }
        //    else if(Input.GetAxis("Vertical") < 0)
        //    {
        //        _moving = true;
        //        destination = transform.position + (Vector3.back * 2);
        //        _rotationCoroutine = Rotate(new Vector3(-90, 0, 0));
        //        StartCoroutine(_rotationCoroutine);
        //    }
        //}
        if (Input.GetAxis("Vertical") < 0 && !_isMoving)
        {
            _isMoving = true;
            _movementCoroutine = StepToDestination(_destination);
            StartCoroutine(_movementCoroutine);
        }
    }

    //Following 2 coroutines would not work for movement on the y vector

    //Finishes movement in multiple 90 degree turns
    IEnumerator StepToDestination(Vector3 destintion, float stepSize = 1)
    {
        Vector3 movementDirection = (destintion - transform.position).normalized * stepSize;
        bool isFinishedMoving = false;
        Vector3 rotationDirection = movementDirection * 90;
        rotationDirection = new Vector3(rotationDirection.z, rotationDirection.y, -rotationDirection.x);

        while (true)
        {
            transform.Rotate(rotationDirection * Time.deltaTime * speed);

            transform.position += movementDirection * Time.deltaTime * speed;
            if((transform.position - destintion).magnitude < .01f)
                isFinishedMoving = true;

            if (isFinishedMoving)
            {
                transform.position = destintion;
                Quaternion newRotation = new Quaternion(Mathf.Round(transform.rotation.x), Mathf.Round(transform.rotation.y), Mathf.Round(transform.rotation.z), Mathf.Round(transform.rotation.w));
                transform.rotation = newRotation;
                Debug.Log("Finished");
                break;
            }
            yield return null;
        }

        //if (Physics.Raycast(transform.position, Vector3.up, out RaycastHit hit, 1))
        //{
        //    FaceBehavior face = hit.collider.gameObject.GetComponent<FaceBehavior>();
        //}
        _isMoving = false;
        StopCoroutine(_movementCoroutine);
    }

    //Finishes movement in a single 90 degree turn
    IEnumerator MoveToDestination(Vector3 destintion)
    {
        Vector3 movementDirection = (destintion - transform.position);
        bool isFinishedMoving = false;
        Vector3 rotationDirection = movementDirection.normalized * 90;
        rotationDirection = new Vector3(rotationDirection.z, rotationDirection.y, -rotationDirection.x);

        while (true)
        {
            transform.Rotate(rotationDirection * Time.deltaTime * speed);

            
            transform.position += movementDirection * Time.deltaTime * speed;
            if ((transform.position - destintion).magnitude < speed * .1f)
                isFinishedMoving = true;

            if (isFinishedMoving)
            {
                transform.position = destintion;
                Quaternion newRotation = new Quaternion(Mathf.Round(transform.rotation.x), Mathf.Round(transform.rotation.y), Mathf.Round(transform.rotation.z), Mathf.Round(transform.rotation.w));
                transform.rotation = newRotation;
                Debug.Log("Finished");
                break;
            }
            yield return null;
        }

        //if (Physics.Raycast(transform.position, Vector3.up, out RaycastHit hit, 1))
        //{
        //    FaceBehavior face = hit.collider.gameObject.GetComponent<FaceBehavior>();
        //}
        _isMoving = false;
        StopCoroutine(_movementCoroutine);
    }
}