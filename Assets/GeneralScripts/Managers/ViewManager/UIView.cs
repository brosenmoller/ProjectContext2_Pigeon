using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class UIView : MonoBehaviour
{
    public bool defaultView;

    protected Selectable primarySelectable = null;

    public virtual void Initialize() { }
    
    protected virtual void OnShow() { }
    protected virtual void OnHide() { }

    public virtual void Hide(bool triggerEvent = true)
    {
        EventSystem.current.SetSelectedGameObject(null);
        gameObject.SetActive(false);
        if (triggerEvent) { OnHide(); }
    }
    public virtual void Show()
    {
        if (primarySelectable != null) { EventSystem.current.SetSelectedGameObject(primarySelectable.gameObject); }
        gameObject.SetActive(true);
        OnShow();
    }
}
