using System.Collections.Generic;
using UnityEngine;
using BuildingSystem;

public class PlayerMove : MonoBehaviour
{
    public float moveTime = 0.2f;
    public Grid grid;
    [SerializeField]
    private ConstructionLayer constructionLayer;

    private bool isMoving = false;
    private float elapsedTime;
    private Vector3 origPos, targetPos;
    private Vector3 moveDirection;
    private Queue<Vector3> moveQueue;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        moveQueue = new Queue<Vector3>();
    }

    private void Update()
    {
        if (isMoving)
        {
            MovePlayer();
        }
        else
        {
            if (moveQueue.Count > 0)
            {
                moveDirection = moveQueue.Dequeue();
                StartMove();
            }
            else
            {
                HandleInput();
            }
        }
    }

    private void HandleInput()
    {
        float up = InputManager.Instance.playerInputActions.Game.Up.ReadValue<float>();
        float right = InputManager.Instance.playerInputActions.Game.Right.ReadValue<float>();

        if (up != 0 || right != 0)
        {
            Vector3 inputDirection = new Vector3(right, up, 0);
            if (moveQueue.Count == 0 && !isMoving)
            {
                moveDirection = inputDirection;
                StartMove();
            }
            else
            {
                moveQueue.Enqueue(inputDirection);
            }
        }
        else
        {
            SetIdleAnimation();
        }
    }

    private void SetRunAnimation()
    {
        if (moveDirection.x > 0)
        {
            animator.Play("Run R");
        }
        else if (moveDirection.x < 0)
        {
            animator.Play("Run L");
        }
        else if (moveDirection.y > 0)
        {
            animator.Play("Run U");
        }
        else if (moveDirection.y < 0)
        {
            animator.Play("Run D");
        }
    }

    private void SetIdleAnimation()
    {
        if (moveDirection.x > 0)
        {
            animator.Play("Idle R");
        }
        else if (moveDirection.x < 0)
        {
            animator.Play("Idle L");
        }
        else if (moveDirection.y > 0)
        {
            animator.Play("Idle Up");
        }
        else if (moveDirection.y < 0)
        {
            animator.Play("Idle Down");
        }
    }

    private void StartMove()
    {
        Vector3 potentialTargetPos = transform.position + moveDirection;
        potentialTargetPos = grid.CellToWorld(grid.WorldToCell(potentialTargetPos));

        // Check for diagonal movement
        if (moveDirection.x != 0 && moveDirection.y != 0)
        {
            Vector3 horizontalMove = new Vector3(moveDirection.x, 0, 0);
            Vector3 verticalMove = new Vector3(0, moveDirection.y, 0);

            Vector3 horizontalTargetPos = transform.position + horizontalMove;
            Vector3 verticalTargetPos = transform.position + verticalMove;

            horizontalTargetPos = grid.CellToWorld(grid.WorldToCell(horizontalTargetPos));
            verticalTargetPos = grid.CellToWorld(grid.WorldToCell(verticalTargetPos));

            bool canMoveHorizontally = constructionLayer.IsEmpty(horizontalTargetPos);
            bool canMoveVertically = constructionLayer.IsEmpty(verticalTargetPos);

            if (canMoveHorizontally && canMoveVertically)
            {
                // Both directions are clear, move diagonally
                isMoving = true;
                elapsedTime = 0;
                origPos = transform.position;
                targetPos = potentialTargetPos;
                SetRunAnimation();
            }
            else if (canMoveHorizontally)
            {
                // Only horizontal move is clear
                moveDirection = horizontalMove;
                StartMove();
            }
            else if (canMoveVertically)
            {
                // Only vertical move is clear
                moveDirection = verticalMove;
                StartMove();
            }
            else
            {
                // Neither direction is clear
                SetIdleAnimation();
            }
        }
        else
        {
            // Non-diagonal movement
            if (constructionLayer.IsEmpty(potentialTargetPos))
            {
                isMoving = true;
                elapsedTime = 0;
                origPos = transform.position;
                targetPos = potentialTargetPos;
                SetRunAnimation();
            }
            else
            {
                SetIdleAnimation();
            }
        }
    }

    private void MovePlayer()
    {
        elapsedTime += Time.deltaTime;
        float percentageComplete = elapsedTime / moveTime;
        transform.position = Vector3.Lerp(origPos, targetPos, percentageComplete);

        if (percentageComplete >= 1.0f)
        {
            isMoving = false;
            transform.position = targetPos;
        }
    }

    public void MoveTo(Vector3Int gridPosition)
    {
        Vector3Int currentGridPos = grid.WorldToCell(transform.position);

        Vector3Int diff = gridPosition - currentGridPos;

        if (diff.x != 0)
        {
            int stepX = diff.x > 0 ? 1 : -1;
            for (int x = 0; x != diff.x; x += stepX)
            {
                moveQueue.Enqueue(new Vector3(stepX, 0, 0));
            }
        }

        if (diff.y != 0)
        {
            int stepY = diff.y > 0 ? 1 : -1;
            for (int y = 0; y != diff.y; y += stepY)
            {
                moveQueue.Enqueue(new Vector3(0, stepY, 0));
            }
        }
    }
}
