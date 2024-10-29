using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanBoard.ViewModel
{
    public class CalculatorViewModel : ObservableObject
    {
        #region Methods

        double totalResult = 0;
        double sum(double a)
        {
            return a + totalResult;
        }

        double ubtraction(double a)
        {
            return a - totalResult;
        }

        double multiplication(double a)
        {
            return a * totalResult;
        }

        double division(double a)
        {
            return a / totalResult;
        }

        void Number_Click(object sender, RoutedEventArgs e)
        {
            totalResult = 0;
        }
        void Number1_Click(object sender, RoutedEventArgs e)
        {
            totalResult = 1;
        }

        void Number2_Click(object sender, RoutedEventArgs e)
        {
            totalResult = 2;
        }

        void Number3_Click(object sender, RoutedEventArgs e)
        {
            totalResult = 3;
        }

        void Number4_Click(object sender, RoutedEventArgs e)
        {
            totalResult = 4;
        }

        void Number5_Click(object sender, RoutedEventArgs e)
        {
            totalResult = 5;
        }

        void Number6_Click(object sender, RoutedEventArgs e)
        {
            totalResult = 6;
        }
        void Number7_Click(object sender, RoutedEventArgs e)
        {
            totalResult = 7;
        }

        void Number8_Click(object sender, RoutedEventArgs e)
        {
            totalResult = 8;
        }

        void Number9_Click(object sender, RoutedEventArgs e)
        {
            totalResult = 9;
        }
        #endregion
    }
}