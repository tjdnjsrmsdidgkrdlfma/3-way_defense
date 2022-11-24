using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class Player : MonoBehaviour
{
    int vertical_position; //현재 위치 위: 0 중간: 1 아래: 2
    int vertical_move_value; //움직여야 하는 거리
    bool go_up, go_down;
    bool vertical_moving;

    float horizontal_move;
    float horizontal_move_speed;

    void Awake()
    {
        vertical_position = 1;
        vertical_move_value = 3;
        go_up = false;
        go_down = false;
        vertical_moving = false;

        horizontal_move = 0;
        horizontal_move_speed = 0.2f;
    }

    void Update()
    {
        GetInput();
    }

    void GetInput()
    {
        if (Input.GetAxisRaw("Vertical") == 1)
        {
            go_up = true;
            go_down = false;
        }
        else if (Input.GetAxisRaw("Vertical") == -1)
        {
            go_up = false;
            go_down = true;
        }
        else
        {
            go_up = false;
            go_down = false;
        }

        horizontal_move = Input.GetAxisRaw("Horizontal");
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        transform.position = new Vector2(transform.position.x + (horizontal_move * horizontal_move_speed), transform.position.y);

        if (vertical_moving == true)
            return;

        if (go_up == false && go_down == false)
            return;

        if ((vertical_position == 0 && go_up == true) || (vertical_position == 2 && go_down == true)) //위에 있는데 위로 가려고 하는 경우
            return;

        StartCoroutine(MoveHorizontal());
    }

    IEnumerator MoveHorizontal()
    {
        vertical_moving = true;

        float delay_time = 0.01f;
        float total_delay_time = 0f;
        float max_delay_time = 0.1f;
        float move_value_ = vertical_move_value / (max_delay_time / delay_time); //delay_time동안 움직이게 될 거리
        float move_cooltime = 0.1f;
        int forward; //위로 갈지 아래로 갈지 부호 결정

        if (go_up == true)
        {
            vertical_position--;
            forward = 1;
        }
        else //if (go_down == true)
        {
            vertical_position++;
            forward = -1;
        }

        while (total_delay_time <= max_delay_time)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + (move_value_ * forward));
            total_delay_time += delay_time;
            yield return new WaitForSecondsRealtime(delay_time);
        }

        yield return new WaitForSecondsRealtime(move_cooltime);

        vertical_moving = false;
    }
}
