using Terraria;
using TerrariaApi.Server;
using Microsoft.Xna.Framework;

namespace BombToGrenade {
    [ApiVersion(2, 1)]
    public class BombToGrenade : TerrariaPlugin {
        public override string Name => "PrimeBombToMissile";
        public override Version Version => new Version(1, 0, 1);
        public override string Author => "Soofa";
        public override string Description => "Changes Skeletron Prime's bombs to missiles.";
        static int ticks = 0;
        public BombToGrenade(Main game) : base(game) {
        }

        public override void Initialize() {
            ServerApi.Hooks.GameUpdate.Register(this, OnGameUpdate);
        }
        protected override void Dispose(bool disposing) {
            if (disposing) {
                ServerApi.Hooks.GameUpdate.Deregister(this, OnGameUpdate);
            }
            base.Dispose(disposing);
        }

        void OnGameUpdate(EventArgs args) {
            foreach (var npc in Main.npc) {
                if (npc.netID == 128 && npc.active) {
                    if (ticks == 100) {
                        Random rand = new Random();
                        Projectile.NewProjectile(Projectile.GetNoneSource(), npc.position, new Vector2(rand.Next(1, 20) - 10, -1 * rand.Next(1, 50)), Type: 350, Damage: 30, KnockBack: 20, ai0: 16, ai1: 16, ai2: 16);
                        ticks = 0;
                    }
                    else {
                        ticks++;
                    }
                }
            }
        }
    }
}