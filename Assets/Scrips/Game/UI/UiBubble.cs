using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scrips.UI;
using Assets.Scripts.Common.UI.Controller;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scrips.Game.UI
{
	public class UiBubble: WindowController
	{
		[SerializeField]
		private TextMeshProUGUI _text;

		[SerializeField]
		private RectTransform _rectTransform;

		[SerializeField]
		private Image _back;

		private float _leftTime;

		private RectTransform rectTransform;
		private Vector2 uiOffset;

		public void Show(Vector2 pos, string text, Color color, float time = 1.0f)
		{
			_text.text = text;
			_back.color = color;
			_rectTransform.anchoredPosition = pos;
			_leftTime = time;
		}

		protected override void OnUpdate()
		{
			if (_leftTime < 0)
			{
				Close();
			}
			else
			{
				_leftTime -= Time.deltaTime;
			}
		}
	}
}
