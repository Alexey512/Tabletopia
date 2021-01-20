using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scrips.Game.Units
{
    public class Cube: MonoBehaviour, IIntersectable3d
    {
	    public event Action<Cube, Cube> CollisionEnter;
	    
	    [SerializeField]
        private Collider _collider;

        [SerializeField]
        private Rigidbody _body;

        [SerializeField]
        private Renderer _renderer;

        private bool _isKinematic;

        private Color _color = Color.white;

        public bool IsIntersect(Ray ray, out Vector3 point)
        {
            if (_collider == null)
            {
                point = Vector3.zero;
                return false;
            }
            
            RaycastHit hit;
            bool result = _collider.Raycast(ray, out hit, Mathf.Infinity);
            point = hit.point;
            return result;
        }

        public void SetColor(Color color)
        {
	        _color = color;
	        if (_renderer != null)
	        {
		        _renderer.material.SetColor("_Color", _color);
	        }
        }

        public Color GetColor()
        {
	        return _color;
        }

        public void StartDrag()
        {
            if (_body == null)
                return;

            _isKinematic = _body.isKinematic;
            _body.isKinematic = true;
        }

        public void StopDrag()
        {
            if (_body == null)
                return;

            _body.isKinematic = _isKinematic;
        }

        private void OnCollisionEnter(Collision collision)
        {
	        var owner = collision.gameObject.GetComponent<Cube>();
	        if (owner != null)
	        {
		        CollisionEnter?.Invoke(this, owner);
	        }
        }
    }
}
