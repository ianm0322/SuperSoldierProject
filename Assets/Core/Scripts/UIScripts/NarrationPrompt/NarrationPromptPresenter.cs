using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace PrSuperSoldier
{
    public class NarrationPromptPresenter : MonoBehaviour
    {
        // TODO: 2025.07.28 작업종료
        [SerializeField]
        private RectTransform _elementTemplate;

        [SerializeField]
        private Transform _content;

        public void AddPrompt(string text)
        {

        }
        private RectTransform MakeItem(string text)
        {
            var item = Instantiate(_elementTemplate);
            item.SetParent(_content);
            item.SetAsLastSibling();
            return item;
        }
    }
}
