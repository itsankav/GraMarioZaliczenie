﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public Vector2 velocity;
    public float jumpVelocity;
    public float gravity;
    public LayerMask wallMask;
    public LayerMask floorMask;

    private bool walk, walk_left, walk_right, jump;
    public enum PlayerState
    {
        jumping,
        idle,
        walking
    }

    private PlayerState playerState = PlayerState.idle;

    private bool grounded = false;

    // Start is called before the first frame update
    void Start()
    {
        //Fall();
        Debug.Log("Gameover");
    }

    // Update is called once per frame
    void Update()
    {
        CheckPlayerInput();

        UpdatePlayerPosition();
    }

    void UpdatePlayerPosition()
    {

        Vector3 pos = transform.localPosition;
        Vector3 scale = transform.localScale;

        if (walk)
        {
            if (walk_left)
            {
                pos.x -= velocity.x * Time.deltaTime;

                scale.x = -1;
            }

            if (walk_right)
            {
                pos.x += velocity.x * Time.deltaTime;

                scale.x = 1;
            }

            pos = CheckWallRays(pos, scale.x);
        }

        if (jump && playerState != PlayerState.jumping)
        {
            playerState = PlayerState.jumping;

            velocity = new Vector2(velocity.x, jumpVelocity);

        }

        if (playerState == PlayerState.jumping)
        {
            pos.y += velocity.y * Time.deltaTime;

            velocity.y -= gravity * Time.deltaTime;
        }

        if(velocity.y <= 0)
        {
            pos = CheckFloorRays(pos);
        }

        if(velocity.y >= 0)
        {
            pos = CheckCeilingRays(pos);
        }

        transform.localPosition = pos;
        transform.localScale = scale;
    }

    void CheckPlayerInput()
    {
        bool input_left = Input.GetKey(KeyCode.LeftArrow);
        bool input_right = Input.GetKey(KeyCode.RightArrow);
        bool input_up = Input.GetKeyDown(KeyCode.UpArrow);

        walk = input_left || input_right;

        walk_left = input_left && !input_right;
        walk_right = !input_left && input_right;

        jump = input_up;
    }


    Vector3 CheckWallRays(Vector3 pos, float direction)
    {
        Vector2 originTop = new Vector2(pos.x + direction * .55f, pos.y + .7f - 0.2f);
        Vector2 originMiddle = new Vector2(pos.x + direction * .55f, pos.y);
        Vector2 originBottom = new Vector2(pos.x + direction * .55f, pos.y - 1f + 0.2f);

        RaycastHit2D wallTop = Physics2D.Raycast(originTop, new Vector2(direction, 0), velocity.x * Time.deltaTime, wallMask);
        RaycastHit2D wallMiddle = Physics2D.Raycast(originMiddle, new Vector2(direction, 0), velocity.x * Time.deltaTime, wallMask);
        RaycastHit2D wallBottom = Physics2D.Raycast(originBottom, new Vector2(direction, 0), velocity.x * Time.deltaTime, wallMask);

        if (wallTop.collider != null || wallMiddle.collider != null || wallBottom.collider != null)
        {
            RaycastHit2D hitRay = wallTop;


            if (wallTop)
            {
                hitRay = wallTop;
            }
            else if (wallMiddle)
            {
                hitRay = wallMiddle;
            }
            else if (wallBottom)
            {
                hitRay = wallBottom;
            }


            pos.x -= velocity.x * Time.deltaTime * direction;

            if (hitRay.collider.tag == "Spikes")
            {
                //GameOver();
                SceneManager.LoadScene("GameOver");
                Debug.Log("GameOver");
            }

        }

        return pos;
    }

    Vector3 CheckFloorRays(Vector3 pos)
    {
        Vector2 originLeft = new Vector2(pos.x - 0.5f + 0.2f, pos.y - 1.4f);
        Vector2 originMiddle = new Vector2(pos.x, pos.y - 1.4f);
        Vector2 originRight = new Vector2(pos.x +0.5f - 0.2f, pos.y - 1.4f);

        RaycastHit2D floorLeft = Physics2D.Raycast(originLeft, Vector2.down, velocity.y * Time.deltaTime, floorMask);
        RaycastHit2D floorMiddle = Physics2D.Raycast(originMiddle, Vector2.down, velocity.y * Time.deltaTime, floorMask);
        RaycastHit2D floorRight = Physics2D.Raycast(originRight, Vector2.down, velocity.y * Time.deltaTime, floorMask);

        if(floorLeft.collider != null || floorMiddle.collider != null || floorRight.collider != null)
        {
            RaycastHit2D hitRay = floorRight;

            if(floorLeft)
            {
                hitRay = floorLeft;
            }
            else if(floorMiddle)
            {
                hitRay = floorMiddle;
            }
            else if (floorRight)
            {
                hitRay = floorRight;
            }

            if (hitRay.collider.tag == "Spikes")
            {
                //GameOver();
                SceneManager.LoadScene("GameOver");
                Debug.Log("Gameover");
            }

            playerState = PlayerState.idle;

            grounded = true;

            velocity.y = 0;

            pos.y = hitRay.collider.bounds.center.y + hitRay.collider.bounds.size.y / 2 + 1;



        }
        else
        {
            if (playerState != PlayerState.jumping)
            {
                Fall();
            }
        }

        return pos;
    }

    Vector3 CheckCeilingRays(Vector3 pos)
    {
        Vector2 originLeft = new Vector2(pos.x - 0.5f + 0.2f, pos.y );
        Vector2 originMiddle = new Vector2(pos.x, pos.y);
        Vector2 originRight = new Vector2(pos.x + 0.5f - 0.2f, pos.y);

        RaycastHit2D ceilLeft = Physics2D.Raycast(originLeft, Vector2.up, velocity.y * Time.deltaTime, floorMask);
        RaycastHit2D ceilMiddle = Physics2D.Raycast(originMiddle, Vector2.up, velocity.y * Time.deltaTime, floorMask);
        RaycastHit2D ceilRight = Physics2D.Raycast(originRight, Vector2.up, velocity.y * Time.deltaTime, floorMask);

        if(ceilLeft.collider != null || ceilMiddle.collider != null || ceilRight.collider != null)
        {
            RaycastHit2D hitRay = ceilLeft;

            if (ceilLeft)
            {
                hitRay = ceilLeft;
            }
            else if (ceilMiddle)
            {
                hitRay = ceilMiddle;
            }
            else if (ceilRight)
            {
                hitRay = ceilRight;
            }

            if (hitRay.collider.tag == "Spikes")
            {
                //GameOver();
                SceneManager.LoadScene("GameOver");
                Debug.Log("Gameover");
            }

            pos.y = hitRay.collider.bounds.center.y - hitRay.collider.bounds.size.y / 2 - 1;

            Fall();
        }

        return pos;
    }

    void Fall()
    {
        velocity.y = 0;

        playerState = PlayerState.jumping;

        grounded = false;
    }
}
