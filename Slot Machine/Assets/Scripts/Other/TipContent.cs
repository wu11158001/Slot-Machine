using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class TipContent : MonoBehaviour
{
    [SerializeField]
    private RectTransform rt;

    [SerializeField]
    private Text tipText;

    private const float speed = 300;
    private const float targetY = -90;
    /// <summary>
    /// 顯示
    /// </summary>
    /// <param name="str"></param>
    public async void Show(string str)
    {       
        tipText.text = str;
        float posY = rt.rect.height;   
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
        rt.anchoredPosition = new Vector2(0, -35);
        rt.localScale = Vector3.one;
        transform.SetSiblingIndex(100);

        while (rt.anchoredPosition.y > targetY)
        {
            float y = rt.anchoredPosition.y - Time.deltaTime * speed;
            if (y <= targetY) y = targetY;
            rt.anchoredPosition = new Vector2(0, y);
            await Task.Delay(1);
        }

        await Task.Delay(2000);
        Over();
    }

    /// <summary>
    /// 結束
    /// </summary>
    public async void Over()
    {
        float targetY = 60;

        while (rt.anchoredPosition.y < targetY)
        {
            float y = rt.anchoredPosition.y + Time.deltaTime * speed;
            if (y >= targetY) y = targetY;
            rt.anchoredPosition = new Vector2(0, y);
            await Task.Delay(1);
        }

        rt.gameObject.SetActive(false);
    }
}
