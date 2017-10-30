using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media.Effects;
using MacpuhnTest.AdditionalClasses;

namespace MacpuhnTest.ViewModels
{
    internal class ImageViewerViewModel: BindableBase
    {

        #region Fields and properties

        public ObservableCollection<ImageViewModel> ImageCollection { get; set; }

        private ImageViewModel _currentImage;

        public ImageViewModel CurrentImage
        {
            get => _currentImage;
            set
            {
                BitmapEffect = null;
                SetProperty(ref _currentImage, value);
            }
        }

        private string _statusText;

        public string StatusText
        {
            get => _statusText;
            set => SetProperty(ref _statusText, value);
        }

        private Effect _bitmapEffect;

        public Effect BitmapEffect
        {
            get => _bitmapEffect;
            set => SetProperty(ref _bitmapEffect, value);
        }

        #endregion

        #region Methods

        private void GetNext()
        {
            if (CurrentImage.NextImage != null)
            {
                CurrentImage = CurrentImage.NextImage;
            }
        }

        private void GetPrevious()
        {
            if (CurrentImage.PreviousImage != null)
            {
                CurrentImage = CurrentImage.PreviousImage;
            }
        }

        private void Blur()
        {
            BlurEffect blurEffect = new BlurEffect
            {
                Radius = 5,
                KernelType = KernelType.Gaussian
            };
            BitmapEffect = blurEffect;

        }

        private void ShowCollectionView()
        {
            OnShowImageCollection();
        }

        #endregion

        #region Commands

        private ICommand _getNextCommand;

        public ICommand GetNextCommand
        {
            get { return _getNextCommand ?? (_getNextCommand = new RelayCommand(o => { GetNext(); })); }
        }

        private ICommand _getPreviousCommand;

        public ICommand GetPreviousCommand
        {
            get { return _getPreviousCommand ?? (_getPreviousCommand = new RelayCommand(o => { GetPrevious(); })); }
        }

        private ICommand _blurCommand;

        public ICommand BlurCommand
        {
            get { return _blurCommand ?? (_blurCommand = new RelayCommand(o => { Blur(); })); }
        }

        private ICommand _showCollectionViewCommand;

        public ICommand ShowCollectionViewCommand
        {
            get
            {
                return _showCollectionViewCommand ??
                       (_showCollectionViewCommand = new RelayCommand(o => { ShowCollectionView(); }));
            }
        }
        #endregion

        #region Events and delegates

        public delegate void ShowImageCollectionDelegate();

        public event ShowImageCollectionDelegate ShowImageCollectionEvent;

        private void OnShowImageCollection()
        {
            ShowImageCollectionEvent?.Invoke();
        }

        #endregion

    }
}
