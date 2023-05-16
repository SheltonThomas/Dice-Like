using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    private IEnumerator _movementCoroutine;
    public float speed = 1;
    private bool _isMoving = false;

    [SerializeField]
    private Node _currentNode;
    private Node[] _currentConnectedNodes;
    private bool _isMovementCoroutineStopped = true;
    private bool _needToUpdateMovementCoroutine = false;
    void Start()
    {
        _movementCoroutine = MoveToDestination(_currentNode.Position);
        _currentConnectedNodes = _currentNode.GetNodes();
    }

    void Update()
    {
        if (Input.GetAxis("Vertical") < 0)  _isMoving = true;

        if (_isMoving && _isMovementCoroutineStopped) StartCoroutine(_movementCoroutine);
        if(_needToUpdateMovementCoroutine) UpdateMovementCoroutine();
    }

    //Finishes movement in either a single 90 degree turn or multiple 90 degree turns
    IEnumerator MoveToDestination(Vector3 destintion, float stepSize = 1f)
    {
        Vector3 movementDirection;
        Vector3 rotationDirection;
        bool isFinishedMoving = false;
        _isMovementCoroutineStopped = false;

        //Multiple 90 degree turns
        if (stepSize != 0)
        {
            movementDirection = (destintion - transform.position).normalized * stepSize;
            rotationDirection = movementDirection * 90;
        }
        //Single 90 degree turn
        else
        {
            movementDirection = destintion - transform.position;
            rotationDirection = movementDirection.normalized * 90;
        }
        rotationDirection = new Vector3(rotationDirection.z, rotationDirection.y, -rotationDirection.x);

        while (!isFinishedMoving)
        {
            transform.Rotate(rotationDirection * Time.deltaTime * speed);

            
            transform.position += movementDirection * Time.deltaTime * speed;
            if ((transform.position - destintion).magnitude < speed * .01f)
                isFinishedMoving = true;

            if (isFinishedMoving)
            {
                transform.position = destintion;
                Quaternion newRotation = new Quaternion(Mathf.Round(transform.rotation.x), Mathf.Round(transform.rotation.y), Mathf.Round(transform.rotation.z), Mathf.Round(transform.rotation.w));
                transform.rotation = newRotation;
                break;
            }
            yield return null;
        }

        //if (Physics.Raycast(transform.position, Vector3.up, out RaycastHit hit, 1))
        //{
        //    FaceBehavior face = hit.collider.gameObject.GetComponent<FaceBehavior>();
        //}
        
        _isMovementCoroutineStopped = true;
        _isMoving = false;
        _needToUpdateMovementCoroutine = true;
        StopCoroutine(_movementCoroutine);
    }

    private void UpdateMovementCoroutine()
    {
        _needToUpdateMovementCoroutine = false;
        if (_currentConnectedNodes == null) return;
        _currentNode = _currentConnectedNodes[0];
        _currentConnectedNodes = _currentNode.GetNodes();
        _movementCoroutine = MoveToDestination(_currentNode.gameObject.transform.position);
    }
}