using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public static Block Instance;

    public GameObject vfxOnSuccess;

    private Vector2 mousePos;

    public Color color;

    private Animator anim;

    private void Awake()
    {
        Instance = this;
        anim = GetComponent<Animator>();
    }

    private void OnMouseDown()
    {
        DrawWithMouse.Instance.StartLine(transform.position);
    }

    private void OnMouseDrag()
    {
        DrawWithMouse.Instance.Updateline();
    }

    private void OnMouseUp()
    {
        DrawWithMouse.Instance.ResetLine();
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.GetInstanceID() == gameObject.GetInstanceID()) return; 
            Block blockConnected = hit.collider.gameObject.GetComponent<Block>();
            if (color == blockConnected.color)
            {
                StartCoroutine(AnimCoroutine());
                blockConnected.StartCoroutine(blockConnected.AnimCoroutine());
            }
        }
    }



    public IEnumerator AnimCoroutine()
    {
        anim.SetTrigger("Scale");

        yield return new WaitForSeconds(1);

        GameObject explosion = Instantiate(vfxOnSuccess, transform.position, transform.rotation);
        Destroy(explosion, .75f);

        yield return new WaitForSeconds(1);

        RemoveFromList();
        Destroy(gameObject);

    }

    public void RemoveFromList()
    {
        GameManager.Instance.levels[GameManager.Instance.GetCurrentIndex()].blocks.Remove(this);
    }

}