using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDNFGenerator.Model.Data;
using TDNFGenerator.ViewModel;

namespace TDNFGenerator.Model
{
    public class SingleTestTask : BindableBase, ITask
    {
        public string Question { get; set; }
        public string CorrectAnswer { get; set; }
        public string Text { get; set; }

        public ObservableCollection<TestOption> _allTestAnswers;
        public ObservableCollection<TestOption> AllTestAnswers
        {
            get { return _allTestAnswers; }
            set
            {
                _allTestAnswers = value;
                OnPropertyChanged(nameof(AllTestAnswers));
            }
        }
        public SingleTestTask()
        {
        }
    }
}
