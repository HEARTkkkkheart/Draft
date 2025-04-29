using Framework;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CapsuleCollider2D))]
public class PlayerBetterJump2D : MonoBehaviour
{
    [SerializeField] private float _jumpDuration = 0.3f;
    [SerializeField] private float _jumpSpeed = 4f;
    [SerializeField] private KeyCode _jumpKey = KeyCode.Space;
    public bool CanJump;

    private Rigidbody2D _rb;
    
    private float _jumpTimer;
    private bool _isJumping;
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        
        InputMgr.Instance.AddGetKeyDownListener(_jumpKey);
        InputMgr.Instance.AddGetKeyStayListener(_jumpKey);
        InputMgr.Instance.AddGetKeyUpListener(_jumpKey);

        EventCenter.Instance.Register<GetKeyDown>(OnGetKeyDown)
            .UnRegisterWhenGameObjectOnDestroy(gameObject);
        EventCenter.Instance.Register<GetKeyStay>(OnGetKeyStay)
            .UnRegisterWhenGameObjectOnDestroy(gameObject);
        EventCenter.Instance.Register<GetKeyUp>(OnGetKeyUp)
            .UnRegisterWhenGameObjectOnDestroy(gameObject);
    }

    private void OnGetKeyDown(GetKeyDown getKeyDown)
    {
        if (getKeyDown.Key == _jumpKey && CanJump)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _jumpSpeed);
            _isJumping = true;
        }
    }
    
    private void OnGetKeyStay(GetKeyStay getKeyStay)
    {
        if (getKeyStay.Key == _jumpKey && _isJumping)
        {
            if (_jumpTimer >= _jumpDuration) // 达到最长持续时间 则返回
                return;

            _jumpTimer += Time.deltaTime;
            _rb.velocity = new Vector2(_rb.velocity.x, _jumpSpeed);   
        }
    }

    private void OnGetKeyUp(GetKeyUp getKeyUp)
    {
        if (getKeyUp.Key == _jumpKey && _isJumping)
        {
            _isJumping = false;
            _jumpTimer = 0f;
        }
    }
}
