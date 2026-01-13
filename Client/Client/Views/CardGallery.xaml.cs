using Client.Helpers;
using Client.Properties.Langs;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Client.Views
{
    public partial class CardGallery : Window
    {
        private readonly string[] _cardNames = {
            "africa", "ana", "ari", "blanca", "emily", "fer",
            "katya", "lala", "linda", "paul", "saddy", "sara"
        };

        public CardGallery()
        {
            InitializeComponent();
            LoadGallery();
        }

        private void LoadGallery()
        {
            var galleryItems = new List<GalleryItem>();

            var categoriesMap = new Dictionary<string, string>
            {
                { "Color", Lang.Gallery_Category_Color },
                { "Normal", Lang.Gallery_Category_Normal }
            };

            foreach (var categoryPair in categoriesMap)
            {
                string folderName = categoryPair.Key;
                string displayName = categoryPair.Value;

                foreach (var name in _cardNames)
                {
                    string path = $"/Client;component/Resources/Images/Cards/Fronts/{folderName}/{name}.png";

                    galleryItems.Add(new GalleryItem
                    {
                        Name = name,
                        DisplayCategory = displayName,
                        FullPath = path
                    });
                }
            }

            GalleryItemsControl.ItemsSource = galleryItems;
        }

        private void Card_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement element && element.DataContext is GalleryItem item)
            {
                try
                {
                    ZoomedImage.Source = new BitmapImage(new Uri(item.FullPath, UriKind.RelativeOrAbsolute));
                    ZoomOverlay.Visibility = Visibility.Visible;
                }
                catch (Exception)
                {
                    MessageBox.Show(string.Format(Lang.Gallery_Error_Load, item.Name));
                }
            }
        }

        private void ZoomOverlay_Click(object sender, MouseButtonEventArgs e)
        {
            ZoomOverlay.Visibility = Visibility.Collapsed;
            ZoomedImage.Source = null;
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationHelper.NavigateTo(this, new MainMenu());
        }
    }

    public class GalleryItem
    {
        public string Name { get; set; }
        public string DisplayCategory { get; set; }
        public string FullPath { get; set; }
    }
}