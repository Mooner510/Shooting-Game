using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Vector3 _velocity;

    [SerializeField] private Camera _camera;
    [SerializeField] private float Speed = 10;
    [SerializeField] private GameObject Arrow;

    private void Start()
    {
        DataManager.Reset();
    }

    private void FixedUpdate()
    {
        transform.localPosition += _velocity;
        var pos = transform.localPosition;
        pos.y += 4;
        pos.z = -10;
        _camera.transform.localPosition = pos + _velocity * 6;
        _camera.orthographicSize = 5 + _velocity.sqrMagnitude * 100;
    }

    private Vector3 MoveTo(Vector3 current, Vector3 additive)
    {
        current.x += (additive.x - current.x) * Time.deltaTime * 2;
        current.y += (additive.y - current.y) * Time.deltaTime * 2;
        current.z += (additive.z - current.z) * Time.deltaTime * 2;
        return current;
    }

    private Vector3 Stop(Vector3 current)
    {
        current.x -= current.x * Time.deltaTime * 4;
        current.y -= current.y * Time.deltaTime * 4;
        current.z -= current.z * Time.deltaTime * 4;
        return current;
    }

    private float protectEndTime;

    public void Protect()
    {
        if(DataManager.Protect)
        {
            protectEndTime += 10;
            return;
        }
        StartCoroutine(Protecting());
    }

    private IEnumerator Protecting()
    {
        DataManager.Protect = true;
        protectEndTime = Time.realtimeSinceStartup + 10;
        for(var i = 0f; i <= protectEndTime; i += Time.deltaTime) yield return null;
        DataManager.Protect = false;
    }

    private float lastShoot;
    private float GetShootTime()
    {
        return DataManager.Weapon switch
        {
            2 => 0.2f,
            3 => 0.125f,
            4 => 0.1f,
            _ => 0.25f,
        };
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.F1)) transform.position += new Vector3(0, 50);
        if (Input.GetKey(KeyCode.F2)) DataManager.MaxWeapon();
        if (Input.GetKey(KeyCode.F4)) DataManager.SetHealth(100);
        if (Input.GetKey(KeyCode.F5)) DataManager.SetFuel(100);
        if (Input.GetKey(KeyCode.F6))
        {
            if(DataManager.Stage == 2) DataManager.FirstStage();
            else DataManager.NextStage();
        }

        if (DataManager.Weapon == 4 && Input.GetKey(KeyCode.Space) || Input.GetKeyDown(KeyCode.Space))
        {
            if (lastShoot <= Time.realtimeSinceStartup)
            {
                lastShoot = Time.realtimeSinceStartup + GetShootTime();
                var posVector = new Vector3(_velocity.x * Time.deltaTime, 0);
                var arrow = Instantiate(Arrow, transform.position + posVector, Quaternion.identity).GetComponent<Arrow>();
                arrow.speed = 8;
                arrow.UpdateVelocity(posVector);
                DataManager.RemoveFuel(0.1f);
            }
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            _velocity = MoveTo(_velocity, Vector3.left * Speed);
        }
        
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            _velocity = MoveTo(_velocity, Vector3.right * Speed);
        }
        
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            _velocity = MoveTo(_velocity, Vector3.up * Speed);
        }
        
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            _velocity = MoveTo(_velocity, Vector3.down * Speed * 0.5f);
        }

        if(!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.UpArrow)) {
            _velocity = Stop(_velocity);
        }

        DataManager.RemoveFuel(_velocity.sqrMagnitude / 5);
    }
}