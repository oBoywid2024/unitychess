using UnityEngine;

public class Chessman : MonoBehaviour
{
    // Object References
    public GameObject controller;
    public GameObject movePlate;

    // Positions (Default = Off-Board)
    private int xBoard = -1;
    private int yBoard = -1;

    // Player Color
    private string player;

    // Sprite References: All possible sprites that our Chessmen can be.
    public Sprite white_king, white_queen, white_rook, white_bishop, white_knight, white_pawn;
    public Sprite black_king, black_queen, black_rook, black_bishop, black_knight, black_pawn;

    // Puts digital piece on visual board and gives it the proper sprite
    public void Activate()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");

        // Take xBoard and yBoard positions and transform them to match with Board in-game
        SetCoords();

        // Absolute knightmare of a switch statement. Will not change.
        switch (this.name)
        {
            case "white_king": this.GetComponent<SpriteRenderer>().sprite = white_king; player = "white"; break;
            case "white_queen": this.GetComponent<SpriteRenderer>().sprite = white_queen; player = "white"; break;
            case "white_rook": this.GetComponent<SpriteRenderer>().sprite = white_rook; player = "white"; break;
            case "white_bishop": this.GetComponent<SpriteRenderer>().sprite = white_bishop; player = "white"; break;
            case "white_knight": this.GetComponent<SpriteRenderer>().sprite = white_knight; player = "white"; break;
            case "white_pawn": this.GetComponent<SpriteRenderer>().sprite = white_pawn; player = "white"; break;

            case "black_king": this.GetComponent<SpriteRenderer>().sprite = black_king; player = "black"; break;
            case "black_queen": this.GetComponent<SpriteRenderer>().sprite = black_queen; player = "black"; break;
            case "black_rook": this.GetComponent<SpriteRenderer>().sprite = black_rook; player = "black"; break;
            case "black_bishop": this.GetComponent<SpriteRenderer>().sprite = black_bishop; player = "black"; break;
            case "black_knight": this.GetComponent<SpriteRenderer>().sprite = black_knight; player = "black"; break;
            case "black_pawn": this.GetComponent<SpriteRenderer>().sprite = black_pawn; player = "black"; break;
        }
    }

    // Take xBoard and yBoard positions and transform them to match with Board in-game
    public void SetCoords()
    {
        float x = xBoard;
        float y = yBoard;

        x *= 0.66f; y *= 0.66f;
        x += -2.3f; y += -2.3f;

        this.transform.position = new Vector3(x, y, -1.0f);
    }

    // xBoard and yBoard Get-Sets
    public int GetXBoard()
    {
        return xBoard;
    }
    public int GetYBoard()
    {
        return yBoard;
    }
    public void SetXBoard(int x)
    {
        xBoard = x;
    }
    public void SetYBoard(int y)
    {
        yBoard = y;
    }

    // Built-In Unity Function | Executes upon lifting Left Click
    private void OnMouseUp()
    {
        // If the game is still going and it's our move, destroy old moveplates and make new ones at the current piece.
        if (!controller.GetComponent<Game>().IsGameOver() && controller.GetComponent<Game>().GetCurrentPlayer() == player)
        {
            DestroyMovePlates();
            InitiateMovePlates();
        }
    }

    // Destroys all moveplates
    public void DestroyMovePlates()
    {
        GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate");
        for (int i = 0; i < movePlates.Length; i++)
        {
            Destroy(movePlates[i]);
        }
    }

    // Spawns varrying moveplates depending on selected piece
    public void InitiateMovePlates()
    {
        switch (this.name)
        {
            case "white_king":
            case "black_king":
                SurroundMovePlate();
                break;
            case "white_queen":
            case "black_queen":
                LineMovePlate(0, 1); // Up
                LineMovePlate(1, 1); // Up-Right
                LineMovePlate(1, 0); // Right
                LineMovePlate(1, -1); // Down-Right
                LineMovePlate(0, -1); // Down
                LineMovePlate(-1, -1); // Down-Left
                LineMovePlate(-1, 0); // Left
                LineMovePlate(-1, 1); // Up-Left
                break;
            case "white_rook":
            case "black_rook":
                LineMovePlate(0, 1); // Up
                LineMovePlate(1, 0); // Right
                LineMovePlate(0, -1); // Down
                LineMovePlate(-1, 0); // Left
                break;
            case "white_bishop":
            case "black_bishop":
                LineMovePlate(1, 1); // Up-Right
                LineMovePlate(1, -1); // Down-Right
                LineMovePlate(-1, -1); // Down-Left
                LineMovePlate(-1, 1); // Up-Left
                break;
            case "white_knight":
            case "black_knight":
                LMovePlate(); // L-Shape Movement
                break;
            case "white_pawn":
                PawnMovePlate(xBoard, yBoard + 1);
                break;
            case "black_pawn":
                PawnMovePlate(xBoard, yBoard - 1);
                break;
        }
    }

    // Queen/Rook/Bishop | Spawns moveplates in line.
    public void LineMovePlate(int xIncrement, int yIncrement)
    {
        Game sc = controller.GetComponent<Game>();

        int x = xBoard + xIncrement;
        int y = yBoard + yIncrement;

        while (sc.PositionOnBoard(x, y) && sc.GetPosition(x, y) == null)
        {
            MovePlateSpawn(x, y);
            x += xIncrement;
            y += yIncrement;
        }

        if (sc.PositionOnBoard(x, y) && sc.GetPosition(x, y).GetComponent<Chessman>().player != player)
        {
            MovePlateAttackSpawn(x, y);
        }
    }

    // Knight | Spawns moveplates in L-Pattern
    public void LMovePlate()
    {
        PointMovePlate(xBoard - 1, yBoard - 2);
        PointMovePlate(xBoard - 1, yBoard + 2);
        PointMovePlate(xBoard + 1, yBoard - 2);
        PointMovePlate(xBoard + 1, yBoard + 2);
        PointMovePlate(xBoard - 2, yBoard - 1);
        PointMovePlate(xBoard - 2, yBoard + 1);
        PointMovePlate(xBoard + 2, yBoard - 1);
        PointMovePlate(xBoard + 2, yBoard + 1);
    }

    // King | Donut moveplates
    public void SurroundMovePlate()
    {
        PointMovePlate(xBoard + 1, yBoard + 1);
        PointMovePlate(xBoard + 0, yBoard + 1);
        PointMovePlate(xBoard - 1, yBoard + 1);
        PointMovePlate(xBoard + 1, yBoard + 0);
        PointMovePlate(xBoard + 0, yBoard + 0);
        PointMovePlate(xBoard - 1, yBoard + 0);
        PointMovePlate(xBoard + 1, yBoard - 1);
        PointMovePlate(xBoard + 0, yBoard - 1);
        PointMovePlate(xBoard - 1, yBoard - 1);
    }

    // General Single-Square Movement; Base for most other moveplates
    public void PointMovePlate(int x, int y)
    {
        Game sc = controller.GetComponent<Game>();
        if (sc.PositionOnBoard(x, y))
        {
            GameObject cp = sc.GetPosition(x, y);
            // No piece at desired location? Spawn MovePlate
            if (cp == null)
            {
                MovePlateSpawn(x, y);
            }
            // Piece in the way; Not friendly. EX-TER-MIN-ATE
            else if (cp.GetComponent<Chessman>().player != player)
            {
                MovePlateAttackSpawn(x, y);
            }
        }
    }

    // god fears even the idea of implementing pawn code
    // en passant will never be real
    public void PawnMovePlate(int x, int y)
    {
        Game sc = controller.GetComponent<Game>();
        if (sc.PositionOnBoard(x, y))
        {
            // Forward Movement
            if (sc.GetPosition(x, y) == null)
            {
                MovePlateSpawn(x, y);

                // Double-Moving
                if (player == "white" && yBoard == 1 && sc.GetPosition(x, y + 1) == null)
                {
                    MovePlateSpawn(x, y + 1);
                }
                else if (player == "black" && yBoard == 6 && sc.GetPosition(x, y - 1) == null)
                {
                    MovePlateSpawn(x, y - 1);
                }
            }

            // Diagonal-Right Attack
            if (sc.PositionOnBoard(x + 1, y) && sc.GetPosition(x + 1, y) != null && sc.GetPosition(x + 1, y).GetComponent<Chessman>().player != player)
            {
                MovePlateAttackSpawn(x + 1, y);
            }

            // Diagonal-Left Attack
            if (sc.PositionOnBoard(x - 1, y) && sc.GetPosition(x - 1, y) != null && sc.GetPosition(x - 1, y).GetComponent<Chessman>().player != player)
            {
                MovePlateAttackSpawn(x - 1, y);
            }
        }
    }

    // Spawn MovePlate at board coordinates
    public void MovePlateSpawn(int matrixX, int matrixY)
    {
        float x = matrixX;
        float y = matrixY;

        // Board -> Game
        x *= 0.66f; y *= 0.66f;
        x += -2.3f; y += -2.3f;

        GameObject mp = Instantiate(movePlate, new Vector3(x, y, -3.0f), Quaternion.identity);

        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.SetReference(gameObject);
        mpScript.SetCoords(matrixX, matrixY);
    }

    // Spawn Attacking MovePlate at board coordinates
    public void MovePlateAttackSpawn(int matrixX, int matrixY)
    {
        float x = matrixX;
        float y = matrixY;

        // Board -> Game
        x *= 0.66f; y *= 0.66f;
        x += -2.3f; y += -2.3f;

        GameObject mp = Instantiate(movePlate, new Vector3(x, y, -3.0f), Quaternion.identity);

        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.attack = true; // Attack mode
        mpScript.SetReference(gameObject);
        mpScript.SetCoords(matrixX, matrixY);
    }
}
