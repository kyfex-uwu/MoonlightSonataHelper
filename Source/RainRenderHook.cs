using Celeste;
using Microsoft.Xna.Framework;
using Monocle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Tracked(true)]
class RainRenderHook : Component{
    public Action<Matrix> RenderRain;
    public RainRenderHook(Action<Matrix> render)
        : base(active: false, visible: true) {
        RenderRain = render;
    }
}
