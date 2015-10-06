using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq.Expressions;
namespace CreditPlanner
{
    public class Requirement : INotifyPropertyChanged
    {
        private int _result;
        public int result
        {
            get
            {
                return _result;
            }

            set { SetField(ref _result, value, () => result); }
        }
        private int _count;
        public int count
        {
            get
            {
                return _count;
            }

            set { SetField(ref _count, value, () => count); }
        }
        private int _required;
        public int required
        {
            get
            {
                return _required;
            }
            set
            {
                _required = value;
                OnPropertyChanged("colorcode");
            }

        }
        public string LINQ { get; set; }
      
        public string description { get; set; }
        public int colorcode
        {
            get
            {
                if (result == -1)
                    return -1;
                if (result >= required)
                    return 1;
                else
                    return 0;
            }
        }
        public void execute(List<Course> target)
        {
            if (String.IsNullOrEmpty(LINQ))
                return;
            try
            {
                int z = 0;
                var l = target.Where(LINQ);
                l.ToList().ForEach(x => z += x.credits);
                result = z;
                count = l.Count();
            }
            catch
            {
                result = -1;
            }
            OnPropertyChanged("colorcode");
            Console.WriteLine(result);
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged<T>(Expression<Func<T>> selectorExpression)
        {
            if (selectorExpression == null)
                throw new ArgumentNullException("selectorExpression");
            MemberExpression body = selectorExpression.Body as MemberExpression;
            if (body == null)
                throw new ArgumentException("The body must be a member expression");
            OnPropertyChanged(body.Member.Name);
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        protected bool SetField<T>(ref T field, T value, Expression<Func<T>> selectorExpression)
        {
       //     if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(selectorExpression);
            return true;
        }
    }
}
