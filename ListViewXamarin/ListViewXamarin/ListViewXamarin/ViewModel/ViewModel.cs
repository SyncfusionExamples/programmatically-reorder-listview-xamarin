using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace ListViewXamarin
{
    public class ViewModel : INotifyPropertyChanged
    {
        #region Fields

        private ObservableCollection<ToDoItem> toDoList;
        private ToDoItem TappedItem;
        #endregion

        #region Constructor

        public ViewModel()
        {
            this.GenerateSource();
            MarkDoneCommand = new Command<object>(MarkItemAsDone);
        }
        #endregion

        #region Property

        public ObservableCollection<ToDoItem> ToDoList
        {
            get { return toDoList;  }
            set { this.toDoList = value; }
        }

        public Command<object> MarkDoneCommand { get; set; }
        #endregion

        #region Method

        public void GenerateSource()
        {
            ToDoListRepository todoRepository = new ToDoListRepository();
            toDoList = todoRepository.GetToDoList();
        }

        private void MarkItemAsDone(object obj)
        {
            var item = obj as ToDoItem;
            var currentindex = this.ToDoList.IndexOf(item);

            if (!item.IsChecked)
            {
                item.IsChecked = true;
                this.ToDoList.Move(currentindex, this.ToDoList.Count - 1);
            }
            else
            {
                item.IsChecked = false;
                this.ToDoList.Move(currentindex, 0);
            }
        }
        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string name)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
        #endregion
    }
}
