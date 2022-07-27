using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SnakeController : MonoBehaviour
{
    [SerializeField]
    private float speed;
    private Vector2 _direction = Vector2.down;

    private Vector2 _areaLimit = new Vector2(12, 24);

    private List<Transform> snake = new List<Transform>();

    [SerializeField]
    private GameObject tailPrefab;
    [SerializeField] private TextMeshPro textScore;
    [SerializeField] private TextMeshPro textGameOver;

    private bool _grow;

    private int score;

    [SerializeField]
    private GameObject food;

    public int Score
    {
        get => score;
        set
        {
            score = value;
            textScore.text = score.ToString();

        }
    }

    IEnumerator Move()
    {

       
        while (true)
        {

            if (_grow)
            {
                Grow();
                _grow = false;
            }

            for (int i = snake.Count-1; i > 0; i--)
            {
                snake[i].position = snake[i - 1].position;
            }



            var position = transform.position;
            position += (Vector3)_direction;
            transform.position = position;
            yield return new WaitForSeconds(speed);
        }
    }
    private void Start()
    {
        textGameOver.enabled = false;
        Score = 0;
        snake.Add(transform);
        StartCoroutine(Move());
        ChangeFoodPosition();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(0);
        }
        if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && _direction != Vector2.right)
        {
            _direction = Vector2.left;
        }
        if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && _direction != Vector2.left)
        {
            _direction = Vector2.right;
        }
        if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && _direction != Vector2.up)
        {
            _direction = Vector2.down;
        }
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && _direction != Vector2.down)
        {
            _direction = Vector2.up;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Food"))
        {
            _grow = true;
        }
        if (collision.CompareTag("Wall"))
        {
            Dead();
        }
    }
    public void Grow()
    {
        Score++;
        var tail = Instantiate(tailPrefab);
        snake.Add(tail.transform);
        snake[snake.Count - 1].position = snake[snake.Count - 2].position;
        ChangeFoodPosition();
    }

    private void ChangeFoodPosition()
    {
        Vector2 newFoodPosition;
        do
        {
            var x = (int)Random.Range(1, _areaLimit.x);
            var y = (int)Random.Range(1, _areaLimit.y);
            newFoodPosition = new Vector2(x, y);

        } while (!canSpawnFood(newFoodPosition));

        food.transform.position = newFoodPosition;
    }

    private bool canSpawnFood(Vector2 newPosition)
    {
        foreach (var item in snake)
        {
            var x = Mathf.RoundToInt(item.position.x);
            var y = Mathf.RoundToInt(item.position.y);
            if (new Vector2(x,y) == newPosition)
                return false;
        }
        return true;
    }

    public void Dead()
    {
        textGameOver.enabled = true;
        StopAllCoroutines();
    }
}
