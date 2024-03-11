using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseColorController : MonoBehaviour
{
    [SerializeField] private Material[] houseMats;
    [Header("House 1 Colors")]
    [Space(16)]
    [SerializeField] private Color[] colors1A;
    [SerializeField] private Color[] colors1B;
    [SerializeField] private Color[] colors1C;
    [Header("House 2 Colors")]
    [Space(16)]
    [SerializeField] private Color[] colors2A;
    [SerializeField] private Color[] colors2B;
    [SerializeField] private Color[] colors2C;
    [Header("House 3 Colors")]
    [Space(16)]
    [SerializeField] private Color[] colors3A;
    [SerializeField] private Color[] colors3B;
    [SerializeField] private Color[] colors3C;
    [Header("House 4 Colors")]
    [Space(16)]
    [SerializeField] private Color[] colors4A;
    [SerializeField] private Color[] colors4B;
    [SerializeField] private Color[] colors4C;
    private string[] varNames = new string[] { "_Col1", "_Col2", "_Col3", "_Col4", "_Col5", "_Col6" };

    private void Start()
    {
        //GameManager.Instance.OnCityLevelChange += ChangeColors;
        ChangeColors(0);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeColors(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeColors(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ChangeColors(2);
        }
    }
    public void ChangeColors(int phaseIndex)
    {
        Color[] currentColors1 = new Color[6];
        Color[] currentColors2 = new Color[6];
        Color[] currentColors3 = new Color[6];
        Color[] currentColors4 = new Color[6];
        switch (phaseIndex)
        {
            case 0:
                currentColors1 = colors1A;
                currentColors2 = colors2A;
                currentColors3 = colors3A;
                currentColors4 = colors4A;
                break;

            case 1:
                currentColors1 = colors1B;
                currentColors2 = colors2B;
                currentColors3 = colors3B;
                currentColors4 = colors4B;
                break;

            case 2:
                currentColors1 = colors1C;
                currentColors2 = colors2C;
                currentColors3 = colors3C;
                currentColors4 = colors4C;
                break;
        }
        for (int i = 0; i < varNames.Length; i++)
        {
            houseMats[0].SetColor(varNames[i], currentColors1[i]);
            houseMats[1].SetColor(varNames[i], currentColors2[i]);
            houseMats[2].SetColor(varNames[i], currentColors3[i]);
            houseMats[3].SetColor(varNames[i], currentColors4[i]);
        }
    }

    private void OnDestroy()
    {
        //GameManager.Instance.OnCityLevelChange += ChangeColors;
        ChangeColors(0);
    }
}