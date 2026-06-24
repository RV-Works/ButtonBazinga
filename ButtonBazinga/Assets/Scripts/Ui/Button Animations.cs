using DG.Tweening;
using UnityEngine;

public class ButtonAnimations : MonoBehaviour
{
    private Vector3 startScale;

    [SerializeField] private float increaseSize;

    [SerializeField] private float increaseSizeSpeed;

    private void Start()
    {
        startScale = transform.localScale;
    }
    public void StartMenuButtonEnter()
    {
        gameObject.transform.DOScale(new Vector3(startScale.x + increaseSize, startScale.y + increaseSize, startScale.z + increaseSize), increaseSizeSpeed);
    }

    public void StartMenuButtonExit()
    {
        gameObject.transform.DOScale(new Vector3(startScale.x, startScale.y, startScale.z), increaseSizeSpeed);
    }
}
