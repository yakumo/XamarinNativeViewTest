using System;
using Test2.uwp.XamarinView;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

// 空白ページのアイテム テンプレートについては、http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 を参照してください

[assembly: ExportRenderer(typeof(Test2.uwp.XamarinView.NativeView), typeof(Test2.uwp.Native.NativeViewRenderer))]
namespace Test2.uwp.Native
{
    public class NativeViewRenderer : ViewRenderer<Test2.uwp.XamarinView.NativeView, Windows.UI.Xaml.Controls.TextBlock>
    {
        Windows.UI.Xaml.Controls.TextBlock nativeView;

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

namespace Test2.uwp.XamarinView
{
    public class NativeView : View
    {
        public static readonly BindableProperty TextProperty = BindableProperty.Create("ItemTextWidth", typeof(string), typeof(string), String.Empty, BindingMode.OneWay, null, null, null, null);

        public string Text
        {
            get { return (string)base.GetValue(NativeView.TextProperty); }
            set { base.SetValue(NativeView.TextProperty, value); }
        }
    }
}

namespace Test2.uwp
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();
            LoadApplication(new Test2App());
        }
    }

    public class Test2App : Xamarin.Forms.Application
    {
        public Test2App()
        {
            MainPage = new ContentPage()
            {
                Content = new StackLayout()
                {
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    Padding = new Thickness(50),
                    BackgroundColor = Color.Aqua,
                    Children = {
                        new Test2.uwp.XamarinView.NativeView()
                        {
                            Text = "Hello, world !!",
                            BackgroundColor = Color.White,
                        },
                    },
                }
            };
        }
    }
}
