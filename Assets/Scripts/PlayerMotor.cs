using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour {

    [SerializeField]
    private Camera cam;

    public Rigidbody rb;
    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private float camRotationX = 0f;
    private Vector3 thrusterForce = Vector3.zero;
    private float cameraRotationLimit = 85f;
    private float cameraCurrentRotationX = 0f;

    


	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
		
	}

    public void Move(Vector3 _velocity)
    {
        velocity = _velocity;
        
    }

    public void Rotation(Vector3 _rotation)
    {
        rotation = _rotation;
    }


    public void CamRotation(float _CamRotationX)
    {
          camRotationX = _CamRotationX;
    }

    public void ApplyThruster(Vector3 _thrusterForce)
    {
        thrusterForce = _thrusterForce;
    }

    private void Update()
    {
        if (PauseMenu.isOn)
            return;

        PerformMovement();
        PerformRotation();

       
    }

    void PerformMovement()
    {
        if (velocity != Vector3.zero)
        {
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }
        if (thrusterForce != Vector3.zero)
        {
            rb.AddForce(thrusterForce*Time.deltaTime,ForceMode.Acceleration);//so that it becomes independent of mass
        }
    }

    void PerformRotation()
    {

       
        if(rotation != Vector3.zero)//if we have made some rotation
        {
            rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
        }

        if (cam != null)
        {
            cameraCurrentRotationX -= camRotationX;
            cameraCurrentRotationX = Mathf.Clamp(cameraCurrentRotationX, -cameraRotationLimit, cameraRotationLimit);
            cam.transform.localEulerAngles = new Vector3(cameraCurrentRotationX, 0f, 0f);
        }
    }

}
