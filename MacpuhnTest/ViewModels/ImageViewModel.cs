using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using MacpuhnTest.AdditionalClasses;

namespace MacpuhnTest.ViewModels
{
    internal class ImageViewModel: BindableBase
    {

        #region Fields and properties

        private BitmapImage _image;

        public BitmapImage Image
        {
            get => _image ?? (_image =
                       ImageHelper.LoadImageFromFile(FilePath, (int)SystemParameters.PrimaryScreenWidth));
            private set => SetProperty(ref _image, value);
        }

        public string FilePath { get; private set; }

        public ImageViewModel NextImage { get; internal set; }

        public ImageViewModel PreviousImage { get; internal set; }

        public BitmapImage Thumbnail { get; set; }

        #endregion

        public ImageViewModel(string filePath)
        {
            Thumbnail = ImageHelper.LoadImageFromFile(filePath, 128);
            FilePath = filePath;
        }

        #region Methods

        private void ShowImageViewer()
        {
            OnShowImageViewer();
        }

        #endregion

        #region Commands

        private ICommand _showImageViewerCommand;

        public ICommand ShowImageViewerCommand
        {
            get
            {
                return _showImageViewerCommand ??
                       (_showImageViewerCommand = new RelayCommand(o => { ShowImageViewer(); }));
            }
        }

        #endregion

        #region Events and delegates

        internal delegate void ImageDelegate(ImageViewModel image);

        public event ImageDelegate ShowImageViewerEvent;

        public void OnShowImageViewer()
        {
            ShowImageViewerEvent?.Invoke(this);
        }

        #endregion
    }

}
