using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDNFGenerator.Model;
using TDNFGenerator.Model.Data;

namespace TDNFGenerator.ViewModel
{
    public class DisplayTestTaskWindowViewModel:BindableBase
    {
        SingleTestTask _selectedTestTask;
        string _message;
        string _answer;
        ObservableCollection<TestOption> _allAnswers;
        MainWindowViewModel _mainWindowViewModel;

        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                _message = value;
                OnPropertyChanged(nameof(Message));
            }
        }
        public string CorrectAnswer
        {
            get
            {
                return _answer;
            }
            set
            {
                _answer = value;
                OnPropertyChanged(nameof(CorrectAnswer));
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
                OnPropertyChanged(nameof(SelectedTestTask));
            }
        }
        public ObservableCollection<TestOption> AllAnswers
        {
            get
            {
                return _allAnswers;
            }
            set
            {
                _allAnswers = value;
                OnPropertyChanged(nameof(AllAnswers));
            }
        }
        public DisplayTestTaskWindowViewModel(MainWindowViewModel mainWindowViewModel, SingleTestTask singleTestTask)
        {
            _mainWindowViewModel = mainWindowViewModel;
            SelectedTestTask = singleTestTask;
            CorrectAnswer = SelectedTestTask.CorrectAnswer;
            Message = mainWindowViewModel.DisplayText + "\n" + SelectedTestTask.Text + "\n" + SelectedTestTask.Question;
            AllAnswers = SelectedTestTask.AllTestAnswers;
        }
    }
}
