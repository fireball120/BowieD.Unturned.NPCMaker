﻿using BowieD.Unturned.NPCMaker.Localization;
using System.Windows;

namespace BowieD.Unturned.NPCMaker.Forms
{
    /// <summary>
    /// Логика взаимодействия для Whats_New.xaml
    /// </summary>
    public partial class Whats_New : Window
    {
        public Whats_New()
        {
            InitializeComponent();
            Title = LocUtil.LocalizeInterface("app_News_Title");
#if LEGACY
#elif true
            updateTitle.Text = App.UpdateManager.Title;
            updateText.Text = App.UpdateManager.Content;
#endif
            mainButton.Content = LocUtil.LocalizeInterface("app_News_OK");
            Height = SystemParameters.PrimaryScreenHeight / 2;
            Width = SystemParameters.PrimaryScreenWidth / 2;
        }

        private void MainButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
