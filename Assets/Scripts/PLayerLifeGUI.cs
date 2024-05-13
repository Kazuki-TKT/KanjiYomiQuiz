using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace KanjiYomi
{
    public class PLayerLifeGUI : MonoBehaviour
    {
        [SerializeField]
        TextMeshProUGUI playerLifeText;

        private void Awake()
        {
            PlayerController.OnPlayerLifeChanged += ChangePlayetLifeText;
        }

        private void OnDestroy()
        {
            PlayerController.OnPlayerLifeChanged -= ChangePlayetLifeText;
        }

        void ChangePlayetLifeText(int playerLife)
        {
            playerLifeText.text = playerLife.ToString();
        }
    }
}
