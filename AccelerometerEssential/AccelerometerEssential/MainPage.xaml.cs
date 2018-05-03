using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Essentials;
using System.Numerics;

namespace AccelerometerEssential
{
    public partial class MainPage : ContentPage
    {
        readonly SKPaint blackPaint = new SKPaint
        {
            Style = SKPaintStyle.Fill,
            Color = SKColors.Black,
            IsAntialias = true
        };

        readonly SKPaint whiteLine = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = SKColors.White,
            StrokeWidth = 1
        };

        readonly SKPaint whitePaint = new SKPaint
        {
            Style = SKPaintStyle.Fill,
            Color = SKColors.Beige,
            IsAntialias = true
        };

        public MainPage()
        {
            InitializeComponent();

            Device.StartTimer(TimeSpan.FromSeconds(1f / 60), () =>
            {
                canvasView.InvalidateSurface();
                return true;
            });
        }

        void canvasView_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            var surface = e.Surface;
            var canvas = surface.Canvas;

            canvas.Clear(SKColors.CornflowerBlue);

            var width = e.Info.Width;
            var height = e.Info.Height;

            // Move the origin to the center
            canvas.Translate(width / 2, height / 2);
            canvas.Scale(width / 300f);

            // Draw base graphics
            canvas.DrawCircle(0, 0, 100, blackPaint);

            // Bubble
            var x = (acceleration.X * RoundingValue);
            var y = (acceleration.Y * RoundingValue);
            canvas.DrawCircle(x, y, 20f, whitePaint);

            canvas.DrawLine(-120, 0, 120, 0, whiteLine);
            canvas.DrawLine(0, -120, 0, 120, whiteLine);

        }

        protected override void OnAppearing()
        {
            Accelerometer.ReadingChanged += Accelerometer_ReadingChanged;
            base.OnAppearing();

            try
            {
                Accelerometer.Start(SensorSpeed.Normal);
            }
            catch (FeatureNotSupportedException ex)
            {
                Console.WriteLine($"Accelerometer - {ex.Message}");
            }
        }

        void Accelerometer_ReadingChanged(AccelerometerChangedEventArgs e)
        {
            acceleration = e.Reading.Acceleration;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            Accelerometer.Stop();
            Accelerometer.ReadingChanged -= Accelerometer_ReadingChanged;
        }

        Vector3 acceleration;
        const float RoundingValue = 100f;
    }
}
