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
    /// This class is used to hold all the information about obstacles and map boundries as well as adjust the entrances when changing rooms
    /// The best way I found to change obstacle location depending on the room is to make a list of rect lists
    /// the area number is used as an index to go through the list from one room to another
    /// each area has several obstacles in their own rect list
    /// </summary>
    class Interact
    {
        public List<List<Rect>> obstacles = new List<List<Rect>>();
        public List<Rect> area1 = new List<Rect>();
        public List<Rect> area2 = new List<Rect>();
        public List<Rect> area3 = new List<Rect>();
        public List<Rect> area4 = new List<Rect>();
        public List<Rect> area5 = new List<Rect>();
        public List<Rect> area6 = new List<Rect>();
        public List<Rect> area7 = new List<Rect>();
        public List<Rect> area8 = new List<Rect>();
        public List<Rect> area9 = new List<Rect>();
        public List<Rect> area10 = new List<Rect>();
        public List<Rect> area11 = new List<Rect>();
        public List<Rect> area12 = new List<Rect>();
        public List<Rect> area13 = new List<Rect>();

        //The boundries of the map
        public List<Rect> boundries = new List<Rect>();
        public Rect mapleft = new Rect(5, 0, 1, 480);
        public Rect mapright = new Rect(705, 0, 1, 480);
        public Rect maptop = new Rect(0, 5, 720, 1);
        public Rect mapbottom = new Rect(0, 465, 720, 1);

        //Location of all the possible entrances
        public List<Rect> entrances = new List<Rect>();
        public Rect left = new Rect(10, 180, 1, 120);
        public Rect right = new Rect(700, 180, 1, 120);
        public Rect top = new Rect(300, 10, 120, 1);
        public Rect bottom = new Rect(300, 460, 120, 1);

        //every room has 2 entrances, these entrances are changed depending on the current room / area
        public Rect back = new Rect(0, 0, 0, 0);
        public Rect forward = new Rect(0, 0, 0, 0);

        //the area number, the player starts in room 7, which is 6 as arrays start from 0 and I named the rooms 1-13
        public int area = 6;

        public Interact()
        {
            //everything gets added to the lists

            obstacles.Add(area1);
            obstacles.Add(area2);
            obstacles.Add(area3);
            obstacles.Add(area4);
            obstacles.Add(area5);
            obstacles.Add(area6);
            obstacles.Add(area7);
            obstacles.Add(area8);
            obstacles.Add(area9);
            obstacles.Add(area10);
            obstacles.Add(area11);
            obstacles.Add(area12);
            obstacles.Add(area13);

            boundries.Add(mapleft);
            boundries.Add(mapright);
            boundries.Add(maptop);
            boundries.Add(mapbottom);

            entrances.Add(left);
            entrances.Add(right);
            entrances.Add(top);
            entrances.Add(bottom);

            area1.Add(new Rect(240, 0, 240, 480));

            area2.Add(new Rect(120, 180, 60, 120));
            area2.Add(new Rect(540, 180, 60, 120));

            area3.Add(new Rect(180, 60, 60, 60));
            area3.Add(new Rect(120, 120, 60, 60));
            area3.Add(new Rect(480, 60, 60, 60));
            area3.Add(new Rect(540, 120, 60, 60));

            area4.Add(new Rect(0, 0, 240, 480));
            area4.Add(new Rect(480, 0, 240, 480));

            area5.Add(new Rect(0, 0, 720, 80));
            area5.Add(new Rect(180, 300, 360, 60));

            area6.Add(new Rect(0, 0, 720, 80));
            area6.Add(new Rect(0, 405, 720, 75));
            area6.Add(new Rect(186, 0, 57, 720));

            area7.Add(new Rect(60, 120, 120, 60));
            area7.Add(new Rect(300, 120, 120, 60));
            area7.Add(new Rect(540, 120, 120, 60));

            area8.Add(new Rect(0, 0, 720, 80));
            area8.Add(new Rect(0, 405, 720, 75));

            area9.Add(new Rect(60, 300, 60, 60));
            area9.Add(new Rect(120, 360, 60, 60));
            area9.Add(new Rect(540, 360, 60, 60));
            area9.Add(new Rect(600, 300, 60, 60));

            area10.Add(new Rect(0, 0, 240, 480));
            area10.Add(new Rect(480, 0, 240, 480));
            area10.Add(new Rect(240, 180, 240, 90));

            area11.Add(new Rect(60, 60, 60, 60));
            area11.Add(new Rect(120, 120, 60, 60));
            area11.Add(new Rect(600, 360, 60, 60));
            area11.Add(new Rect(540, 300, 60, 60));

            area12.Add(new Rect(0, 0, 720, 120));
            area12.Add(new Rect(0, 360, 720, 120));
            area12.Add(new Rect(120, 120, 120, 240));
            area12.Add(new Rect(480, 120, 120, 240));

            area13.Add(new Rect(240, 0, 240, 480));
        }

        /// <summary>
        /// This adjusts the entrances depending on the area the player is in
        /// </summary>
        public void CreateArea()
        {
            switch (area)
            {
                case 0:
                    back = right;
                    forward = left;
                    break;

                case 1:
                    back = right;
                    forward = left;
                    break;

                case 2:
                    back = right;
                    forward = top;
                    break;

                case 3:
                    back = bottom;
                    forward = top;
                    break;

                case 4:
                    back = bottom;
                    forward = right;
                    break;

                case 5:
                    back = left;
                    forward = right;
                    break;

                case 6:
                    back = left;
                    forward = right;
                    break;

                case 7:
                    back = left;
                    forward = right;
                    break;

                case 8:
                    back = left;
                    forward = bottom;
                    break;

                case 9:
                    back = top;
                    forward = bottom;
                    break;

                case 10:
                    back = top;
                    forward = left;
                    break;

                case 11:
                    back = right;
                    forward = left;
                    break;

                case 12:
                    back = right;
                    forward = left;
                    break;

                default:
                    break;
            }
        }
    }
}
