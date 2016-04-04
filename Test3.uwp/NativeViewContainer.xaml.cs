using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Test3.uwp.Native;
using Test3.uwp.XamarinView;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

// 空白ページのアイテム テンプレートについては、http://go.microsoft.com/fwlink/?LinkId=234238 を参照してください
[assembly: ExportRenderer(typeof(Test3.uwp.XamarinView.NativeView), typeof(Test3.uwp.Native.NativeViewRenderer))]
namespace Test3.uwp.Native
{
    public class NativeViewRenderer : ViewRenderer<Test3.uwp.XamarinView.NativeView, Windows.UI.Xaml.Controls.TextBlock>
    {
        Windows.UI.Xaml.Controls.TextBlock nativeView;

        public NativeViewRenderer()
        {
            this.LayoutUpdated += NativeViewRenderer_LayoutUpdated;
        }

        private void NativeViewRenderer_LayoutUpdated(object sender, object e)
        {
            Rect r = new Rect(new Windows.Foundation.Point(0, 0), this.DesiredSize);
            foreach (var c in Children)
            {
                c.Arrange(r);
            }
        }

        protected override void OnElementChanged(ElementChangedEventArgs<NativeView> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                nativeView = new Windows.UI.Xaml.Controls.TextBlock()
                {
                    FontSize = 60,
                };
                SetNativeControl(nativeView);
            }

            Unbind(e.OldElement);
            Bind(e.NewElement);
        }

        private void Unbind(NativeView view)
        {
            if (view != null)
            {
                view.PropertyChanging -= NativeView_PropertyChanging;
                view.PropertyChanged -= NativeView_PropertyChanged;
            }
        }

        private void Bind(NativeView view)
        {
            if (view != null)
            {
                view.PropertyChanging += NativeView_PropertyChanging;
                view.PropertyChanged += NativeView_PropertyChanged;

                nativeView.Text = view.Text;
            }
        }

        private void NativeView_PropertyChanging(object sender, PropertyChangingEventArgs e)
        {
        }

        private void NativeView_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            NativeView nv = null;
            if (sender is NativeView)
            {
                nv = sender as NativeView;
            }

            if (e.PropertyName == "Text" && nv != null)
            {
                nativeView.Text = nv.Text;
            }
        }
    }
}

namespace Test3.uwp.XamarinView
{
    public class NativeView : View
    {
        public static readonly BindableProperty TextProperty = BindableProperty.Create("ItemTextWidth", typeof(string), typeof(string), String.Empty, Xamarin.Forms.BindingMode.OneWay, null, null, null, null);

        public string Text
        {
            get { return (string)base.GetValue(NativeView.TextProperty); }
            set { base.SetValue(NativeView.TextProperty, value); }
        }
    }
}

namespace Test3.uwp
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class NativeViewContainer : StackPanel
    {
        public NativeViewContainer()
        {
            this.InitializeComponent();

            Label l = new Label()
            {
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Start,
                Text = "test !!",
                FontSize=40,
            };
            LabelRenderer tr = (LabelRenderer)l.GetOrCreateRenderer();
            this.Children.Add(tr);

            // this.Loaded += NativeViewContainer_Loaded;
            this.LayoutUpdated += NativeViewContainer_LayoutUpdated;
        }

        private void NativeViewContainer_LayoutUpdated(object sender, object e)
        {
            object o = this.Parent;
            while (o != null)
            {
                if (o is UIElement)
                {
                    break;
                }
                if (o is FrameworkElement)
                {
                    o = ((FrameworkElement)o).Parent;
                    continue;
                }
                break;
            }
            if (o is UIElement)
            {
                Rect r = new Rect(new Windows.Foundation.Point(0, 0), ((UIElement)o).DesiredSize);
                foreach (var c in Children)
                {
                    c.Arrange(r);
                }
            }
        }

        private void NativeViewContainer_Loaded(object sender, RoutedEventArgs e)
        {
            StackPanel sp = new StackPanel()
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Background = new SolidColorBrush(Windows.UI.Colors.Blue),
            };

            NativeView nv = new NativeView()
            {
                Text = "Hello, world !",
                BackgroundColor = Color.Teal,
            };
            NativeViewRenderer nvr = (NativeViewRenderer)nv.GetOrCreateRenderer();
            sp.Children.Add(nvr);
            this.Children.Add(sp);
        }
    }
}
