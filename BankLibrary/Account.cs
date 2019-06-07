using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankLibrary
{
    public class Account : IAccount
    {
        //Событие, возникающее при выводе денег
        protected internal event AccountStateHandler Withdrawed;
        // Событие возникающее при добавление на счет
        protected internal event AccountStateHandler Added;
        // Событие возникающее при открытии счета
        protected internal event AccountStateHandler Opened;
        // Событие возникающее при проверке счета
        protected internal event AccountStateHandler Checked;

        protected int _id;
        static int counter = 0;

        protected decimal _sum; // Переменная для хранения суммы
        protected int _percentage; // Переменная для хранения процента

        protected int _days = 0; // время с момента открытия счета

        public Account(decimal sum, int percentage)
        {
            _sum = sum;
            _percentage = percentage;
            _id = ++counter;
        }

        // Текущая сумма на счету
        public decimal CurrentSum
        {
            get { return _sum; }
        }

        public int Percentage
        {
            get { return _percentage; }
        }

        public int Id
        {
            get { return _id; }
        }
        // вызов событий
        private void CallEvent(AccountEventArgs e, AccountStateHandler handler)
        {
            if (handler != null && e != null)
                handler(this, e);
        }

        // вызов отдельных событий. Для каждого события определяется свой витуальный метод
        protected virtual void OnOpened(AccountEventArgs e)
        {
            CallEvent(e, Opened);
        }

        protected virtual void OnWithdrawed(AccountEventArgs e)
        {
            CallEvent(e, Withdrawed);
        }

        protected virtual void OnAdded(AccountEventArgs e)
        {
            CallEvent(e, Added);
        }

        protected virtual void OnChecked(AccountEventArgs e)
        {
            CallEvent(e, Checked);
        }

        public virtual void Put(decimal sum)
        {
            _sum += sum;
            OnAdded(new AccountEventArgs("На счет поступило " + sum, sum));
        }

        public virtual decimal Withdraw(decimal sum)
        {
            decimal result = 0;
            if (sum <= _sum)
            {
                _sum -= sum;
                result = sum;
                OnWithdrawed(new AccountEventArgs("Сумма " + sum + " снята со счета " + _id, sum));
            }
            else
            {
                OnWithdrawed(new AccountEventArgs("Недостаточно денег на счете " + _id, 0));
            }
            return result;
        }

        public virtual decimal Check()
        {
            OnChecked(new AccountEventArgs("Баланс на вашем счете: " + this._sum, this._id));
            return _sum;
        }
        // открытие счета
        protected internal virtual void Open()
        {
            OnOpened(new AccountEventArgs("Открыт новый счет! Id счета: " + this._id, this._sum));
        }

        protected internal void IncrementDays()
        {
            _days++;
        }
    }
}
