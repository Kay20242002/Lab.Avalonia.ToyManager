using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Avalonia.ReactiveUI;
using lab2.ViewModels;
using ReactiveUI;
using System.Linq;

namespace lab2.Views
{
    public partial class StoreView : ReactiveUserControl<StoreViewModel>
    {
        public StoreView()
        {
            InitializeComponent();


            this.WhenActivated(d =>
            {
                d(this
                    .ViewModel!
                    .ChooseExportFile
                    .RegisterHandler(
                        async interaction =>
                        {
                            var topLevel = TopLevel.GetTopLevel(this);
                            var storage = topLevel.StorageProvider;

                            var file = await storage.SaveFilePickerAsync(new Avalonia.Platform.Storage.FilePickerSaveOptions()
                            {
                                DefaultExtension = "json",
                                Title = "Экспорт"
                            });

                            interaction.SetOutput(file);
                        }));

                d(this
                    .ViewModel!
                    .ChooseStoreFile
                    .RegisterHandler(
                        async interaction =>
                        {
                            var topLevel = TopLevel.GetTopLevel(this);
                            var storage = topLevel.StorageProvider;

                            var file = await storage.OpenFilePickerAsync(new()
                            {
                                AllowMultiple = false,
                                Title = "Выберите файл для импорта"
                            });

                            interaction.SetOutput(file.ElementAtOrDefault(0));
                        }));
            });
        }
    }
}
