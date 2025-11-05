using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivesUIManager : MonoBehaviour
{

    public static LivesUIManager Instance;

    [Header("UI Elements")]
    [SerializeField] private List<Image> lifeImages; 

    private void Awake()
    {
        Instance = this;
    }

    // Cập nhật lại hiển thị mạng
    public void UpdateLives(int currentLives)
    {
        for (int i = 0; i < lifeImages.Count; i++)
        {
            // Nếu chỉ muốn ẩn đi khi mất mạng:
            lifeImages[i].enabled = (i < currentLives);
        }
    }
}
