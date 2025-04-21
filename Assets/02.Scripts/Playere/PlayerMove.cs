using System.Collections;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEditor.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    // ��ǥ: wasd�� ������ ĳ���͸� �̵���Ű�� �ʹ�.
    // �ʿ� �Ӽ�:
    // - �̵��ӵ�
    public float OriginalSpeed = 7f;
    public float MoveSpeed = 7f;
    public float DashSpeed = 12f;
    public float RollingSpeed = 50f;
    public float climbSpeed = 3f;
    public float JumpPower = 5f;
    public bool _isJumping;
    public bool _isDubbleJumping;
    public bool _isLolling;
    public Slider SteminaSlider;
    public bool _isUsingStemina;
    private float _recoveryInterval = 1f;
    private float _recoveryTime;
    

    
    private const float GRAVITY = -0.98f;  // �߷�
    private float _yVelocity = 0f;        // �߷°��ӵ�
    private CharacterController _characterController;
    

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        SteminaSlider.value = 1f;
    }

    // ���� ����:
    // 1. Ű���� �Է��� �޴´�.
    // 2. �Է����κ��� ������ �����Ѵ�.
    // 3. ���⿡ ���� �÷��̾ �̵��Ѵ�.

    private void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(h, 0, v);
        dir = dir.normalized;

        // ���� ī�޶� �������� ������ ��ȯ�Ѵ�.
        dir = Camera.main.transform.TransformDirection(dir);
        // TransformDirection: ���� ������ ���͸� ���� ������ ���ͷ� �ٲ��ִ� �Լ�

        if (_characterController.isGrounded)
        //if(_charactorController.collisionFlags == CollisionFlags.Below)
        {
            _isJumping = false;
            _isDubbleJumping = false;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(Rolling());
        }





        // 3.���� ����
        if (Input.GetButtonDown("Jump"))
        {
            if (_isJumping == true && _isDubbleJumping == false)
            {
                _yVelocity = JumpPower;
                _isDubbleJumping = true;
            }
            else if(_isJumping == false)
            {
                _yVelocity = JumpPower;
                _isJumping = true;
            }
        }

        // ���׹̳� ���
        if(Input.GetButtonDown("Debug Multiplier"))
        {

            _isUsingStemina = true;
            if (_isUsingStemina)
            {
                MoveSpeed = DashSpeed;
                StartCoroutine(UsingStemina());
            }
        }

        // ���׹̳� ��� ����
        if(Input.GetButtonUp("Debug Multiplier"))
        {
            _isUsingStemina = false;
            MoveSpeed = OriginalSpeed;
        }

        if(_isUsingStemina == false)
        {
            _recoveryTime += Time.deltaTime;
                
            if(_recoveryTime >=_recoveryInterval)
            {
                _recoveryTime = 0;
                SteminaSlider.value += 0.05f;
            }
        }

        if ((_characterController.collisionFlags & CollisionFlags.Sides) != 0 && Input.GetKey(KeyCode.W))
        {
            // ��Ÿ�� ��: �߷� �����ϰ� ���� �̵�
            dir = Vector3.up * climbSpeed;
            _isUsingStemina = true;
            
        }
        else
        {
            // �Ϲ� �̵� or �߷� ����
            //dir.y += Physics.gravity.y * Time.deltaTime;
            // �߷� ����
            _yVelocity += GRAVITY * Time.deltaTime;
            dir.y = _yVelocity;
            _isUsingStemina = false;
        }

        //transform.position += dir * MoveSpeed * Time.deltaTime;
        _characterController.Move(dir * MoveSpeed * Time.deltaTime);
    }


    IEnumerator UsingStemina()
    {
        while (SteminaSlider.value >= 0f && _isUsingStemina)
        {
            SteminaSlider.value -= 0.01f;
            yield return new WaitForSeconds(0.1f);
            if (SteminaSlider.value <= 0f)
            {
                _isUsingStemina = false;
            }
        }
    }

    IEnumerator Rolling()
    {
        MoveSpeed = DashSpeed;
        SteminaSlider.value -= 0.3f;
        yield return new WaitForSeconds(0.5f);
        MoveSpeed = 7f;

    }

    

}
