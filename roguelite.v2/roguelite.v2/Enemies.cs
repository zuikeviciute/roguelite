using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace roguelite
{
    /// <summary>
    /// This class is used to store enemy location, health and their damage
    /// </summary>
    public class Enemies
    {
        public List<Rect> enemy = new List<Rect>();
        public List<int> enemyhp = new List<int>();
        public List<int> enemydmg = new List<int>();

        public Enemies()
        {
            enemy.Add(new Rect(120, 60, 66, 51));
            enemy.Add(new Rect(420, 180, 66, 51));
            enemy.Add(new Rect(240, 180, 66, 51));
            enemy.Add(new Rect(360, 180, 66, 51));
            enemy.Add(new Rect(600, 300, 66, 51));
            enemy.Add(new Rect(300, 120, 66, 51));
            enemy.Add(new Rect(600, 360, 66, 51));
            enemy.Add(new Rect(540, 180, 66, 51));
            enemy.Add(new Rect(480, 240, 66, 51));
            enemy.Add(new Rect(240, 300, 66, 51));
            enemy.Add(new Rect(480, 240, 66, 51));
            enemy.Add(new Rect(60, 300, 66, 51));
            enemy.Add(new Rect(540, 360, 66, 51));

            enemyhp.Add(10);
            enemyhp.Add(10);
            enemyhp.Add(9);
            enemyhp.Add(9);
            enemyhp.Add(7);
            enemyhp.Add(5);
            enemyhp.Add(3);
            enemyhp.Add(5);
            enemyhp.Add(7);
            enemyhp.Add(9);
            enemyhp.Add(9);
            enemyhp.Add(10);
            enemyhp.Add(10);

            enemydmg.Add(7);
            enemydmg.Add(7);
            enemydmg.Add(5);
            enemydmg.Add(5);
            enemydmg.Add(3);
            enemydmg.Add(2);
            enemydmg.Add(1);
            enemydmg.Add(2);
            enemydmg.Add(3);
            enemydmg.Add(5);
            enemydmg.Add(5);
            enemydmg.Add(7);
            enemydmg.Add(7);
        }
    }
}
