using System;

namespace Games
{
    /**
    Параметры конфигурации с выбором
    */
    public class ConfigParam
    {
        public string ConfigTitle;
        public string[] PossibleValues;
        public string SelectedValue => PossibleValues[SelectedValueIndex];
        public int SelectedValueIndex
        {
            get
            {
                return _selectedValue;
            }
            set
            {
                _selectedValue = value;
                if (value < 0 || value >= PossibleValues.Length)
                    throw new IndexOutOfRangeException();
                OnValueChanged?.Invoke(PossibleValues[value]);
            }
        }
        int _selectedValue;
        public Action<string> OnValueChanged;

        public ConfigParam(string title, string[] values)
        {
            this.ConfigTitle = title;
            this.PossibleValues = values;
            this.SelectedValueIndex = 0;
        }

        public ConfigParam(string title, string[] values, int defaultSelected) : this(title, values)
        {
            this.SelectedValueIndex = defaultSelected;
        }

        public ConfigParam BindTo<T>(IConfigValue<T> v)
        {
            this.OnValueChanged += (s) => v.ChangeValueTo(s);
            return this;
        }

        public ConfigParam DefaultSelected(int index)
        {
            this.SelectedValueIndex = index;
            return this;
        }

        public string SelectNextValue()
        {
            if (SelectedValueIndex == PossibleValues.Length - 1)
                SelectedValueIndex = 0;
            else
                SelectedValueIndex++;
            return this.ToString();
        }

        public string SelectPrevValue()
        {
            if (SelectedValueIndex == 0)
                SelectedValueIndex = PossibleValues.Length - 1;
            else
                SelectedValueIndex--;
            return this.ToString();
        }

        public override string ToString()
        {
            return $"{this.ConfigTitle} [{this.PossibleValues[SelectedValueIndex]}]";
        }
    }
}