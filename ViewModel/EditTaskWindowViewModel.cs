using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TDNFGenerator.Model;
using TDNFGenerator.Model.Data;

namespace TDNFGenerator.ViewModel
{
    public class EditTaskWindowViewModel:BindableBase
    {
        string _question;
        string _answer;
        string _firstWrong;
        string _secondWrong;
        int correnctAnswerIndex;
        MainWindowViewModel _mainWindowViewModel;
        string _thirdWrong;
        RelayCommand.RelayCommand _saveCommand;
        public string Question
        {
            get
            {
                return _question;
            }
            set
            {
                _question = value;
                OnPropertyChanged(nameof(Question));
            }
        }
        public string Answer
        {
            get
            {
                return _answer;
            }
            set
            {
                _answer = value;
                OnPropertyChanged(nameof(Answer));
            }
        }
        public string FirstWrong
        {
            get
            {
                return _firstWrong;
            }
            set
            {
                _firstWrong = value;
                OnPropertyChanged(nameof(FirstWrong));
            }
        }
        public string SecondWrong
        {
            get
            {
                return _secondWrong;
            }
            set
            {
                _secondWrong = value;
                OnPropertyChanged(nameof(SecondWrong));
            }
        }
        public string ThirdWrong
        {
            get
            {
                return _thirdWrong;
            }
            set
            {
                _thirdWrong = value;
                OnPropertyChanged(nameof(ThirdWrong));
            }
        }
        public ICommand SaveCommmand
        {
            get
            {
                if (_saveCommand == null)
                    _saveCommand = new RelayCommand.RelayCommand(
                        () =>
                        {
                            Save();
                        });

                return _saveCommand;
            }
        }
        public EditTaskWindowViewModel(SingleTestTask singleTestTask, MainWindowViewModel mainWindowViewModel)
        {
            _mainWindowViewModel = mainWindowViewModel;
            Question = singleTestTask.Question;
            Answer = singleTestTask.CorrectAnswer;
            var wrongAnswers = singleTestTask.AllTestAnswers.Where(k => k.Validity==false).ToList();
            correnctAnswerIndex = singleTestTask.AllTestAnswers.IndexOf(singleTestTask.AllTestAnswers.Where(l => l.Content.Equals(Answer)).FirstOrDefault());
            FirstWrong = wrongAnswers[0].Content;
            SecondWrong = wrongAnswers[1].Content;
            ThirdWrong = wrongAnswers[2].Content;
        }
        private void Save()
        {
            var newWrongList = new ObservableCollection<TestOption>();
            newWrongList.Add(new TestOption() { Content = FirstWrong, Validity = false }) ;
            newWrongList.Add(new TestOption() { Content = SecondWrong, Validity = false });
            newWrongList.Add(new TestOption() { Content = ThirdWrong, Validity = false });
            newWrongList.Insert(correnctAnswerIndex, new TestOption() { Content = _mainWindowViewModel.SelectedTestTask.CorrectAnswer, Validity = true });
            _mainWindowViewModel.SelectedTestTask.AllTestAnswers = newWrongList;
            var x = _mainWindowViewModel.TestTasksList.Where(k => k.Question.Equals(_mainWindowViewModel.SelectedTestTask.Question)).FirstOrDefault() as SingleTestTask;
            x.AllTestAnswers = newWrongList;
        }
    }
}
