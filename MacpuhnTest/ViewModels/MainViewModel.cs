using System.Windows.Input;
using MacpuhnTest.AdditionalClasses;

namespace MacpuhnTest.ViewModels
{
    class MainViewModel: BindableBase
    {

        #region Fields and properties
        private BindableBase _dataContext;

        public BindableBase DataContext
        {
            get => _dataContext;
            set => SetProperty(ref _dataContext, value);
        }

        private ImageCollectionViewModel _imageCollectionViewModel = new ImageCollectionViewModel();

        public ImageCollectionViewModel ImageCollectionViewModel
        {
            get => _imageCollectionViewModel;
            set => SetProperty(ref _imageCollectionViewModel, value);
        }

        private ImageViewerViewModel _imageViewerViewModel = new ImageViewerViewModel();

        public ImageViewerViewModel ImageViewerViewModel
        {
            get => _imageViewerViewModel;
            set => SetProperty(ref _imageViewerViewModel, value);
        }
        #endregion

        public MainViewModel()
        {
            DataContext = ImageCollectionViewModel;
            ImageViewerViewModel.ImageCollection = ImageCollectionViewModel.ImageCollection;
            ImageCollectionViewModel.ShowImageViewerEvent += (image) =>
            {
                DataContext = ImageViewerViewModel;
                ImageViewerViewModel.CurrentImage = image;
            };
            ImageViewerViewModel.ShowImageCollectionEvent += () =>
            {
                DataContext = ImageCollectionViewModel;
            };
        }

        private void OnWindowClosing()
        {
            ImageCollectionViewModel.WriteImagePathes();
        }

        #region Commands

        private ICommand _closeWindowCommand;

        public ICommand CloseWindowCommand
        {
            get { return _closeWindowCommand ?? (_closeWindowCommand = new RelayCommand(o => { OnWindowClosing(); })); }
        }

        #endregion  

    }
}
