using Avalonia.Controls;
using Avalonia.ReactiveUI;
using lab2.ViewModels;
using ReactiveUI;

namespace lab2.Views
{
    public partial class FilterView : ReactiveUserControl<FilterViewModel>
    {
        public FilterView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                d(this
                    .ViewModel!
                    .ExportFilterResultInteraction
                    .RegisterHandler(
                        async interaction =>
                        {
                            var topLevel = TopLevel.GetTopLevel(this);
                            var storage = topLevel.StorageProvider;

                            var file = await storage.SaveFilePickerAsync(new Avalonia.Platform.Storage.FilePickerSaveOptions()
                            {
                                DefaultExtension = "json",
                                Title = "Ёкспорт"
                            });

                            interaction.SetOutput(file);
                        }));
            });

        }

    }
}
