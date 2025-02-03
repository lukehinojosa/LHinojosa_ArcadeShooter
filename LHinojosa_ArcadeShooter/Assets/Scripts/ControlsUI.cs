using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsUI : MonoBehaviour
{
    [SerializeField] private SpriteRenderer moveKeysSR;
    [SerializeField] private Sprite wasdSprite;
    [SerializeField] private Sprite arrowsSprite;
    private int _animationCycles = 2;
    private float _moveTime = 1f;
    private float _moveAmount = 0.1f;
    void Start()
    {
        StartCoroutine(Co_Animation());
    }

    private IEnumerator Co_Animation()
    {
        for (int i = 0; i < 2; i++)
        {
            yield return new WaitForSeconds(_moveTime);
            moveKeysSR.sprite = arrowsSprite;
            yield return new WaitForSeconds(_moveTime);
            transform.Translate(new Vector3(0, _moveAmount, 0));
            moveKeysSR.sprite = wasdSprite;
            yield return new WaitForSeconds(_moveTime);
            moveKeysSR.sprite = arrowsSprite;
            yield return new WaitForSeconds(_moveTime);
            transform.Translate(new Vector3(0, -_moveAmount, 0));
            moveKeysSR.sprite = wasdSprite;
        }
        
        Destroy(gameObject);
    }
}
