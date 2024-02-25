using Avalonia.Controls;
using lab2.Data.Enums;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2.ViewModels;


/// <summary>
/// Представляет собой игрушку из задания
/// </summary>
public class ToyViewModel : ViewModelBase
{

    [Reactive]
    public string Name { get; set; } = string.Empty; 

    [Reactive]
    public Gender Gender { get; set; } = Gender.Unknown;

    [Reactive]
    public int Age { get; set; } = 0;
    
}
