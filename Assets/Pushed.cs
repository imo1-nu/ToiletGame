using UnityEngine;
using UnityEngine.UI;

public class Pushed : MonoBehaviour
{
    public Button[] buttons = new Button[9]; // 9個のボタンをアタッチするための配列
    public Text[] texts = new Text[9]; // 9個のテキストをアタッチするための配列
    public Button dai;
    public Button syo;
    public Button cancel;
    private ComputerTurn computerTurn;
    int idx;

    int[] grundy = new int[] {0, 0, 1, 1, 0, 2, 1, 0, 0, 1, 1};

    void Start()
    {
        // 各ボタンにクリックイベントを追加
        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i] != null)
            {
                int index = i; // ローカル変数を使用してクロージャ問題を回避
                buttons[i].onClick.AddListener(() => OnButtonClick(index));
            }
        }

        int XOR_Sum = 0;
        while (XOR_Sum == 0)
        {
            for (int i = 0; i < texts.Length; i++)
            {
                int randomNumber = Random.Range(5, 11);
                XOR_Sum ^= grundy[randomNumber];
                if (texts[i] != null)
                {
                    texts[i].text = randomNumber.ToString();
                }
            }
        }
        Debug.Log("XOR_Sum: " + XOR_Sum);

        if (dai != null)
        {
            dai.onClick.AddListener(OnDaiButtonClick);
        }

        if (syo != null)
        {
            syo.onClick.AddListener(OnSyoButtonClick);
        }

        if (cancel != null)
        {
            cancel.onClick.AddListener(OnCancelButtonClick);
        }

        // 初期状態でdai, syo, cancelのボタンを非表示にする
        SetButtonsActive(false);

        // 同じシーンにあるComputerTurnコンポーネントを取得
        computerTurn = Object.FindFirstObjectByType<ComputerTurn>();
    }

    void OnButtonClick(int index)
    {
        // 各ボタンがクリックされたときの処理
        Debug.Log("Button " + index + " clicked!");
        idx = index;

        // 3個のボタンを表示にする
        SetButtonsActive(true);
    }

    void OnDaiButtonClick()
    {
        // daiボタンがクリックされたときの処理
        Debug.Log("Dai button clicked!");

        // 選択されたボタンの数字を5減らす
        if (texts[idx] != null)
        {
            int currentNumber;
            if (int.TryParse(texts[idx].text, out currentNumber))
            {
                currentNumber -= 5;
                if (currentNumber < 0) return;
                texts[idx].text = currentNumber.ToString();
            }
        }

        // dai, syo, cancelのボタンを非表示にする
        SetButtonsActive(false);

        // ComputerTurnのCalculate関数を呼び出す
        if (computerTurn != null)
        {
            computerTurn.Calculate();
        }
    }

    void OnSyoButtonClick()
    {
        // syoボタンがクリックされたときの処理
        Debug.Log("Syo button clicked!");

        // 選択されたボタンの数字を2減らす
        if (texts[idx] != null)
        {
            int currentNumber;
            if (int.TryParse(texts[idx].text, out currentNumber))
            {
                currentNumber -= 2;
                if (currentNumber < 0) return;
                texts[idx].text = currentNumber.ToString();
            }
        }

        // dai, syo, cancelのボタンを非表示にする
        SetButtonsActive(false);

        // ComputerTurnのCalculate関数を呼び出す
        if (computerTurn != null)
        {
            computerTurn.Calculate();
        }
    }

    void OnCancelButtonClick()
    {
        // cancelボタンがクリックされたときの処理
        Debug.Log("Cancel button clicked!");

        // dai, syo, cancelのボタンを非表示にする
        SetButtonsActive(false);
    }

    void SetButtonsActive(bool isActive)
    {
        if (dai != null)
        {
            dai.gameObject.SetActive(isActive);
        }
        if (syo != null)
        {
            syo.gameObject.SetActive(isActive);
        }
        if (cancel != null)
        {
            cancel.gameObject.SetActive(isActive);
        }
    }
}