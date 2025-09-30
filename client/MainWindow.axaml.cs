using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Layout;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bingo;

public partial class MainWindow : Window
{
    private Button[,] cells = new Button[5, 5];
    private bool[,] clicked = new bool[5, 5];

    private List<BingoPattern> patterns;
    private List<BingoPattern> priority_patterns;
    BingoPattern selected_priority_pattern;

    public MainWindow()
    {
        InitializeComponent();

        for (int i = 0; i < 5; i++)
        {
            MainGrid.RowDefinitions.Add(new RowDefinition(GridLength.Star));
            MainGrid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
        }

        for (int row = 0; row < 5; row++)
        {
            for (int col = 0; col < 5; col++)
            {
                var btn = new Button
                {
                    Content = $"{row},{col}",
                    HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch,
                    VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
                    Margin = new Avalonia.Thickness(2),
                    Background = new SolidColorBrush(Colors.Gray),
                    FontSize = 64
                };

                btn.Click += OnCellClick;

                Grid.SetRow(btn, row);
                Grid.SetColumn(btn, col);
                MainGrid.Children.Add(btn);

                cells[row, col] = btn;
            }
        }

        //cells[0, 0].Content = "Start";
        //cells[2, 3].Content = "Changed";
        //cells[4, 4].Content = "End";

        patterns = new List<BingoPattern>
        {
            new BingoPattern("HLine1", new string[]
            {
                "XXXXX",
                ".....",
                ".....",
                ".....",
                "....."
            }),
            new BingoPattern("HLine2", new string[]
            {
                ".....",
                "XXXXX",
                ".....",
                ".....",
                "....."
            }),
            new BingoPattern("HLine3", new string[]
            {
                ".....",
                ".....",
                "XXXXX",
                ".....",
                "....."
            }),
            new BingoPattern("HLine4", new string[]
            {
                ".....",
                ".....",
                ".....",
                "XXXXX",
                "....."
            }),
            new BingoPattern("HLine5", new string[]
            {
                ".....",
                ".....",
                ".....",
                ".....",
                "XXXXX"
            })
        };


        priority_patterns = new List<BingoPattern>
        {
            new BingoPattern("Smile", new string[]
            {
                ".X.X.",
                ".....",
                "X...X",
                ".XXX.",
                "....."
            }),
            new BingoPattern("Smile", new string[]
            {
                "X...X",
                ".X.X.",
                "..X..",
                "..X..",
                "..X.."
            }),
            new BingoPattern("Smile", new string[]
            {
                "..X..",
                "..X..",
                "XXXXX",
                "..X..",
                "..X.."
            }),
            new BingoPattern("Smile", new string[]
            {
                "X.X.X",
                "..X.",
                ".X.X.",
                "..X..",
                "X.X.X"
            }),
            new BingoPattern("Smile", new string[]
            {
                "X...X",
                ".X.X.",
                "..X..",
                ".X.X.",
                "X...X"
            }),
            new BingoPattern("Smile", new string[]
            {
                "..X..",
                ".X.X.",
                "X...X",
                ".X.X.",
                "..X.."
            }),
            new BingoPattern("Smile", new string[]
            {
                "XXXXX",
                "X...X",
                "X...X",
                "X...X",
                "XXXXX"
            })
            
        };

        //TODO FIX SO THAT THIS PATTERN IS SELECTED BY SEEED
        selected_priority_pattern = priority_patterns[new Random().Next(priority_patterns.Count)]; 
    }

    private void OnCellClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (sender is Button btn)
        {
            var row = Grid.GetRow(btn);
            var col = Grid.GetColumn(btn);

            clicked[row, col] = !clicked[row, col];
            btn.Background = clicked[row, col]
            ? new LinearGradientBrush
            {
                StartPoint = new Avalonia.RelativePoint(0.5, 0, Avalonia.RelativeUnit.Relative),
                EndPoint = new Avalonia.RelativePoint(0.5, 1, Avalonia.RelativeUnit.Relative),
                GradientStops =
                    {
                        new GradientStop(Colors.Orange, 0),
                        new GradientStop(Colors.Magenta, 1)
                    }
            }
            : btn.Background = new SolidColorBrush(Colors.Gray);


            List<BingoPattern> completed_patterns = new List<BingoPattern>();

            foreach (BingoPattern pattern in patterns)
            {
                if (pattern.IsComplete(clicked))
                {
                    for (int y = 0; y < pattern.Layout.Length; y++)
                    {
                        for (int x = 0; x < pattern.Layout[y].Length; x++)
                        {
                            if (pattern.Layout[y][x] == 'X')
                            {
                                completed_patterns.Add(pattern);
                            }
                        }
                    }
                }
                else
                {
                    for (int y = 0; y < pattern.Layout.Length; y++)
                    {
                        for (int x = 0; x < pattern.Layout[y].Length; x++)
                        {
                            if (pattern.Layout[y][x] == 'X')
                            {
                                cells[y, x].Background = clicked[y, x]
                                ? new SolidColorBrush(Colors.LightGray)
                                : cells[y, x].Background = new SolidColorBrush(Colors.Gray);

                            }
                        }
                    }
                }
            }

            foreach (BingoPattern completed_pattern in completed_patterns)
            {
                for (int y = 0; y < completed_pattern.Layout.Length; y++)
                {
                    for (int x = 0; x < completed_pattern.Layout[y].Length; x++)
                    {
                        if (completed_pattern.Layout[y][x] == 'X')
                        {
                            cells[y, x].Background = new LinearGradientBrush
                            {
                                StartPoint = new Avalonia.RelativePoint(0.5, 0, Avalonia.RelativeUnit.Relative),
                                EndPoint = new Avalonia.RelativePoint(0.5, 1, Avalonia.RelativeUnit.Relative),
                                GradientStops =
                                    {
                                        new GradientStop(Colors.Yellow, 0),
                                        new GradientStop(Colors.Orange, 1)
                                    }
                            };
                        }
                    }
                }
            }

            if (selected_priority_pattern.IsComplete(clicked))
            {
                for (int y = 0; y < selected_priority_pattern.Layout.Length; y++)
                {
                    for (int x = 0; x < selected_priority_pattern.Layout[y].Length; x++)
                    {
                        if (selected_priority_pattern.Layout[y][x] == 'X')
                        {
                            cells[y, x].Background = new LinearGradientBrush
                            {
                                StartPoint = new Avalonia.RelativePoint(0.5, 0, Avalonia.RelativeUnit.Relative),
                                EndPoint = new Avalonia.RelativePoint(0.5, 1, Avalonia.RelativeUnit.Relative),
                                GradientStops =
                                    {
                                        new GradientStop(Colors.Red, 0),
                                        new GradientStop(Colors.Purple, 1)
                                    }
                            };
                        }
                    }
                }
            }


            bool black_out = true;

            foreach (bool click in clicked)
            {
                if (!click)
                {
                    black_out = false;
                    break;
                }
            }

            if (black_out)
            {
                foreach (Button button in cells)
                {
                    button.Background = new LinearGradientBrush
                    {
                        StartPoint = new Avalonia.RelativePoint(0.5, 0, Avalonia.RelativeUnit.Relative),
                        EndPoint = new Avalonia.RelativePoint(0.5, 1, Avalonia.RelativeUnit.Relative),
                        GradientStops =
                            {
                                new GradientStop(Colors.DarkBlue, 0),
                                new GradientStop(Colors.Black, 1)
                            }
                    };
                }
            }

            CheckCurrentPattern();
        }
    }

    private void OnSeedClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (CodeInput.Text.Length >= 25)
        {
            for (int row = 0; row < 5; row++)
            {
                for (int col = 0; col < 5; col++)
                {
                    cells[col, row].Content = Convert.ToInt32(Char.ToString(CodeInput.Text[5 * row + col]), 16) + 15 * row + 1;
                }
            }
        }
    }

    private void CheckCurrentPattern()
    {

    }
}