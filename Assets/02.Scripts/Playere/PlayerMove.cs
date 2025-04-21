using System.Collections;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEditor.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    // 목표: wasd를 누르면 캐릭터를 이동시키고 싶다.
    // 필요 속성:
    // - 이동속도
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
    

    
    private const float GRAVITY = -0.98f;  // 중력
    private float _yVelocity = 0f;        // 중력가속도
    private CharacterController _characterController;
    

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        SteminaSlider.value = 1f;
    }

    // 구현 순서:
    // 1. 키보드 입력을 받는다.
    // 2. 입력으로부터 방향을 설정한다.
    // 3. 방향에 따라 플레이어를 이동한다.

    private void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(h, 0, v);
        dir = dir.normalized;

        // 메인 카메라를 기준으로 방향을 변환한다.
        dir = Camera.main.transform.TransformDirection(dir);
        // TransformDirection: 지역 공간의 벡터를 월드 공간의 벡터로 바꿔주는 함수

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





        // 3.점프 적용
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

        // 스테미나 사용
        if(Input.GetButtonDown("Debug Multiplier"))
        {

            _isUsingStemina = true;
            if (_isUsingStemina)
            {
                MoveSpeed = DashSpeed;
                StartCoroutine(UsingStemina());
            }
        }

        // 스테미나 사용 끝남
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
            // 벽타기 중: 중력 제거하고 위로 이동
            dir = Vector3.up * climbSpeed;
            _isUsingStemina = true;
            
        }
        else
        {
            // 일반 이동 or 중력 적용
            //dir.y += Physics.gravity.y * Time.deltaTime;
            // 중력 적용
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
