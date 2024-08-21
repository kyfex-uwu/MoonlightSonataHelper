using Celeste;
using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monocle;
using System;
using System.Collections.Generic;
using System.IO;

[CustomEntity("MoonlightSonataHelper/Rain"), Tracked(true)]
class Rain : Entity {

    public readonly List<SkewCollider> sections = new List<SkewCollider>();
    public float skew = 0;
    public readonly Vector2 origPos;
    public readonly float origWidth;

    public Rain(EntityData data, Vector2 offset)
            : this(data.Position + offset, data.Width) {
    }
    public Rain(Vector2 position, float width)
        : base(position) {
        this.origPos = position;
        this.origWidth = width;
        base.Collider = new ColliderList();
        this.addSection(0,0,width);
        base.Depth = -8500;

        this.Add(new PlayerCollider(OnCollide));
    }
    public override void Added(Scene scene) {
        base.Added(scene);
        if (this.SceneAs<Level>().Tracker.GetEntity<RainController>() == null) {
            this.Scene.Add(new RainController());
        }
    }
    private void addSection(float x, float y, float w) {
        var toAdd = new SkewCollider(this.skew, w, 200, x, y);
        this.sections.Add(toAdd);
        ((ColliderList)this.Collider).Add(toAdd);
    }

    private void OnCollide(Player player) {
        player.Die(new Vector2(0,0));
    }

    public override void Update() {
        base.Update();
        //this.CollideDo<Entity>(onCollide);
    }
    private void onCollide(Entity other) {
        //TODO
    }

    internal void updateSkew(float newSkew) {
        this.skew = newSkew;
        foreach (var section in this.sections) section.Skew = newSkew;
    }

    //--

    private class RainChecker : Entity {
        public RainChecker(Vector2 pos, Vector2 end) {
            //this.Collider = 
        }
    }
}
