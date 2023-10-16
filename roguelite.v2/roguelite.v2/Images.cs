using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace roguelite
{
    /// <summary>
    /// This class is used to store all the images
    /// </summary>
    class Images
    {
        public List<Uri> background = new List<Uri>();

        public List<Uri> run = new List<Uri>();
        public List<Uri> runleft = new List<Uri>();
        public List<Uri> rundown = new List<Uri>();
        public List<Uri> runup = new List<Uri>();

        public List<Uri> death = new List<Uri>();

        public List<Uri> enemy = new List<Uri>();
        public List<Uri> enemyleft = new List<Uri>();

        public Uri idle = new Uri(@"D:\roguelite\roguelite-v3\player\idle.png");
        public Uri idleleft = new Uri(@"D:\roguelite\roguelite-v3\player\left\idle left.png");

        public Uri attack = new Uri(@"D:\roguelite\roguelite-v3\player\attack1.png");
        public Uri attackleft = new Uri(@"D:\roguelite\roguelite-v3\player\left\attack1.png");
        public Uri attackup = new Uri(@"D:\roguelite\roguelite-v3\player\up\attackup.png");
        public Uri attackdown = new Uri(@"D:\roguelite\roguelite-v3\player\down\attackdown.png");

        public Uri hurt = new Uri(@"D:\roguelite\roguelite-v3\player\hurt1.png");
        public Uri hurtleft = new Uri(@"D:\roguelite\roguelite-v3\player\left\hurt.png");

        //a list of tips show when the player dies
        public List<string> tips = new List<string>();

        public Images()
        {
            //room backgrounds
            background.Add(new Uri(@"D:\roguelite\roguelite-v3\dungeon\1.png"));
            background.Add(new Uri(@"D:\roguelite\roguelite-v3\dungeon\2.png"));
            background.Add(new Uri(@"D:\roguelite\roguelite-v3\dungeon\3.png"));
            background.Add(new Uri(@"D:\roguelite\roguelite-v3\dungeon\4.png"));
            background.Add(new Uri(@"D:\roguelite\roguelite-v3\dungeon\5.png"));
            background.Add(new Uri(@"D:\roguelite\roguelite-v3\dungeon\6.png"));
            background.Add(new Uri(@"D:\roguelite\roguelite-v3\dungeon\7.png"));
            background.Add(new Uri(@"D:\roguelite\roguelite-v3\dungeon\8.png"));
            background.Add(new Uri(@"D:\roguelite\roguelite-v3\dungeon\9.png"));
            background.Add(new Uri(@"D:\roguelite\roguelite-v3\dungeon\10.png"));
            background.Add(new Uri(@"D:\roguelite\roguelite-v3\dungeon\11.png"));
            background.Add(new Uri(@"D:\roguelite\roguelite-v3\dungeon\12.png"));
            background.Add(new Uri(@"D:\roguelite\roguelite-v3\dungeon\13.png"));

            //run animations for every direction
            runleft.Add(new Uri(@"D:\roguelite\roguelite-v3\player\left\run1 left.png"));
            runleft.Add(new Uri(@"D:\roguelite\roguelite-v3\player\left\run2 left.png"));
            runleft.Add(new Uri(@"D:\roguelite\roguelite-v3\player\left\run3 left.png"));
            runleft.Add(new Uri(@"D:\roguelite\roguelite-v3\player\left\run4 left.png"));
            runleft.Add(new Uri(@"D:\roguelite\roguelite-v3\player\left\run5 left.png"));
            runleft.Add(new Uri(@"D:\roguelite\roguelite-v3\player\left\run6 left.png"));

            run.Add(new Uri(@"D:\roguelite\roguelite-v3\player\run1.png"));
            run.Add(new Uri(@"D:\roguelite\roguelite-v3\player\run2.png"));
            run.Add(new Uri(@"D:\roguelite\roguelite-v3\player\run3.png"));
            run.Add(new Uri(@"D:\roguelite\roguelite-v3\player\run4.png"));
            run.Add(new Uri(@"D:\roguelite\roguelite-v3\player\run5.png"));
            run.Add(new Uri(@"D:\roguelite\roguelite-v3\player\run6.png"));

            rundown.Add(new Uri(@"D:\roguelite\roguelite-v3\player\down\down1.png"));
            rundown.Add(new Uri(@"D:\roguelite\roguelite-v3\player\down\down2.png"));
            rundown.Add(new Uri(@"D:\roguelite\roguelite-v3\player\down\down3.png"));
            rundown.Add(new Uri(@"D:\roguelite\roguelite-v3\player\down\down4.png"));
            rundown.Add(new Uri(@"D:\roguelite\roguelite-v3\player\down\down5.png"));
            rundown.Add(new Uri(@"D:\roguelite\roguelite-v3\player\down\down6.png"));

            runup.Add(new Uri(@"D:\roguelite\roguelite-v3\player\up\up1.png"));
            runup.Add(new Uri(@"D:\roguelite\roguelite-v3\player\up\up2.png"));
            runup.Add(new Uri(@"D:\roguelite\roguelite-v3\player\up\up3.png"));
            runup.Add(new Uri(@"D:\roguelite\roguelite-v3\player\up\up4.png"));
            runup.Add(new Uri(@"D:\roguelite\roguelite-v3\player\up\up5.png"));
            runup.Add(new Uri(@"D:\roguelite\roguelite-v3\player\up\up6.png"));

            //death animation
            death.Add(new Uri(@"D:\roguelite\roguelite-v3\player\death1.png"));
            death.Add(new Uri(@"D:\roguelite\roguelite-v3\player\death2.png"));
            death.Add(new Uri(@"D:\roguelite\roguelite-v3\player\death3.png"));
            death.Add(new Uri(@"D:\roguelite\roguelite-v3\player\death4.png"));
            death.Add(new Uri(@"D:\roguelite\roguelite-v3\player\death5.png"));

            //enemy animations
            enemy.Add(new Uri(@"D:\roguelite\roguelite-v3\bat\bat1.png"));
            enemy.Add(new Uri(@"D:\roguelite\roguelite-v3\bat\bat2.png"));
            enemy.Add(new Uri(@"D:\roguelite\roguelite-v3\bat\bat3.png"));
            enemy.Add(new Uri(@"D:\roguelite\roguelite-v3\bat\bat4.png"));
            enemy.Add(new Uri(@"D:\roguelite\roguelite-v3\bat\bat5.png"));

            enemyleft.Add(new Uri(@"D:\roguelite\roguelite-v3\bat\leftbat1.png"));
            enemyleft.Add(new Uri(@"D:\roguelite\roguelite-v3\bat\leftbat2.png"));
            enemyleft.Add(new Uri(@"D:\roguelite\roguelite-v3\bat\leftbat3.png"));
            enemyleft.Add(new Uri(@"D:\roguelite\roguelite-v3\bat\leftbat4.png"));
            enemyleft.Add(new Uri(@"D:\roguelite\roguelite-v3\bat\leftbat5.png"));

            tips.Add("Tip: Be careful not to jump into a hole!");
            tips.Add("Tip: Enemies get stronger the further you go!");
            tips.Add("Tip: Use jump to move faster through the map!");
            tips.Add("Tip: The right side of the map is a bit easier!");
            tips.Add("Tip: The game is NEVER paused!");
        }
    }
}
