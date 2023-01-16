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
using System.Windows.Shapes;
using System.Windows.Navigation;
using System.IO;

namespace Шашки
{
    public partial class gamewnd : Window
    {
        int player = 1, q = 0, cntgo = 0;
        int[] cor = new int[2] { -1, -1 };
        int[,,] field = {
                        {{0, 0}, {-1, 0}, {0, 0}, {-1, 0}, {0, 0}, {-1, 0}, {0, 0}, {-1, 0}},
                        {{-1, 0},{ 0, 0}, {-1, 0}, {0, 0}, {-1, 0}, {0, 0}, {-1, 0}, {0, 0} },
                        {{0, 0}, { -1, 0}, { 0, 0}, { -1, 0}, { 0, 0}, { -1, 0}, { 0, 0}, { -1, 0}},
                        {{0, 0}, { 0,0}, { 0,0}, { 0,0}, { 0,0}, { 0,0}, { 0,0}, { 0,0}},
                        {{0, 0}, { 0,0}, { 0,0}, { 0,0}, { 0,0}, { 0,0}, { 0,0}, { 0,0}},
                        {{ 1, 0}, { 0, 0}, { 1, 0}, { 0, 0}, { 1, 0}, { 0, 0}, { 1, 0}, { 0, 0}},
                        {{0, 0}, {1,0}, {0,0}, {1,0}, {0,0}, {1,0}, {0,0}, {1,0} },
                        {{ 1, 0}, { 0, 0}, { 1, 0}, { 0, 0}, { 1, 0}, { 0, 0}, { 1, 0}, { 0, 0}}
            };
        readonly int[,,] new_field = {
                        {{0, 0}, {-1, 0}, {0, 0}, {-1, 0}, {0, 0}, {-1, 0}, {0, 0}, {-1, 0}},
                        {{-1, 0},{ 0, 0}, {-1, 0}, {0, 0}, {-1, 0}, {0, 0}, {-1, 0}, {0, 0} },
                        {{0, 0}, { -1, 0}, { 0, 0}, { -1, 0}, { 0, 0}, { -1, 0}, { 0, 0}, { -1, 0}},
                        {{0, 0}, { 0,0}, { 0,0}, { 0,0}, { 0,0}, { 0,0}, { 0,0}, { 0,0}},
                        {{0, 0}, { 0,0}, { 0,0}, { 0,0}, { 0,0}, { 0,0}, { 0,0}, { 0,0}},
                        {{ 1, 0}, { 0, 0}, { 1, 0}, { 0, 0}, { 1, 0}, { 0, 0}, { 1, 0}, { 0, 0}},
                        {{0, 0}, {1,0}, {0,0}, {1,0}, {0,0}, {1,0}, {0,0}, {1,0} },
                        {{ 1, 0}, { 0, 0}, { 1, 0}, { 0, 0}, { 1, 0}, { 0, 0}, { 1, 0}, { 0, 0}}
            };
        public gamewnd()
        {
            
            InitializeComponent();
            Draw();
            int b = 0, w = 0;
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    if (field[y, x, 0] == 1) w += 1;
                    else if (field[y, x, 0] == -1) b += 1;
                }
            }
            l.Content = w;
            l2.Content = b;
            turn.Content = "Ход белых";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (cntgo == 0)
            {
                cor[0] = -1;
                cor[1] = -1;
            }
        }

        private void can_MouseDown(object sender, MouseButtonEventArgs e)
        {
            int mbx = Convert.ToInt32(Math.Ceiling(e.GetPosition(null).X)) / Convert.ToInt32(cell.Width), mby = Convert.ToInt32(Math.Ceiling(e.GetPosition(null).Y)) / Convert.ToInt32(cell.Height);
            if (cor[0] == -1 && cor[1] == -1) //выбор
            {
                q = 0;
                if (field[mby, mbx, 0] == player)
                {
                    cor[0] = mbx;
                    cor[1] = mby;
                    if (field[mby, mbx, 1] == 1) q = 1; // королева ли выбранная шашка
                }
            }
            else //ход
            {
                if (field[mby, mbx, 0] == 0 && q == 0) //не королева
                {
                    if ((mbx-1 == cor[0] || mbx + 1 == cor[0]) && mby == cor[1] - 1 && player == 1 && cntgo == 0) //обычный ход белой шашки
                    {
                        field[mby, mbx, 0] = player;
                        field[cor[1], cor[0], 0] = 0;
                        cor[0] = -1;
                        cor[1] = -1;
                        player *= -1;
                    }
                    else if ((mbx - 1 == cor[0] || mbx + 1 == cor[0]) && mby == cor[1] + 1 && player == -1 && cntgo == 0) //обычный ход черной шашки
                    {
                        field[mby, mbx, 0] = player;
                        field[cor[1], cor[0], 0] = 0;
                        cor[0] = -1;
                        cor[1] = -1;
                        player *= -1;
                    }
                    else if (Check()) // поедание
                    {
                        if (cor[0] + 2 < 8 && cor[1] + 2 < 8 && field[cor[1] + 2, cor[0] + 2, 0] == 0 && field[cor[1] + 1, cor[0] + 1, 0] == player * -1 && cor[0] + 2 ==mbx && cor[1] + 2 == mby)
                        {
                            field[mby, mbx, 0] = player;
                            field[cor[1], cor[0], 0] = 0;
                            field[cor[1] + 1, cor[0] + 1, 0] = 0;
                            field[cor[1] + 1, cor[0] + 1, 1] = 0;
                            cor[0] = mbx;
                            cor[1] = mby;
                            if (Check())
                            {
                                cntgo = 1;
                            }
                            else
                            {
                                cor[0] = -1;
                                cor[1] = -1;
                                player *= -1;
                                cntgo = 0;
                            }
                        }
                        else if (cor[0] - 2 > -1 && cor[1] - 2 > -1 && field[cor[1] - 2, cor[0] - 2, 0] == 0 && field[cor[1] - 1, cor[0] - 1, 0] == player * -1 && cor[0] - 2 == mbx && cor[1] - 2 == mby)
                        {
                            field[mby, mbx, 0] = player;
                            field[cor[1], cor[0], 0] = 0;
                            field[cor[1] - 1, cor[0] - 1, 0] = 0;
                            field[cor[1] - 1, cor[0] - 1, 1] = 0;
                            cor[0] = mbx;
                            cor[1] = mby;
                            if (Check())
                            {
                                cntgo = 1;
                            }
                            else
                            {
                                cor[0] = -1;
                                cor[1] = -1;
                                player *= -1;
                                cntgo = 0;
                            }
                        }
                        else if (cor[0] - 2 > -1 && cor[1] + 2 < 8 && field[cor[1] + 2, cor[0] - 2, 0] == 0 && field[cor[1] + 1, cor[0] - 1, 0] == player * -1 && cor[0] - 2 == mbx && cor[1] + 2 == mby)
                        {
                            field[mby, mbx, 0] = player;
                            field[cor[1], cor[0], 0] = 0;
                            field[cor[1] + 1, cor[0] - 1, 0] = 0;
                            field[cor[1] + 1, cor[0] - 1, 1] = 0;
                            cor[0] = mbx;
                            cor[1] = mby;
                            if (Check())
                            {
                                cntgo = 1;
                            }
                            else
                            {
                                cor[0] = -1;
                                cor[1] = -1;
                                player *= -1;
                                cntgo = 0;
                            }
                        }
                        else if (cor[0] + 2 < 8 && cor[1] - 2 > -1 && field[cor[1] - 2, cor[0] + 2, 0] == 0 && field[cor[1] - 1, cor[0] + 1, 0] == player * -1 && cor[0] + 2 == mbx && cor[1] - 2 == mby)
                        {
                            field[mby, mbx, 0] = player;
                            field[cor[1], cor[0], 0] = 0;
                            field[cor[1] - 1, cor[0] + 1, 0] = 0;
                            field[cor[1] - 1, cor[0] + 1, 1] = 0;
                            cor[0] = mbx;
                            cor[1] = mby;
                            if (Check())
                            {
                                cntgo = 1;
                            }
                            else
                            {
                                cor[0] = -1;
                                cor[1] = -1;
                                player *= -1;
                                cntgo = 0;
                            }
                        }
                    }
                }
                else if (field[mby, mbx, 0] == 0 && q == 1) //королева
                {
                    if (Q_go_Check(mbx, mby) && cntgo == 0)
                    {
                        field[mby, mbx, 0] = player;
                        field[mby, mbx, 1] = 1;
                        field[cor[1], cor[0], 0] = 0;
                        field[cor[1], cor[0], 1] = 0;
                        cor[0] = -1;
                        cor[1] = -1;
                        player *= -1;
                    }
                    else if (Q_eat_check())
                    {
                        int check = 0, x_del=-1, y_del=-1;
                        if (mbx > cor[0] && mby > cor[1])
                        {
                            for (int y = cor[1] + 1; y <= mby; y++)
                            {
                                if (field[y, cor[0] + (y - cor[1]), 0] != 0)
                                {
                                    check += 1;
                                    x_del = cor[0] + (y - cor[1]);
                                    y_del = y;
                                }
                            }
                        }
                        else if (mbx < cor[0] && mby < cor[1])
                        {
                            for (int y = cor[1] - 1; y >= mby; y--)
                            {
                                if (field[y, cor[0] - (cor[1] - y), 0] != 0)
                                {
                                    check += 1;
                                    x_del = cor[0] - (cor[1] - y);
                                    y_del = y;
                                }
                            }
                        }
                        else if (mbx > cor[0] && mby < cor[1])
                        {
                            for (int y = cor[1] - 1; y >= mby; y--)
                            {
                                if (field[y, cor[0] + (cor[1] - y), 0] != 0)
                                {
                                    check += 1;
                                    x_del = cor[0] + (cor[1] - y);
                                    y_del = y;
                                }
                            }
                        }
                        else if (mbx < cor[0] && mby > cor[1])
                        {
                            for (int y = cor[1] + 1; y <= mby; y++)
                            {
                                if (field[y, cor[0] - (y - cor[1]), 0] != 0)
                                {
                                    check += 1;
                                    x_del = cor[0] - (y - cor[1]);
                                    y_del = y;
                                }
                            }
                        }
                        if (check == 1)
                        {
                            field[mby, mbx, 0] = player;
                            field[mby, mbx, 1] = 1;
                            field[cor[1], cor[0], 0] = 0;
                            field[cor[1], cor[0], 1] = 0;
                            field[y_del, x_del, 0] = 0;
                            field[y_del, x_del, 1] = 0;
                            cor[0] = mbx;
                            cor[1] = mby;
                            if (Q_eat_check())
                            {
                                cntgo = 1;
                            }
                            else
                            {
                                cor[0] = -1;
                                cor[1] = -1;
                                player *= -1;
                                cntgo = 0;
                            }
                        }
                    }
                }
            }
            if (cntgo == 0)
            {
                for (int i = 0; i < 8; i++) if (field[0, i, 0] == 1) field[0, i, 1] = 1;
                for (int i = 0; i < 8; i++) if (field[7, i, 0] == -1) field[7, i, 1] = 1;
            }
            int b = 0, w = 0;
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    if (field[y, x, 0] == 1) w += 1;
                    else if (field[y, x, 0] == -1) b += 1;
                }
            }
            l.Content = w;
            l2.Content = b;
            if (w == 0){ 
                win.Content = "ЧЕРНЫЕ ПОБЕДИЛИ";
                }
            else if (b == 0){ 
                win.Content = "БЕЛЫЕ ПОБЕДИЛИ"; 
                }
            if (w!=0 && b != 0)
            {
                if (player == 1) turn.Content = "Ход белых";
                else turn.Content = "Ход черных";
            }
            else turn.Content = "";
            Draw();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            player = 1;
            q = 0;
            cntgo = 0;
            cor[0] = -1;
            cor[1] = -1;
            field = new_field;
            int b = 0, w = 0;
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    if (field[y, x, 0] == 1) w += 1;
                    else if (field[y, x, 0] == -1) b += 1;
                }
            }
            l.Content = w;
            l2.Content = b;
            win.Content = "";
            turn.Content = "Ход белых";
            Draw();
        }

        private void Draw() //отрисовка
        {
            can.Children.RemoveRange(64, 100);
            for (int i = 0; i < 8; i++)
            {
                for (int f = 0; f < 8; f++)
                {
                    Image img = new Image();
                    Uri uri = new Uri("", UriKind.Relative);
                    if (field[i, f, 0] != 0)
                    {
                        if (field[i, f, 0] == 1)
                        {
                            if (field[i, f, 1] == 0) uri = new Uri(Directory.GetCurrentDirectory() + "/res/w.png", UriKind.RelativeOrAbsolute);
                            else uri = new Uri(Directory.GetCurrentDirectory() + "/res/wq.png", UriKind.RelativeOrAbsolute);
                        }
                        else if (field[i, f, 0] == -1)
                        {
                            if (field[i, f, 1] == 0) uri = new Uri(Directory.GetCurrentDirectory() + "/res/b.png", UriKind.RelativeOrAbsolute);
                            else uri = new Uri(Directory.GetCurrentDirectory() + "/res/bq.png", UriKind.RelativeOrAbsolute);
                        }
                        img.Source = new BitmapImage(uri);
                        img.Height = cell.Height;
                        img.Width = cell.Width;
                        Canvas.SetTop(img, cell.Height * i);
                        Canvas.SetLeft(img, cell.Width * f);
                        can.Children.Add(img);
                    }
                }
            }
        }
        private bool Check() //можно ли съесть
        {
            if ((cor[0] + 2 < 8 && cor[1] + 2 < 8 && field[cor[1] + 2, cor[0] + 2, 0] == 0 && field[cor[1] + 1, cor[0] + 1, 0] == player*-1) || (cor[0] - 2 > -1 && cor[1] - 2 > -1 && field[cor[1] - 2, cor[0] - 2, 0] == 0 && field[cor[1] - 1, cor[0] - 1, 0] == player * -1) || (cor[0] - 2 > -1 && cor[1] + 2 < 8 && field[cor[1] + 2, cor[0] - 2, 0] == 0 && field[cor[1] + 1, cor[0] - 1, 0] == player * -1) || (cor[0] + 2 < 8 && cor[1] - 2 > -1 && field[cor[1] - 2, cor[0] + 2, 0] == 0 && field[cor[1] - 1, cor[0] + 1, 0] == player * -1)) return true;
            else return false;
        }
        private bool Q_go_Check(int mbx, int mby)
        {
            int check = 0;
            if (mbx > cor[0] && mby > cor[1])
            {
                for (int y = cor[1]+1; y <= mby; y++)
                {
                    if (field[y, cor[0] + (y-cor[1]), 0] != 0) check += 1;
                }
            }
            else if (mbx < cor[0] && mby < cor[1])
            {
                for (int y = cor[1]-1; y >= mby; y--)
                {
                    if (field[y, cor[0] - (cor[1]-y), 0] != 0) check += 1;
                }
            }
            else if (mbx > cor[0] && mby < cor[1])
            {
                for (int y = cor[1]-1; y >= mby; y--)
                {
                    if (field[y, cor[0] + (cor[1] - y), 0] != 0) check += 1;
                }
            }
            else if (mbx < cor[0] && mby > cor[1])
            {
                for (int y = cor[1]+1; y <= mby; y++)
                {
                    if (field[y, cor[0] - (y - cor[1]), 0] != 0) check += 1;
                }
            }
            if (check == 0) return true;
            else return false;
        }
        private bool Q_eat_check()
        {
            bool can1 = false, can2 = false, can3 = false, can4 = false;
            for (int y = cor[1] + 1; y < 8; y++)
            {
                if (y <= 7 && cor[0] + (y - cor[1]) <= 7 && field[y, cor[0] + (y - cor[1]), 0] == player * -1)
                {
                    if (y<7 && cor[0] + (y - cor[1])<7 && field[y + 1, cor[0] + (y - cor[1]) + 1, 0] == 0)
                    {
                        can1 = true;
                        break;
                    }
                }
                else if(y <= 7 && cor[0] + (y - cor[1]) <= 7 && field[y, cor[0] + (y - cor[1]), 0] == player)
                {
                    break;
                }
            }
            for (int y = cor[1] - 1; y >= 0; y--)
            {
                if (y >= 0 && cor[0] - (cor[1] - y) >= 0 && field[y, cor[0] - (cor[1] - y), 0] == player * -1)
                {
                    if (y>0 && cor[0] - (cor[1] - y) > 0 && field[y - 1, cor[0] - (cor[1] - y) - 1, 0] == 0)
                    {
                        can2 = true;
                        break;
                    }
                }
                else if (y >= 0 && cor[0] - (cor[1] - y) >= 0 && field[y, cor[0] - (cor[1] - y), 0] == player)
                {
                    break;
                }
            }
            for (int y = cor[1] - 1; y >= 0; y--)
            {
                if (y >= 0 && cor[0] + (cor[1] - y) <= 7 && field[y, cor[0] + (cor[1] - y), 0] == player * -1)
                {
                    if (y>0 && cor[0] + (cor[1] - y)<7 && field[y - 1, cor[0] + (cor[1] - y) + 1, 0] == 0)
                    {
                        can3 = true;
                        break;
                    }
                }
                else if (y >= 0 && cor[0] + (cor[1] - y) <= 7 && field[y, cor[0] + (cor[1] - y), 0] == player)
                {
                    break;
                }
            }
            for (int y = cor[1] + 1; y < 8; y++)
            {
                if (y <= 7 && cor[0] - (y - cor[1]) >= 0 && field[y, cor[0] - (y - cor[1]), 0] == player * -1)
                {
                    if (y<7 && cor[0] - (y - cor[1])>0 && field[y + 1, cor[0] - (y - cor[1]) - 1, 0] == 0)
                    {
                        can4 = true;
                        break;
                    }
                }
                else if (y <= 7 && cor[0] - (y - cor[1]) >= 0 && field[y, cor[0] - (y - cor[1]), 0] == player)
                {
                    break;
                }
            }
            if (can1 || can2 || can3 || can4) return true;
            else return false;
        }
    }
}
