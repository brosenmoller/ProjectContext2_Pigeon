using UnityEngine;

public class CityMaterialLevel : MonoBehaviour
{
    [SerializeField] private Material[] materials;

    private MeshRenderer meshRenderer;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        GameManager.Instance.OnCityLevelChange += SetLevel;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnCityLevelChange -= SetLevel;
    }

    private void SetLevel(int level)
    {
        if (level >= materials.Length)
        {
            return;
        }

        meshRenderer.material = materials[level];
    }
}
