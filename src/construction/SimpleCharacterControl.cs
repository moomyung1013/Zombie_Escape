using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SimpleCharacterControl : MonoBehaviour
{

    public float m_moveSpeed = 2;
    public float m_turnSpeed = 200;
    public float m_jumpForce = 4;
    public Animator m_animator;
    public Rigidbody m_rigidBody;
    public int escapeItemCount = 0;

    public Text countText; //열쇠 남은 개수 
    public Text warningText; //열쇠를 모으지 않았을 때 출구에 부딪혔을때
    public Text countdownText; //게임 시작할때 카운트다운
    public float LimitTime = 6000.0f; //제한 시간
    public Text txtTimer;
    private bool countTF = false;
    private double count = 3;
    private float countdown = 3;

    private float m_currentV = 0; //현재 가상 수직선 위치
    private float m_currentH = 0; //현재 가상 수평선 위치
    private readonly float m_interpolation = 10;
    private readonly float m_backwardRunScale = 0.66f;

    private bool m_wasGrounded;
    private Vector3 m_currentDirection = Vector3.zero;

    private float m_jumpTimeStamp = 0;
    private float m_minJumpInterval = 0.1f;

    private bool m_isGrounded;
    private List<Collider> m_collisions = new List<Collider>();

    private void OnCollisionEnter(Collision collision) //충돌 입장 처리
    {
        ContactPoint[] contactPoints = collision.contacts;
        for (int i = 0; i < contactPoints.Length; i++)
        {
            if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
            {
                if (!m_collisions.Contains(collision.collider))
                {
                    m_collisions.Add(collision.collider);
                }
                m_isGrounded = true;
            }

            if (collision.gameObject.CompareTag("EscapeZone"))
            {
                if (escapeItemCount < 2)
                {
                    warningText.text = "탈출 아이템을 다 모아야 합니다!";
                    Invoke("SetText", 2.0f);
                }
                else
                {
                    SceneManager.LoadScene("ConGameClearScene");
                }
            }
        }
    }
    private void SetText()
    {
        warningText.text = "";
    }
    private void OnCollisionStay(Collision collision) //충돌 중 처리
    {
        ContactPoint[] contactPoints = collision.contacts;
        bool validSurfaceNormal = false;
        for (int i = 0; i < contactPoints.Length; i++)
        {
            if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
            {
                validSurfaceNormal = true; break;
            }
        }

        if (validSurfaceNormal)
        {
            m_isGrounded = true;
            if (!m_collisions.Contains(collision.collider))
            {
                m_collisions.Add(collision.collider);
            }
        }
        else
        {
            if (m_collisions.Contains(collision.collider))
            {
                m_collisions.Remove(collision.collider);
            }
            if (m_collisions.Count == 0)
            {
                m_isGrounded = false;
            }
        }
    }

    private void OnCollisionExit(Collision collision) //충돌 나올 때 처리
    {
        if (m_collisions.Contains(collision.collider))
        {
            m_collisions.Remove(collision.collider);
        }
        if (m_collisions.Count == 0)
        {
            m_isGrounded = false;
        }
    }

    void Update()
    {//메인 캐릭터 업데이트
        m_animator.SetBool("Grounded", m_isGrounded);
        TankUpdate();
        m_wasGrounded = m_isGrounded;
        countText.text = "Escape Item : " + escapeItemCount.ToString();
    }

    private void TankUpdate() //tank모드
    {
        if (countTF == true)
        {
            float v = Input.GetAxis("Vertical"); //상하 이동 W키 : 0~1, S키: -1~0
            float h = Input.GetAxis("Horizontal"); //좌우 이동 D키: 0~1, A키: -1~0

            bool walk = Input.GetKey(KeyCode.LeftShift);

            if (v < 0)
            { //s일때 뒤로 걷는 속도 적용
                v *= m_backwardRunScale;
            }

            m_currentV = Mathf.Lerp(m_currentV, v, Time.deltaTime * m_interpolation); //상하 갱신
            m_currentH = Mathf.Lerp(m_currentH, h, Time.deltaTime * m_interpolation); //좌우 갱신

            transform.position += transform.forward * m_currentV * m_moveSpeed * Time.deltaTime; //상하 이동
            transform.position += transform.right * m_currentH * m_moveSpeed * Time.deltaTime; //좌우 이동
            if (Input.GetMouseButton(0)) //마우스 왼쪽 버튼을 눌렀을 때
            {
                float _yRotation = Input.GetAxisRaw("Mouse X");
                transform.Rotate(0, _yRotation * m_turnSpeed * Time.deltaTime, 0); //로테이션
            }

            m_animator.SetFloat("MoveSpeed", m_currentV); //애니메이션 갱신
            m_animator.SetFloat("MoveSpeed2", m_currentH); //애니메이션 갱신

            JumpingAndLanding();
            LimitTime -= Time.deltaTime; //프레임과 프레임 간의 시간
            txtTimer.text = "남은 시간 : " + Mathf.Round(LimitTime) + "초"; //round 소수점 이하 반올림
            if (Mathf.Round(LimitTime) == 0)
            {
                SceneManager.LoadScene("ConGameOverScene");
            }
        }
        if (countTF == false)
        {
            countdown -= Time.deltaTime; //프레임과 프레임 간의 시간
            countdownText.text = Mathf.Round(countdown).ToString(); //round 소수점 이하 반올림
            if (Mathf.Round(countdown) == 0)
            {
                countTF = true;
                countdownText.text = "";

            }
        }
    }


    private void JumpingAndLanding()
    {
        bool jumpCooldownOver = (Time.time - m_jumpTimeStamp) >= m_minJumpInterval;

        if (jumpCooldownOver && m_isGrounded && Input.GetKey(KeyCode.Space))
        {
            m_jumpTimeStamp = Time.time;
            m_rigidBody.AddForce(Vector3.up * m_jumpForce, ForceMode.Impulse);
        }

        if (!m_wasGrounded && m_isGrounded)
        {
            m_animator.SetTrigger("Land");
        }

        if (!m_isGrounded && m_wasGrounded)
        {
            m_animator.SetTrigger("Jump");
        }
    }
}
