using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class ComputerTurn : MonoBehaviour
{
    public Text[] texts = new Text[9]; // 9個のテキストをアタッチするための配列
    public Text explainText;
    public Button[] buttons = new Button[9]; // 9個のボタンをアタッチするための配列

    int[] grundy = new int[] {0, 0, 1, 1, 0, 2, 1, 0, 0, 1, 1};

    void Start()
    {
        // explainTextを初期化
        explainText.text = "Your turn";
    }

    public void Calculate()
    {
        if (IsFinished())
        {
            explainText.text = "You win";
            Debug.Log("Game is finished");
            
            // 5秒後にSceneをリロード
            Invoke("ReloadScene", 5.0f);
            return;
        }

        StartCoroutine(ComputerTurnRoutine());
    }

    IEnumerator ComputerTurnRoutine()
    {
        // プレイヤーの操作を無効にする
        SetButtonsInteractable(false);

        explainText.text = "Computer's turn";
        yield return new WaitForSeconds(1.0f);

        // コンピュータのターンの処理
        Debug.Log("Computer's turn");

        int[] numbers = new int[9];

        for (int i = 0; i < texts.Length; i++)
        {
            if (texts[i] != null)
            {
                int number;
                if (int.TryParse(texts[i].text, out number))
                {
                    numbers[i] = number;
                }
                else
                {
                    Debug.LogWarning("Text " + i + " is not a valid number.");
                }
            }
        }

        // numbers配列に9個のテキストの数字が代入されている
        Debug.Log("Numbers: " + string.Join(", ", numbers));

        int XOR_Sum = 0;
        for (int i = 0; i < numbers.Length; i++)
        {
            XOR_Sum ^= grundy[numbers[i]];
        }

        Debug.Log("XOR Sum: " + XOR_Sum);

        if (XOR_Sum == 0)
        {
            // コンピュータが勝てる手がない場合
            Debug.Log("You can win");

            // 実行可能な手の中の集合を計算
            int[] possibleMoves = new int[18];
            int possibleMovesCount = 0;
            for (int i = 0; i < numbers.Length; i++)
            {
                if (numbers[i] >= 2)
                {
                    possibleMoves[possibleMovesCount] = i;
                    possibleMovesCount++;
                }
                if (numbers[i] >= 5)
                {
                    possibleMoves[possibleMovesCount] = i + 9;
                    possibleMovesCount++;
                }
            }

            // 実行可能な手の中からランダムに選択
            int randomIndex = Random.Range(0, possibleMovesCount);
            int move = possibleMoves[randomIndex];

            if (move < 9)
            {
                // 2減らす
                numbers[move] -= 2;
            }
            else
            {
                // 5減らす
                numbers[move - 9] -= 5;
            }
        }
        else
        {
            // コンピュータが勝てる手を探す
            Debug.Log("You can't win");

            // XOR_Sumが0になるような手を探す
            for (int i = 0; i < numbers.Length; i++)
            {
                if (numbers[i] >= 2)
                {
                    if ((XOR_Sum ^ grundy[numbers[i]] ^ grundy[numbers[i] - 2]) == 0)
                    {
                        numbers[i] -= 2;
                        break;
                    }
                    if (numbers[i] >= 5)
                    {
                        if ((XOR_Sum ^ grundy[numbers[i]] ^ grundy[numbers[i] - 5]) == 0)
                        {
                            numbers[i] -= 5;
                            break;
                        }
                    }
                }
            }
        }

        // コンピュータの操作後のXOR_Sumを計算
        XOR_Sum = 0;
        for (int i = 0; i < numbers.Length; i++)
        {
            XOR_Sum ^= grundy[numbers[i]];
        }
        Debug.Log("XOR Sum after computer's turn: " + XOR_Sum);

        // テキストの数字を更新
        for (int i = 0; i < texts.Length; i++)
        {
            if (texts[i] != null)
            {
                if (texts[i].text != numbers[i].ToString())
                {
                    texts[i].text = numbers[i].ToString();
                    // 3秒間赤色に変更して待機してから元の色に戻す
                    StartCoroutine(ChangeColor(i));
                    break;
                }
            }
        }

        if (IsFinished())
        {
            explainText.text = "Computer win";
            Debug.Log("Game is finished!");
            
            // 5秒後にSceneをリロード
            Invoke("ReloadScene", 5.0f);
        }
        else
        {
            // プレイヤーの操作を有効にする
            yield return new WaitForSeconds(2.0f);
            // explainTextを更新
            explainText.text = "Your turn";
            SetButtonsInteractable(true);
        }
    }

    public bool IsFinished()
    {
        // ゲームが終了したかどうかを返す
        for (int i = 0; i < texts.Length; i++)
        {
            if (texts[i] != null)
            {
                int number;
                if (int.TryParse(texts[i].text, out number))
                {
                    if (number >= 2)
                    {
                        return false;
                    }
                }
            }
        }

        return true;
    }

    void ReloadScene()
    {
        // 現在のSceneをリロード
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator ChangeColor(int index)
    {
        // テキストの色を2秒間赤色に変更して待機してから元の色に戻す
        if (texts[index] != null)
        {
            texts[index].color = Color.red;
            yield return new WaitForSeconds(2.0f);
            texts[index].color = Color.black;
        }
    }

    void SetButtonsInteractable(bool interactable)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i] != null)
            {
                buttons[i].interactable = interactable;
            }
        }
    }
}