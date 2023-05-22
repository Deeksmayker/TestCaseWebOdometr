using UnityEngine;
using UnityEngine.UIElements;

public class MenuPanelUiController : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void HandleMenuButton()
    {
        if (menuPanel.activeSelf)
        {
            _animator.SetTrigger("Close");
        }
        else
        {
            _animator.SetTrigger("Open");
        }
    }
}
