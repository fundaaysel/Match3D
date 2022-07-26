using System.Collections;
using UnityEngine;

public class Dot : MonoBehaviour
{
    [Header("Board Variables")] public int column;
    public int row;

    public int previousColumn;
    public int previousRow;

    public int targetX;
    public int targetY;

    public bool isMatched = false;

    private BoardManager _boardManager;
    private GameObject otherDot;

    private Vector2 firstTouchPos;
    private Vector2 finalTouchPos;
    private Vector2 tempPos;

    public float swipeAngle = 0;
    public float swipeResist = 1f;

    private SpriteRenderer mySprite;

    private void Start()
    {
        _boardManager = FindObjectOfType<BoardManager>();
        // targetX = (int) transform.position.x;
        // targetY = (int) transform.position.y;

        // row = targetY;
        // column = targetX;
        mySprite = GetComponent<SpriteRenderer>();

        // previousColumn = column;
        // previousRow = row;
    }

    private void Update()
    {
        FindMatches();
        if (isMatched)
        {
            mySprite.color = new Color(1f, 1f, 1f, .2f);
        }

        targetX = column;
        targetY = row;

        if (Mathf.Abs(targetX - transform.position.x) > .1f)
        {
            //Move towards to target
            tempPos = new Vector2(targetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPos, .6f);
            if (_boardManager.allDots[column, row] != gameObject)
            {
                _boardManager.allDots[column, row] = this.gameObject;

            }
        }
        else
        {
            //directly set the pos
            tempPos = new Vector2(targetX, transform.position.y);
            transform.position = tempPos;
        }

        if (Mathf.Abs(targetY - transform.position.y) > .1f)
        {
            //Move towards to target
            tempPos = new Vector2(transform.position.x, targetY);
            transform.position = Vector2.Lerp(transform.position, tempPos, .6f);
            if (_boardManager.allDots[column, row] != gameObject)
            {
                _boardManager.allDots[column, row] = this.gameObject;

            }
        }
        else
        {
            //directly set the pos
            tempPos = new Vector2(transform.position.x, targetY);
            transform.position = tempPos;
        }
    }

    public IEnumerator CheckMoveCo()
    {
        yield return new WaitForSeconds(.5f);
        if (otherDot != null)
        {
            if (!isMatched && !otherDot.GetComponent<Dot>().isMatched)
            {
                otherDot.GetComponent<Dot>().row = row;
                otherDot.GetComponent<Dot>().column = column;

                row = previousRow;
                column = previousColumn;
            }else
            {
                _boardManager.DestroyMatches();
            }

            otherDot = null;
        }
        
    }

    private void OnMouseDown()
    {
        firstTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseUp()
    {
        finalTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        CalculateAngle();
    }

    private void CalculateAngle()
    {
        if (Mathf.Abs(finalTouchPos.y - firstTouchPos.y) > swipeResist ||
            Mathf.Abs(finalTouchPos.x - firstTouchPos.x) > swipeResist)
        {
            swipeAngle = Mathf.Atan2(finalTouchPos.y - firstTouchPos.y, finalTouchPos.x - firstTouchPos.x) * 180 /
                         Mathf.PI;
            MovePieces();
        }
    }

    private void MovePieces()
    {
        if (swipeAngle > -45 && swipeAngle <= 45 && column < _boardManager.width - 1)
        {
            //rightSwipe   
            otherDot = _boardManager.allDots[column + 1, row];
            previousColumn = column;
            previousRow = row;
            otherDot.GetComponent<Dot>().column -= 1;
            column += 1;
        }
        else if (swipeAngle > 45 && swipeAngle <= 135 && row < _boardManager.height - 1)
        {
            //upSwipe   
            otherDot = _boardManager.allDots[column, row + 1];
            previousColumn = column;
            previousRow = row;
            otherDot.GetComponent<Dot>().row -= 1;
            row += 1;
        }
        else if ((swipeAngle > 135 || swipeAngle <= -135) && column > 0)
        {
            //leftSwipe   
            otherDot = _boardManager.allDots[column - 1, row];
            previousColumn = column;
            previousRow = row;
            otherDot.GetComponent<Dot>().column += 1;
            column -= 1;
        }
        else if (swipeAngle < -45 && swipeAngle >= -135 && row > 0)
        {
            //downSwipe   
            otherDot = _boardManager.allDots[column, row - 1];
            previousColumn = column;
            previousRow = row;
            otherDot.GetComponent<Dot>().row += 1;
            row -= 1;
        }

        StartCoroutine(CheckMoveCo());
    }

    private void FindMatches()
    {
        if (column > 0 && column < _boardManager.width - 1)
        {
            GameObject leftDot1 = _boardManager.allDots[column - 1, row];
            GameObject rightDot1 = _boardManager.allDots[column + 1, row];
            if (leftDot1 != null && rightDot1 != null)
            {
                if (leftDot1.CompareTag(gameObject.tag) && rightDot1.CompareTag(gameObject.tag))
                {
                    leftDot1.GetComponent<Dot>().isMatched = true;
                    rightDot1.GetComponent<Dot>().isMatched = true;
                    isMatched = true;
                }
            }
        }

        if (row > 0 && row < _boardManager.height - 1)
        {
            GameObject upDot1 = _boardManager.allDots[column, row + 1];
            GameObject downDot1 = _boardManager.allDots[column, row - 1];
            if (upDot1 != null && downDot1 != null)
            {
                if (upDot1.CompareTag(gameObject.tag) && downDot1.CompareTag(gameObject.tag))
                {
                    upDot1.GetComponent<Dot>().isMatched = true;
                    downDot1.GetComponent<Dot>().isMatched = true;
                    isMatched = true;
                }
            }
        }
    }
}