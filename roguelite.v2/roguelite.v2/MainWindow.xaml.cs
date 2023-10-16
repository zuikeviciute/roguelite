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
using System.Windows.Threading;

namespace roguelite
{
    public partial class MainWindow : Window
    {
        Interact itr = new Interact();
        Images img = new Images();
        Enemies enm = new Enemies();

        //used to track current direction and if the player is moving or idle
        public enum Vector { idle, idleleft, idleup, idledown, left, right, up, down};
        Vector vector = Vector.idle;

        //used as a copy of the player to check for collision and interactions with other objects,
        //only rect can use IntersectsWith(..) function, but rect can't be visualy displayed in wpf
        //so any object in the game has to have a rectangle version (for being visible) and a rect version (to be able to interact with other rect objects)
        Rect player1;
        Random rnd = new Random();

        int x, y; //used to track the players coordinates
        int aindx = 0, h = 0, t = 0, enmyindx = 0; // indexes used for animations
        int speed = 5, hp = 100, dmg = 1, timescore=0; //player speed, health and damage
        int complete = 0;
        int strwin = 0;

        public MainWindow()
        {
            InitializeComponent();
            itr.CreateArea();

            //enables adding score at the end of the game
            AddScore.IsEnabled = true;

            //making overlays hidden until they're activated with a keybind
            Options.Visibility = Visibility.Hidden;
            Dead.Visibility = Visibility.Hidden;
            GameComplete.Visibility = Visibility.Hidden;
            ControlsOverlayGame.Visibility = Visibility.Hidden;

            //setting the position of the enemy creature that is later adjusted depending on the room
            Canvas.SetLeft(slime, enm.enemy[itr.area].X);
            Canvas.SetTop(slime, enm.enemy[itr.area].Y);

            hptxt.Text = hp.ToString();

            //timers are used for animations
            var timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(50);
            timer.Start();
            timer.Tick += _timer_Tick;

            var timerenemy = new System.Windows.Threading.DispatcherTimer();
            timerenemy.Interval = TimeSpan.FromMilliseconds(50); 
            timerenemy.Start();
            timerenemy.Tick += _timer_Tick_enemy;

            //this timer is used for smoother damage taking
            var timerdamage = new System.Windows.Threading.DispatcherTimer();
            timerdamage.Interval = TimeSpan.FromMilliseconds(500);
            timerdamage.Start();
            timerdamage.Tick += _timer_Tick_damage;
        }

        /// <summary>
        /// The key up function, active while the player is not moving
        /// This function sets the idle position of the character depending on which direction it was moving
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Idle(object sender, KeyEventArgs e)
        {
            //if the players health is 0 this function is no longer needed
            if (hp <= 0) return;

            //fill the players rectangle with an idle image, so even if the player stops mid run he will be in a standing position and not a runing  one
            switch (vector)
            {
                case Vector.idleleft:
                case Vector.left:
                    player.Fill = new ImageBrush { ImageSource = new BitmapImage(img.idleleft) };
                    vector = Vector.idleleft;
                    break;

                case Vector.idle:
                case Vector.right:
                    player.Fill = new ImageBrush { ImageSource = new BitmapImage(img.idle) };
                    vector = Vector.idle;
                    break;

                case Vector.idleup:
                case Vector.up:
                    player.Fill = new ImageBrush { ImageSource = new BitmapImage(img.runup[0]) };
                    vector = Vector.idleup;
                    break;

                case Vector.idledown:
                case Vector.down:
                    player.Fill = new ImageBrush { ImageSource = new BitmapImage(img.rundown[0]) };
                    vector = Vector.idledown;
                    break;

                default:
                    break;
            }
            player.Width = 45;
            player.Height = 60;
        }

        /// <summary>
        /// Key down function, controls key inputs - player movement with arrow keys or WASD, spacebar (jump), q (attack) and ESC to open the options menu
        /// Updates the player location on the canvas depending on the keys pressed
        /// Calls collision function with different list parameters to check collision with different objects (obstacles, enemies)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Interactions(object sender, KeyEventArgs e)
        {
            //if the players health is 0 he cannot move or take any other actions, if the player defeats all the enemies he completes the game
            if (hp <= 0 || complete == 13) return;

            //sets the collision and boundries variables to a value returned by the collision function
            Collision(itr.obstacles[itr.area]);
            Collision(itr.boundries);

            //gets the most recent location of the player (rectangle) and sets it to the player1 (rect) as only rect can use function .IntersectsWith(...) which is the way I check player and obstacle collision
            player1 = new Rect(Canvas.GetLeft(player), Canvas.GetTop(player), player.Width, player.Height);

            //sets the x and y value to the current location of the player
            x = (int)Canvas.GetLeft(player);
            y = (int)Canvas.GetTop(player);

            //checks key presses and responds accordingly
            switch (e.Key)
            {
                //movement keys, moves speed/sec while one of the direction keys is pressed down
                case Key.Left:
                case Key.A:
                    vector = Vector.left;
                    x -= speed;
                    break;

                case Key.Right:
                case Key.D:
                    vector = Vector.right;
                    x += speed;
                    break;

                case Key.Up:
                case Key.W:
                    vector = Vector.up;
                    y -= speed;
                    break;

                case Key.Down:
                case Key.S:
                    vector = Vector.down;
                    y += speed;
                    break;

                //the jump key, instantly moves the player forward in any direction the player is facing, this allows the player to jump over obstacles, but also jump into holes, which results in death
                case Key.Space:
                    switch (vector)
                    {
                        case Vector.idleleft:
                        case Vector.left:
                            x = (int)Canvas.GetLeft(player) - 207;
                            if (x < 15) x = 17;
                            player1 = new Rect(x, Canvas.GetTop(player), player.Width, player.Height);
                            break;

                        case Vector.idle:
                        case Vector.right:
                            x = (int)Canvas.GetLeft(player) + 207;
                            if (x > 705) x = 703 - (int)player.Width;
                            player1 = new Rect(x, Canvas.GetTop(player), player.Width, player.Height);
                            break;

                        case Vector.idleup:
                        case Vector.up:
                            y = (int)Canvas.GetTop(player) - 160;
                            if (y < 15) y = 17;
                            player1 = new Rect(Canvas.GetLeft(player), y, player.Width, player.Height);
                            break;

                        default:
                            y = (int)Canvas.GetTop(player) + 160;
                            if (y > 465) y = 464 - (int)player.Height;
                            player1 = new Rect(Canvas.GetLeft(player), y, player.Width, player.Height);
                            break;
                    }

                    //if the jump lands inside of an obstacle (all of the obstacles are holes in the ground) the player dies
                    foreach (Rect ob in itr.obstacles[itr.area])
                    {
                        if (ob.IntersectsWith(player1))
                        {
                            hp = 0;
                            Canvas.SetLeft(player, x);
                            Canvas.SetTop(player, y);
                            return;
                        }
                    }
                    break;

                //opens the options menu where u can select to return to the game or go back to main menu
                case Key.Escape:
                    if (t == 0)
                    {
                        Options.Visibility = Visibility.Visible;
                        t++;
                    }
                    else
                    {
                        Options.Visibility = Visibility.Hidden;
                        t = 0;
                    }
                    break;

                //allows the player to attack, increases the players size as the image for attacking is wider
                case Key.Q:
                    switch (vector)
                    {
                        case Vector.idleleft:
                        case Vector.left:
                            player.Width = 78;
                            player.Fill = new ImageBrush { ImageSource = new BitmapImage(img.attackleft) };
                            break;

                        case Vector.idle:
                        case Vector.right:
                            player.Width = 78;
                            player.Fill = new ImageBrush { ImageSource = new BitmapImage(img.attack) };
                            break;

                        case Vector.idleup:
                        case Vector.up:
                            player.Height = 68;
                            player.Fill = new ImageBrush { ImageSource = new BitmapImage(img.attackup) };
                            break;

                        default:
                            player.Height = 84;
                            player.Fill = new ImageBrush { ImageSource = new BitmapImage(img.attackdown) };
                            break;
                    }

                    player1 = new Rect(Canvas.GetLeft(player), Canvas.GetTop(player), player.Width, player.Height);
                    //checks the enemy in the room intersects with the player
                    if (player1.IntersectsWith(enm.enemy[itr.area]))
                    {
                        enm.enemyhp[itr.area] -= dmg;
                        if (enm.enemyhp[itr.area] == 0)
                        {
                            complete++;
                            enemiesdefeated.Text = complete.ToString();
                            Canvas.SetLeft(slime, Canvas.GetLeft(remove));
                            Canvas.SetTop(slime, Canvas.GetTop(remove));
                            enm.enemy[itr.area] = new Rect(Canvas.GetLeft(remove), Canvas.GetTop(remove), enm.enemy[itr.area].Width, enm.enemy[itr.area].Height);
                        }
                    }
                    break;

                default:
                    break;
            }
            //checks if player is intersecting with any entrances
            Collision(itr.entrances);

            //sets the players location if it has changed after every key press
            Canvas.SetLeft(player, x);
            Canvas.SetTop(player, y);

            //updates the player1 (rect) with the new location
            player1 = new Rect(x, y, player.Width, player.Height);

        }

        /// <summary>
        /// Checks players collision with obstacles and enemies, when called by the Interactions function or a timer
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public void Collision(List<Rect> list)
        {
            //checks if the given parameter is the enemy list
            if(list == enm.enemy)
            {
                player1 = new Rect(Canvas.GetLeft(player), Canvas.GetTop(player), 45, 60);
                //checks if the player intersects with the enemy in the current area
                if (enm.enemy[itr.area].IntersectsWith(player1))
                {
                    //if the player is touching the enemy he takes damage, this damage depends on the area, rooms further from the starting room (room 7) have enemies that have more health and do more damage
                    hp -= enm.enemydmg[itr.area];
                    hptxt.Text = hp.ToString();
                    //there are 2 images for taking damage one facing left and the other facing right, they are used when the player is intersecting with an enemy depending on the characters direction
                    if (vector == Vector.left || vector == Vector.idleleft) player.Fill = new ImageBrush { ImageSource = new BitmapImage(img.hurtleft) };
                    else player.Fill = new ImageBrush { ImageSource = new BitmapImage(img.hurt) };
                }
            }

            //if the obstacles intersect with the player and the given list is either the obstacle or the boundry list the value is return, which cooresponds to a direction the player cannot move in
            if (list == itr.obstacles[itr.area] || list == itr.boundries)
            {
                //checks if any of the obstacles in the given list intersect with the player
                foreach (Rect rct in list)
                {
                    if (rct.IntersectsWith(player1))
                    {
                        int index = list.IndexOf(rct);

                        switch (vector)
                        {
                            case Vector.idleleft:
                            case Vector.left:
                                Canvas.SetLeft(player, Canvas.GetLeft(player) + speed);
                                return;

                            case Vector.idle:
                            case Vector.right:
                                Canvas.SetLeft(player, Canvas.GetLeft(player) - speed);
                                return;

                            case Vector.idleup:
                            case Vector.up:
                                Canvas.SetTop(player, Canvas.GetTop(player) + speed);
                                return;

                            case Vector.idledown:
                            case Vector.down:
                                Canvas.SetTop(player, Canvas.GetTop(player) - speed);
                                return;

                            default: return;
                        }
                    }
                }
            }
                
            if (list == itr.entrances)
            {
                //the player is going backward so the area number is decreased
                if (itr.back.IntersectsWith(player1))
                {
                    itr.area--;
                    //The room image is changed here depending on the room number
                    gameArea.Fill = new ImageBrush { ImageSource = new BitmapImage(img.background[itr.area]) };

                    if (itr.back == itr.left) x = 650;
                    if (itr.back == itr.right) x = 25;
                    if (itr.back == itr.top) y = 399;
                    if (itr.back == itr.bottom) y = 25;

                    //enemy location also changes depending on area / room number so it has to be adjusted in this function aswell
                    Canvas.SetLeft(slime, enm.enemy[itr.area].X);
                    Canvas.SetTop(slime, enm.enemy[itr.area].Y);
                }

                //the player is going forward so the area / room number is increased
                if (itr.forward.IntersectsWith(player1))
                {
                    itr.area++;
                    //The room image is changed here depending on the room number
                    gameArea.Fill = new ImageBrush { ImageSource = new BitmapImage(img.background[itr.area]) };

                    if (itr.forward == itr.left) x = 650;
                    if (itr.forward == itr.right) x = 25;
                    if (itr.forward == itr.top) y = 399;
                    if (itr.forward == itr.bottom) y = 25;

                    //enemy location also changes depending on area / room number so it has to be adjusted in this function aswell
                    Canvas.SetLeft(slime, enm.enemy[itr.area].X);
                    Canvas.SetTop(slime, enm.enemy[itr.area].Y);
                }

                //The function itr.CreateArea changes the value of back and forward depending on the location of the doors in different rooms
                itr.CreateArea();
            }
        }
        
        //
        //              <<<     button and time functions below     >>>
        //


        /// <summary>
        /// Esc opens the options menu, the button Return returns the user to the game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _return_Click(object sender, RoutedEventArgs e)
        {
            Options.Visibility = Visibility.Hidden;
            t = 0;
        }

        /// <summary>
        /// Returns the user to the first window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void main_menu_Click(object sender, RoutedEventArgs e)
        {
            StartingWindow swin = new StartingWindow();
            swin.Owner = this;
            swin.Show();
            this.Hide();
        }

        /// <summary>
        /// Closes the game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void endgame_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Displays the controls overlay
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void controls_Click(object sender, RoutedEventArgs e)
        {
            ControlsOverlayGame.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Closes the controls overlay
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void close_Click(object sender, RoutedEventArgs e)
        {
            ControlsOverlayGame.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Adds your score to the leaderboard, if you complete the game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddScore_Click(object sender, RoutedEventArgs e)
        {
            double num;
            num = ((1000 / (timescore / 2)) + hp) * 10;
            string score = ScoreboardName.Text + "\t" + num + "\n";
            System.IO.File.AppendAllText(@"C:\roguelite\roguelite-v3\Scoreboard.txt", score);
            AddScore.IsEnabled = false;
        }

        /// <summary>
        /// Timer used for run and death animations
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _timer_Tick(object sender, EventArgs e)
        {
            //Changes the window from the game window to the starting window, which has the options to play the game, check controls, check credits or exit
            if(strwin==0)
            {
                StartingWindow swin = new StartingWindow();
                swin.Owner = this;
                swin.Show();
                this.Hide();
                strwin++;
            }

            //used for the players death animation when h reaches -1 the animation is finished and the function is no longer accesible as the player is unable to continue and must restart the game
            if (h == -1) return;

            //Diplays the players movement animation depending on the current movement direction (vector)
            //each movement direction has 6 images for its animation, after the 6th images is reached it starts over from the first image
            switch (vector)
            {
                case Vector.left:
                    if (aindx > 5) aindx = 0;
                    player.Fill = new ImageBrush { ImageSource = new BitmapImage(img.runleft[aindx]) };
                    aindx++;
                    break;

                case Vector.right:
                    if (aindx > 5) aindx = 0;
                    player.Fill = new ImageBrush { ImageSource = new BitmapImage(img.run[aindx]) };
                    aindx++;
                    break;

                case Vector.down:
                    if (aindx > 5) aindx = 0;
                    player.Fill = new ImageBrush { ImageSource = new BitmapImage(img.rundown[aindx]) };
                    aindx++;
                    break;

                case Vector.up:
                    if (aindx > 5) aindx = 0;
                    player.Fill = new ImageBrush { ImageSource = new BitmapImage(img.runup[aindx]) };
                    aindx++;
                    break;

                default:
                    break;
            }

            //if the players hp reaches 0 begins the death animation
            if (hp <= 0)
            {
                Dead.Visibility = Visibility.Visible;
                player.Width = 54;
                player.Fill = new ImageBrush { ImageSource = new BitmapImage(img.death[h]) };
                int _h = (int)Canvas.GetTop(player);
                h++;
                //the death animation has 5 images so when all of them are shown, the h is set to -1 and this function becomes no longer available
                if (h > 4)
                {
                    int rndindx = rnd.Next(0, 5); //random index
                    tip.Text = img.tips[rndindx]; //random tip when a player dies
                    h = -1;
                }
            }

            //displays "congratulations you have completed the game" when the player defeats all the enemies
            if (complete == 13)
            {
                GameComplete.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Timer used for enemy animations, this timer updates more frequently so the animation is smoother
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _timer_Tick_enemy(object sender, EventArgs e)
        {
            if (hp <= 0) return;

            //the enemy (slime) animation has 9 images, after reaching the 9th image it starts over again from the begining
            if (enmyindx > 4) enmyindx = 0;
            
            //gets the most current enemy and player positions
            int playerPosX = (int)Canvas.GetLeft(player);
            int enemyPosX = (int)Canvas.GetLeft(slime);
            int playerPosY = (int)Canvas.GetTop(player);
            int enemyPosY = (int)Canvas.GetTop(slime);

            //enemies chasing the player logic updates both rectangle and rect form of the enemy
            if (playerPosX < enemyPosX && playerPosY < enemyPosY)
            {
                Canvas.SetLeft(slime, Canvas.GetLeft(slime) - 1);
                Canvas.SetTop(slime, Canvas.GetTop(slime) - 1);
                enm.enemy[itr.area] = new Rect(Canvas.GetLeft(slime), Canvas.GetTop(slime), slime.Width, slime.Height);
                slime.Fill = new ImageBrush { ImageSource = new BitmapImage(img.enemyleft[enmyindx]) };
            }
            else if (playerPosX > enemyPosX && playerPosY > enemyPosY)
            {
                Canvas.SetLeft(slime, Canvas.GetLeft(slime) + 1);
                Canvas.SetTop(slime, Canvas.GetTop(slime) + 1);
                enm.enemy[itr.area] = new Rect(Canvas.GetLeft(slime), Canvas.GetTop(slime), slime.Width, slime.Height);
                slime.Fill = new ImageBrush { ImageSource = new BitmapImage(img.enemy[enmyindx]) };
            }
            else if (playerPosX < enemyPosX && playerPosY > enemyPosY)
            {
                Canvas.SetLeft(slime, Canvas.GetLeft(slime) - 1);
                Canvas.SetTop(slime, Canvas.GetTop(slime) + 1);
                enm.enemy[itr.area] = new Rect(Canvas.GetLeft(slime), Canvas.GetTop(slime), slime.Width, slime.Height);
                slime.Fill = new ImageBrush { ImageSource = new BitmapImage(img.enemyleft[enmyindx]) };
            }
            else if(playerPosX > enemyPosX && playerPosY < enemyPosY)
            {
                Canvas.SetLeft(slime, Canvas.GetLeft(slime) + 1);
                Canvas.SetTop(slime, Canvas.GetTop(slime) - 1);
                enm.enemy[itr.area] = new Rect(Canvas.GetLeft(slime), Canvas.GetTop(slime), slime.Width, slime.Height);
                slime.Fill = new ImageBrush { ImageSource = new BitmapImage(img.enemy[enmyindx]) };
            }
            else if(playerPosX < enemyPosX && playerPosY == enemyPosY)
            {
                Canvas.SetLeft(slime, Canvas.GetLeft(slime) - 1);
                enm.enemy[itr.area] = new Rect(Canvas.GetLeft(slime), Canvas.GetTop(slime), slime.Width, slime.Height);
                slime.Fill = new ImageBrush { ImageSource = new BitmapImage(img.enemyleft[enmyindx]) };
            }
            else if (playerPosX > enemyPosX && playerPosY == enemyPosY)
            {
                Canvas.SetLeft(slime, Canvas.GetLeft(slime) + 1);
                enm.enemy[itr.area] = new Rect(Canvas.GetLeft(slime), Canvas.GetTop(slime), slime.Width, slime.Height);
                slime.Fill = new ImageBrush { ImageSource = new BitmapImage(img.enemy[enmyindx]) };
            }
            else if (playerPosX == enemyPosX && playerPosY < enemyPosY)
            {
                Canvas.SetTop(slime, Canvas.GetTop(slime) - 1);
                enm.enemy[itr.area] = new Rect(Canvas.GetLeft(slime), Canvas.GetTop(slime), slime.Width, slime.Height);
                slime.Fill = new ImageBrush { ImageSource = new BitmapImage(img.enemy[enmyindx]) };
            }
            else if (playerPosX == enemyPosX && playerPosY > enemyPosY)
            {
                Canvas.SetTop(slime, Canvas.GetTop(slime) + 1);
                enm.enemy[itr.area] = new Rect(Canvas.GetLeft(slime), Canvas.GetTop(slime), slime.Width, slime.Height);
                slime.Fill = new ImageBrush { ImageSource = new BitmapImage(img.enemy[enmyindx]) };
            }
            enmyindx++;
        }

        /// <summary>
        /// Timer used to make damage taken by the player from the enemies a bit smoother
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _timer_Tick_damage(object sender, EventArgs e)
        {
            if (hp <= 0) return;

            if (hp <= 0 || complete == 13) return;
            timescore++;

            //checks if the player is intersecting with any enemies every 500msec, this makes the game a bit easier as before the damage was overwhelming
            Collision(enm.enemy);
        }
    }
}
