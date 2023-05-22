using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using TDNFGenerator.Model;
using TDNFGenerator.Model.Interfaces;
using TDNFGenerator.Model.Data;
using TDNFGenerator.Views;
using TDNFGenerator.Model.XmlSavers;
using TDNFGenerator.Model.Alghorithms;

namespace TDNFGenerator.ViewModel
{
    public class MainWindowViewModel : BindableBase
    {
        ObservableCollection<ITask> selectedTaskType;
        //  IXmlSaveMethod chosenXmlSaveMethod;
        XmlSaverContext xmlSaverContext;
          AlghorithmsContext alghorithmsContext;
        SingleTask _selectedItem;
        SingleTestTask _selectedTestTask;
        ObservableCollection<ITask> _tasks;
        ObservableCollection<ITask> _testTasks;
        Visibility _testAnswersVisibility;
        Visibility _shortAnswersVisibility;
        RelayCommand.RelayCommand _generateCommand;
        RelayCommand.RelayCommand _calculateCommand;
        RelayCommand.RelayCommand _addCommand;
        RelayCommand.RelayCommand _editTestTaskCommand;
        RelayCommand.RelayCommand _saveCommand;
        RelayCommand.RelayCommand _openMessageWindow;
        RelayCommand.RelayCommand _removeCommand;
        RelayCommand.RelayCommand _displayCommand;
        List<int> _amountOfDnfs;
        ObservableCollection<TestOption> _testAnswers;
        readonly List<int> _amountOfArguments;
        readonly List<string> _algorithmList;
        readonly List<string> _answersTypeList;
        string selectedAnswerType;
        string selectedAlgorithm;
        string _ddnf;
        string _text;
        string _tdnf;
        string _displayedTask;
        int _selectedAmountOfArguments;
        int _selectedAmountOfDnf;
        bool _enableRemoveButton;
        bool _enableAddButton;
        bool _setMessageForAllTasks;
        bool _isTestEnabled = false;
        #region Properties
        public Visibility TestAnswersVisibility
        {
            get
            {
                return _testAnswersVisibility;
            }
            set
            {
                _testAnswersVisibility = value;
                OnPropertyChanged(nameof(TestAnswersVisibility));
            }
        }
        public Visibility ShortAnswersVisibility
        {
            get
            {
                return _shortAnswersVisibility;
            }
            set
            {
                _shortAnswersVisibility = value;
                OnPropertyChanged(nameof(ShortAnswersVisibility));
            }
        }
        public string SelectedAlgorithm
        {
            get
            {
                return selectedAlgorithm;
            }
            set
            {
                selectedAlgorithm = value;
                if (selectedAlgorithm == "Градієнтного спуску")
                {
                    alghorithmsContext?.SetAlgorithm(new TDNF());
                    Taskslist?.Clear();
                    TestTasksList?.Clear();
                }
                else
                {
                    alghorithmsContext?.SetAlgorithm(new QuineMcClaski());
                    Taskslist?.Clear();
                    TestTasksList?.Clear();
                }
                selectedAlgorithm = value;
                OnPropertyChanged(nameof(SelectedAlgorithm));
            }
        }
        public string SelectedAnswerType
        {
            get
            {
                return selectedAnswerType;
            }
            set
            {
                selectedAnswerType = value;
                if (selectedAnswerType== "Тестове завдання")
                {
                    xmlSaverContext?.SetXmlSaveMethod( new XMLTestSaver());
                    selectedTaskType = TestTasksList;
                }
                else
                {
                    xmlSaverContext?.SetXmlSaveMethod(new XMLShortAnswerSaver());
                    selectedTaskType = Taskslist;
                }
                OnPropertyChanged(nameof(SelectedAnswerType));
            }
        }
        public List<string> AlgorithmList
        {
            get
            {
                return _algorithmList;
            }
        }
        public List<string> AnswersTypeList
        {
            get
            {
                return _answersTypeList;
            }
        }
        public ObservableCollection<ITask> Taskslist
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
        public ObservableCollection<TestOption> TestAnswers
        {
            get
            {
                return _testAnswers;
            }
            set
            {
                _testAnswers = value;
                OnPropertyChanged(nameof(TestAnswers));
            }
        }
        public ObservableCollection<ITask> TestTasksList
        {
            get
            {
                return _testTasks;
            }
            set
            {
                _testTasks = value;
                OnPropertyChanged(nameof(TestTasksList));
            }
        }
        public string DisplayText
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
                OnPropertyChanged(nameof(DisplayText));
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
        public bool SetMessageForAllTasks
        {
            get
            {
                return _setMessageForAllTasks;
            }
            set
            {
                _setMessageForAllTasks = value;
                OnPropertyChanged(nameof(SetMessageForAllTasks));
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
        public SingleTestTask SelectedTestTask
        {
            get
            {
                return _selectedTestTask;
            }
            set
            {
                _selectedTestTask = value;
                EnableRemoveButton = true;
                OnPropertyChanged(nameof(SelectedTestTask));
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
        public ICommand WrongAnswersCommand
        {
            get
            {
                if (_editTestTaskCommand == null)
                    _editTestTaskCommand = new RelayCommand.RelayCommand(
                        () =>
                        {
                            if (SelectedTestTask == null)
                            {
                                MessageBox.Show("Запитання не вибрано");
                            }
                            else
                            {
                                EditTaskWindow window = new EditTaskWindow(SelectedTestTask, this);
                                window.ShowDialog();
                            }
                        }); 

                return _editTestTaskCommand;
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
        #endregion
        public MainWindowViewModel()
        {
            _amountOfArguments = new List<int>() { 2, 3, 4 };
            _algorithmList = new List<string>() { "Градієнтного спуску", "Квайна-МакКласкі" };
            _answersTypeList = new List<string>() { "Відкрита коротка відповідь", "Тестове завдання" };
            SelectedAnswerType = AnswersTypeList[0];
            SelectedAlgorithm = AlgorithmList[0];
            SelectedAmountOfArguments = AmountOfArguments[0];
            SelectedAmountOfDnf = AmountOfDnfs[0];
            TestTasksList = new ObservableCollection<ITask>();
            ShortAnswersVisibility = Visibility.Visible;
            TestAnswersVisibility = Visibility.Hidden;
            Taskslist = new ObservableCollection<ITask>();
            EnableRemoveButton = false;
            EnableAddButton = false;
            alghorithmsContext = new AlghorithmsContext(new TDNF());
            xmlSaverContext = new XmlSaverContext(new XMLShortAnswerSaver());
            selectedTaskType = Taskslist;
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
            if (DDNF != null)
            {
                Tdnf = alghorithmsContext.ExecuteMinimization(DDNF);
                EnableAddButton = true;
                if (SelectedAnswerType == "Тестове завдання")
                {
                    var gen = new WrongAnswersGenerator(Tdnf, DDNF, alghorithmsContext);
                    _isTestEnabled = true;
                    TestAnswers = new ObservableCollection<TestOption>(gen.Answers);
                }
                else
                {
                    _isTestEnabled = false;
                }
            }
            else
            {
                MessageBox.Show("Введіть вхідну ДДНФ");
            }
           
        }
        void AddItem()
        {
            if (!_isTestEnabled)
            {
               // if (SetMessageForAllTasks)
                Taskslist.Add(new SingleTask() { Question = DDNF, CorrectAnswer = Tdnf, Text = DisplayText });
                ShortAnswersVisibility = Visibility.Visible;
                TestAnswersVisibility = Visibility.Hidden;
            }
            else
            {
                TestTasksList.Add(new SingleTestTask() { CorrectAnswer = Tdnf, Question = DDNF, AllTestAnswers = TestAnswers, Text = DisplayText });
                ShortAnswersVisibility = Visibility.Hidden;
                TestAnswersVisibility = Visibility.Visible;
            }
        }
        void RemoveItem()
        {
            if (_isTestEnabled)
            {
                TestTasksList.Remove(SelectedTestTask);
                EnableRemoveButton = false;
            }
            else
            {
                Taskslist.Remove(SelectedTask);
                EnableRemoveButton = false;
            }
        }
        void Save()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "XML file (*.xml)|*.xml";
            if (saveFileDialog.ShowDialog() == true)
            {              
                   xmlSaverContext.ExecuteXmlSaveMethod(selectedTaskType, alghorithmsContext,  saveFileDialog.FileName);              
            }
        }

        void OpenWindow()
        {
            MessageWindow messageWindow = new MessageWindow(this);
            messageWindow.ShowDialog();
        }
        void Display()
        {
            if (!_isTestEnabled)
            {
                DisplayWindow displayWindow = new DisplayWindow(this);
                DisplayedTask = SelectedTask.Text + "\n" + SelectedTask.Question;
                displayWindow.ShowDialog();
            }
            else
            {
                DisplayTestTaskWindow displayTestTaskWindow = new DisplayTestTaskWindow(this, SelectedTestTask);
                displayTestTaskWindow.ShowDialog();
            }
        }
    }
}
