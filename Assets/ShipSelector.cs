using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShipSelector : MonoBehaviour {

    public Transform P1Text;
    public Transform P2Text;

    private bool p1axisInUse;
    private bool p2axisInUse;

    ShipOption[] options;
    int p1;
    int p2;

    ShipOption p1selected;
    ShipOption p2selected;

    private void Start()
    {
        options = new ShipOption[transform.childCount];
        for (int i = 0; i < options.Length; i++)
        {
            options[i] = transform.GetChild(i).GetComponent<ShipOption>();
        }

        p2 = 0;
        SelectShip(false, true);

        p1 = options.Length;
        SelectShip(true, true);
    }

    private void Update()
    {
        if (Input.GetAxisRaw("P1MovHor") > 0f)
        {
            if (p1axisInUse == false && p1selected == null)
            {
                p1axisInUse = true;
                SelectShip(true, true);
            }
        }
        if (Input.GetAxisRaw("P1MovHor") < 0f)
        {
            if (p1axisInUse == false && p1selected == null)
            {
                p1axisInUse = true;
                SelectShip(true, false);
            }
        }
        if (Input.GetAxisRaw("P1MovHor") == 0)
        {
            p1axisInUse = false;
        }
        if (Input.GetAxisRaw("P2MovHor") > 0f)
        {
            if (p2axisInUse == false && p2selected == null)
            {
                p2axisInUse = true;
                SelectShip(false, true);
            }
        }
        if (Input.GetAxisRaw("P2MovHor") < 0f)
        {
            if (p2axisInUse == false && p2selected == null)
            {
                p2axisInUse = true;
                SelectShip(false, false);
            }
        }
        if (Input.GetAxisRaw("P2MovHor") == 0)
        {
            p2axisInUse = false;
        }

        if (Input.GetButtonDown("P1Select"))
        {
            p1selected = options[p1];
            p1selected.image.color = new Color(0f, .5f, 0f);
            if (p2selected != null)
            {
                StartGame();
            }
        }
        else if (Input.GetButtonDown("P1Unselect"))
        {
            if (p1selected != null)
            {
                p1selected.image.color = new Color(1f, 1f, 1f);
                p1selected = null;
            }
        }
        if (Input.GetButtonDown("P2Select"))
        {
            p2selected = options[p2];
            p2selected.image.color = new Color(0f, .5f, 0f);
            if (p1selected != null)
            {
                StartGame();
            }
        }
        else if (Input.GetButtonDown("P2Unselect"))
        {
            if (p2selected != null)
            {
                p2selected.image.color = new Color(1f, 1f, 1f);
                p2selected = null;
            }
        }
    }

    void StartGame()
    {
        SelectedShip.Ship1 = p1selected.ship;
        SelectedShip.Ship2 = p2selected.ship;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void SelectShip(bool player, bool right)
    {
        if (player)
        {
            if (p1 < options.Length)
            {
                //Unselect previous one
                ShipOption shipOption = options[p1];
                shipOption.Select(false);
            }

            //Number
            int next;
            if (right)
                next = p1 + 1;
            else
                next = p1 - 1;

            if (next >= options.Length)
                next = 0;
            else if (next < 0)
                next = options.Length-1;

            //Check if free
            ShipOption option = options[next];
            while(option.isSelected)
            {
                if (right)
                    next += 1;
                else
                    next -= 1;

                if (next >= options.Length)
                    next = 0;
                else if (next < 0)
                    next = options.Length-1;

                option = options[next];
            }

            //Set
            p1 = next;
            option.Select(true);
            P1Text.position = new Vector2(option.transform.position.x, P1Text.position.y);
        }
        else
        {
            //Unselect previous one
            if (p2 < options.Length)
            {
                ShipOption shipOption = options[p2];
                shipOption.Select(false);
            }

            //Number
            int next;
            if (right)
                next = p2 + 1;
            else
                next = p2 - 1;

            if (next >= options.Length)
                next = 0;
            else if (next < 0)
                next = options.Length-1;

            //Check if free
            ShipOption option = options[next];
            while (option.isSelected)
            {
                if (right)
                    next += 1;
                else
                    next -= 1;

                if (next >= options.Length)
                    next = 0;
                else if (next < 0)
                    next = options.Length-1;

                option = options[next];
            }

            //Set
            p2 = next;
            option.Select(true);
            P2Text.position = new Vector2(option.transform.position.x, P2Text.position.y);
        }
    }
}
