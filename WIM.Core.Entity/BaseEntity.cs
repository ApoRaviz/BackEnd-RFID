using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WIM.Core.Entity
{
    public class BaseEntity : INotifyPropertyChanged
    {
        public BaseEntity()
        {
            IsActive = true;
            CreateAt = DateTime.Now;
            UpdateAt = DateTime.Now;
        }       

        public Nullable<bool> IsActive { get; set; }
        public string CreateBy { get; set; }
        public Nullable<DateTime> CreateAt { get; set; }
        public string UpdateBy { get; set; }
        public Nullable<DateTime> UpdateAt { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }

            if (propertyName.ToLower().EndsWith("idsys"))
            {

            }
        }
       
    }    
}
