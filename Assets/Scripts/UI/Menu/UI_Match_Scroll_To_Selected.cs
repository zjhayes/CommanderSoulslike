using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Match_Scroll_To_Selected : MonoBehaviour
{
    [SerializeField] RectTransform contentPanel;
    [SerializeField] ScrollRect scrollRect;

    GameObject currentSelected;
    GameObject previouslySelected;
    RectTransform currentSelectedTransform;

    private void Update()
    {
        currentSelected = EventSystem.current.currentSelectedGameObject;

        if(currentSelected != null && currentSelected != previouslySelected)
        {
            previouslySelected = currentSelected;
            currentSelectedTransform = currentSelected.GetComponent<RectTransform>();
            SnapTo(currentSelectedTransform);
        }
            
    }

    private void SnapTo(RectTransform target)
    {
        Canvas.ForceUpdateCanvases();

        Vector2 newPosition = (Vector2) scrollRect.transform.InverseTransformPoint(contentPanel.position) - (Vector2) scrollRect.transform.InverseTransformPoint(target.position);

        newPosition.x = 0;

        contentPanel.anchoredPosition = newPosition;
    }
}
