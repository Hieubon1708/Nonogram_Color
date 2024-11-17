using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
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
    public float timeTarget;

    public RectTransform[] line1;
    public RectTransform[] line2;
    public RectTransform[] line3;
    public RectTransform[] line4;
    public RectTransform[] line5;

    public RowCluster[] rowClusters;
    public ColCluster[] colClusters;

    public Box[] boxLine1;
    public Box[] boxLine2;
    public Box[] boxLine3;
    public Box[] boxLine4;
    public Box[] boxLine5;

    public Color[] colorFade;

    public Box[][] boxes = new Box[5][];

    public GameObject[] lights;
    public GameObject light;
    public GameObject[] lightStep9;

    public Box[] boxCheck;

    public ButtonSelector[] buttonSelectors;
    public Animation[] handButtonAni;
    public Image[] handButtonImage;

    public List<GameObject> boxPassed = new List<GameObject>();
    public List<GameObject> boxPassedFade = new List<GameObject>();

    public string targetHex;
    public string[] hex;

    bool isDrag;
    bool isStepOk = true;
    bool isRoting;
    public bool isCanClickBox;
    public int totalSelect;
    public int indexBox;
    public int totalToWin;
    public int totalX;
    public int totalBoxSelected;
    public int indexButtonCanSelect;
    Tween delayCallStep;
    Tween delayStep1;
    Tween delayStep2;

    public CanvasGroup[] canvasBeforeWin;
    public CanvasGroup frameWin;
    public CanvasGroup panelWinFront;
    public CanvasGroup panelWinBack;

    public GameObject panelEndTutorial;

    public Image bgMask;
    public Mask mask;
    public GameObject wooden;
    public RectTransform boxAreaParent;
    public RectTransform target;
    public GameObject tutorial;
    public GameObject home;
    public GameObject gamePlay;

    void ActiveLightStep9(bool isActive)
    {
        for (int i = 0; i < lightStep9.Length; i++)
        {
            lightStep9[i].SetActive(isActive);
        }
    }

    public void Skip()
    {
        UIController.instance.uICommon.DOLayerCover(1f, 0.5f, true, delegate
        {
            DoKill();
            tutorial.SetActive(false);
            UIController.instance.uICommon.DOLayerCover(0f, 0.5f, false, null);
        });
    }

    public void HidePanelEnd()
    {
        panelEndTutorial.SetActive(false);
        PlayerPrefs.SetInt("Tutorial", 1);
    }

    public void NextLevel()
    {
        UIController.instance.uICommon.DOLayerCover(1f, 0.5f, true, delegate
        {
            DoKill();
            tutorial.SetActive(false);
            home.SetActive(false);
            gamePlay.SetActive(true);
            GameController.instance.LoadLevel(PlayerPrefs.GetInt("Level", 1));
            UIController.instance.uICommon.DOLayerCover(0f, 0.5f, false, null);
        });
    }

    public void Home()
    {
        UIController.instance.uICommon.DOLayerCover(1f, 0.5f, true, delegate
        {
            DoKill();
            tutorial.SetActive(false);
            UIController.instance.uICommon.DOLayerCover(0f, 0.5f, false, null);
        });
    }

    public void StartTutorial()
    {
        boxes[0] = boxLine1;
        boxes[1] = boxLine2;
        boxes[2] = boxLine3;
        boxes[3] = boxLine4;
        boxes[4] = boxLine5;

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
            if (!isCanClickBox) return;
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
                if (e.CompareTag("Box") && !boxPassed.Contains(e) && IsContains(e))
                {
                    boxPassed.Add(e);
                    Box box = GetBox(e);
                    if (box.mainHex != targetHex || box.isVisible) return;
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
                if (box.mainHex != targetHex || box.isVisible) return;
                box.image.color = colorFade[box.mainHex == "#D80E0E" ? 0 : 1];
            }
        }
    }

    void ResetBoxHover(Box box)
    {
        for (int i = 0; i < boxCheck.Length; i++)
        {
            if (!boxCheck[i].isVisible && boxCheck[i] != box) boxCheck[i].image.DOColor(Vector4.one, 0.1f).SetEase(Ease.Linear).SetUpdate(true);
        }
    }

    bool IsContains(GameObject box)
    {
        if (targetHex == "#FFFFFF")
        {
            for (int i = 0; i < boxes.Length; i++)
            {
                for (int j = 0; j < boxes[i].Length; j++)
                {
                    if (boxes[i][j].gameObject == box) return true;
                }
            }
        }
        for (int i = 0; i < boxCheck.Length; i++)
        {
            if (boxCheck[i].gameObject == box) return true;
        }
        return false;
    }

    Box GetBox(GameObject box)
    {
        if (targetHex == "#FFFFFF")
        {
            for (int i = 0; i < boxes.Length; i++)
            {
                for (int j = 0; j < boxes[i].Length; j++)
                {
                    if (boxes[i][j].gameObject == box) return boxes[i][j];
                }
            }
        }
        for (int i = 0; i < boxCheck.Length; i++)
        {
            if (boxCheck[i].gameObject == box) return boxCheck[i];
        }
        return null;
    }

    public void BoxSelect(Box box)
    {
        if (box.isVisible) return;
        DoKill();
        ResetBoxHover(box);
        hand.gameObject.SetActive(false);
        boxPassedFade.Clear();
        box.isVisible = true;
        isRoting = false;
        float time = 2f;
        if (box.mainHex == "#FFFFFF")
        {
            totalX++;
            box.x.gameObject.SetActive(true);
            if (totalX == totalSelect)
            {
                isStepOk = true;
                isCanClickBox = false;
                ActiveLightStep9(false);
                time = 0.75f;
                step++;
            }
            else isStepOk = false;
        }
        else
        {
            Color color;
            if (GameController.instance.ColorConvert(box.mainHex, out color))
            {
                box.image.DOColor(color, 0.1f);
            }
            else
            {
                Debug.LogWarning(box + " " + box.mainHex);
            }
            if (GetAmountBoxSelect() == totalSelect)
            {
                isCanClickBox = false;
                light.SetActive(false);
                isStepOk = true;
                time = 0.75f;
                if(step == 12)
                {
                    totalToWin = 20;
                    Win1();
                }
                step++;
                if (step == 5)
                {
                    handButtonImage[2].transform.parent.gameObject.SetActive(true);
                }
            }
            else isStepOk = false;
            CheckLine(box);
        }
        delayCallStep = DOVirtual.DelayedCall(time, delegate
        {
            Step();
        });
    }

    public void Win1()
    {
        for (int i = 0; i < boxes.Length; i++)
        {
            for (int j = 0; j < boxes[i].Length; j++)
            {
                Box box = boxes[j][i];
                DOVirtual.DelayedCall(0.25f * (j * 0.25f), delegate
                {
                    box.EndDo(0.25f);
                });
            }
        }
        DOVirtual.DelayedCall((boxes.Length) * (0.25f * 0.25f), delegate
        {
            Win2();
        });
    }

    public void Win2()
    {
        for (int i = 0; i < canvasBeforeWin.Length; i++)
        {
            canvasBeforeWin[i].DOFade(0f, 0.5f).SetEase(Ease.Linear);
        }
        DOVirtual.DelayedCall(0.5f, delegate
        {
            mask.enabled = true;
            bgMask.enabled = true;
            frameWin.gameObject.SetActive(true);
            frameWin.DOFade(1f, 0.5f).SetEase(Ease.Linear).OnComplete(delegate
            {
                DOVirtual.DelayedCall(0.5f, delegate
                {
                    wooden.SetActive(true);
                    panelWinFront.gameObject.SetActive(true);
                    panelWinFront.DOFade(1f, 0.5f).SetEase(Ease.Linear);
                    panelWinBack.gameObject.SetActive(true);
                    panelWinBack.DOFade(1f, 0.5f).SetEase(Ease.Linear).OnComplete(delegate
                    {
                        panelEndTutorial.SetActive(true);
                    });
                });
                boxAreaParent.DOMove(target.position, 0.5f).SetEase(Ease.Linear);
                boxAreaParent.DOScale(0.725f, 0.5f).SetEase(Ease.Linear);
            });
        });
    }

    int GetBoxStart()
    {
        int index = 0;
        for (int i = 0; i < boxCheck.Length; i++)
        {
            if (!boxCheck[i].isVisible && boxCheck[i].mainHex == targetHex) return i;
        }
        return index;
    }

    public void MoveHand()
    {
        if (indexBox < boxCheck.Length)
        {
            if (boxCheck[indexBox].mainHex != targetHex || boxCheck[indexBox].isVisible)
            {
                CheckBox();
                DoKill();
                indexBox = GetIndexBoxCanSelect(indexBox);
                if (indexBox == -1)
                {
                    EndHand();
                    return;
                }
                handAni.Play("TutorialHandUp");
                isRoting = false;
            }
            else
            {
                if (!isRoting)
                {
                    hand.DORotate(new Vector3(0, 0, -13.4f), 1f).SetEase(Ease.Linear);
                    isRoting = true;
                }
                hand.DOMoveX(boxCheck[indexBox].transform.position.x, 1f / 5f).SetEase(Ease.Linear).OnComplete(delegate
                {
                    indexBox++;
                    MoveHand();
                }).OnUpdate(delegate
                {
                    CheckBox();
                });
            }
        }
        else
        {
            CheckBox();
            EndHand();
        }
    }

    void EndHand()
    {
        DoKill();
        DOVirtual.DelayedCall(0.5f, delegate
        {
            isRoting = false;
            indexBox = GetBoxStart() + 1;
            boxPassedFade.Clear();
            ResetBoxHover(null);
            handAni.Play("TutorialHandHide");
        });
    }

    int GetIndexBoxCanSelect(int currentIndex)
    {
        for (int i = currentIndex; i < boxCheck.Length; i++)
        {
            if (!boxCheck[i].isVisible && boxCheck[i].mainHex == targetHex) return i;
        }
        return -1;
    }

    public void HandUp()
    {
        hand.DORotate(new Vector3(0, 0, 31f), 0.4f).SetEase(Ease.Linear);
        hand.DOMoveX(boxCheck[indexBox].transform.position.x, 0.4f).SetEase(Ease.Linear).OnComplete(delegate
        {
            DoKill();
            handAni.Play("TutorialHandDown");
        });
    }

    public void HandDown()
    {
        CheckBox();
        indexBox++;
        MoveHand();
    }

    public void ButtonSelect(int index)
    {
        if (index != indexButtonCanSelect) return;
        isStepOk = true;
        DoKill();
        if (step == 1)
        {
            light = lights[0];
            light.SetActive(true);
            handButtonImage[1].gameObject.SetActive(false);
            Color color = handButtonImage[1].color;
            color.a = 0f;
            handButtonImage[1].color = color;
        }
        if (step == 5)
        {
            light = lights[3];
            light.SetActive(true);
            handButtonImage[2].gameObject.SetActive(false);
            isCanClickBox = true;
            Color color = handButtonImage[2].color;
            color.a = 0f;
            handButtonImage[2].color = color;
        }
        if (step == 7)
        {
            light = lights[4];
            light.SetActive(true);
            isCanClickBox = true;
            handButtonImage[1].gameObject.SetActive(false);
            Color color = handButtonImage[1].color;
            color.a = 0f;
            handButtonImage[1].color = color;
        }
        if (step == 9)
        {
            ActiveLightStep9(true);
            isCanClickBox = true;
            handButtonImage[0].gameObject.SetActive(false);
        }
        if (step == 11)
        {
            light = lights[5];
            light.SetActive(true);
            isCanClickBox = true;
            handButtonImage[2].gameObject.SetActive(false);
        }
        UIController.instance.ButtonSelect(buttonSelectors, buttonSelectors[index], 0.15f, 0.25f);
        indexButtonCanSelect = -1;
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
                indexButtonCanSelect = 1;
                targetHex = hex[0];
                delayStep1 = DOVirtual.DelayedCall(1f, delegate
                {
                    handButtonImage[1].gameObject.SetActive(true);
                    handButtonImage[1].DOFade(1f, 0.25f).SetEase(Ease.Linear);
                    handButtonAni[1].Play();
                });
            }

            if (step == 2)
            {
                boxCheck = boxLine1;
                totalSelect = 5;
                targetHex = hex[0];
                isCanClickBox = true;
                delayStep1 = DOVirtual.DelayedCall(1f, delegate
                {
                    int indexStart = GetBoxStart();
                    indexBox = indexStart + 1;
                    hand.gameObject.SetActive(true);
                    startHand = line1[indexStart].position;
                    StartHand();
                });
            }

            if (step == 3)
            {
                if (isStepOk)
                {
                    MoveRow(1);
                    MoveLine();
                    light = lights[1];
                    boxCheck = boxLine2;
                    totalSelect = 4;
                    targetHex = hex[0];
                }
                delayStep2 = DOVirtual.DelayedCall(1f, delegate
                {
                    int indexStart = GetBoxStart();
                    indexBox = indexStart + 1;
                    hand.gameObject.SetActive(true);
                    startHand = line2[indexStart].position;
                    StartHand();
                });
            }
            if (step == 4)
            {
                if (isStepOk)
                {
                    MoveRow(2);
                    MoveLine();
                    light = lights[2];
                    boxCheck = boxLine3;
                    totalSelect = 4;
                    targetHex = hex[0];
                }
                delayStep2 = DOVirtual.DelayedCall(1f, delegate
                {
                    int indexStart = GetBoxStart();
                    indexBox = indexStart + 1;
                    hand.gameObject.SetActive(true);
                    startHand = line3[indexStart].position;
                    StartHand();
                }); 
            }

            //line 4 3 cai mau xanh
            if (step == 5)
            {
                MoveRow(3);
                MoveLine();
                boxCheck = boxLine4;
                totalSelect = 3;
                indexButtonCanSelect = 2;
                targetHex = hex[1];
                delayStep2 = DOVirtual.DelayedCall(1f, delegate
                {
                    handButtonImage[2].gameObject.SetActive(true);
                    handButtonImage[2].DOFade(1f, 0.25f).SetEase(Ease.Linear);
                    handButtonAni[2].Play();
                });
            }

            if (step == 6)
            {
                delayStep2 = DOVirtual.DelayedCall(1f, delegate
                {
                    int indexStart = GetBoxStart();
                    indexBox = indexStart + 1;
                    hand.gameObject.SetActive(true);
                    startHand = line4[indexStart].position;
                    StartHand();
                });
            }

            //line 4 2 cai mau do
            if (step == 7)
            {
                indexButtonCanSelect = 1;
                totalSelect = 5;
                targetHex = hex[0];
                delayStep2 = DOVirtual.DelayedCall(1f, delegate
                {
                    handButtonImage[1].gameObject.SetActive(true);                   
                    handButtonImage[1].DOFade(1f, 0.25f).SetEase(Ease.Linear);
                    handButtonAni[1].Play();
                });
            }

            if (step == 8)
            {
                delayStep2 = DOVirtual.DelayedCall(1f, delegate
                {
                    int indexStart = GetBoxStart();
                    indexBox = indexStart + 1;
                    hand.gameObject.SetActive(true);
                    startHand = line4[indexStart].position;
                    StartHand();
                });
            }

            if (step == 9)
            {
                MoveRow(4);
                MoveLine();
                DOVirtual.DelayedCall(0.25f, delegate
                {
                    delayStep1 = DOVirtual.DelayedCall(0.25f, delegate
                    {
                        MoveRow(0);
                        maskLine.DOAnchorPosY(maskLine.anchoredPosition.y - 170, 0.25f).SetEase(Ease.Linear);
                        indexButtonCanSelect = 0;
                        totalSelect = 6;
                        targetHex = hex[2];
                        delayStep2 = DOVirtual.DelayedCall(1f, delegate
                        {
                            handButtonImage[0].gameObject.SetActive(true);
                            handButtonImage[0].DOFade(1f, 0.25f).SetEase(Ease.Linear);
                            handButtonAni[0].Play();
                        });
                    });
                });
            }

            if (step == 11)
            {
                indexButtonCanSelect = 2;
                totalSelect = 5;
                boxCheck = boxLine5;
                targetHex = hex[1];
                delayStep2 = DOVirtual.DelayedCall(1f, delegate
                {
                    handButtonImage[2].gameObject.SetActive(true);
                    handButtonImage[2].DOFade(1f, 0.25f).SetEase(Ease.Linear);
                    handButtonAni[2].Play();
                });
            }

            if (!isStepOk) return;
            if(step != 8 && step != 12)
            {
                popupContent.SetActive(false);
                popupContent.SetActive(true);
            }
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
        maskLine.DOSizeDelta(new Vector2(-230, maskLine.sizeDelta.y + 170), 0.25f).SetEase(Ease.Linear).OnComplete(delegate
        {
            if (step == 3 || step == 4)
            {
                isCanClickBox = true;
            }
        });
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
        for (int i = 0; i < boxes.Length; i++)
        {
            for (int j = 0; j < boxes[i].Length; j++)
            {
               boxes[i][j].DOKill();
            }
        }
        delayCallStep.Kill();
        delayStep1.Kill();
        delayStep2.Kill();
        for (int i = 0; i < canvasBeforeWin.Length; i++)
        {
            canvasBeforeWin[i].DOKill();
        }
        panelWinBack.DOKill();
        panelWinFront.DOKill();
        frameWin.DOKill();
    }

    public int GetRow(Box box)
    {
        for (int i = 0; i < boxes.Length; i++)
        {
            for (int j = 0; j < boxes[i].Length; j++)
            {
                if (boxes[i][j] == box)
                {
                    return i;
                }
            }
        }
        return -1;
    }

    public int GetCol(Box box)
    {
        for (int i = 0; i < boxes.Length; i++)
        {
            for (int j = 0; j < boxes[i].Length; j++)
            {
                if (boxes[i][j] == box)
                {
                    return j;
                }
            }
        }
        return -1;
    }

    public void CheckLine(Box box)
    {
        int row = GetRow(box);
        int col = GetCol(box);

        bool isRowOk = true;
        bool isColOk = true;

        if (totalToWin != totalBoxSelected)
        {
            for (int i = 0; i < boxes[row].Length; i++)
            {
                if (!boxes[row][i].isVisible
                    && boxes[row][i].mainHex != "#FFFFFF") isRowOk = false;
            }
            if (isRowOk)
            {
                for (int i = col; i >= 0; i--)
                {
                    if (boxes[row][i].mainHex == "#FFFFFF")
                    {
                        int index = i;
                        DOVirtual.DelayedCall(0.15f * ((col - i) * 0.15f), delegate
                        {
                            boxes[row][index].ani.Play("Flicker");
                        });
                    }
                }
                DOVirtual.DelayedCall(0.15f * ((col + 1) * 0.15f), delegate
                {
                    rowClusters[row].Flicker();
                    CheckRowClusterIndex(boxes, row);
                });
                for (int i = col; i < boxes[row].Length; i++)
                {
                    if (boxes[row][i].mainHex == "#FFFFFF")
                    {
                        int index = i;
                        DOVirtual.DelayedCall(0.15f * ((col - i) * 0.15f), delegate
                        {
                            boxes[row][index].ani.Play("Flicker");
                        });
                    }
                }
            }
            /*for (int i = 0; i < boxes.Length; i++)
            {
                if (!boxes[i][col].isVisible
                    && boxes[i][col].mainHex != "#FFFFFF") isColOk = false;
            }
            if (isColOk)
            {
                for (int i = row; i >= 0; i--)
                {
                    if (boxes[i][col].mainHex == "#FFFFFF")
                    {
                        int index = i;
                        DOVirtual.DelayedCall(0.15f * ((row - i) * 0.15f), delegate
                        {
                            boxes[index][col].ani.Play("Flicker");
                        });
                    }
                }
                DOVirtual.DelayedCall(0.15f * ((row + 1) * 0.15f), delegate
                {
                    colClusters[col].Flicker();
                    CheckColClusterIndex(boxes, col);
                });
                for (int i = row; i < boxes.Length; i++)
                {
                    if (boxes[i][col].mainHex == "#FFFFFF")
                    {
                        int index = i;
                        DOVirtual.DelayedCall(0.15f * ((row - i) * 0.15f), delegate
                        {
                            boxes[index][col].ani.Play("Flicker");
                        });
                    }
                }
            }*/
        }

        bool isClusterOk = true;
        for (int i = 0; i < box.rowClusters.Count; i++)
        {
            if (!box.rowClusters[i].isVisible) isClusterOk = false;
        }
        if (isClusterOk)
        {
            box.rowClusterIndex.CompletedCluster();
            if (!isRowOk)
            {
                CheckRowClusterIndex(boxes, row);
            }
        }
        /*isClusterOk = true;
        for (int i = 0; i < box.colClusters.Count; i++)
        {
            if (!box.colClusters[i].isVisible) isClusterOk = false;
        }
        if (isClusterOk)
        {
            box.colClusterIndex.CompletedCluster();
            if (!isColOk)
            {
                CheckColClusterIndex(boxes, col);
            }
        }*/
    }

    void CheckRowClusterIndex(Box[][] boxes, int index)
    {
        for (int i = 0; i < boxes[index].Length; i++)
        {
            if (boxes[index][i].isVisible)
            {
                if (boxes[index][i].rowClusterIndex == null) continue;
                if (boxes[index][i].rowClusterIndex.isDone)
                {
                    boxes[index][i].rowClusterIndex.Flicker();
                }
            }
            else break;
        }
        for (int i = boxes.Length - 1; i >= 0; i--)
        {
            if (boxes[index][i].isVisible)
            {
                if (boxes[index][i].rowClusterIndex == null) continue;
                if (boxes[index][i].rowClusterIndex.isDone)
                {
                    boxes[index][i].rowClusterIndex.Flicker();
                }
            }
            else break;
        }
    }

    void CheckColClusterIndex(Box[][] boxes, int index)
    {
        for (int i = 0; i < boxes.Length; i++)
        {
            if (boxes[i][index].isVisible)
            {
                if (boxes[i][index].colClusterIndex == null) continue;
                if (boxes[i][index].colClusterIndex.isDone)
                {
                    boxes[i][index].colClusterIndex.Flicker();
                }
            }
            else break;
        }
        for (int i = boxes.Length - 1; i >= 0; i--)
        {
            if (boxes[i][index].isVisible)
            {
                if (boxes[i][index].colClusterIndex == null) continue;
                if (boxes[i][index].colClusterIndex.isDone)
                {
                    boxes[i][index].colClusterIndex.Flicker();
                }
            }
            else break;
        }
    }

    public void OnDestroy()
    {
        DoKill();
    }
}
