using Terraria;
using Terraria.ID;
using TerrariaApi.Server;
using Microsoft.Xna.Framework;
using TShockAPI;
using TShockAPI.Hooks;

namespace BombToGrenade {
    [ApiVersion(2, 1)]
    public class BombToGrenade : TerrariaPlugin {
        public override string Name => "PrimeBombToMissile";
        public override Version Version => new Version(1, 0, 3);
        public override string Author => "Soofa, Sors";
        public override string Description => "Changes Skeletron Prime's bombs to missiles.";

        static int SlowShootingTimer = 0;
        static int FastShootingTimer = 0;
        public BombToGrenade(Main game) : base(game) {
        }

        public override void Initialize() {
            GeneralHooks.ReloadEvent += OnReloadEvent;
            ServerApi.Hooks.GameUpdate.Register(this, OnGameUpdate);

            TShock.Config.Settings.DisablePrimeBombs = true;
        }

        private void OnGameUpdate(EventArgs args) {
            foreach (var npc in Main.npc)
            {
                if (npc.netID == NPCID.PrimeCannon && npc.active)
                {
                    if (npc.ai[2] == 0 && ((npc.ai[3] > 0 && npc.ai[3] < 599) || (npc.ai[3] > 600 && npc.ai[3] <= 1100))) //trash aim, long firing interval, head is not spinning
                    {
                        if (++SlowShootingTimer == 140) // = 2.333 seconds, means that it will fire every 2.333 seconds
                        {
                            Vector2 targetPosition = Main.rand.NextVector2Unit();
                            Projectile.NewProjectile(null, npc.position, 20 * targetPosition, ProjectileID.Missile, npc.damage, 1f);
                            Projectile.NewProjectile(null, npc.position, 20 * targetPosition.RotatedBy(MathHelper.ToRadians(20f)), ProjectileID.Missile, npc.damage, 1f);
                            Projectile.NewProjectile(null, npc.position, 20 * targetPosition.RotatedBy(MathHelper.ToRadians(-20f)), ProjectileID.Missile, npc.damage, 1f);
                            SlowShootingTimer = 0;
                        }
                    }
                    if (npc.ai[2] == 0 && (npc.ai[3] == 599 || npc.ai[3] == 600 || npc.ai[3] == 0)) //trash aim, short firing interval, head is spinning 
                    {
                        if (++FastShootingTimer == 40) // = 0.666 seconds, means that it will fire every 0.666 seconds
                        {
                            Vector2 targetPosition = Main.rand.NextVector2Unit();
                            Projectile.NewProjectile(null, npc.position, 20 * targetPosition, ProjectileID.Missile, npc.damage, 1f);
                            Projectile.NewProjectile(null, npc.position, 20 * targetPosition.RotatedBy(MathHelper.ToRadians(5f)), ProjectileID.Missile, npc.damage, 1f);
                            Projectile.NewProjectile(null, npc.position, 20 * targetPosition.RotatedBy(MathHelper.ToRadians(-5f)), ProjectileID.Missile, npc.damage, 1f);
                            FastShootingTimer = 0;
                        }
                    }
                    if (npc.ai[2] == 1 && npc.ai[3] >= 0 && npc.ai[3] <= 300) //godly aim, short firing interval, head is spinning, change missile to rocket for fun
                    {
                        if (++FastShootingTimer == 40) // = 0.666 seconds, means that it will fire every 0.666 seconds
                        {
                            Vector2 targetPosition = Main.player[npc.target].position;
                            Vector2 direction = targetPosition - npc.position;
                            direction.Normalize();
                            Projectile.NewProjectile(null, npc.position, 20 * direction, ProjectileID.RocketSkeleton, npc.damage, 1f);
                            Projectile.NewProjectile(null, npc.position, 20 * direction.RotatedBy(MathHelper.ToRadians(8f)), ProjectileID.RocketSkeleton, npc.damage, 1f);
                            Projectile.NewProjectile(null, npc.position, 20 * direction.RotatedBy(MathHelper.ToRadians(-8f)), ProjectileID.RocketSkeleton, npc.damage, 1f);
                            FastShootingTimer = 0;
                        }
                    }
                }
            }
        }

        private void OnReloadEvent(ReloadEventArgs args) {
            TShock.Config.Settings.DisablePrimeBombs = true;
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                ServerApi.Hooks.GameUpdate.Deregister(this, OnGameUpdate);
                GeneralHooks.ReloadEvent -= OnReloadEvent;
            }
            base.Dispose(disposing);
        }
    }
}