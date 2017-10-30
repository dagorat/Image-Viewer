using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml;
using MacpuhnTest.AdditionalClasses;

namespace MacpuhnTest.ViewModels
{
    internal class ImageCollectionViewModel: BindableBase
    {

        public ImageCollectionViewModel()
        {
            try
            {
                ImageCollection.CollectionChanged += ImageCollectionOnCollectionChanged;
                ReadImagePathes();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        #region

        public ObservableCollection<ImageViewModel> ImageCollection { get; set; } = new ObservableCollection<ImageViewModel>();

        private ImageViewModel _selectedImage;

        public ImageViewModel SelectedImage
        {
            get => _selectedImage;
            set => SetProperty(ref _selectedImage, value);
        }

        #endregion

        #region Methods

        private void ImageCollectionOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            switch (notifyCollectionChangedEventArgs.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    AddImageToCollection(notifyCollectionChangedEventArgs.NewItems);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    RemoveImageFromCollection(notifyCollectionChangedEventArgs.OldItems);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    //TODO Implement
                    break;
                case NotifyCollectionChangedAction.Move:
                    //TODO Implement
                    break;
                case NotifyCollectionChangedAction.Reset:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void AddImageToCollection(IEnumerable newItems)
        {
            foreach (var imageObject in newItems)
            {
                if (imageObject is ImageViewModel image)
                {
                    ImageViewModel previousImage = null;
                    ImageViewModel nextImage = null;
                    int imageIndex = ImageCollection.IndexOf(image);
                    if (imageIndex > 0)
                    {
                        previousImage = ImageCollection[imageIndex - 1];
                    }
                    if (imageIndex < ImageCollection.Count - 2)
                    {
                        nextImage = ImageCollection[imageIndex + 1];
                    }
                    if (nextImage != null)
                    {
                        image.NextImage = nextImage;
                        nextImage.PreviousImage = image;
                    }
                    if (previousImage != null)
                    {
                        image.PreviousImage = previousImage;
                        previousImage.NextImage = image;
                    }
                }
            }
        }

        private static void RemoveImageFromCollection(IEnumerable oldItems)
        {
            foreach (var imageObject in oldItems)
            {
                if (imageObject is ImageViewModel image)
                {
                    if (image.PreviousImage != null)
                    {
                        image.PreviousImage.NextImage = image.NextImage;
                    }
                    if (image.NextImage != null)
                    {
                        image.NextImage.PreviousImage = image.PreviousImage;
                    }
                }
            }
        }

        public async void AddImagesAsync(StringCollection files)
        {

            foreach (string file in files)
            {
                if (ImageHelper.IsImageExtension(Path.GetExtension(file)))
                {
                    await AddImageAsync(file);
                }
            };
        }

        public async Task AddImageAsync(string fileName)
        {
            try
            {
                ImageViewModel image = null;
                await Task.Run(() =>
                {
                    image = new ImageViewModel(fileName);
                });
                ImageCollection.Add(image);
                image.ShowImageViewerEvent += ShowImageViewer;
            }
            catch(Exception)
            {
                MessageBox.Show($"Image {fileName} cannot be added to image viewer");
            }
        }

        private void ShowImageViewer(ImageViewModel image)
        {
            OnShowImageViewer(image);
        }

        public void WriteImagePathes()
        {
            using (XmlWriter xmlWriter = XmlWriter.Create("cache.xml"))
            {
                xmlWriter.WriteStartDocument();
                xmlWriter.WriteStartElement("images");
                foreach (ImageViewModel image in ImageCollection)
                {
                    xmlWriter.WriteStartElement("image");
                    xmlWriter.WriteString(image.FilePath);
                    xmlWriter.WriteEndElement();
                }
                xmlWriter.WriteEndElement();
            }
        }

        public async void ReadImagePathes()
        {
            if (File.Exists("cache.xml"))
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load("cache.xml");
                XmlNode imagesNode = xmlDocument.GetElementsByTagName("images")[0];
                foreach (XmlNode node in imagesNode.ChildNodes)
                {
                    await AddImageAsync(node.InnerText);
                };
            }
        }

        private void DeleteSelectedImage()
        {
            ImageCollection.Remove(SelectedImage);
        }

        #endregion

        #region Commands

        private ICommand _deleteSelectedImageCommand;

        public ICommand DeleteSelectedImageCommand
        {
            get { return _deleteSelectedImageCommand ?? (_deleteSelectedImageCommand = new RelayCommand(o => { DeleteSelectedImage(); })); }
        }

        #endregion

        #region Events and delegates

        public delegate void ShowImageViewerDelegate(ImageViewModel image);

        public event ShowImageViewerDelegate ShowImageViewerEvent;

        private void OnShowImageViewer(ImageViewModel image)
        {
            ShowImageViewerEvent?.Invoke(image);
        }

        #endregion

    }
}
