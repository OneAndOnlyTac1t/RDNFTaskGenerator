using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using TDNFGenerator.Model;

namespace TDNFGenerator.ViewModel
{
    public class MainWindowViewModel:MainWindowViewModelBase
    {
        SingleTask _selectedItem;
        ObservableCollection<SingleTask> _tasks;
        RelayCommand.RelayCommand _generateCommand;
        RelayCommand.RelayCommand _calculateCommand;
        RelayCommand.RelayCommand _addCommand;
        RelayCommand.RelayCommand _saveCommand;
        RelayCommand.RelayCommand _openMessageWindow;
        RelayCommand.RelayCommand _removeCommand;
        RelayCommand.RelayCommand _displayCommand;
        List<int> _amountOfDnfs;
        readonly List<int> _amountOfArguments;
        readonly List<string> _algorithmList;
        string selectedAlgorithm;
        string _ddnf;
        string _text;
        string _tdnf;
        string _displayedTask;
        int _selectedAmountOfArguments;
        int _selectedAmountOfDnf;
        bool _enableRemoveButton;
        bool _enableAddButton;
        public string SelectedAlgorithm
        {
            get
            {
                return selectedAlgorithm;
            }
            set
            {
                selectedAlgorithm = value;
                OnPropertyChanged(nameof(SelectedAlgorithm));
            }
        }
        public List<string> AlgorithmList
        {
            get
            {
                return _algorithmList;
            }            
        }
        public ObservableCollection<SingleTask> Taskslist
        {
            get
            {
                return _tasks;
            }
            set
            {
                _tasks = value;
                 OnPropertyChanged(nameof(Taskslist));
            }
        }
        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
                OnPropertyChanged(nameof(Text));
            }
        }
        public bool EnableRemoveButton
        {
            get
            {
                return _enableRemoveButton;
            }
            set
            {
                _enableRemoveButton = value;
                OnPropertyChanged(nameof(EnableRemoveButton));
            }
        }
        public bool EnableAddButton
        {
            get
            {
                return _enableAddButton;
            }
            set
            {
                _enableAddButton = value;
                OnPropertyChanged(nameof(EnableAddButton));
            }
        }
        public string DisplayedTask
        {
            get
            {
                return _displayedTask;
            }
            set
            {
                _displayedTask = value;
                OnPropertyChanged(nameof(DisplayedTask));
            }
        }
        public SingleTask SelectedTask
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                _selectedItem = value;
                EnableRemoveButton = true;
                OnPropertyChanged(nameof(SelectedTask));
            }
        }
        public ICommand AddTask
        {
            get
            {
                if (_addCommand == null)
                    _addCommand = new RelayCommand.RelayCommand(
                        () => this.AddItem());

                return _addCommand;
            }
        }
        public ICommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                    _saveCommand = new RelayCommand.RelayCommand(
                        () => this.Save());

                return _saveCommand;
            }
        }
        public ICommand DisplayCommand
        {
            get
            {
                if (_displayCommand == null)
                    _displayCommand = new RelayCommand.RelayCommand(
                        () => this.Display());

                return _displayCommand;
            }
        }
        public ICommand OpenWindowCommand
        {
            get
            {
                if (_openMessageWindow == null)
                    _openMessageWindow = new RelayCommand.RelayCommand(
                        () => this.OpenWindow());

                return _openMessageWindow;
            }
        }
        public ICommand RemoveTask
        {
            get
            {
                if (_removeCommand == null)
                    _removeCommand = new RelayCommand.RelayCommand(
                        () => this.RemoveItem());

                return _removeCommand;
            }
        }
        public ICommand GenerateDdnf
        {
            get
            {
                if (_generateCommand == null)
                    _generateCommand = new RelayCommand.RelayCommand(
                        () => this.Generate());

                return _generateCommand;
            }
        }
        public ICommand CalculateTdnf
        {
            get
            {
                if (_calculateCommand == null)
                    _calculateCommand = new RelayCommand.RelayCommand(
                        () => this.Calculate());

                return _calculateCommand;
            }
        }
        public int SelectedAmountOfArguments
        {
            get
            {
                return _selectedAmountOfArguments;
            }
            set
            {
                _selectedAmountOfArguments = value;
                AmountOfDnfs = CalculateDnfAmount();
                OnPropertyChanged(nameof(SelectedAmountOfArguments));
            }
        }
        public string DDNF
        {
            get
            {
                return _ddnf;
            }
            set
            {
                _ddnf = value;
                OnPropertyChanged(nameof(DDNF));
            }
        }
        public string Tdnf
        {
            get
            {
                return _tdnf;
            }
            set
            {
                _tdnf = value;
                OnPropertyChanged(nameof(Tdnf));
            }
        }
        public List<int> AmountOfDnfs
        {
            get
            {
                return _amountOfDnfs;
            }
            set
            {
                _amountOfDnfs = value;
                OnPropertyChanged(nameof(AmountOfDnfs));
            }
        }
        public List<int> AmountOfArguments
        {
            get
            {
                return _amountOfArguments;
            }
        }
        public int SelectedAmountOfDnf
        {
            get
            {
                return _selectedAmountOfDnf;
            }
            set
            {
                _selectedAmountOfDnf = value;
                OnPropertyChanged(nameof(SelectedAmountOfDnf));
            }
        }
        public MainWindowViewModel()
        {
            _amountOfArguments = new List<int>() { 2, 3, 4};
            _algorithmList = new List<string>() {"Алгоритм спрощення", "Квайна-МакКласкі" };
            SelectedAlgorithm = AlgorithmList[0];
            SelectedAmountOfArguments = AmountOfArguments[0];
            SelectedAmountOfDnf = AmountOfDnfs[0];
            Taskslist = new ObservableCollection<SingleTask>();
            EnableRemoveButton = false;
            EnableAddButton = false;
        }
        List<int> CalculateDnfAmount()
        {
            var number = Math.Pow(2, SelectedAmountOfArguments);
            List<int> result = new List<int>();
            for (int i = 1; i <= number; i++)
            {
                result.Add(i);
            }
            return result;
        }
        void Generate()
        {
            DDNF = Model.DDNF.GenerateDDNF(SelectedAmountOfArguments, SelectedAmountOfDnf, (int)Math.Pow(2, SelectedAmountOfArguments));
        }
        void Calculate()
        {
            if (SelectedAlgorithm == "Алгоритм спрощення")
            {
                if (DDNF != null)
                {
                    Tdnf = Model.TDNF.SimplifyDDNF(DDNF);
                    EnableAddButton = true;
                }
                else
                {
                    MessageBox.Show("Введіть вхідну ДДНФ");
                }
            }else if (SelectedAlgorithm == "Квайна-МакКласкі")
            {
                if(DDNF != null)
                {
                    Tdnf = Model.QuineMcClaski.QuineMcClaskiAlgorithm(DDNF);
                    EnableAddButton = true;
                }
                else
                {
                    MessageBox.Show("Введіть вхідну ДДНФ");
                }                
            }
        }
        void AddItem()
        {
            Taskslist.Add(new SingleTask(Text) { Question = DDNF, Answer = Tdnf});
        }
        void RemoveItem()
        {
            Taskslist.Remove(SelectedTask);
            EnableRemoveButton = false;
        }
        void Save()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "XML file (*.xml)|*.xml";
            if (saveFileDialog.ShowDialog() == true)
                Model.XmlQuizWriter.CreateXmlFile(Taskslist, saveFileDialog.FileName);

        }
        void OpenWindow()
        {
            MessageWindow messageWindow = new MessageWindow(this);
            messageWindow.ShowDialog();
        }
        void Display()
        {
            DisplayWindow displayWindow = new DisplayWindow(this);
            DisplayedTask = SingleTask.Text + SelectedTask.Question;
            displayWindow.ShowDialog();
        }
    }
}
