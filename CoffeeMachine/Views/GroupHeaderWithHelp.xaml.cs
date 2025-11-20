using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace CoffeeMachineWPF.Views
{
    public partial class GroupHeaderWithHelp : UserControl
    {
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            nameof(Title), typeof(string), typeof(GroupHeaderWithHelp), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty HelpTextProperty = DependencyProperty.Register(
            nameof(HelpText), typeof(string), typeof(GroupHeaderWithHelp), new PropertyMetadata(string.Empty));

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public string HelpText
        {
            get => (string)GetValue(HelpTextProperty);
            set => SetValue(HelpTextProperty, value);
        }

        public GroupHeaderWithHelp()
        {
            InitializeComponent();
            DataContext = this;

            Loaded += GroupHeaderWithHelp_Loaded;
            Unloaded += GroupHeaderWithHelp_Unloaded;
        }

        public static readonly DependencyProperty CloseOnScrollProperty = DependencyProperty.Register(
            nameof(CloseOnScroll), typeof(bool), typeof(GroupHeaderWithHelp), new PropertyMetadata(true));

        public bool CloseOnScroll
        {
            get => (bool)GetValue(CloseOnScrollProperty);
            set => SetValue(CloseOnScrollProperty, value);
        }

        private ScrollViewer? _parentScrollViewer;

        private void GroupHeaderWithHelp_Loaded(object sender, RoutedEventArgs e)
        {
            // Найти ближайший ScrollViewer в иерархии визуальных элементов
            DependencyObject? parent = this;
            while (parent != null && _parentScrollViewer == null)
            {
                parent = VisualTreeHelper.GetParent(parent);
                if (parent is ScrollViewer sv)
                {
                    _parentScrollViewer = sv;
                    _parentScrollViewer.ScrollChanged += ParentScrollViewer_ScrollChanged;
                    break;
                }
            }
        }

        private void GroupHeaderWithHelp_Unloaded(object sender, RoutedEventArgs e)
        {
            if (_parentScrollViewer != null)
            {
                _parentScrollViewer.ScrollChanged -= ParentScrollViewer_ScrollChanged;
                _parentScrollViewer = null;
            }
        }

        private void ParentScrollViewer_ScrollChanged(object? sender, ScrollChangedEventArgs e)
        {
            if (CloseOnScroll && HelpPopup.IsOpen)
            {
                HelpPopup.IsOpen = false;
            }
            else if (!CloseOnScroll && HelpPopup.IsOpen)
            {
                // Попытка скорректировать позицию popup: сброс смещения заставит Popup пересчитать позицию.
                HelpPopup.HorizontalOffset += 0.1;
                HelpPopup.HorizontalOffset -= 0.1;
            }
        }

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            HelpPopup.IsOpen = true;
        }

        private void CloseHelp_Click(object sender, RoutedEventArgs e)
        {
            HelpPopup.IsOpen = false;
        }
    }
}
