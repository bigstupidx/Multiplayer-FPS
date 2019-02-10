using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(ConfigurableJoint))]//if not there it will automatically add
[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float lookSensitivity = 3f;
    [SerializeField]
    private float thrusterForce = 100f;
    [SerializeField]
    private float thrusterFuelBurnSpeed = 1f;
    [SerializeField]
    private float thrusterFuelRegenSpeed = 0.3f;
    private float thrusterFillAmount = 1f;
    

    [SerializeField]
    private LayerMask dohovering;
    

    [Header("Joint Settings")]
    [SerializeField]
    private float jointSpring = 20f;
    [SerializeField]
    private float jointMaxForce = 10f;

    public ConfigurableJoint joint;
    private PlayerMotor motor;
    private Animator animator;

    public float GetThrusterFillAmount()
    {
        return thrusterFillAmount;
    }



    private void Start()
    {
        motor = GetComponent<PlayerMotor>();
        joint = GetComponent<ConfigurableJoint>();
        animator = GetComponent<Animator>();
        SetJointSettings(jointSpring);


    }

    void Update()
    {
        if (PauseMenu.isOn)
        {
            if (Cursor.lockState != CursorLockMode.None)
                Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            motor.CamRotation(0f);
            motor.Rotation(Vector3.zero);
            motor.Move(Vector3.zero);
            return;
        }

        if (Cursor.lockState != CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
           

        //setting target position for spring
        //this makes the physics act right whne it comes to
        //applying gravity whne flying over objects
        RaycastHit _hit;
        if(Physics.Raycast(transform.position,Vector3.down,out _hit,100f,dohovering))
        {
            joint.targetPosition = new Vector3(0f, -_hit.point.y+4, 0f);
        }
        else
        {
            joint.targetPosition = new Vector3(0f, 0f, 0f);
        }

        //for linear movement
        float _xMov = Input.GetAxis("Horizontal");//by raw here we mean we don't want any pre processing like lerping and all and we want full control
        float _zMov = Input.GetAxis("Vertical");

        Vector3 _movHorizontal = transform.right * _xMov;//local right and left
        Vector3 _moveVertical = transform.forward * _zMov;

        //final movement vector
        Vector3 _Velocity = (_movHorizontal + _moveVertical) * speed;

        motor.Move(_Velocity);

        //thruster animation
        animator.SetFloat("ForwardVelocity", _zMov);



        //for rotational movement
        float _yRot = Input.GetAxis("Mouse X");//on moving right and left in mouse/ touchpad

        Vector3 _rotation = new Vector3(0f, _yRot, 0f)*lookSensitivity;//here it goes on y axis

        motor.Rotation(_rotation);

        //for camera rotation of turning around
        float _xRot = Input.GetAxisRaw("Mouse Y");

        float _CamRotationX = _xRot*lookSensitivity;//here it goes on x axis
       

        
        motor.CamRotation(_CamRotationX);


        //for thruster
        Vector3 _thrusterForce = Vector3.zero;

        if (Input.GetButton("Jump")&&thrusterFillAmount>=0f)
        {
            thrusterFillAmount -= thrusterFuelBurnSpeed*Time.deltaTime;

            if(thrusterFillAmount >= 0.1f)
            {
                _thrusterForce = Vector3.up * thrusterForce;
                SetJointSettings(0f);
            }
            
           

        }
        else
        {
            thrusterFillAmount += thrusterFuelRegenSpeed*Time.deltaTime;
            SetJointSettings(jointSpring);
        }
        thrusterFillAmount =Mathf.Clamp(thrusterFillAmount, 0f, 1f);

        motor.ApplyThruster(_thrusterForce);
        

    }

    private void SetJointSettings(float _jointSpring)
    {
        joint.yDrive = new JointDrive
        {
            positionSpring = _jointSpring,
            maximumForce = jointMaxForce

        };//type of struct
    }

    


}
