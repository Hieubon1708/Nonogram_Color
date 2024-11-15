using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public GameObject home;
    public GameObject gamePlay;
    public int step = 1;
    public string[] content;
    public GameObject panelStart;
    public GameObject popupContent;
    public TextMeshProUGUI textContent;
    public TextMeshProUGUI textStep;
    public RectTransform[] lines;
    public RectTransform maskLine;
    public RectTransform buttonControl;
    public RectTransform hand;
    public Animation handAni;
    public Vector2 startHand;
    public Vector2 targetHand;
    public float timeTarget;

    public RectTransform[] line1;
    public RectTransform[] line2;
    public RectTransform[] line3;
    public RectTransform[] line4;
    public RectTransform[] line5;

    public Box[] boxLine1;
    public Box[] boxLine2;
    public Box[] boxLine3;
    public Box[] boxLine4;
    public Box[] boxLine5;

    public GameObject[] lights;

    public Box[] boxCheck;

    public ButtonSelector[] buttonSelectors;
    public Animation[] handButtonAni;
    public Image[] handButtonImage;

    public GameObject layerCover;

    public List<GameObject> boxPassed = new List<GameObject>();
    public List<GameObject> boxPassedFade = new List<GameObject>();

    bool isDrag;
    bool isStepOk = true;
    public int totalSelect;
    public int indexBox;

    public void Start()
    {
        Step();
    }

    public void StartTutorial()
    {
        layerCover.SetActive(true);
        panelStart.SetActive(false);
        Step();
    }

    public void StartHand()
    {
        hand.position = startHand;
        handAni.Play("TutorialHandShow");
    }

    float GetSameBoxPassed(ref int isSameRow)
    {
        float value = 0f;
        if (boxPassed[0].transform.position.x == boxPassed[1].transform.position.x)
        {
            value = boxPassed[0].transform.position.x;
            isSameRow = 1;
        }
        if (boxPassed[0].transform.position.y == boxPassed[1].transform.position.y)
        {
            value = boxPassed[0].transform.position.y;
            isSameRow = -1;
        }
        return value;
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDrag = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDrag = false;
            boxPassed.Clear();
        }

        if (isDrag)
        {
            Vector2 mousePosition = Input.mousePosition;
            if (boxPassed.Count >= 2)
            {
                int isSameRow = 0;
                float value = GetSameBoxPassed(ref isSameRow);

                float x = isSameRow == 1 ? value : Input.mousePosition.x;
                float y = isSameRow == -1 ? value : Input.mousePosition.y;

                mousePosition = new Vector2(x, y);
            }
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = mousePosition;
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

            for (int i = 0; i < results.Count; i++)
            {
                GameObject e = results[i].gameObject;
                if (e.CompareTag("Box") && !boxPassed.Contains(e))
                {
                    boxPassed.Add(e);
                    Box box = GetBox(e);
                    if (box.mainHex == "" || box.isVisible) return;
                    BoxSelect(box);
                }
            }
        }
    }

    void CheckBox()
    {
        Vector2 mousePosition = hand.position;
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = mousePosition;
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        for (int i = 0; i < results.Count; i++)
        {
            GameObject e = results[i].gameObject;
            if (e.CompareTag("Box") && !boxPassedFade.Contains(e) && IsContains(e))
            {
                boxPassedFade.Add(e);
                Box box = GetBox(e);
                if (box.mainHex == "" || box.isVisible) return;
                Color color;
                if (GameController.instance.ColorConvert(box.mainHex, out color))
                {
                    color.a = 0.85f;
                    box.image.color = color;
                }
                else
                {
                    Debug.LogWarning(e + " " + box.mainHex);
                }
            }
        }
    }

    void ResetBoxHover()
    {
        for (int i = 0; i < boxCheck.Length; i++)
        {
            if (!boxCheck[i].isVisible) boxCheck[i].image.DOColor(Vector4.one, 0.1f).SetEase(Ease.Linear).SetUpdate(true);
        }
    }

    bool IsContains(GameObject box)
    {
        for (int i = 0; i < boxCheck.Length; i++)
        {
            if (boxCheck[i].gameObject == box) return true;
        }
        return false;
    }

    Box GetBox(GameObject box)
    {
        for (int i = 0; i < boxCheck.Length; i++)
        {
            if (boxCheck[i].gameObject == box) return boxCheck[i];
        }
        return null;
    }

    public void BoxSelect(Box box)
    {
        DoKill();
        ResetBoxHover();
        hand.gameObject.SetActive(false);
        Color color;
        box.isVisible = true;
        if (GameController.instance.ColorConvert(box.mainHex, out color))
        {
            box.image.DOColor(color, 0.1f);
        }
        else
        {
            Debug.LogWarning(box + " " + box.mainHex);
        }
        float time = 2f;
        if (GetAmountBoxSelect() == totalSelect)
        {
            isStepOk = true;
            time = 0f;
            step++;
        }
        DOVirtual.DelayedCall(time, delegate
        {
            isStepOk = false;
            Step();
        });
    }

    public void MoveHand()
    {
        if (indexBox < boxCheck.Length)
        {
            bool isContinue = false;
            hand.DOMoveX(boxCheck[indexBox].transform.position.x, 1f / 5f).SetEase(Ease.Linear).OnComplete(delegate
            {
                if (boxCheck[indexBox].mainHex == "#FFFFFF" || boxCheck[indexBox].isVisible)
                {
                    indexBox += 2;
                    hand.DOKill();
                    handAni.Play("TutorialHandUp");
                }
                else
                {
                    MoveHand();
                    isContinue = true;
                }
            }).OnUpdate(delegate
            {
                CheckBox();
            });
            if (!isContinue) hand.DORotate(new Vector3(0, 0, -13.4f), 1f).SetEase(Ease.Linear);

        }
        else
        {
            boxPassedFade.Clear();
            ResetBoxHover();
            hand.DOKill();
            handAni.Play("TutorialHandHide");
        }
    }

    public void HandUp()
    {
        hand.DORotate(new Vector3(0, 0, 31f), 0.4f).SetEase(Ease.Linear);
        hand.DOMoveX(boxCheck[indexBox].transform.position.x, 0.4f).SetEase(Ease.Linear).OnComplete(delegate
        {
            hand.DOKill();
            handAni.Play("TutorialHandDown");
        });
    }

    public void HandDown()
    {
        hand.DOMoveX(boxCheck[indexBox].transform.position.x, 1f / line1.Length).SetEase(Ease.Linear).OnComplete(delegate
        {
            indexBox++;
            MoveHand();
        });
    }

    public void ButtonSelect(int index)
    {
        isStepOk = true;
        DoKill();
        if (step == 1)
        {
            handButtonImage[1].gameObject.SetActive(false);
            lights[0].SetActive(true);
            UIController.instance.ButtonSelect(buttonSelectors, buttonSelectors[index], 0.15f, 0.25f);
            layerCover.SetActive(true);
        }
        step++;
        Step();
    }

    int GetAmountBoxSelect()
    {
        int amount = 0;
        for (int i = 0; i < boxCheck.Length; i++)
        {
            if (boxCheck[i].isVisible) amount++;
        }
        return amount;
    }

    public void Step()
    {
        if (step <= 12)
        {
            textContent.text = content[step - 1];
            textStep.text = "step " + step + "/12";

            if (step == 1)
            {
                DOVirtual.DelayedCall(1f, delegate
                {
                    layerCover.SetActive(false);
                    handButtonImage[1].gameObject.SetActive(true);
                    handButtonImage[1].DOFade(1f, 0.25f).SetEase(Ease.Linear);
                    handButtonAni[1].Play();
                });
            }

            if (step == 2)
            {
                DOVirtual.DelayedCall(1f, delegate
                {
                    indexBox = 1;
                    totalSelect = 5;
                    boxCheck = boxLine1;
                    layerCover.SetActive(false);
                    hand.gameObject.SetActive(true);
                    startHand = line1[0].position;
                    targetHand = line1[4].position;
                    StartHand();
                });
            }

            if (step == 3)
            {
                MoveRow(1);
                MoveLine();
            }
            if (step == 4)
            {
                MoveRow(2);
                MoveLine();
            }
            if (step == 5)
            {
                MoveRow(3);
                MoveLine();
            }
            if (step == 9)
            {
                MoveRow(4);
                MoveLine();

                DOVirtual.DelayedCall(0.25f, delegate
                {
                    MoveRow(0);
                    maskLine.DOAnchorPosY(maskLine.anchoredPosition.y - 170, 0.25f).SetEase(Ease.Linear);
                });
            }

            if (!isStepOk) return;
            popupContent.SetActive(false);
            popupContent.SetActive(true);
        }
    }

    void MoveRow(int index)
    {
        for (int i = index; i < lines.Length; i++)
        {
            lines[i].DOAnchorPosY(lines[i].anchoredPosition.y - 170, 0.25f).SetEase(Ease.Linear);
        }
        buttonControl.DOAnchorPosY(buttonControl.anchoredPosition.y - 170, 0.25f).SetEase(Ease.Linear);
    }
    void MoveLine()
    {
        maskLine.DOSizeDelta(new Vector2(-230, maskLine.sizeDelta.y + 170), 0.25f).SetEase(Ease.Linear);
    }

    void DoKill()
    {
        for (int i = 0; i < lines.Length; i++)
        {
            lines[i].DOKill();

        }
        maskLine.DOKill();
        hand.DOKill();
        buttonControl.DOKill();
        for (int i = 0; i < handButtonAni.Length; i++)
        {
            handButtonAni[i].DOKill();
            handButtonImage[i].DOKill();
        }
        for (int i = 0; i < boxCheck.Length; i++)
        {
            boxCheck[i].DOKill();
        }
    }

    public void OnDestroy()
    {
        DoKill();
    }
}
