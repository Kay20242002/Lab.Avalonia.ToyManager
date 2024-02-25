using lab2.Data.Enums;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Collections.Generic;
using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Linq;
using System.Reactive.Linq;
using Avalonia.Platform.Storage;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Encodings.Web;
using DynamicData;

namespace lab2.ViewModels;

public class StoreViewModel : ViewModelBase
{

    private static JsonSerializerOptions SerializerOptions { get; } = new(JsonSerializerDefaults.Web)
    {
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
    };

    public static IEnumerable<Gender> Genders { get; } = Enum.GetValues<Gender>();

    [Reactive]
    public Gender Gender { get; set; } = Gender.Unknown;

    [Reactive]
    public string Input { get; set; } = "";

    public ObservableCollection<ToyViewModel> Toys { get; init; } = [];

    public ReactiveCommand<Unit, Unit> CreateToy { get; init; }
    public ReactiveCommand<ToyViewModel, Unit> DeleteToy { get; init; }
    public ReactiveCommand<Unit, Unit> Export { get; init; }
    public ReactiveCommand<Unit, Unit> Import { get; init; }

    public Interaction<Unit, IStorageFile?> ChooseExportFile { get; } = new();

    public Interaction<Unit, IStorageFile?> ChooseStoreFile { get; } = new();

    public StoreViewModel()
    {
        CreateToy = ReactiveCommand.Create(
            CreateToyImpl,
            this.WhenAnyValue(x => x.Input).Select(x => x.Contains(';')));

        DeleteToy = ReactiveCommand.Create<ToyViewModel>(DeleteToyImpl);

        Export = ReactiveCommand.CreateFromTask(ExportImpl);
        Import = ReactiveCommand.CreateFromTask(ImportImpl);
    }

    private async Task ExportImpl()
    {
        var file = await ChooseExportFile.Handle(Unit.Default);
        if (file is null)
            return;

        using var fs = await file.OpenWriteAsync();
        await JsonSerializer.SerializeAsync(fs, Toys, options: SerializerOptions);
    }

    private async Task ImportImpl()
    {
        var file = await ChooseStoreFile.Handle(Unit.Default);
        if (file is null)
            return;

        using var fs = await file.OpenReadAsync();
        var toys = await JsonSerializer.DeserializeAsync<IEnumerable<ToyViewModel>>(fs, options: SerializerOptions);

        if (toys is null)
            return;

        Toys.Clear();
        Toys.AddRange(toys);
    }

    private void DeleteToyImpl(ToyViewModel toy) => 
        Toys.Remove(toy);

    private void CreateToyImpl()
    {
        /*
        " dsadas dsad ; 595 "
        [" dsadas dsad "," 595 "]
        ["dsadas dsad","595"]
        check length == 2
        check empty  | ["","568"]
        unique name | ["ads", "sdads", "qew"] -> ["ADS", "SDADS", "qew"] | "DSADAS DSAD"
        try parse age
        add
         */
        var parts = Input
            .Split(';')
            .Select(x => x.Trim())
            .ToList();

        if (parts.Count != 2)
            return;

        if (parts.Any(string.IsNullOrWhiteSpace))
            return;

        var name = parts[0];

        var normalizedName = name.ToUpper();
        if (Toys.Select(x => x.Name.ToUpper()).Any(x => x == normalizedName))
            return;

        if (!int.TryParse(parts[1], out var age))
            return;

        Toys.Add(new()
        {
            Name = name,
            Age = age,
            Gender = Gender
        });
    }

}
