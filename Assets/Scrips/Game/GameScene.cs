using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scrips.Common.InputSystem;
using Assets.Scrips.Common.Utils;
using Assets.Scrips.Common.Visual;
using Assets.Scrips.Game.Config;
using Assets.Scrips.Game.UI;
using Assets.Scrips.Game.Units;
using Assets.Scrips.HOG.Visual;
using Assets.Scripts.Common.UI;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Assets.Scrips.Game
{
    public class GameScene: MonoBehaviour, IInitializable, IDisposable
    {
        [SerializeField]
        private Collider _plane;
        
        [Inject] 
        private IGameConfig _config;

        [Inject]
        private IPrefabsFactory _factory;

        [Inject]
        private IInputManager _inputManager;

        [Inject]
        private IUIManager _uiManager;

        [Inject]
        private EffectsHelper _effectsHelper;

        private readonly List<Cube> _cubes = new List<Cube>();

        private Cube _selectedCube;

        private Vector3 _startDelta = new Vector3();

        private MainMenu _mainMenu;

        public void Initialize()
        {
	        InitUI();

	        SubscribeEvent();

	        CreateCubes();
        }

        private void CreateCubes()
        {
	        int cubesCount = _config.CubesCount;
	        float respRadius = _config.RespRadius;
	        float respHeight = _config.RespHeight;

	        for (int i = 0; i < cubesCount; i++)
	        {
		        var cube = _factory.Create<Cube>("Cube", transform);
		        if (cube == null)
			        return;
		        var pos = Random.insideUnitCircle * respRadius;
		        cube.transform.localPosition = new Vector3(pos.x, respHeight, pos.y);
		        cube.CollisionEnter += OnCubeCollisionEnter;

		        var color = Random.ColorHSV();
		        cube.SetColor(color);

		        _cubes.Add(cube);

		        if (_mainMenu != null)
		        {
			        _mainMenu.AddButton(i, color);
		        }
	        }
        }

        private void SubscribeEvent()
        {
	        if (_inputManager == null)
                return;

	        _inputManager.MouseDown += OnMouseDown;
	        _inputManager.MouseUp += OnMouseUp;
	        _inputManager.MouseMove += OnMouseMove;
        }

        private void UnsubscribeEvent()
        {
	        if (_inputManager == null)
		        return;

	        _inputManager.MouseDown -= OnMouseDown;
	        _inputManager.MouseUp -= OnMouseUp;
	        _inputManager.MouseMove -= OnMouseMove;
        }

        private void InitUI()
        {
	        _mainMenu = _uiManager.Open<MainMenu>();
	        if (_mainMenu == null)
                return;

	        _mainMenu.CubeButtonClick += OnCubeButtonCubeButtonClick;
	        _mainMenu.RecreateClick += OnRecreateClick;
        }

        private void OnRecreateClick()
        {
	        RecreateCubes();
        }

        private void OnCubeButtonCubeButtonClick(int index)
        {
	        if (index < 0 || index >= _cubes.Count)
                return;

	        var cube = _cubes[index];
	        var pos = cube.transform.position;
	        pos.y += _config.RespHeight;
	        _effectsHelper.ShowCubeBubble(pos, _config.RespHeight * 3.0f);
        }

        private void OnCubeCollisionEnter(Cube cube, Cube owner)
        {
            if (_selectedCube != cube)
                return;

	        var canvas = GameObject.FindObjectOfType<Canvas>();
	        var screenPos = canvas.WorldToCanvas(owner.transform.position, Camera.main);
	        
	        _uiManager.Open<UiBubble>().Show(screenPos, "Bump!", owner.GetColor(), 0.3f);
        }

        private bool GetIntersectPlane(Vector3 mousePos, out Vector3 point)
        {
	        Ray ray = Camera.main.ScreenPointToRay(mousePos);
	        
	        RaycastHit hit;
	        if (!_plane.Raycast(ray, out hit, Mathf.Infinity))
	        {
		        point = Vector3.zero;
		        return false;
	        }

	        point = hit.point;
	        return true;
        }

        private Cube GetIntersectCube(Vector3 mousePos)
        {
            Ray ray = Camera.main.ScreenPointToRay(mousePos);

            foreach (var cube in _cubes)
            {
                if (cube.IsIntersect(ray, out Vector3 intersectPoint))
                {
	                return cube;
                }
            }

            return null;
        }

        private void OnMouseDown(Vector3 mousePos)
        {
	        if (!GetIntersectPlane(mousePos, out Vector3 planePos))
	        {
		        return;
	        }

	        _selectedCube = GetIntersectCube(mousePos);
            if (_selectedCube != null)
            {
	            _startDelta = planePos - _selectedCube.transform.position; 
                
	            _selectedCube.StartDrag();
            }
        }

        private void OnMouseUp(Vector3 mousePos)
        {
            if (_selectedCube != null)
            {
                _selectedCube.StopDrag();
                _selectedCube = null;
            }
        }

        private void OnMouseMove(Vector3 mousePos, Vector3 downMousePos)
        {
            if (_selectedCube == null)
                return;

            if (!GetIntersectPlane(mousePos, out Vector3 planePos))
            {
                return;
            }

            _selectedCube.transform.position = planePos - _startDelta;
        }

        private void RecreateCubes()
        {
	        ClearCubes();
            CreateCubes();
        }

        private void ClearCubes()
        {
	        foreach (var cube in _cubes)
	        {
		        cube.CollisionEnter -= OnCubeCollisionEnter;

		        _factory.Remove(cube.gameObject);
	        }

	        _cubes.Clear();

	        _mainMenu.ClearButtons();
        }

        public void Dispose()
        {
	        UnsubscribeEvent();

            if (_mainMenu != null)
            {
	            _mainMenu.CubeButtonClick -= OnCubeButtonCubeButtonClick;
	            _mainMenu.RecreateClick -= OnRecreateClick;

	            _mainMenu.Close();
            }
        }
    }
}
