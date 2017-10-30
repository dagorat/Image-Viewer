using System.Windows;
using System.Windows.Interactivity;
using MacpuhnTest.ViewModels;

namespace MacpuhnTest.AdditionalClasses
{
    internal class DropBehavior: Behavior<ImageCollectionView>
    {

        #region Dependency properties

        public static ImageCollectionViewModel GetDataContext(DependencyObject obj)
        {
            return (ImageCollectionViewModel)obj.GetValue(DataContextProperty);
        }

        public static void SetDataContext(DependencyObject obj, ImageCollectionViewModel value)
        {
            obj.SetValue(DataContextProperty, value);
        }

        public static readonly DependencyProperty DataContextProperty =
            DependencyProperty.RegisterAttached("DataContext", typeof(ImageCollectionViewModel),
                typeof(DropBehavior), null);

        public ImageCollectionViewModel DataContext
        {
            get => (ImageCollectionViewModel)GetValue(DataContextProperty);
            set => SetValue(DataContextProperty, value);
        }

        #endregion

        protected override void OnAttached()
        {
            AssociatedObject.Drop += UIElement_OnDrop;
        }

        private void UIElement_OnDrop(object sender, DragEventArgs e)
        {
            if (e.Data is DataObject data && data.ContainsFileDropList())
            {
                DataContext.AddImagesAsync(data.GetFileDropList());
            }
        }
    }
}
