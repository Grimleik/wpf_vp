using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AForge.Video;
using AForge.Video.DirectShow;

namespace VideoProcessing
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IDisposable
    {
        private FilterInfoCollection videoDevices;
        private VideoCaptureDevice videoSource;
        private BitmapSource sourceBitmap;

        public MainWindow()
        {
            InitializeComponent();

            // TODO: Let user select a camera before we do this.
            InitializeWebCam();
            Closing += (s, e) => Dispose();
        }

        public void Dispose()
        {
            Shutdown();
        }

        private void Shutdown()
        {
            videoSource.NewFrame -= videoSource_NewFrame;
            videoSource.SignalToStop();
        }
           
        private void InitializeWebCam()
        {
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if(videoDevices.Count == 0)
            {
                MessageBox.Show("No camera found");
                return;
            }

            videoSource = new VideoCaptureDevice(videoDevices[0].MonikerString);
            videoSource.NewFrame += videoSource_NewFrame;
            videoSource.Start();
        }

        private void videoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Dispatcher.Invoke(() =>
            {
                videoImage.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    eventArgs.Frame.GetHbitmap(),
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());
            });
        }
    }
}