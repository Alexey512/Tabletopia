using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scrips.Game.Units
{
    public interface IIntersectable3d
    {
        bool IsIntersect(Ray ray, out Vector3 point);
    }
}
