using Avalonia.Platform.Storage;
using lab2.Data.Enums;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

namespace lab2.ViewModels;

public class FilterViewModel : ViewModelBase
{
    public static IEnumerable<Gender> Genders { get; } = Enum.GetValues<Gender>();

    private IEnumerable<ToyViewModel> Source { get; init; }


    private ObservableAsPropertyHelper<IEnumerable<ToyViewModel>> _toys;
    public IEnumerable<ToyViewModel> Toys => _toys.Value;


    private ObservableAsPropertyHelper<IEnumerable<int>> _ages;
    public IEnumerable<int> Ages => _ages.Value;


    public Interaction<Unit, IStorageFile?> ExportFilterResultInteraction { get; } = new();
    public ReactiveCommand<Unit, Unit> Export { get; init; }


    [Reactive]
    public int Age { get; set; }


    [Reactive]
    public Gender Gender { get; set; } = Gender.Unknown;

    private static JsonSerializerOptions SerializerOptions { get; } = new(JsonSerializerDefaults.Web)
    {
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
    };

    public FilterViewModel(IEnumerable<ToyViewModel> items)
    {
        Source = items;

        this
            .WhenAnyValue(
                x => x.Source,
                x => x.Age,
                x => x.Gender,
                (x, age, gender) => x
                    .Where(x =>
                        x.Gender == gender
                        && x.Age == age))
            .ToProperty(this, x => x.Toys, out _toys);

        this
          .WhenAnyValue(
              x => x.Source)
          .Select(x => x.Select(x => x.Age).Distinct())
          .ToProperty(this, x => x.Ages, out _ages);

        Export = ReactiveCommand.CreateFromTask(ExportImpl);
    }


    private async Task ExportImpl()
    {
        var file = await this.ExportFilterResultInteraction.Handle(Unit.Default);
        if (file is null) return;

        using var fileStream = await file.OpenWriteAsync();
        await JsonSerializer.SerializeAsync(fileStream, Toys, options: SerializerOptions);
    }

}
