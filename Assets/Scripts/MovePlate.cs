using UnityEngine;

public class MovePlate : MonoBehaviour
{
    public GameObject controller;

    // Will reference piece being moved
    GameObject reference = null;

    // Positions on board (Not world)
    int matrixX;
    int matrixY;

    // False = Moving | True = Attacking
    public bool attack = false;

    public void Start()
    {
        if (attack)
        {
            //Change moveplate to red
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        }
    }

    public void OnMouseUp()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");

        if (attack)
        {
            GameObject cp = controller.GetComponent<Game>().GetPosition(matrixX, matrixY);

            // Who needs Check when you can blunder the king?
            if (cp.name == "white_king") controller.GetComponent<Game>().Winner("black");
            if (cp.name == "black_king") controller.GetComponent<Game>().Winner("white");

            /*
            A visitor? Hm, indeed, I have slept long enough.
            The kingdom of Heaven has long since forgotten my name,
            and I am EAGER to make them remember...
            However, the blood of Minos stains your hands,
            and I must admit, I'm curious about your skills, weapon.
            And so, before I tear down the cities, and CRUSH the armies of Heaven,
            you shall do as an appetizer.
            Come forth, child of man, and DIE.
            */
            Destroy(cp);
        }

        controller.GetComponent<Game>().SetPositionEmpty(
            reference.GetComponent<Chessman>().GetXBoard(), reference.GetComponent<Chessman>().GetYBoard()
            );

        reference.GetComponent<Chessman>().SetXBoard(matrixX);
        reference.GetComponent<Chessman>().SetYBoard(matrixY);
        reference.GetComponent<Chessman>().SetCoords();

        controller.GetComponent<Game>().SetPosition(reference);

        controller.GetComponent<Game>().NextTurn();

        reference.GetComponent<Chessman>().DestroyMovePlates();
    }

    public void SetCoords(int x, int y)
    {
        matrixX = x;
        matrixY = y;
    }

    public void SetReference(GameObject obj)
    {
        reference = obj;
    }

    public GameObject GetReference()
    {
        return reference;
    }
}
