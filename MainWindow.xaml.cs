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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;


namespace Шашки {
    public partial class MainWindow : Window {
        MediaPlayer m = new MediaPlayer();
        public MainWindow() {
            InitializeComponent();
            m.Open(new Uri(Directory.GetCurrentDirectory() + "/res/mix1.mp3", UriKind.RelativeOrAbsolute));
            m.Volume = 1;
            m.Position = new TimeSpan(0, 0, 0, 0, 0);
            m.Play();
            this.Title = "Menu";
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if(bt2.Content==""){
                bt2.Content = " ";
                bt2.Background = new ImageBrush(new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "/res/noa.png", UriKind.RelativeOrAbsolute)));
                m.Volume = 0;
            }
            else
            {
                bt2.Content = "";
                bt2.Background = new ImageBrush(new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "/res/a.png", UriKind.RelativeOrAbsolute)));
                m.Volume = 1;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            gamewnd wnd = new gamewnd();
            wnd.Title = "checkers";
            wnd.Show();
        }
    }
}
