using System;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Common.UI.Controller
{
	public class WindowController: MonoBehaviour, IWindowController
    {
		public WindowMode Mode { get; protected set; } = WindowMode.Single;
		
		public Transform Owner => this.transform;

		[SerializeField]
		private Animator _animator;

        protected IUIManager Manager;

        protected Animator Animator => _animator;

        [Inject]
        protected void Construct(IUIManager uiManager)
        {
            Manager = uiManager;

			gameObject.SetActive(false);

            OnInit();
        }

		public void Open(Action<IWindowController> callback = null)
		{
			gameObject.SetActive(true);
			OnOpen();
		}

		public void Close(Action<IWindowController> callback = null)
        {
			gameObject.SetActive(false);
			Manager.Close(this);
			OnClose();
        }

		private void Update()
		{
			OnUpdate();
		}

		protected virtual void OnInit()
		{

		}

        protected virtual void OnOpen()
        {

        }

        protected virtual void OnClose()
        {

        }

        protected virtual void OnUpdate()
        {

        }
    }
}
