using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scrips.UI;
using Assets.Scripts.Common.UI.Controller;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Scrips.Game.UI
{
    public class MainMenu: WindowController
    {
	    public event Action<int> CubeButtonClick;

	    public event Action RecreateClick; 

	    public class ButtonsContainer: SlotsContainer<CubeButton> {}

	    [SerializeField]
	    private CubeButton _slotPref;

	    [SerializeField]
	    private Transform _container;

	    private ButtonsContainer _buttons = new ButtonsContainer();

	    [SerializeField]
	    private Button _recreateButton;

	    protected override void OnInit()
	    {
		    _buttons.Initialize(_slotPref, _container);
		    _recreateButton.onClick.AddListener((() =>
		    {
				RecreateClick?.Invoke();
		    }));
	    }

	    public void AddButton(int index, Color color)
	    {
		    var btn = _buttons.AddSlot();
		    if (btn == null)
		    {
				return;
		    }

		    btn.Index = index;
		    btn.SetColor(color);
		    btn.Click += OnButtonClick;
	    }

	    public void ClearButtons()
	    {
		    foreach (var btn in _buttons.Slots)
		    {
			    btn.Click -= OnButtonClick;
		    }
		    _buttons.Clear();
	    }

	    private void OnButtonClick(int index)
	    {
		    CubeButtonClick?.Invoke(index);
	    }

	    private void OnDestroy()
	    {
		    ClearButtons();
	    }
    }
}
