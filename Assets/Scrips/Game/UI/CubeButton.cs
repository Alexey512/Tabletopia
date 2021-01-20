using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scrips.Game.UI
{
	public class CubeButton: MonoBehaviour
	{
		public event Action<int> Click; 
		
		[SerializeField]
		private Button _button;

		[SerializeField]
		private Image _back;

		public int Index { get; set; }

		private void Awake()
		{
			if (_button != null)
			{
				_button.onClick.AddListener(() =>
				{
					Click?.Invoke(Index);
				});
			}
		}

		public void SetColor(Color color)
		{
			if (_back != null)
			{
				_back.color = color;
			}
		}
	}
}
