using UnityEngine;
using System.Collections;


[RequireComponent(typeof(CharacterController))]
public class ThirdPersonController : MonoBehaviour 
{
    public float MovementSpeed = 10f;

    private Transform _mainCameraTransform;
    private Transform _transform;
    private CharacterController _characterController;

    protected dfCharacterUnityObject _character = null;

    void Start()
    {
        _character = gameObject.GetComponent<dfCharacterUnityObject>();
    }

    private void OnEnable()
    {
        _mainCameraTransform = Camera.main.GetComponent<Transform>();
        _characterController = GetComponent<CharacterController>();
        _transform = GetComponent<Transform>();
    }

    public void Update()
    {
        // Just use CnInputManager. instead of Input. and you're good to go
        var inputVector = new Vector3(dfInputManager.Instance.GetAxis("Horizontal"), 0.0f, dfInputManager.Instance.GetAxis("Vertical"));
        Vector3 movementVector = Vector3.zero;


        if (inputVector.sqrMagnitude > 0.1f)
        {
            movementVector = _mainCameraTransform.TransformDirection(inputVector);
            movementVector.Normalize();
            movementVector.y = 0f;
            _transform.forward = movementVector;
            /*
            float angle = Mathf.Abs(Vector3.Dot(_transform.forward, movementVector));
            if (angle > 0.1f)
            {
                _transform.forward = movementVector;
            }
            */

            //Debug.Log(angle.ToString());

            _character.MoveCharacter(movementVector);// * Time.deltaTime);
        }

        

        //movementVector += Physics.gravity;
        //_characterController.Move(movementVector * Time.deltaTime * 100.0f);

        //_character.MoveCharacter(movementVector);// * Time.deltaTime);
    }
}
