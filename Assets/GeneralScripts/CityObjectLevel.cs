using UnityEngine;

public class CityObjectLevel : MonoBehaviour
{
    private Transform[] children;

    private void Awake()
    {
        children = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            children[i] = transform.GetChild(i);
        }
    }

    private void Start()
    {
        GameManager.Instance.OnCityLevelChange += SetLevel;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnCityLevelChange -= SetLevel;
    }

    private void SetLevel(int level)
    {
        for (int i = 0; i < children.Length; i++)
        {
            if (i == level)
            {
                children[i].gameObject.SetActive(true);
            }
            else
            {
                children[i].gameObject.SetActive(false);
            }
        }
    }
}
