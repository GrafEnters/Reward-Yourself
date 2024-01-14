using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ColorMe : MonoBehaviour {
    private TextMeshProUGUI _text;
    private Text _textLegacy;
    private Image _image;
    private Outline _outline;

    [SerializeField]
    private Colors _color, _outlineColor;

    private void Start() {
        Color color = Loader.ColorsConfig.GetColor(_color);
        _image = GetComponent<Image>();
        if (_image) {
            _image.color = color;
        }

        _text = GetComponent<TextMeshProUGUI>();
        if (_text) {
            _text.color = color;
        }
        _textLegacy = GetComponent<Text>();
        if (_textLegacy) {
            _textLegacy.color = color;
        }
        
        _outline = GetComponent<Outline>();
        if (_outline) {
            _outline.effectColor = Loader.ColorsConfig.GetColor(_outlineColor);
        }
    }
}