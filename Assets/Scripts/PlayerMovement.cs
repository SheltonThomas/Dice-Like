using System.Collections;
using System;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class PlayerMovement : MonoBehaviour
{
    public GameObject Cube;

    private IEnumerator _movementCoroutine;
    public float speed = 1;
    public float StepCount = 1;
    private bool _isMoving = false;

    [SerializeField]
    private GameObject _dice;
    [SerializeField]
    private Node _currentNode;
    private Node[] _currentConnectedNodes;
    private bool _isMovementCoroutineStopped = true;
    private bool _needToUpdateMovementCoroutine = false;
    void Start()
    {
        _movementCoroutine = RotateCube(_currentNode.transform);
        _currentConnectedNodes = _currentNode.GetNodes();
    }

    void Update()
    {
        if (Input.GetAxis("Vertical") < 0) _isMoving = true;

        if (_isMoving && _isMovementCoroutineStopped) StartCoroutine(_movementCoroutine);
        if (_needToUpdateMovementCoroutine) UpdateMovementCoroutine();
        if (!_isMovementCoroutineStopped) transform.position += (_currentNode.transform.position - transform.position).normalized * Time.deltaTime * speed;
    }

    //Update to use the new dice game object attached to the player object
    //Movement will translate the ENTIRE player gameobject while using the previously made code to seperately rotate the dice gameobject

    //Finishes movement in either a single 90 degree turn or multiple 90 degree turns
    IEnumerator RotateCube(Transform destintion)
    {
        
        GameObject currentTarget = Instantiate(Cube);
        currentTarget.transform.SetPositionAndRotation(transform.position, transform.rotation);
        float rotationAmount = 90;
        Vector3 rotationAxis = new Vector3(0, 0, 0);
        _isMovementCoroutineStopped = false;

        Vector3 distanceFromNode = destintion.position - transform.position;
        GameObject rotationPoint = Instantiate(Cube);
        rotationPoint.transform.parent = transform;

        //Find a way to use step count with this method

        //Make a switch for the 4 different directions
        if (distanceFromNode.x > 0)
        {
            rotationAmount *= -1;
            rotationAxis.z = 1;
        }
        else if (distanceFromNode.x < 0) rotationAxis.z = 1;

        if (distanceFromNode.z < 0)
        {
            rotationAmount *= -1;
            rotationAxis.x = 1;
        }
        else if (distanceFromNode.z < 0) rotationAxis.x = 1;

        rotationPoint.transform.position = (distanceFromNode.normalized + Vector3.down) / 2;
        currentTarget.transform.RotateAround(rotationPoint.transform.position, rotationAxis, rotationAmount);
        while (_isMoving)
        {
            //if ((transform.position - currentTargetTransform.transform.position).magnitude < speed * .1f)
            //{
            //    currentTargetTransform.transform.position = new Vector3((float)Math.Round(currentTargetTransform.transform.position.x),
            //                                                            (float)Math.Round(currentTargetTransform.transform.position.y),
            //                                                            (float)Math.Round(currentTargetTransform.transform.position.z));
            //    rotationPoint.transform.position = (distanceFromNode.normalized + Vector3.down) + transform.position / 2;
            //    transform.SetPositionAndRotation(currentTargetTransform.transform.position, currentTargetTransform.transform.rotation);
            //    currentTargetTransform.transform.RotateAround(rotationPoint.transform.position, rotationAxis, rotationAmount);
            //}
            transform.RotateAround(rotationPoint.transform.position, rotationAxis, rotationAmount * Time.fixedDeltaTime * speed);
            transform.position -= currentTarget.transform.position.normalized * Time.fixedDeltaTime * speed;

            //Update Position here

            //Vector3 movementVector = currentTargetTransform.transform.position - transform.position;
            //movementVector.Normalize();
            //if ((transform.position - destintion.position).magnitude < speed * .1f)
            //{
            //    transform.rotation = _currentNode.transform.rotation;
            //    transform.position = _currentNode.transform.position;
            //    break;
            //}

            yield return null;
        }

        if (Physics.Raycast(transform.position, Vector3.up, out RaycastHit hit, 1))
        {
            hit.collider.gameObject.GetComponent<FaceBehavior>().SayFaceColor();

        }

        Destroy(currentTarget);
        Destroy(rotationPoint);
        _isMovementCoroutineStopped = true;
        _isMoving = false;
        _needToUpdateMovementCoroutine = true;
        StopCoroutine(_movementCoroutine);
    }

    private void UpdateMovementCoroutine(int pathChoice = 0)
    {
        _needToUpdateMovementCoroutine = false;
        if (_currentConnectedNodes.Length == 0) return;

        if (_currentConnectedNodes.Length == 1)
        {
            _currentNode = _currentConnectedNodes[0];
            _isMoving = true;
        }
        else _currentNode = _currentConnectedNodes[pathChoice];
        _currentConnectedNodes = new Node[0];

        if(_currentNode.GetNodes().Length > 0)
            _currentConnectedNodes = _currentNode.GetNodes();


        _movementCoroutine = RotateCube(_currentNode.transform);
    }
}