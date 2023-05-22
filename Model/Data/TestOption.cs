using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDNFGenerator.ViewModel;

namespace TDNFGenerator.Model.Data
{
    public class TestOption: BindableBase
    {
        public bool Validity { get; set; }
        private string _content;
        public string Content 
        { 
            get 
            { 
                return _content; 
            }
            set 
            {
                _content = value; 
                OnPropertyChanged(nameof(Content));
            }
        }

    }
}
