using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
    public enum LevelType {
        TIMER,
        MOVES
    }

    public GridTwo grid;
    protected int currentScore;
    protected LevelType type;

    public float targetScore;
    public FloatValue storedEnemyHealth;
    public FloatValue storedNumMoves;

    public Text currentScoreText;
    public Text enemyHealthText;
    public Text outcomeText;

    public LevelType getLevelType() {
        return type;
    }

    // Start is called before the first frame update
    public void Start()
    {
        targetScore = storedEnemyHealth.RuntimeValue;
        outcomeText.enabled = false;
        if (enemyHealthText != null) {
            enemyHealthText.text = "Tiles Needed:\n" + storedEnemyHealth.RuntimeValue.ToString();
        }
        type = LevelType.MOVES;
        currentScoreText.text = "Tiles Collected:\n0";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GameWin() {
        Debug.Log("You win!");
        grid.GameOver();
        StartCoroutine(DisplayOutcome("VICTORY!"));
        
    }
    public virtual void GameLose() {
        Debug.Log("You lose.");
        StartCoroutine(DisplayOutcome("DEFEAT!"));
        grid.GameOver();
    }

    private IEnumerator DisplayOutcome(string text) {
        outcomeText.enabled = true;
        outcomeText.text = text;
        yield return new WaitForSeconds(2.5f);
        if (text == "VICTORY!") {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
        else {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
    public void OnMove() {
        storedNumMoves.RuntimeValue -= 1;

        if (storedNumMoves.RuntimeValue == 0) {
            if (currentScore >= targetScore) {
                GameWin();
            }
            else {
                GameLose();
            }
        }
    }
    public void OnPieceCleared(GamePiece piece) {
        currentScore += piece.score;
        currentScoreText.text = "Tiles Collected:\n" + currentScore.ToString();
        Debug.Log("Score: " + currentScore);
        if (currentScore >= targetScore) {
            GameWin();
        }
    }
}
