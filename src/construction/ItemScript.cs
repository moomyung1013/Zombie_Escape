using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ItemScript : MonoBehaviour
{
    GameObject target;
    GameObject Item1, Item2, Item3, Item4, Item5, Item6;
    GameObject zombie;
    public Text zombieCreateText;
    public Text lossItemText;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("MainPlayer");
        Item1 = GameObject.Find("Item1");
        Item2 = GameObject.Find("Item2");
        Item3 = GameObject.Find("Item3");
        Item4 = GameObject.Find("Item4");
        Item5 = GameObject.Find("Item5");
        Item6 = GameObject.Find("Item6");
        zombie = GameObject.Find("Zombie");
    }

    // Update is called once per frame
    void Update()
    {
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == target)
        {
            ItemEvent();
            this.gameObject.SetActive(false);
        }
    }
    void ItemEvent()
    {
        GameObject thisObject = this.gameObject;
        if (thisObject == Item1)
            Item1Event();
        else if (thisObject == Item2)
            Item2Event();
        else if (thisObject == Item3)
            Item3Event();
        else if (thisObject == Item4)
            Item4Event();
        else if (thisObject == Item5)
            Item1Event();
        else if (thisObject == Item6)
            Item1Event();
    }

    void Item1Event() //탈출 아이템을 먹었을 때
    {
        var escape = target.gameObject.GetComponent<SimpleCharacterControl>();
        escape.escapeItemCount++;
    }
    void Item2Event() //시작지점으로 워프
    {
        target.transform.position = new Vector3((float)-47.65, 0, (float)-45.46);
    }
    void Item3Event() //좀비 생성
    {
        Instantiate(zombie, new Vector3((float)6.72, 0, (float)-30.87),Quaternion.identity);
        zombieCreateText.text = "좀비가 한 마리 더 생성 되었습니다!";
        Invoke("SetText", 2.0f);
        Destroy(zombie, 15);
    }
    void Item4Event() //탈출 아이템 한개 리셋
    {
        var escape = target.gameObject.GetComponent<SimpleCharacterControl>();
        if (escape.escapeItemCount >= 1)
        {
            escape.escapeItemCount--;
            lossItemText.text = "탈출 아이템을 하나 잃었습니다!";
            Invoke("SetText2", 2.0f);
        }
        else
        {
        }
    }
    private void SetText()
    {
        zombieCreateText.text = "";
    }
    private void SetText2()
    {
       lossItemText.text = "";
    }
}