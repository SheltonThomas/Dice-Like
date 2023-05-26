using System.Collections;
using System;
using UnityEngine;
using UnityEngine.Assertions.Must;
using Unity.VisualScripting;

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
    }

    //Update to use the new dice game object attached to the player object
    //Movement will translate the ENTIRE player gameobject while using the previously made code to seperately rotate the dice gameobject

    //After closer inspection the cube is slightly moving from its original position after its rotation

    //Finishes movement in either a single 90 degree turn or multiple 90 degree turns
    IEnumerator RotateCube(Transform destintion)
    {
        
        GameObject currentTarget = Instantiate(new GameObject());
        currentTarget.transform.SetPositionAndRotation(_dice.transform.position, _dice.transform.rotation);
        float rotationAmount = 90;
        Vector3 rotationAxis = new Vector3(0, 0, 0);
        _isMovementCoroutineStopped = false;

        //For some reason cube is rotation diagonally when trying to rotate to a target straight on the x axis

        //Player also didnt start in correct position when changing player position before playing game
        //Maybe just a bug with (current code)*

        //Another thing to note is that this (current code)* almost works without the dice being a child of the player gameobject
        // but for some reason not having the dice be a part of the game object.
        //Check the actual postition of the rotation relative to local rotation and check the movement direction, 
        // even though that shouldnt effect the code
        //* = Current code refers to anything that is actual logic, varible that are used can be replaced

        //Maybe just try to find a solution that would work with animation
        GameObject distanceFromNode = Instantiate(Cube);
        distanceFromNode.transform.position = destintion.position - _dice.transform.position;
        GameObject rotationPoint = Instantiate(Cube);
        rotationPoint.transform.parent = transform;

        //Find a way to use step count with this method

        //Make a switch for the 4 different directions
        if (distanceFromNode.transform.position.x > 0)
        {
            rotationAmount *= -1;
            rotationAxis.z = 1;
        }
        else if (distanceFromNode.transform.position.x < 0) rotationAxis.z = 1;

        if (distanceFromNode.transform.position.z < 0)
        {
            rotationAmount *= -1;
            rotationAxis.x = 1;
        }
        else if (distanceFromNode.transform.position.z < 0) rotationAxis.x = 1;

        rotationPoint.transform.position = (distanceFromNode.transform.position.normalized + Vector3.down) / 2;
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
            _dice.transform.RotateAround(rotationPoint.transform.position, rotationAxis, rotationAmount * Time.fixedDeltaTime * speed);
            _dice.transform.position = new Vector3(0, _dice.transform.position.y, 0);

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